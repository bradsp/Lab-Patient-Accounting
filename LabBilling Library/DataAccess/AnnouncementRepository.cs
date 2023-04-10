using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class AnnouncementRepository : RepositoryBase<Announcement>
    {
        public AnnouncementRepository(string connectionString) : base(connectionString) { }

        public AnnouncementRepository(Database database) : base(database) { }

        public List<Announcement> GetActive()
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(Announcement.StartDate))} <= @0",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now })
                .Where($"({GetRealColumn(nameof(Announcement.EndDate))} >= @0 or {GetRealColumn(nameof(Announcement.EndDate))} is null)",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now });

            var results = dbConnection.Fetch<Announcement>(sql);

            return results;
        }

    }
}
