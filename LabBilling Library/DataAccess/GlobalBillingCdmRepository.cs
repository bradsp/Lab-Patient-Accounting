using LabBilling.Core.Models;
using LabBilling.Logging;
using NPOI.SS.Formula.Functions;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Log = LabBilling.Logging.Log;

namespace LabBilling.Core.DataAccess
{
    public sealed class GlobalBillingCdmRepository : RepositoryBase<GlobalBillingCdm>
    {
        public GlobalBillingCdmRepository(IAppEnvironment appEnvironment) : base(appEnvironment)  { }

        public GlobalBillingCdm GetCdm(string cdm)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(GlobalBillingCdm.Cdm))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

            return dbConnection.SingleOrDefault<GlobalBillingCdm>(sql);
        }

    }
}
