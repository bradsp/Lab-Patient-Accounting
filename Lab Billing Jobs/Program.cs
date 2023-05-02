using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using log4net.Config;
using LabBilling.Core.DataAccess;

namespace LabBillingJobs
{
    internal class Program
    {
        //public static string ConnectionString { get; set; }
        //public static string Server { get; set; }
        //public static string Database { get; set; }
        //public static string LogDatabase { get; set; }

        public static AppEnvironment AppEnvironment { get; set; } = new AppEnvironment();

        static void Main(string[] args)
        {

            AppEnvironment.DatabaseName = Properties.Settings.Default.DbName;
            AppEnvironment.ServerName = Properties.Settings.Default.DbServer;
            AppEnvironment.LogDatabaseName = Properties.Settings.Default.LogDbName;
            AppEnvironment.RunAsService = true;
            AppEnvironment.ServiceUsername = Properties.Settings.Default.Username;
            AppEnvironment.ServicePassword = Properties.Settings.Default.Password;

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XmlConfigurator.Configure(new System.IO.FileInfo($"{directory}\\log4net.config"));

            var exitcode = HostFactory.Run(x =>
            {

                x.RunAsLocalSystem();
                x.UseLog4Net();
                x.SetServiceName("LabBillingJobsService");
                x.SetDisplayName("Lab Billing Jobs Service");

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
