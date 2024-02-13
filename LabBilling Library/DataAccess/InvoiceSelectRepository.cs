using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class InvoiceSelectRepository : RepositoryBase<InvoiceSelect>
    {
        public InvoiceSelectRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public List<InvoiceSelect> GetByClientAndDate(string clientMnem, DateTime throughDate)
        {
            var sql = Sql.Builder
                .Where($"{this.GetRealColumn(nameof(InvoiceSelect.ClientMnem))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem })
                .Where($"{this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.TransactionDate))} <= @1", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = throughDate });

            return Context.Fetch<InvoiceSelect>(sql);
        }

    }
}
