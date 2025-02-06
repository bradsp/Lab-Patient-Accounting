using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using PetaPoco;
using PetaPoco.Providers;
using System;
using System.Data;
using System.Reflection;

namespace LabBilling.Core.Services;

public class AuthenticationService
{
    private readonly string _connectionString;
    private IDatabase _db;

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
        var sql = Sql.Builder
                      .From("emp")
                      .Where("name = @0", username);
        return _db.SingleOrDefault<UserAccount>(sql);
    }

}
