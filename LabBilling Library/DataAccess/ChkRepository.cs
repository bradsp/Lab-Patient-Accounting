using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Models;

namespace LabBilling.DataAccess
{
    public class ChkRepository : RepositoryBase<Chk>
    {
        public ChkRepository(string connection) : base("chk", connection)
        {

        }

        public override Chk GetById(int id)
        {
            Log.Instance.Trace("$Entering");
            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append("FROM chk ")
                .Append("WHERE pay_no = @0", id);

            var result = dbConnection.SingleOrDefault<Chk>(sql);

            return result;

        }

        public IEnumerable<Chk> GetByAccount(string account)
        {
            Log.Instance.Trace("$Entering");

            var sql = PetaPoco.Sql.Builder
                .Append("SELECT * ")
                .Append("FROM chk ")
                .Append("WHERE account = @0 ", account);

            sql.Append("order by pay_no");

            var records = dbConnection.Fetch<Chk>(sql);

            return records;
        }

        public IEnumerable<Chk> GetByAccount(string account, bool includeInvoiced)
        {
            Log.Instance.Trace("$Entering");
            if (includeInvoiced)
            {
                return this.GetByAccount(account);
            }
            else
            {
                var sql = PetaPoco.Sql.Builder
                    .Append("SELECT * ")
                    .Append("FROM chk ")
                    .Append("WHERE account = @0 and invoice IS NULL", account);

                sql.Append("order by pay_no");

                var records = dbConnection.Fetch<Chk>(sql);
                return records;
            }

        }

        public override object Add(Chk table)
        {

            return base.Add(table);
        }

        public bool AddBatch(List<Chk> chks)
        {

            Log.Instance.Trace("$Entering");

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
