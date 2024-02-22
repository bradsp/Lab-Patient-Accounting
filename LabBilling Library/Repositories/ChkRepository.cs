using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChkRepository : RepositoryBase<Chk>
    {
        public ChkRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public Chk GetById(int id)
        {
            Log.Instance.Trace($"Entering - id {id}");
            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(Chk.PaymentNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = id });

            var result = Context.SingleOrDefault<Chk>(sql);

            return result;
        }

        public List<Chk> GetByCheckNo(string checkno)
        {
            Log.Instance.Trace($"Entering - check no {checkno}");

            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(Chk.CheckNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = checkno });

            var result = Context.Fetch<Chk>(sql);

            return result;
        }

        public List<Chk> GetByAccount(string account, DateTime? asOfDate = null)
        {
            Log.Instance.Trace($"Entering - {account}");

            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where("account = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            if (asOfDate != null)
                sql.Where($"{GetRealColumn(nameof(Chk.UpdatedDate))} > @0",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate });

            sql.OrderBy("pay_no");

            var records = Context.Fetch<Chk>(sql);

            return records;
        }

        public List<Chk> GetByAccount(string account, bool includeInvoiced)
        {
            Log.Instance.Trace($"Entering - {account}");
            if (includeInvoiced)
            {
                return this.GetByAccount(account);
            }
            else
            {
                var sql = PetaPoco.Sql.Builder
                    .From(_tableName)
                    .Where("account = @0 and invoice IS NULL", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

                sql.OrderBy("pay_no");

                var records = Context.Fetch<Chk>(sql);
                return records;
            }

        }

        public override Chk Add(Chk table)
        {
            table.Status = "NEW";

            return base.Add(table);
        }
    }
}
