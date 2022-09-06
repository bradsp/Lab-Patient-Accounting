using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using LabBilling.Core.DataAccess;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using static Quartz.Logging.OperationName;

namespace LabBillingMQService
{
    public class JobProcessor
    {
        private IScheduler scheduler;
        private const string Group1 = "BusinessTasks";
        private const string Job = "Job";

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public JobProcessor()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public async void OnStart()
        {
            StdSchedulerFactory schedulerFactory = new StdSchedulerFactory();
            scheduler = await schedulerFactory.GetScheduler();

            await scheduler.Start();

            AddJobs();
        }
        public void OnPaused() =>
            scheduler.PauseAll();
        public void OnContinue() =>
            scheduler.ResumeAll();
        public void OnStop() =>
            scheduler.Shutdown();
        

        private void AddJobs()
        {
            AddAccountValidationJob();
        }

        public void AddAccountValidationJob()
        {
            const string trigger1 = "AccountValidation";

            IJob myJob = new AccountValidationJob();
            var jobDetail = new JobDetailImpl(trigger1 + Job, Group1, myJob.GetType());

            var trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity(trigger1 + Job, Group1)
                .StartAt(DateTime.Now.AddMinutes(1)) // some Date 
                .ForJob(trigger1 + Job, Group1) // identify job with name, group strings
                .Build();

            //var trigger = new CronTriggerImpl(
            //    trigger1,
            //    Group1,
            //    "0 0 15 55 * ?" /* every 10 minutes */
            //    )
            //{ TimeZone = TimeZoneInfo.Local };
            scheduler.ScheduleJob(jobDetail, trigger);
            var nextFireTime = trigger.GetNextFireTimeUtc();
            Console.WriteLine($"Account Validation Job initialized. Next run time - {nextFireTime}");
            //if (nextFireTime != null)
            //    Log.Info(Group1 + "+" + trigger1, new Exception(nextFireTime.Value.ToString("u")));
        }

        public class AccountValidationJob : IJob
        {

            public Task Execute(IJobExecutionContext context)
            {
                Task retVal;
                try
                {
                    retVal =  RunValidation();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    retVal = Task.CompletedTask;
                }
                return retVal;
            }

            public async Task RunValidation()
            {
                AccountRepository accountRepository = new AccountRepository("Server=WTHMCLBILL;Database=MCLTEST;Trusted_Connection=True;");
                Console.WriteLine("Starting RunValidation job");
                await Task.Run(() => accountRepository.ValidateUnbilledAccountsAsync());
            }

        }

    }
}
