using DataLayer.Enums;
using DataLayer.Models.Data;
using Hangfire;
using Humanizer;
using ServiceLayer.Service;
using System.Threading;

namespace IMS.Jobs
{
    public class BillDueStatusJob : IRecurringJob
    {
        private readonly ILogger<BillDueStatusJob> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IBillService _billService;

        public BillDueStatusJob(ILogger<BillDueStatusJob> logger, IBillService billService)
        {
            _logger = logger;
            _billService = billService;
        }

        public string JobId => "899b4b0f-7773-4aef-b6ca-3190c253a561";

        public string JobName => nameof(BillDueStatusJob).Humanize();

        public string CronExpression => Cron.Daily(9);

        public async Task ExecuteAsync()
        {
            await _semaphore.WaitAsync();
            _logger.LogInformation("Executing {jobName}:{jobId}", nameof(BillDueStatusJob).Humanize(LetterCasing.Title), JobId);

            try
            {
                await _billService.MarkBillsDueAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong executing {jobName} with Id: {jobId}", nameof(BillDueStatusJob), JobId);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
