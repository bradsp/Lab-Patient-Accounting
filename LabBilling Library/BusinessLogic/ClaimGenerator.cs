using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Data.Common;
using System.Collections;

namespace LabBilling.Core.BusinessLogic
{
    public class ClaimGenerator
    {
        private SystemParametersRepository parametersdb;

        string dBserverName = null;
        string dBName = null;
        ArrayList m_alNameSuffix = new ArrayList() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };

        public string propProductionEnvironment { get; set; }
        private string _connectionString;

        private Account currentAccount;
        private AccountRepository accountRepository;
        private List<Chrg> accountCharges;
        private ChrgRepository chrgRepository;
        private ChkRepository chkRepository;


        public ClaimGenerator(string connectionString)
        {
            _connectionString = connectionString;

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = connectionString;

            dBserverName = (string)dbConnectionStringBuilder["Server"];
            dBName = (string)dbConnectionStringBuilder["Database"];

            parametersdb = new SystemParametersRepository(_connectionString);

            propProductionEnvironment = dBName.Contains("LIVE") ? "P" : "T";
            string[] strArgs = new string[3];
            strArgs[0] = dBName.Contains("LIVE") ? "/LIVE" : "/TEST";
            strArgs[1] = dBserverName;
            strArgs[2] = dBName;

            accountRepository = new AccountRepository(_connectionString);
            chrgRepository = new ChrgRepository(_connectionString);
            chkRepository = new ChkRepository(_connectionString);

        }


        public void CompileProfessionalBilling()
        {

        }

        public void CompileInstituationalBilling()
        {

        }


        /// <summary>
        /// Generates single professional claim
        /// </summary>
        /// <param name="account"></param>
        public ClaimData GenerateProfessionalClaim(string account)
        {
            //do all the fun work of building the ClaimData object & return it.

            ClaimData claimData = new ClaimData
            {
                claimAccount = accountRepository.GetByAccount(account)

            };
            claimData.claimAccount.Charges = chrgRepository.GetByAccount(account, false).ToList();
            claimData.claimAccount.Payments = chkRepository.GetByAccount(account).ToList();

            try
            {
                claimData.SubmitterId = parametersdb.GetByKey("fed_tax_id");
                claimData.SubmitterName = parametersdb.GetByKey("billing_entity_name");
                claimData.SubmitterContactName = parametersdb.GetByKey("billing_contact");
                claimData.SubmitterContactEmail = parametersdb.GetByKey("billing_phone");
                claimData.SubmitterContactPhone = parametersdb.GetByKey("billing_email");
            }
            catch(InvalidParameterValueException ex)
            {
                throw new InvalidParameterValueException("Parameter value not found", ex);
            }


            return claimData;
        }






    }
}
