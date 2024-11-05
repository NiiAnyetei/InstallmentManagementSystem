using DataLayer.Enums;
using DataLayer.Models.Data;
using DataLayer.Models.Paystack.Data;
using DataLayer.Models.Paystack.DTOs;
using DataLayer.Paystack.DTOs;
using Hangfire;
using Humanizer;
using IMS.Extensions;
using ServiceLayer.External.Paystack.Service;
using ServiceLayer.Provider;
using ServiceLayer.Service;
using System.Threading;

namespace IMS.Jobs
{
    public class BillCollectionJob : IRecurringJob
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILogger<BillCollectionJob> _logger;
        private readonly IBillService _billService;
        private readonly IPaystackService _paystackService;
        private readonly INotificationService _notificationService;

        public BillCollectionJob(ILogger<BillCollectionJob> logger, IBillService billService, IPaystackService paystackService, INotificationService notificationService)
        {
            _logger = logger;
            _billService = billService;
            _paystackService = paystackService;
            _notificationService = notificationService;
        }

        public string JobId => "8d8cde45-b028-489e-841d-f6a58493dc4a";

        public string JobName => nameof(BillCollectionJob).Humanize();

        public string CronExpression => Cron.Daily(8);

        public async Task ExecuteAsync()
        {
            await _semaphore.WaitAsync();
            _logger.LogInformation("Executing {jobName}:{jobId}", nameof(BillCollectionJob).Humanize(LetterCasing.Title), JobId);

            try
            {
                var bills = await _billService.GetDueBillsAsync();

                foreach (var bill in bills)
                {
                    await InitializeTransaction(bill);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong executing {jobName} with Id: {jobId}", nameof(BillCollectionJob), JobId);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task CreateBillChargeAsync(List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                var customer = bill.Installment.Customer;
                var charge = new CreateChargeDto()
                {
                    Amount = bill.Amount,
                    Email = customer.Email,
                    Metadata = new Metadata()
                    {
                        InstallmentId = bill.InstallmentId,
                        BillId = bill.Id
                    },
                    MobileMoney = new MobileMoney()
                    {
                        Phone = customer.PhoneNumber.GetNationalNumber(),
                        //Phone = "0551234987",
                        Provider = bill.PaymentChannel
                    }
                };

                await _paystackService.CreateCharge(charge);
            }
        }

        private async Task InitializeTransaction(Bill bill)
        {
            var transaction = new InitializePaymentDto(bill.Installment.Customer.Email, bill.Amount, new Metadata() { InstallmentId = bill.InstallmentId, BillId = bill.Id });
            var response = await _paystackService.InitializeTransaction(transaction);

            if (response.Status)
            {
                await SendNotificationsAsync(bill, response.Data!.AuthorizationUrl!);
            }
            else
            {
                _logger.LogInformation("Initialize Transaction result: {result}", response.ToJson());
            }
        }

        private async Task SendNotificationsAsync(Bill bill, string paymentLink)
        {
            var customer = bill.Installment.Customer;
            var amount = string.Format("{0:#.00}", bill.Amount);
            var item = bill.Installment.Item;
            var message = string
                .Format(
                "Dear {0}, reminder: GHS{1} for {2} is due. Use payment link: {3} . Regards, EKD PHONES.",
                customer.FirstName, amount, item, paymentLink
            );

            var response = await _notificationService.SendSmsAsync(customer.PhoneNumber, message);
            if (!response.IsSuccess) _logger.LogInformation("Send Bill Due Notification result: {result}", response.ToJson());
        }
    }
}
