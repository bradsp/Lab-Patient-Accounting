using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using LabBilling.Logging;
using System;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChkBatchDetailRepository : RepositoryBase<ChkBatchDetail>
    {
        public ChkBatchDetailRepository(IAppEnvironment environment) : base(environment)
        {
        }

        public ChkBatchDetail GetById(double id)
        {
            Log.Instance.Trace("Entering");

            return dbConnection.SingleOrDefault<ChkBatchDetail>(id);
        }

        public List<ChkBatchDetail> GetByBatch(int batch)
        {
            Log.Instance.Trace("Entering");

            AccountRepository accountRepository = new AccountRepository(_appEnvironment);

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(ChkBatchDetail.Batch))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Int, Value = batch })
                .OrderBy($"{GetRealColumn(nameof(ChkBatchDetail.Id))}");

            var details = dbConnection.Fetch<ChkBatchDetail>(sql);

            details.ForEach(x =>
            {
                if (!string.IsNullOrEmpty(x.AccountNo))
                {
                    var acc = accountRepository.GetByAccount(x.AccountNo);
                    x.PatientName = acc.PatFullName;
                    x.Balance = acc.Balance;
                }
            }
            );

            return details;
        }

        public int DeleteBatch(int batch)
        {
            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(ChkBatchDetail.Batch))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.Int, Value = batch });

            return dbConnection.Delete<ChkBatchDetail>(sql);
        }

        public new (bool successFlag, int newId) Save(ChkBatchDetail table)
        {
            var existing = GetById(table.Id);

            bool success = false;
            int newId = -1;

            if(existing != null)
            {
                success = base.Update(table);
            }
            else
            {
                newId = Convert.ToInt32(base.Add(table));
            }

            return (success, newId);            
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
