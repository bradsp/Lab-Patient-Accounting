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

        public BillingHistoryRepository(string connection) : base(connection)
        {

        }

        public BillingHistoryRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override BillingHistory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override object Add(BillingHistory table)
        {
            if (table.InsComplete == DateTime.MinValue)
                table.InsComplete = null;

            return base.Add(table);
        }


    }
}
