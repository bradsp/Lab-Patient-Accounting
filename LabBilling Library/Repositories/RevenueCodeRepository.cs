using System;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class RevenueCodeRepository : RepositoryBase<RevenueCode>
    {
        public RevenueCodeRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
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

            return Context.SingleOrDefault<RevenueCode>($"where {colName} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = revenueCode });
        }

    }
}
