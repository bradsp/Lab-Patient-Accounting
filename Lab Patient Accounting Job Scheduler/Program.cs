﻿using Lab_Patient_Accounting_Job_Scheduler;
using LabBilling.Core.DataAccess;
using log4net.Config;
using System;
using Topshelf;

namespace LabBillingJobs;

internal class Program
{

    public static AppEnvironment AppEnvironment { get; set; } = new AppEnvironment();

    static void Main(string[] args)
    {

        AppEnvironment.DatabaseName = Settings.Default.DbName;
        AppEnvironment.ServerName = Settings.Default.DbServer;
        AppEnvironment.LogDatabaseName = Settings.Default.LogDbName;
        AppEnvironment.RunAsService = true;
        AppEnvironment.ServiceUsername = Settings.Default.Username;
        AppEnvironment.ServicePassword = Settings.Default.Password;

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
