using LabBilling.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;
public interface ISystemService
{
    UserAccount AddUser(UserAccount user);
    IList<UserAccount> GetActiveUsers();
    Task<IList<UserAccount>> GetActiveUsersAsync();
    IEnumerable<UserProfile> GetRecentAccounts(string username);
    Task<IEnumerable<UserProfile>> GetRecentAccountsAsync(string username);
    UserAccount GetUser(string username);
    IList<UserAccount> GetUsers();
    ApplicationParameters LoadSystemParameters();
    bool LoginCheck(string username, string password);
    void SaveSystemParameter(SysParameter systemParameter);
    UserAccount UpdateUser(UserAccount user);
}