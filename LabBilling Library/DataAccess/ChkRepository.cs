using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class ChkRepository : RepositoryBase<Chk>
    {
        public ChkRepository(string connection) : base(connection)
        {

        }

        public ChkRepository(PetaPoco.Database db) : base(db)
        {

        }

        public Chk GetById(int id)
        {
            Log.Instance.Trace($"Entering - id {id}");
            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where("pay_no = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = id });

            var result = dbConnection.SingleOrDefault<Chk>(sql);

            return result;
        }

        public List<Chk> GetByCheckNo(string checkno)
        {
            Log.Instance.Trace($"Entering - check no {checkno}");

            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(Chk.CheckNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = checkno });

            var result = dbConnection.Fetch<Chk>(sql);

            return result;
        }

        public List<Chk> GetByAccount(string account, DateTime? asOfDate = null)
        {
            Log.Instance.Trace($"Entering - {account}");

            var sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where("account = @0 ", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

            if (asOfDate != null)
                sql.Where($"{GetRealColumn(nameof(Chk.mod_date))} > @0",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = asOfDate });

            sql.OrderBy("pay_no");

            var records = dbConnection.Fetch<Chk>(sql);

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

                var records = dbConnection.Fetch<Chk>(sql);
                return records;
            }

        }

        public override object Add(Chk table)
        {
            table.Status = "NEW";

            AccountRepository accountRepository = new AccountRepository(dbConnection);

            return base.Add(table);
        }

        public bool AddBatch(List<Chk> chks)
        {

            Log.Instance.Trace("Entering");

            try
            {
                dbConnection.BeginTransaction();

                // Some transactional DB work
                foreach (Chk chk in chks)
                {
                    this.Add(chk);
                }

                dbConnection.CompleteTransaction();
            }
            catch (Exception e)
            {
                Log.Instance.Fatal(e, $"Exception adding chk record");
                dbConnection.AbortTransaction();
                return false;
            }

            return true;

        }

    }
}
