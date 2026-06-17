using LabBilling.Core.Models;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class AnnouncementRepository : RepositoryBase<Announcement>
{
    public AnnouncementRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

    public List<Announcement> GetActive()
    {
        Log.Instance.Trace("Entering");

        var sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(Announcement.StartDate))} <= @0",
                DateTime.Now)
            .Where($"({GetRealColumn(nameof(Announcement.EndDate))} >= @0 or {GetRealColumn(nameof(Announcement.EndDate))} is null)",
                DateTime.Now);

        var results = Context.Fetch<Announcement>(sql);

        return results;
    }

}
