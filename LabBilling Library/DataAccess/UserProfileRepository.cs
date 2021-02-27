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
            string select = "select TOP " + numEntries + " *";

            var command = PetaPoco.Sql.Builder
                .Append(select)
                .Append("from UserProfile")
                .Append("where UserName = @0 ", user)
                .Append("and Parameter = @0", "RecentAccount")
                .Append("order by ModDate desc");

            return dbConnection.Fetch<UserProfile>(command);
        }

    }
}
