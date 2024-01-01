using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Core.DataAccess
{
    public sealed class AnnouncementRepository : RepositoryBase<Announcement>
    {
        public AnnouncementRepository(IAppEnvironment appEnvironment) : base(appEnvironment) { }

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
