using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using RFClassLibrary;

namespace LabBillingConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;";
            
            Client client = new Client();
            ClientRepository clientRepository = new ClientRepository(connectionString);

            clientRepository.Find(c => c.Name.Contains("Medical"));



        }
    }
}
