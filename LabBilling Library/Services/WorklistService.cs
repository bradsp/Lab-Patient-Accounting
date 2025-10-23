using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using NPOI.OpenXmlFormats.Dml.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

public class WorklistService
{
    private readonly AppEnvironment _appEnvironment;

    public WorklistService(AppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
    }

    public async Task<List<AccountSearch>> GetAccountsForWorklistAsync(string selectedQueue, IUnitOfWork uow  = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        DateTime thruDate = DateTime.Today.AddDays(_appEnvironment.ApplicationParameters.BillingInitialHoldDays * -1);
        (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters =
        {
            (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.PaidOut),
            (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Closed),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, AccountStatus.Client)
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
            case Worklists.Wellpoint:
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
                parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, _appEnvironment.ApplicationParameters.SelfPayFinancialCode)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ClaimSubmitted)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Statements)).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                break;
            case Worklists.ManualHold:
                parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.Hold)).ToArray();
                break;
            case Worklists.InitialHold:
                parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.GreaterThanOrEqual, thruDate.ToString())).ToArray();
                parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.NotEqual, "C")).ToArray();
                break;
            case Worklists.ErrorFinCode:
                parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, _appEnvironment.ApplicationParameters.InvalidFinancialCode)).ToArray();
                break;
            case Worklists.ClientBill:
                parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.Equal, _appEnvironment.ApplicationParameters.ClientFinancialTypeCode)).ToArray();
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
            case Worklists.CreditBalance:
                parameters = parameters.Append((nameof(AccountSearch.Balance), AccountSearchRepository.operation.LessThan, "0.00")).ToArray();
                break;
            default:
                break;
        }
        var accounts = (List<AccountSearch>)await Task.Run(() =>
        {
            return uow.AccountSearchRepository.GetBySearch(parameters);
        });
        return accounts;
    }

    public static bool AccountMeetsWorklistCriteria(string selectedQueue, Account account)
    {
        List<string> accStatuses = new();
        switch (selectedQueue)
        {
            case Worklists.MedicareCigna:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "A" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.BlueCross:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "B" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.Champus:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "C" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.TenncareBCBS:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "D" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.CommercialInst:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "H" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.CommercialProf:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "L" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.UHCCommunityPlan:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "M" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.PathwaysTNCare:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "P" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.Wellpoint:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "Q" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.SelfPay:
                accStatuses = new()
                {
                    AccountStatus.Hold,
                    AccountStatus.ProfSubmitted,
                    AccountStatus.InstSubmitted,
                    AccountStatus.ClaimSubmitted,
                    AccountStatus.Statements
                };
                return account.FinCode == "E" && !accStatuses.Contains(account.Status) && account.ClaimBalance != 0.00;
            case Worklists.ManualHold:
                return account.Status == AccountStatus.Hold;
            case Worklists.InitialHold:
                return account.Fin.FinClass != "C";
            case Worklists.ErrorFinCode:
                return account.FinCode == "K";
            case Worklists.ClientBill:
                return account.Fin.FinClass == "C";
            case Worklists.SubmittedInstitutional:
                return account.Status == AccountStatus.InstSubmitted;
            case Worklists.SubmittedProfessional:
                return account.Status == AccountStatus.ProfSubmitted;
            case Worklists.SubmittedOtherClaim:
                return account.Status == AccountStatus.ClaimSubmitted;
            case Worklists.ReceivingStatements:
                return account.Status == AccountStatus.Statements;
            default:
                return false;
        }
    }

}
