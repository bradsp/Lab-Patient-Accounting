using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using PetaPoco;
using PetaPoco.Providers;
using System;
using System.Data;

namespace LabBilling.Core.UnitOfWork;
public class UnitOfWorkSystem : IUnitOfWorkSystem
{
    private readonly bool _useDispose;
    private Transaction _transaction;

    public SystemParametersRepository SystemParametersRepository { get; private set; }
    public UserAccountRepository UserAccountRepository { get; private set; }
    public UserProfileRepository UserProfileRepository { get; private set; }

    /// <summary>
    /// For use with other Database instances outside of Unit Of Work
    /// </summary>
    /// <param name="context"></param>
    public UnitOfWorkSystem(PetaPoco.Database context)
    {
        Context = context;
        _useDispose = false;
        InitializeRepositories();
    }

    public UnitOfWorkSystem(IAppEnvironment appEnvironment)
    {
        Context = Initialize(appEnvironment.ConnectionString);
        InitializeRepositories(appEnvironment);
        _useDispose = true;
    }

    private static IDatabase Initialize(string connectionString)
    {
        Log.Instance.Trace("Initializing UnitOfWorkMain");
        return DatabaseConfiguration
            .Build()
            .UsingConnectionString(connectionString)
            .UsingProvider<CustomSqlMsDatabaseProvider>(new CustomSqlMsDatabaseProvider())
            .UsingCommandTimeout(180)
            .WithAutoSelect()
            .UsingDefaultMapper<MyMapper>(new MyMapper())
            .Create();
    }

    private void InitializeRepositories(IAppEnvironment appEnvironment = null)
    {
        SystemParametersRepository = new SystemParametersRepository(appEnvironment, Context);
        UserAccountRepository = new UserAccountRepository(appEnvironment, Context);
        UserProfileRepository = new UserProfileRepository(appEnvironment, Context);
    }

    public PetaPoco.IDatabase Context { get; private set; }

    public void StartTransaction()
    {
        if (_transaction != null) return;
        _transaction = new Transaction(Context);
    }

    public void Commit()
    {
        if (_transaction == null) return;

        _transaction.Complete();
        _transaction.Dispose();
        _transaction = null;
    }

    public void Rollback()
    {
        if (_transaction == null) return;

        _transaction.Dispose();
        _transaction = null;
    }

    public void Dispose()
    {
        var doThrowTransactionException = false;

        if (_transaction != null)
        {
            Context.AbortTransaction();
            _transaction.Dispose();
            doThrowTransactionException = true;
        }

        if (_useDispose && Context != null)
        {
            Context.Dispose();
            Context = null;
        }

        GC.SuppressFinalize(this);
        if (doThrowTransactionException)
            throw new DataException("Transaction was aborted");
    }

}
