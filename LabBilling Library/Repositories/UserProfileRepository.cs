using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class UserProfileRepository : RepositoryBase<UserProfile>
{
    protected override bool RequireValidEnvironment => false;
    public UserProfileRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public void InsertRecentAccount(string account, string user)
    {
        UserProfile userProfile = new UserProfile();

        Context.Delete<UserProfile>($"where {this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.UserName))} = @0 and Parameter = @1 and ParameterData = @2",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = user },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "RecentAccount" },
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });


        userProfile.UserName = user;
        userProfile.Parameter = "RecentAccount";
        userProfile.ParameterData = account;

        this.Add(userProfile);
    }

    public IEnumerable<UserProfile> GetRecentAccount(string user, int numEntries = 10)
    {
        string sortColumn = this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.ModDate));

        var command = PetaPoco.Sql.Builder
            .Append($"SELECT TOP (@0) * FROM {_tableName}", numEntries)
            .Where("UserName = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = user })
            .Where("Parameter = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "RecentAccount" })
            .OrderBy($"{sortColumn} desc");

        return Context.Fetch<UserProfile>(command);
    }

}
