using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;
using System.Collections;
using System.Collections.Generic;

namespace LabBilling.Core.Repositories;
public sealed class AuditReportRepository : RepositoryBase<AuditReport>
{
    public AuditReportRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public List<string> GetMenus()
    {
        var list = Context.Fetch<string>($"SELECT DISTINCT {GetRealColumn(nameof(AuditReport.Button))} FROM {TableName}");

        return list;
    }


}
