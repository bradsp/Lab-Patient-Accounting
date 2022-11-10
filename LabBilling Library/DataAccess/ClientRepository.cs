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
            Log.Instance.Trace("Entering");
            PetaPoco.Sql sql;

            if (includeInactive)
            {
                sql = PetaPoco.Sql.Builder
                    .From(_tableName);
            }
            else
            {
                sql = PetaPoco.Sql.Builder
                    .From(_tableName)
                    .Where($"{GetRealColumn(nameof(Client.IsDeleted))} = @0", false);
            }
            var queryResult = dbConnection.Fetch<Client>(sql);
            Log.Instance.Debug(dbConnection.LastSQL);

            return queryResult;
        }

        public Client GetClient(string clientMnem)
        {
            Log.Instance.Debug($"Entering - {clientMnem}");
            ClientDiscountRepository clientDiscountRepository = new ClientDiscountRepository(dbConnection);

            if (clientMnem == null)
            {
                throw new ArgumentNullException("clientMnem");
            }

            var record = dbConnection.SingleOrDefault<Client>("where cli_mnem = @0", new SqlParameter() { SqlDbType = System.Data.SqlDbType.VarChar, Value = clientMnem });
            Log.Instance.Debug(dbConnection.LastSQL);
            if (record != null)
                record.Discounts = clientDiscountRepository.GetByClient(clientMnem);
            Log.Instance.Debug(dbConnection.LastSQL);
            return record;
        }

        public override object Add(Client table)
        {
            if (string.IsNullOrEmpty(table.BillProfCharges))
                table.BillProfCharges = "NO";

            return base.Add(table);
        }

        public override bool Save(Client table)
        {
            var record = this.GetClient(table.ClientMnem);

            bool success;
            if (record != null)
            {
                success = this.Update(table);
            }
            else
            {
                try
                {
                    this.Add(table);
                    success = true;
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    success = false;
                }
            }
            Log.Instance.Debug(dbConnection.LastSQL);
            return success;
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
