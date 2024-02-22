using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using LabBilling.Logging;
using System;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChkBatchDetailRepository : RepositoryBase<ChkBatchDetail>
    {

        public ChkBatchDetailRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public ChkBatchDetail GetById(double id)
        {
            Log.Instance.Trace("Entering");

            return Context.SingleOrDefault<ChkBatchDetail>((object)id);
        }

        public List<ChkBatchDetail> GetByBatch(int batch)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(ChkBatchDetail.Batch))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Int, Value = batch })
                .OrderBy($"{GetRealColumn(nameof(ChkBatchDetail.Id))}");

            var details = Context.Fetch<ChkBatchDetail>(sql);

            return details;
        }

        public int DeleteBatch(int batch)
        {
            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(ChkBatchDetail.Batch))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.Int, Value = batch });

            return Context.Delete<ChkBatchDetail>(sql);
        }

        public override ChkBatchDetail Save(ChkBatchDetail table)
        {
            var existing = GetById(table.Id);

            if (string.IsNullOrEmpty(table.AccountNo))
            {
                Log.Instance.Error($"{nameof(table.AccountNo)} cannot be empty.");
                throw new ApplicationException($"{nameof(table.AccountNo)} cannot be empty.");
            }
            try
            {
                if (existing != null)
                {
                    return base.Update(table);
                }
                else
                {
                    return base.Add(table);
                }
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex, "Error writing ChkBatchDetail");
                throw new ApplicationException("Error writing ChkBatchDetail", ex);
            }     
        }

        public override bool Delete(ChkBatchDetail table)
        {
            return base.Delete(table);
        }

        public bool Delete(int id)
        {
            var detail = GetById(id);
            if(detail != null)
            {
                return Delete(detail);
            }

            return false;
        }

    }

}
