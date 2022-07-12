using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class BillingHistoryRepository : RepositoryBase<BillingHistory>
    {

        public BillingHistoryRepository(string connection) : base("data_billing_history", connection)
        {

        }

        public BillingHistoryRepository(string connection, PetaPoco.Database db) : base("data_billing_history", connection, db)
        {

        }

        public override BillingHistory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override object Add(BillingHistory table)
        {
            if (table.ins_complete == DateTime.MinValue)
                table.ins_complete = null;

            return base.Add(table);
        }


    }
}
