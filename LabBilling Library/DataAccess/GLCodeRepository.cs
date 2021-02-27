using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class GLCodeRepository : RepositoryBase<GLCode>
    {
        public GLCodeRepository(string connection) : base("dict_general_ledger_codes", connection)
        {

        }


        public override GLCode GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
