using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class ClientDiscountRepository : RepositoryBase<ClientDiscount>
    {
        public ClientDiscountRepository(string connectionString) : base(connectionString)
        {

        }

        public ClientDiscountRepository(PetaPoco.Database db) : base(db)
        {

        }

        public List<ClientDiscount> GetByClient(string clientMnem, bool includeDeleted = false)
        {
            Log.Instance.Trace($"Entering - Client {clientMnem}");
            List<ClientDiscount> results = null;
            if (!includeDeleted)
            {
                results = dbConnection.Fetch<ClientDiscount>($"where {this.GetRealColumn(nameof(ClientDiscount.ClientMnem))} = @0 and {this.GetRealColumn(nameof(ClientDiscount.IsDeleted))} = 0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
            }
            else
            {
                results = dbConnection.Fetch<ClientDiscount>($"where {this.GetRealColumn(nameof(ClientDiscount.ClientMnem))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
            }
            return results;
        }

        public ClientDiscount GetDiscount(string clientMnem, string cdm)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} cdm {cdm}");

            var command = PetaPoco.Sql.Builder;

            command.Where($"{GetRealColumn(nameof(ClientDiscount.ClientMnem))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
            command.Where($"{GetRealColumn(nameof(ClientDiscount.IsDeleted))} = 0");
            command.Where($"{GetRealColumn(nameof(ClientDiscount.StartCdmRange))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });


            return dbConnection.SingleOrDefault<ClientDiscount>(command);
        }

        public override ClientDiscount GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
