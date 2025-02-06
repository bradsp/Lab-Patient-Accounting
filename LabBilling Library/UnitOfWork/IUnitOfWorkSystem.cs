using LabBilling.Core.DataAccess;
using PetaPoco;

namespace LabBilling.Core.UnitOfWork;
public interface IUnitOfWorkSystem
{
    IDatabase Context { get; }
    SystemParametersRepository SystemParametersRepository { get; }
    UserAccountRepository UserAccountRepository { get; }
    UserProfileRepository UserProfileRepository { get; }

    void Commit();
    void Dispose();
    void Rollback();
    void StartTransaction();
}