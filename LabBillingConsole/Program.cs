using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.BusinessLogic;

namespace LabBillingConsole
{
    internal class Program
    {
        public const string connectionString = "Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            HL7Processor hL7Processor = new HL7Processor(connectionString);

            hL7Processor.ProcessMessages();

            Console.WriteLine("Messages processed.");

        }

        public static void SwapInsurance()
        {

            var accountRepository = new AccountRepository(connectionString);

            accountRepository.InsuranceSwap("L17436110", InsCoverage.Primary, InsCoverage.Secondary);

        }



    }


}
