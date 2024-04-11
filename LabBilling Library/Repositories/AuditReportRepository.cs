using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.Repositories;
public sealed class AuditReportRepository : RepositoryBase<AuditReport>
{
    public AuditReportRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }


}
