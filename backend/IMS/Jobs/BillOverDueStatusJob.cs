using DataLayer.Enums;
using DataLayer.Models.Data;
using Hangfire;
using Humanizer;
using ServiceLayer.Service;
using System.Threading;

namespace IMS.Jobs
{
    public class BillOverDueStatusJob : IRecurringJob
    {
        private readonly ILogger<BillOverDueStatusJob> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IBillService _billService;

        public BillOverDueStatusJob(ILogger<BillOverDueStatusJob> logger, IBillService billService)
        {
            _logger = logger;
            _billService = billService;
        }

        public string JobId => "7db189dc-5196-427a-afac-26848cf53e5e";

        public string JobName => nameof(BillOverDueStatusJob).Humanize();

        public string CronExpression => Cron.Daily();

        public async Task ExecuteAsync()
        {
            await _semaphore.WaitAsync();
            _logger.LogInformation("Executing {jobName}:{jobId}", nameof(BillOverDueStatusJob).Humanize(LetterCasing.Title), JobId);

            try
            {
                await _billService.MarkBillsOverDueAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong executing {jobName} with Id: {jobId}", nameof(BillOverDueStatusJob), JobId);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
