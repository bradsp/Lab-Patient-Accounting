using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace LabBillingMQService
{
    public class ChargeInterfaceProcessing : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Starting Charge Interface Processing {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
