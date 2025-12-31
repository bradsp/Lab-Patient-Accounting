using LabBilling.Core.DataAccess;
using PetaPoco;
using System;

namespace LabBilling.Core.UnitOfWork;
public interface IUnitOfWorkSystem : IDisposable
{
    IDatabase Context { get; }
    SystemParametersRepository SystemParametersRepository { get; }
    UserAccountRepository UserAccountRepository { get; }
    UserProfileRepository UserProfileRepository { get; }

    void Commit();
    void Rollback();
    void StartTransaction();
}