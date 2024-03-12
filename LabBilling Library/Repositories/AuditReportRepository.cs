using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Repositories;
public class AuditReportRepository : RepositoryBase<AuditReport>
{
    public AuditReportRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }


}
