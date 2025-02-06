using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Utilities;

namespace LabBilling.Core.Services;

public class SystemService
{
    private readonly IAppEnvironment _appEnvironment;
    private readonly IUnitOfWorkSystem _uow;

    public SystemService(IAppEnvironment appEnvironment, IUnitOfWorkSystem uow)
    {
        Log.Instance.Trace("Initializing SystemService");
        this._appEnvironment = appEnvironment;
        this._uow = uow;

        //check if temp directory exists - if not create it
        if (!Directory.Exists(_appEnvironment.TempFilePath))
            Directory.CreateDirectory(_appEnvironment.TempFilePath);
    }

    public ApplicationParameters LoadSystemParameters() => _uow.SystemParametersRepository.LoadParameters();

    public void SaveSystemParameter(SysParameter systemParameter) => _uow.SystemParametersRepository.Update(systemParameter, new[] { nameof(SysParameter.Value) });

    public UserAccount UpdateUser(UserAccount user)
    {
        _uow.StartTransaction();
        var userAccount = _uow.UserAccountRepository.Update(user);
        _uow.Commit();
        return userAccount;
    }

    public UserAccount AddUser(UserAccount user)
    {
        _uow.StartTransaction();
        var retval = _uow.UserAccountRepository.Add(user);
        _uow.Commit();

        return user;
    }

    public IList<UserAccount> GetUsers() => _uow.UserAccountRepository.GetAll();

    public UserAccount GetUser(string username) => _uow.UserAccountRepository.GetByUsername(username);

    public async Task<IList<UserAccount>> GetActiveUsersAsync() => await Task.Run(() => GetActiveUsers());
    public IList<UserAccount> GetActiveUsers() => _uow.UserAccountRepository.GetActiveUsers();

    public bool LoginCheck(string username, string password) => _uow.UserAccountRepository.LoginCheck(username, password);

    public async Task<IEnumerable<UserProfile>> GetRecentAccountsAsync(string username) => await Task.Run(() => GetRecentAccounts(username));
    public IEnumerable<UserProfile> GetRecentAccounts(string username) => _uow.UserProfileRepository.GetRecentAccount(username);

}
