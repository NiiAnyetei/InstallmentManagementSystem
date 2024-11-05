using Hangfire;
using Serilog;
using ServiceLayer.Service;
using System.Text.RegularExpressions;

namespace IMS.Extensions
{
    public static class HostExtensionMethods
    {
        public static IApplicationBuilder RegisterRecurringJobs(this IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();

            try
            {
                var manager = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
                var jobs = scope.ServiceProvider.GetRequiredService<IEnumerable<IRecurringJob>>();

                foreach (var job in jobs)
                {
                    try
                    {
                        manager.AddOrUpdate(job.JobId, () => job.ExecuteAsync(), job.CronExpression);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "Unable to register job with id: {id}", job.JobId);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Unable to register jobs");
            }

            return app;
        }
    }

    /// <summary>
    /// Special route naming convention
    /// </summary>
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc />
        public string TransformOutbound(object value)
        {
            return value == null ? null : Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2").ToLower();
        }
    }
}
