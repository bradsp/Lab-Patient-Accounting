using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using log4net.Config;

namespace Lab_Billing_Jobs
{
    internal class Program
    {
        static void Main(string[] args)
        {
            XmlConfigurator.Configure(new System.IO.FileInfo(@"D:\Users\bpowers\source\repos\Lab-Billing\Lab Billing Jobs\log4net.config"));

            var exitcode = HostFactory.Run(x =>
            {

                x.RunAsLocalSystem();
                x.UseLog4Net();
                x.SetServiceName("LabBillingMQService");
                x.SetDisplayName("Lab Billing Message Queue Service");

                x.Service<JobProcessor>(s =>
                {
                    s.ConstructUsing(hostSettings => new JobProcessor());
                    s.WhenStarted(service => service.OnStart());
                    s.WhenStopped(service => service.OnStop());
                    s.WhenContinued(service => service.OnContinue());
                    s.WhenPaused(service => service.OnPaused());
                });

            });

            int exitCodeValue = (int)Convert.ChangeType(exitcode, exitcode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }
    }
}
