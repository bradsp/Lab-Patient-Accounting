using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.StyledXmlParser.Node;
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

        public RevenueCode GetByCode(string revenueCode)
        {
            string colName = this.GetRealColumn(typeof(RevenueCode), nameof(RevenueCode.Code));
            return dbConnection.SingleOrDefault<RevenueCode>($"where {colName} = @0", revenueCode);
        }

        public override RevenueCode GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
