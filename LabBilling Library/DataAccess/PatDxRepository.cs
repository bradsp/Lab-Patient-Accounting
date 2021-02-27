using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class PatDxRepository : RepositoryBase<PatDx>
    {
        public PatDxRepository(string connection) : base("patdx", connection)
        {

        }

        public override PatDx GetById(int id)
        {
            throw new NotImplementedException();
        }


    }
}
