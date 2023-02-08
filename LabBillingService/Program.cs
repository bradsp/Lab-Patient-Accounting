using System;
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
            Program.Database = Properties.Settings.Default.DbName;
            Program.Server = Properties.Settings.Default.DbServer;
            Program.LogDatabase = Properties.Settings.Default.LogDbName;

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XmlConfigurator.Configure(new System.IO.FileInfo($"{directory}\\log4net.config"));

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
