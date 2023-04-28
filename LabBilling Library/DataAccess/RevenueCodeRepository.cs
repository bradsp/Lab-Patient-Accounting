using System;
using System.Data.SqlClient;
using System.Data;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Core.DataAccess
{
    public sealed class RevenueCodeRepository : RepositoryBase<RevenueCode>
    {
        public RevenueCodeRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public RevenueCode GetByCode(string revenueCode)
        {
            Log.Instance.Trace("Entering");

            string colName = this.GetRealColumn(typeof(RevenueCode), nameof(RevenueCode.Code));

            if (revenueCode == null)
            {
                return new RevenueCode();
            }

            return dbConnection.SingleOrDefault<RevenueCode>($"where {colName} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = revenueCode });
        }

    }
}
