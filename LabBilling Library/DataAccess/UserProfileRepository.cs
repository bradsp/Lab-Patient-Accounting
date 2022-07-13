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

            dbConnection.Delete<UserProfile>("where UserName = @0 and Parameter = @1 and ParameterData = @2",
                user, "RecentAccount", account);
           

            userProfile.UserName = user;
            userProfile.Parameter = "RecentAccount";
            userProfile.ParameterData = account;

            this.Add(userProfile);
        }

        public IEnumerable<UserProfile> GetRecentAccount(string user, int numEntries = 10)
        {
            string select = "TOP " + numEntries + " *";

            var command = PetaPoco.Sql.Builder
                .Select(select)
                .From("UserProfile")
                .Where("UserName = @0 ", user)
                .Where("Parameter = @0", "RecentAccount")
                .OrderBy("ModDate desc");

            return dbConnection.Fetch<UserProfile>(command);
        }

    }
}
