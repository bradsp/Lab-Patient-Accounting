using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using log4net.Config;
using System;
using Topshelf;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace LabBillingService;

static class Program
{

    public static AppEnvironment AppEnvironment { get; set; }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    static void Main()
    {
        try
        {

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables("LABPA_");

            IConfiguration config = builder.Build();

            string envDatabase = config.GetValue<string>("LABPA_DATABASE");
            string envDbServer = config.GetValue<string>("LABPA_DB_SERVER");
            string envDbUser = config.GetValue<string>("LABPA_DB_USER");
            string envDbPass = config.GetValue<string>("LABPA_DB_PASS");
            string envLogDb = config.GetValue<string>("LABPA_LOG_DB");

            AppEnvironment = new AppEnvironment
            {
                DatabaseName = envDatabase ?? config.GetValue<string>("Connection:DatabaseName"),
                ServerName = envDbServer ?? config.GetValue<string>("Connection:DatabaseServer"),
                LogDatabaseName = envLogDb ?? config.GetValue<string>("Connection:LogDatabaseName"),
                RunAsService = true,
                ServicePassword = envDbPass ?? config.GetValue<string>("ServiceAccount:ServiceUserName"),
                ServiceUsername = envDbUser ?? config.GetValue<string>("ServiceAccount:ServicePassword")
            };

            SystemService systemService = new(AppEnvironment);

            AppEnvironment.ApplicationParameters = systemService.LoadSystemParameters();

            string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(path);

            XmlConfigurator.Configure(new System.IO.FileInfo($"{directory}/log4net.config"));

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
        catch (Exception ex)
        {

        }
    }
}
