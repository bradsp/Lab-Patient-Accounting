using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class RevenueCodeRepository : RepositoryBase<RevenueCode>
    {
        public RevenueCodeRepository(string connection) : base(connection)
        {

        }

        public RevenueCodeRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override RevenueCode GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
