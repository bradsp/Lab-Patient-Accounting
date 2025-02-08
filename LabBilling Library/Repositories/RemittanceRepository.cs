using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco;
using NLog;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.Repositories;

public sealed class RemittanceRepository : RepositoryBase<RemittanceFile>
{
    public RemittanceRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public List<RemittanceFile> GetRemittances(bool includePosted = false, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(AppEnvironment.ConnectionString);
        PetaPoco.Sql sql = PetaPoco.Sql.Builder;

        if (includePosted)
        {
            return this.GetAll();
        }
        else
        {
            sql.Where($"{GetRealColumn(nameof(RemittanceFile.PostedDate))} IS NULL");
            var queryResult = Context.Fetch<RemittanceFile>(sql);
            return queryResult.ToList();
        }
    }

    public RemittanceFile GetByFilename(string filename)
    {
        Log.Instance.Trace("Entering");
        PetaPoco.Sql sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(RemittanceFile.FileName))} = @0", filename);
        var queryResult = Context.FirstOrDefault<RemittanceFile>(sql);
        return queryResult;
    }
}

public sealed class RemittanceClaimRepository : RepositoryBase<RemittanceClaim>
{
    public RemittanceClaimRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public List<RemittanceClaim> GetByRemitId(int remitId)
    {
        Log.Instance.Trace("Entering");
        PetaPoco.Sql sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(RemittanceClaim.RemittanceId))} = @0", remitId);
        var queryResult = Context.Fetch<RemittanceClaim>(sql);
        Log.Instance.Debug(Context.LastSQL);
        Log.Instance.Debug(Context.LastArgs);
        return queryResult;
    }

}

public sealed class RemittanceClaimDetailRepository : RepositoryBase<RemittanceClaimDetail>
{
    public RemittanceClaimDetailRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public List<RemittanceClaimDetail> GetByClaimId(int claimId)
    {
        Log.Instance.Trace("Entering");
        PetaPoco.Sql sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(RemittanceClaimDetail.RemittanceClaimId))} = @0", claimId);
        var queryResult = Context.Fetch<RemittanceClaimDetail>(sql);
        Log.Instance.Debug(Context.LastSQL);
        Log.Instance.Debug(Context.LastArgs);
        return queryResult;
    }
}

public sealed class RemittanceClaimAdjustmentRepository : RepositoryBase<RemittanceClaimAdjustment>
{
    public RemittanceClaimAdjustmentRepository(IAppEnvironment environment, IDatabase context) : base(environment, context)
    {
    }

    public List<RemittanceClaimAdjustment> GetByClaimDetailId(int claimDetailId)
    {
        Log.Instance.Trace("Entering");
        PetaPoco.Sql sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(RemittanceClaimAdjustment.RemittanceClaimDetailId))} = @0", claimDetailId);
        var queryResult = Context.Fetch<RemittanceClaimAdjustment>(sql);
        Log.Instance.Debug(Context.LastSQL);
        Log.Instance.Debug(Context.LastArgs);
        return queryResult;
    }
}
