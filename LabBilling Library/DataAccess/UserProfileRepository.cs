using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class UserProfileRepository : RepositoryBase<UserProfile>
    {
        public UserProfileRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            
        }

        public void InsertRecentAccount(string account, string user)
        {
            UserProfile userProfile = new UserProfile();

            dbConnection.Delete<UserProfile>($"where {this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.UserName))} = @0 and Parameter = @1 and ParameterData = @2",
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
            UserProfile userProfile = new UserProfile();
            string select = "TOP " + numEntries + " *";
            string sortColumn = this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.ModDate));

            var command = PetaPoco.Sql.Builder
                .Select(select)
                .From(_tableName) 
                .Where("UserName = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = user })
                .Where("Parameter = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "RecentAccount" })
                .OrderBy($"{sortColumn} desc");

            return dbConnection.Fetch<UserProfile>(command);
        }

    }
}
