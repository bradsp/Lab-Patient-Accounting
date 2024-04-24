using Quartz;
using Quartz.Impl;
using System;

namespace LabBillingJobs;

public partial class JobProcessor
{
    private static readonly log4net.ILog _log =
        log4net.LogManager.GetLogger
        (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

    private IScheduler _scheduler;
    private const string _group1 = "BusinessTasks";
    private const string _job = "Job";

    public JobProcessor()
    {
    }

    public async void OnStart()
    {
        StdSchedulerFactory schedulerFactory = new();
        _scheduler = await schedulerFactory.GetScheduler();

        await _scheduler.Start();

        AddJobs();
    }

    public void OnPaused() => _scheduler.PauseAll();
    public void OnContinue() => _scheduler.ResumeAll();
    public void OnStop() => _scheduler.Shutdown();

    private void AddJobs()
    {
        AddAccountValidationJob();
        AddClaimProcessingJob();
        AddNotesProcessingJob();
    }

    public void AddAccountValidationJob()
    {
        const string trigger1 = "AccountValidation";

        IJob myJob = new AccountValidationJob();
        var jobDetail = new JobDetailImpl(trigger1 + _job, _group1, myJob.GetType());

        var trigger = TriggerBuilder.Create()
            .WithIdentity(trigger1 + _job, _group1)
            .WithCronSchedule("0 0 5 ? * * *")
            //.StartAt(DateTime.Now.AddSeconds(5)) // some Date 
            .ForJob(trigger1 + _job, _group1) // identify job with name, group strings
            .Build();

        _scheduler.ScheduleJob(jobDetail, trigger);
        var nextFireTime = trigger.GetNextFireTimeUtc();
        Console.WriteLine($"{trigger1} Job initialized. Next run time - {nextFireTime}");
        _log.Info($"{trigger1} Job initialized. Next run time - {nextFireTime}");


    }

    public void AddClaimProcessingJob()
    {
        const string trigger1 = "ClaimProcessing";

        IJob myJob = new ClaimsProcessingJob();
        var jobDetail = new JobDetailImpl(trigger1 + _job, _group1, myJob.GetType());

        var trigger = TriggerBuilder.Create()
            .WithIdentity(trigger1 + _job, _group1)
            .WithCronSchedule("0 0 21 ? * SUN,MON,TUE,WED,THU *")
            //.StartAt(DateTime.Now.AddSeconds(5)) // some Date 
            .ForJob(trigger1 + _job, _group1) // identify job with name, group strings
            .Build();

        _scheduler.ScheduleJob(jobDetail, trigger);
        var nextFireTime = trigger.GetNextFireTimeUtc();
        Console.WriteLine($"{trigger1} Job initialized. Next run time - {nextFireTime}");
        _log.Info($"{trigger1} Job initialized. Next run time - {nextFireTime}");
    }

    public void AddNotesProcessingJob()
    {
        const string trigger1 = "NotesProcessing";

        IJob myJob = new NotesProcessingJob();
        var jobDetail = new JobDetailImpl(trigger1 + _job, _group1, myJob.GetType());

        var trigger = TriggerBuilder.Create()
            .WithIdentity(trigger1 + _job, _group1)
            .WithCronSchedule("0 0 7 * * ?")
            //.StartAt(DateTime.Now.AddSeconds(5)) // some Date 
            .ForJob(trigger1 + _job, _group1) // identify job with name, group strings
            .Build();

        _scheduler.ScheduleJob(jobDetail, trigger);
        var nextFireTime = trigger.GetNextFireTimeUtc();
        Console.WriteLine($"{trigger1} Job initialized. Next run time - {nextFireTime}");
        _log.Info($"{trigger1} Job initialized. Next run time - {nextFireTime}");
    }


}
