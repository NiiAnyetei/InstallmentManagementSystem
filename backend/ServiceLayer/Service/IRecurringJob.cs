using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Service
{
    public interface IRecurringJob
    {
        string JobId { get; }
        string JobName { get; }
        string CronExpression { get; }

        Task ExecuteAsync();
    }
}
