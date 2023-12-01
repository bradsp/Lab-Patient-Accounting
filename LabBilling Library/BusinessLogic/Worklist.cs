using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.BusinessLogic
{
    public class Worklist
    {
        private readonly AppEnvironment _appEnvironment;

        public Worklist(AppEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        public async Task<List<AccountSearch>> GetAccountsForWorklistAsync(string selectedQueue)
        {
            AccountSearchRepository accountSearchRepository = new AccountSearchRepository(_appEnvironment);

            DateTime thruDate = DateTime.Today.AddDays(_appEnvironment.ApplicationParameters.BillingInitialHoldDays*-1);
            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters =
            {
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT")
            };

            switch (selectedQueue)
            {
                case Worklists.MedicareCigna:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "A")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.BlueCross:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "B")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.Champus:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.TenncareBCBS:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "D")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.CommercialInst:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "H")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.CommercialProf:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "L")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.UHCCommunityPlan:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "M")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.PathwaysTNCare:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "P")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.Amerigroup:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "Q")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.SelfPay:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "E")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.ManualHold:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "HOLD")).ToArray();
                    break;
                case Worklists.InitialHold:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.GreaterThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.NotEqual, "C")).ToArray();
                    break;
                case Worklists.ErrorFinCode:
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "K")).ToArray();
                    break;
                case Worklists.ClientBill:
                    parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    break;
                case Worklists.SubmittedInstitutional:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.InstSubmitted)).ToArray();
                    break;
                case Worklists.SubmittedProfessional:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.ProfSubmitted)).ToArray();
                    break;
                case Worklists.SubmittedOtherClaim:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.ClaimSubmitted)).ToArray();
                    break;
                case Worklists.ReceivingStatements:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.Statements)).ToArray();
                    break;
                default:
                    break;
            }

            var accounts = (List<AccountSearch>)await Task.Run(() =>
            {
                return accountSearchRepository.GetBySearch(parameters);
            });


            return accounts;
        }


    }

    public static class Worklists
    {

        public const string MedicareCigna = "Medicare/Cigna";
        public const string BlueCross = "BlueCross";
        public const string Champus = "Champus";
        public const string TenncareBCBS = "Tenncare BC/BS";
        public const string CommercialInst = "Commercial Institutional";
        public const string CommercialProf = "Commercial Professional";
        public const string UHCCommunityPlan = "UHC Community Plan";
        public const string PathwaysTNCare = "Pathways TNCare";
        public const string Amerigroup = "Amerigroup";
        public const string SelfPay = "Self Pay";
        public const string ManualHold = "Manual Hold";
        public const string InitialHold = "Initial Hold";
        public const string ErrorFinCode = "Error Fin Code";
        public const string ClientBill = "Client Bill";
        public const string SubmittedInstitutional = "Submitted Institutional";
        public const string SubmittedProfessional = "Submitted Professional";
        public const string SubmittedOtherClaim = "Submitted Other";
        public const string ReceivingStatements = "Receiving Statements";


        public static List<string> ToList()
        {
            List<string> values = new List<string>();
            foreach(var prop in typeof(Worklists).GetFields())
            {
                values.Add(prop.GetValue(null) as string);
            }

            return values;
        }

    }
}
