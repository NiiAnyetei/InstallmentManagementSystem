using DataLayer.Enums;
using DataLayer.Models.Data;
using DataLayer.Models.Paystack.Data;
using DataLayer.Paystack.DTOs;
using Hangfire;
using Humanizer;
using IMS.Extensions;
using ServiceLayer.Provider;
using ServiceLayer.Service;
using ServiceLayer.Utils;
using System.Text;
using System.Threading;

namespace IMS.Jobs
{
    public class BillDueNotificationJob : IRecurringJob
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly ILogger<BillDueNotificationJob> _logger;
        private readonly IBillService _billService;
        private readonly INotificationService _notificationService;

        public BillDueNotificationJob(ILogger<BillDueNotificationJob> logger, IBillService billService, INotificationService notificationService)
        {
            _logger = logger;
            _billService = billService;
            _notificationService = notificationService;
        }

        public string JobId => "70ca66e4-7d7c-43fa-bcaa-ea00d9dfee80";

        public string JobName => nameof(BillDueNotificationJob).Humanize();

        public string CronExpression => "0 7,13 * * *";

        public async Task ExecuteAsync()
        {
            await _semaphore.WaitAsync();
            _logger.LogInformation("Executing {jobName}:{jobId}", nameof(BillDueNotificationJob).Humanize(LetterCasing.Title), JobId);

            try
            {
                var bills = await _billService.GetDueBillsAsync();
                await SendNotificationsAsync(bills);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong executing {jobName} with Id: {jobId}", nameof(BillDueNotificationJob), JobId);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task SendNotificationsAsync(List<Bill> bills)
        {
            foreach (var bill in bills)
            {
                var customer = bill.Installment.Customer;
                var amount = string.Format("{0:#,##0.00}", bill.Amount);
                var item = bill.Installment.Item;
                var message = string
                    .Format(
                    "Dear {0}, reminder: GHS{1} for {2} is due. Please make payment upon prompt to avoid service interruptions. Regards, EKD PHONES.",
                    customer.FirstName, amount, item
                    );

                var result = await _notificationService.SendSmsAsync(customer.PhoneNumber, message);
                if(!result.IsSuccess) _logger.LogInformation("Send Bill Due Notification result: {result}", result.ToJson());
            }
        }
    }
}
