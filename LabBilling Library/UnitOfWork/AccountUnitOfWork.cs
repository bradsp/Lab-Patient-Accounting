using LabBilling.Core.DataAccess;

namespace LabBilling.Core.UnitOfWork;

public class AccountUnitOfWork : UnitOfWorkMain, IAccountUnitOfWork
{
    public AccountUnitOfWork(PetaPoco.Database context) : base(context) { }

    public AccountUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction) { }



}
