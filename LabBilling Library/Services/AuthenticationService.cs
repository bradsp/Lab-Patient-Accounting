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

    public bool Authenticate(string username, string password)
    {
        var user = _uow.UserAccountRepository.GetByUsername(username);
        if (user == null)
        {
            return false;
        }

        var encryptedPassword = EncryptPassword(password);
        return user.Password == encryptedPassword;
    }

    private string EncryptPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(password);
        var hash = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }

}
