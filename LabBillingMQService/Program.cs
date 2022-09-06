// See https://aka.ms/new-console-template for more information
using Topshelf;
using LabBillingMQService;
using log4net.Config;


XmlConfigurator.Configure(new System.IO.FileInfo(@"D:\Users\bpowers\source\repos\Lab-Billing\LabBillingMQService\log4net.config"));

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

