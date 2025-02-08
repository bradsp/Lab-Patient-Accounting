using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using PetaPoco;
using PetaPoco.Providers;
using System.Text;
using System;
using System.Security.Cryptography;

namespace LabBilling.Core.Services;

public class AuthenticationService
{
    private readonly string _connectionString;
    private IDatabase _db;
    private IUnitOfWorkSystem _uow;

    public AuthenticationService(IUnitOfWorkSystem uow)
    {
        _db = uow.Context;
        _uow = uow;
    }

    public AuthenticationService(string connectionString)
    {
        _connectionString = connectionString;

        _db = DatabaseConfiguration
            .Build()
            .UsingConnectionString(connectionString)
            .UsingProvider<CustomSqlMsDatabaseProvider>(new CustomSqlMsDatabaseProvider())
            .UsingCommandTimeout(180)
            .WithAutoSelect()
            .UsingDefaultMapper<MyMapper>(new MyMapper())
            .Create();

    }

    public UserAccount AuthenticateIntegrated(string username)
    {
        var user = _uow.UserAccountRepository.GetByUsername(username);
        return user;
    }

    public (bool isAuthenticated, UserAccount user) Authenticate(string username, string password)
    {
        bool isAuthenticated = false;
        var user = _uow.UserAccountRepository.GetByUsername(username);
        if (user == null)
        {

        }

        //check that provided password matches database
        if(user.Password == EncryptPassword(password))
        {
            isAuthenticated = true;
            return (isAuthenticated, user);
        }
        else
        {
            return (isAuthenticated, null);
        }

    }

    private string EncryptPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

}
