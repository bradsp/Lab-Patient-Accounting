﻿using LabBilling.Core.Services;
using Quartz;
using System;
using System.Threading.Tasks;

namespace LabBillingJobs;

public partial class JobProcessor
{
    public class AccountValidationJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Starting Account validation. {DateTime.Now}");
            _log.Info($"Starting Account validation. {DateTime.Now}");

            try
            {
                Console.WriteLine("Try Task.Run() => RunValidation()");
                _log.Debug("Try Task.Run() => RunValidation()");
                await Task.Run(() => RunValidation());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.Error(ex);
                throw new JobExecutionException(ex, true);
            }

        }

        public static void RunValidation()
        {
            Console.WriteLine("In RunValidation() - Starting RunValidation job");
            _log.Info("In RunValidation() - Starting RunValidation job");
            AccountService accountService = new(Program.AppEnvironment);

            accountService.ValidateUnbilledAccounts();

            Console.WriteLine("In RunValidation() - Finished RunValidation job");
            _log.Info("In RunValidation() - Finished RunValidation job");
        }

    }

}