using Quartz;
using System;
using System.Threading.Tasks;

namespace LabBillingJobs;

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
