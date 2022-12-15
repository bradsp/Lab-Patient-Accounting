using System;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using Quartz;

namespace Lab_Billing_Jobs
{
    public partial class JobProcessor
    {
        public class AccountValidationJob : IJob
        {

            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Starting Account validation. {DateTime.Now}");

                try
                {
                    Console.WriteLine("Try Task.Run() => RunValidation()");
                    await Task.Run(() => RunValidation());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new JobExecutionException(ex, true);
                }

            }

            public void RunValidation()
            {
                AccountRepository accountRepository = new AccountRepository("Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;");
                Console.WriteLine("In RunValidation() - Starting RunValidation job");
                accountRepository.ValidateUnbilledAccounts();
            }

        }

    }
}
