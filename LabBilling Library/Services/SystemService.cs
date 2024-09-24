using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;

public class SystemService
{
    private readonly IAppEnvironment _appEnvironment;

    public SystemService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;

        //check if temp directory exists - if not create it
        if (!Directory.Exists(_appEnvironment.TempFilePath))
            Directory.CreateDirectory(_appEnvironment.TempFilePath);
    }

    public ApplicationParameters LoadSystemParameters()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.SystemParametersRepository.LoadParameters();
    }

    public void SaveSystemParameter(SysParameter systemParameter)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        uow.SystemParametersRepository.Update(systemParameter, new[] { nameof(SysParameter.Value) });
    }

    public UserAccount UpdateUser(UserAccount user)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var userAccount = uow.UserAccountRepository.Update(user);
        uow.Commit();
        return userAccount;
    }

    public UserAccount AddUser(UserAccount user)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var retval = uow.UserAccountRepository.Add(user);
        uow.Commit();

        return user;
    }

    public IList<UserAccount> GetUsers()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.UserAccountRepository.GetAll();

    }

    public UserAccount GetUser(string username)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.UserAccountRepository.GetByUsername(username);
    }

    public async Task<IList<UserAccount>> GetActiveUsersAsync() => await Task.Run(() => GetActiveUsers());
    public IList<UserAccount> GetActiveUsers()
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.UserAccountRepository.GetActiveUsers();
    }

    public bool LoginCheck(string username, string password) 
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.UserAccountRepository.LoginCheck(username, password);
    }

    public async Task<IEnumerable<UserProfile>> GetRecentAccountsAsync(string username) => await Task.Run(() => GetRecentAccounts(username));
    public IEnumerable<UserProfile> GetRecentAccounts(string username)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.UserProfileRepository.GetRecentAccount(username);
    }
}
