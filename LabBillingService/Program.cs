using System;
using System.Configuration;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using log4net.Config;
using Topshelf;

namespace LabBillingService
{
    static class Program
    {

        public static string ConnectionString { get; set; }
        public static string Server { get; set; }
        public static string Database { get; set; }
        public static string LogDatabase { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Program.Database = "LabBillingTest"; // Properties.Settings.Default.DbName;
            Program.Server = "WTHMCLBILL"; // Properties.Settings.Default.DbServer;
            Program.LogDatabase = "NLog"; // Properties.Settings.Default.LogDbName;

            XmlConfigurator.Configure(new System.IO.FileInfo(@"D:\Users\bpowers\source\repos\Lab-Billing\LabBillingService\log4net.config"));

            var exitcode = HostFactory.Run(x =>
            {

                x.RunAsLocalSystem();
                x.UseLog4Net();
                x.SetServiceName("LabBillingInterfaceService");
                x.SetDisplayName("Lab Patient Accounting Interface");

                x.Service<InterfaceProcessor>(s =>
                {
                    s.ConstructUsing(hostSettings => new InterfaceProcessor());
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                    s.WhenContinued(service => service.Continue());
                    s.WhenPaused(service => service.Paused());
                });

            });

            int exitCodeValue = (int)Convert.ChangeType(exitcode, exitcode.GetTypeCode());
            Environment.ExitCode = exitCodeValue;
        }


    }
}
