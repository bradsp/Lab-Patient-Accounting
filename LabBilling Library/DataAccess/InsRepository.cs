using System;
using System.Collections.Generic;
using LabBilling.Logging;
using LabBilling.Models;

namespace LabBilling.DataAccess
{
    public class InsRepository : RepositoryBase<Ins>
    {
        public InsRepository(string connection) : base("ins", connection)
        {

        }

        public override Ins GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Ins> GetByAccount(string account)
        {
            Log.Instance.Debug("$Entering");

            var records = dbConnection.Fetch<Ins>("where account = @0", account);

            foreach(Ins ins in records)
            {
                ins.InsCompany = dbConnection.SingleOrDefault<InsCompany>("where code = @0", ins.InsCode);
            }

            return records;
        }

        public override bool Update(Ins table)
        {
            return base.Update(table);
        }
    }
}
