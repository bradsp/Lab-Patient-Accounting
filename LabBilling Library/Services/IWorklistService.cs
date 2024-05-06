using LabBilling.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;
public interface IWorklistService
{
    Task<List<AccountSearch>> GetAccountsForWorklistAsync(string selectedQueue);
}