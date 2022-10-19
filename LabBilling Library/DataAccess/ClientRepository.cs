using System;
using System.Data.SqlClient;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using NPOI.XWPF.UserModel;
using System.Collections.Generic;

namespace LabBilling.Core.DataAccess
{
    public class ClientRepository : RepositoryBase<Client>
    {
        public ClientRepository(string connection) : base(connection)
        {

        }

        public ClientRepository(PetaPoco.Database db) : base(db)
        {

        }

        public List<Client> GetAll(bool includeInactive)
        {
            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName)
                .Where($"{GetRealColumn(nameof(Client.IsDeleted))} = @0", includeInactive);

            var queryResult = dbConnection.Fetch<Client>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult;
        }

        public Client GetClient(string clientMnem)
        {
            Log.Instance.Debug($"Entering");
            if (clientMnem == null)
            {
                throw new ArgumentNullException("clientMnem");
            }

            var record = dbConnection.SingleOrDefault<Client>("where cli_mnem = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });
            if (record == null)
                record = new Client();

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

            var c = Sql.Builder.Append("SELECT total FROM vw_chrg_bal_cbill WHERE account = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });

            double chrgResult = dbConnection.ExecuteScalar<double?>(c) ?? 0.0;

            var p = Sql.Builder.Append("SELECT total FROM vw_chk_bal_cbill WHERE account = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });

            double chkResult = dbConnection.ExecuteScalar<double?>(p) ?? 0.0;

            double BalanceReturn = chrgResult - chkResult;

            return BalanceReturn;

        }

    }
}
