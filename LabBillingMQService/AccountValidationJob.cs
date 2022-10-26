using LabBilling.Core.DataAccess;
using Quartz;

namespace LabBillingMQService
{
    public partial class JobProcessor
    {
        public class AccountValidationJob : IJob
        {

            public Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Starting Account validation. {DateTime.Now}");
                //Task retVal;
                //try
                //{
                //    retVal =  RunValidation();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    retVal = Task.CompletedTask;
                //}
                //return retVal;
                return Task.CompletedTask;
            }

            public async Task RunValidation()
            {
                AccountRepository accountRepository = new AccountRepository("Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;");
                Console.WriteLine("Starting RunValidation job");
                await Task.Run(() => accountRepository.ValidateUnbilledAccountsAsync());
            }

        }

    }
}
