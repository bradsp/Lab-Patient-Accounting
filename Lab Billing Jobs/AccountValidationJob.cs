using System;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using Quartz;

namespace LabBillingJobs
{
    public partial class JobProcessor
    {
        public class AccountValidationJob : IJob
        {

            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Starting Account validation. {DateTime.Now}");
                log.Info($"Starting Account validation. {DateTime.Now}");

                try
                {
                    Console.WriteLine("Try Task.Run() => RunValidation()");
                    log.Debug("Try Task.Run() => RunValidation()");
                    await Task.Run(() => RunValidation());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    log.Error(ex);
                    throw new JobExecutionException(ex, true);
                }

            }

            public void RunValidation()
            {
                AccountRepository accountRepository = new AccountRepository(Helper.ConnVal);
                Console.WriteLine("In RunValidation() - Starting RunValidation job");
                log.Info("In RunValidation() - Starting RunValidation job");

                accountRepository.ValidateUnbilledAccounts();
                
                Console.WriteLine("In RunValidation() - Finished RunValidation job");
                log.Info("In RunValidation() - Finished RunValidation job");
            }

        }

    }
}
