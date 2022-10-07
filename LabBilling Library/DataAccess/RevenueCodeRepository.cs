using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.StyledXmlParser.Node;
using LabBilling.Core.Models;
using PetaPoco;
using LabBilling.Logging;

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

        public RevenueCode GetByCode(string revenueCode)
        {
            string colName = this.GetRealColumn(typeof(RevenueCode), nameof(RevenueCode.Code));

            if (revenueCode == null)
            {
                Log.Instance.Error("Null value passed to Revenue Code GetByCode.");
                return new RevenueCode();
            }

            return dbConnection.SingleOrDefault<RevenueCode>($"where {colName} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = revenueCode });
        }

        public override RevenueCode GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
