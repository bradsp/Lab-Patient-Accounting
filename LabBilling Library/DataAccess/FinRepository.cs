using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class FinRepository : RepositoryBase<Fin>
    {
        public FinRepository(string connection) : base(connection)
        {

        }

        public FinRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override Fin GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Fin GetFin(string finCode)
        {
            return dbConnection.SingleOrDefault<Fin>("where fin_code = @0", finCode);
        }
    }
}
