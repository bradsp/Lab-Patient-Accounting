using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess;

public sealed class AccountAlertRepository : RepositoryBase<AccountAlert>, IRepositoryBase<AccountAlert>
{
    public AccountAlertRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

    public AccountAlert GetByAccount(string accountNo)
    {
        return Context.SingleOrDefault<AccountAlert>((object)accountNo);
    }

}
