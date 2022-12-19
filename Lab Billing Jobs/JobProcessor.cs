using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Timers;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using static Quartz.Logging.OperationName;

namespace LabBillingJobs
{


    public partial class JobProcessor
    {
        private static readonly log4net.ILog log =
            log4net.LogManager.GetLogger
            (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private IScheduler scheduler;
        private const string Group1 = "BusinessTasks";
        private const string Job = "Job";

//#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public JobProcessor()
//#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public async void OnStart()
        {
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();

            await scheduler.Start();

            AddJobs();
        }

        public void OnPaused() => scheduler.PauseAll();
        public void OnContinue() => scheduler.ResumeAll();
        public void OnStop() => scheduler.Shutdown();

        private void AddJobs()
        {
            AddAccountValidationJob();
        }

        public void AddAccountValidationJob()
        {
            const string trigger1 = "AccountValidation";

            IJob myJob = new AccountValidationJob();
            var jobDetail = new JobDetailImpl(trigger1 + Job, Group1, myJob.GetType());

            var trigger = TriggerBuilder.Create()
                .WithIdentity(trigger1 + Job, Group1)
                .WithCronSchedule("0 0 18 * * ?")
                //.StartAt(DateTime.Now.AddMinutes(1)) // some Date 
                .ForJob(trigger1 + Job, Group1) // identify job with name, group strings
                .Build();

            scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            Console.WriteLine($"Account Validation Job initialized. Next run time - {nextFireTime}");
            log.Info($"Account Validation Job initialized. Next run time - {nextFireTime}");


        }



    }
}
