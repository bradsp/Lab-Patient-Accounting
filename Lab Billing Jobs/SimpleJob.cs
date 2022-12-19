using System;
using System.Threading;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using Quartz;

namespace LabBillingJobs
{
    public partial class JobProcessor
    {
        public class SimpleJob : IJob
        {
            public Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"SimpleJob running at {DateTime.Now}");


                return Task.CompletedTask;
            }
        }
    }
}
