using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.Repositories;

public sealed class RemittanceRepository : RepositoryBase<RemittanceFile>
{
    public RemittanceRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

}

public sealed class RemittanceClaimRepository : RepositoryBase<RemittanceClaim>
{
    public RemittanceClaimRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

}

public sealed class RemittanceClaimDetailRepository : RepositoryBase<RemittanceClaimDetail>
{
    public RemittanceClaimDetailRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }
}

public sealed class RemittanceClaimAdjustmentRepository : RepositoryBase<ClaimAdjustment>
{
    public RemittanceClaimAdjustmentRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }
}
