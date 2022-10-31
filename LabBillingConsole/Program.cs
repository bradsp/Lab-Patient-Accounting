using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBillingConsole
{
    internal class Program
    {
        public const string connectionString = "Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            SwapInsurance();

        }

        public static void SwapInsurance()
        {

            var accountRepository = new AccountRepository(connectionString);

            accountRepository.InsuranceSwap("L17436110", InsCoverage.Primary, InsCoverage.Secondary);



        }



    }
    

}
