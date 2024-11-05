using DataLayer.Enums;
using DataLayer.Models.Data;
using Hangfire;
using Humanizer;
using ServiceLayer.Service;
using System.Threading;

namespace IMS.Jobs
{
    public class InstallmentCompletionJob : IRecurringJob
    {
        private readonly ILogger<InstallmentCompletionJob> _logger;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly IInstallmentService  _installmentService;

        public InstallmentCompletionJob(ILogger<InstallmentCompletionJob> logger, IInstallmentService installmentService)
        {
            _logger = logger;
            _installmentService = installmentService;
        }

        public string JobId => "ba6138a1-7862-4c85-9e1a-89cc4b6f57fb";

        public string JobName => nameof(InstallmentCompletionJob).Humanize();

        public string CronExpression => Cron.Minutely();

        public async Task ExecuteAsync()
        {
            await _semaphore.WaitAsync();
            _logger.LogInformation("Executing {jobName}:{jobId}", nameof(InstallmentCompletionJob).Humanize(LetterCasing.Title), JobId);

            try
            {
                await _installmentService.CompleteInstallmentsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong executing {jobName} with Id: {jobId}", nameof(InstallmentCompletionJob), JobId);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}
