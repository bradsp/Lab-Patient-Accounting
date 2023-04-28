using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public sealed class MutuallyExclusiveEditRepository : RepositoryBase<MutuallyExclusiveEdit>
    {
        public MutuallyExclusiveEditRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public MutuallyExclusiveEdit GetEdit(string cpt1, string cpt2)
        {
            string cpt1RealName = this.GetRealColumn(typeof(MutuallyExclusiveEdit), nameof(MutuallyExclusiveEdit.Cpt1));
            string cpt2RealName = this.GetRealColumn(typeof(MutuallyExclusiveEdit), nameof(MutuallyExclusiveEdit.Cpt1));
            return dbConnection.SingleOrDefault<MutuallyExclusiveEdit>($"where {cpt1RealName} = @0 and {cpt2RealName} = @1", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt1 }, 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt2 });
        }
    }
}
