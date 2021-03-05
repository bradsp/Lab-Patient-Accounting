using System;
using System.Data.SqlClient;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(string connection) : base("client", connection)
        {

        }

        public Client GetClient(string clientMnem)
        {
            Log.Instance.Debug($"Entering");
            if (clientMnem == null)
            {
                throw new ArgumentNullException("clientMnem");
            }

            var record = dbConnection.SingleOrDefault<Client>("where cli_mnem = @0", clientMnem);

            return record;
        }

        public override Client GetById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Compute the client balance. Uses the vw_chrg_bal_cbill and vw_chk_bal_cbill
        /// which is essentially the same as the vw_chrg_bal and vw_chk_bal
        /// except it excludes charges/checks that have not been on a cbill
        /// (by using the invoice field).
        /// </summary>
        /// <param name="clientMnem"></param>
        /// <returns></returns>
        public double Balance(string clientMnem)
        {
            Log.Instance.Debug($"Entering");

            if(clientMnem == null)
            {
                throw new ArgumentNullException("clientMnem");
            }

            var c = Sql.Builder.Append("SELECT total FROM vw_chrg_bal_cbill WHERE account = @0", clientMnem);

            double chrgResult = dbConnection.ExecuteScalar<double?>(c) ?? 0.0;

            var p = Sql.Builder.Append("SELECT total FROM vw_chk_bal_cbill WHERE account = @0", clientMnem);

            double chkResult = dbConnection.ExecuteScalar<double?>(p) ?? 0.0;

            double BalanceReturn = chrgResult - chkResult;

            return BalanceReturn;

        }

    }
}
