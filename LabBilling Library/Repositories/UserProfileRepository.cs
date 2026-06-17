using LabBilling.Core.Models;
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
            user,
            "RecentAccount",
            account);


        userProfile.UserName = user;
        userProfile.Parameter = "RecentAccount";
        userProfile.ParameterData = account;

        this.Add(userProfile);
    }

    public IEnumerable<UserProfile> GetRecentAccount(string user, int numEntries = 10)
    {
        string select = "TOP " + numEntries + " *";
        string sortColumn = this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.ModDate));

        var command = PetaPoco.Sql.Builder
            .Select(select)
            .From(_tableName)
            .Where("UserName = @0 ", user)
            .Where("Parameter = @0", "RecentAccount")
            .OrderBy($"{sortColumn} desc");

        return Context.Fetch<UserProfile>(command);
    }

}
