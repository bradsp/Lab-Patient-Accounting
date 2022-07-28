using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class UserProfileRepository : RepositoryBase<UserProfile>
    {
        public UserProfileRepository(string connection) : base("UserProfile", connection)
        {
            
        }

        public UserProfileRepository(string connection, PetaPoco.Database db) : base("UserProfile", connection)
        {

        }

        public override UserProfile GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void InsertRecentAccount(string account, string user)
        {
            UserProfile userProfile = new UserProfile();

            dbConnection.Delete<UserProfile>($"where {this.GetRealColumn(typeof(UserProfile), nameof(UserProfile.UserName))} = @0 and Parameter = @1 and ParameterData = @2",
                user, "RecentAccount", account);
           

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
                .Where("UserName = @0 ", user)
                .Where("Parameter = @0", "RecentAccount")
                .OrderBy($"{sortColumn} desc");

            return dbConnection.Fetch<UserProfile>(command);
        }

    }
}
