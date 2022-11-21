using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using LabBilling.Core.Models;
using LabBilling.Logging;
//using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

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

            CdmRepository cdmRepository = new CdmRepository(dbConnection);

            foreach(ClientDiscount clientDiscount in results)
            {
                var cdm = cdmRepository.GetCdm(clientDiscount.Cdm);
                if (cdm != null)
                    clientDiscount.CdmDescription = cdm.Description;
                else
                    clientDiscount.CdmDescription = "Cdm not found";

            }

            return results;
        }

        public ClientDiscount GetDiscount(string clientMnem, string cdm)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} cdm {cdm}");

            var command = PetaPoco.Sql.Builder;

            command.Where($"{GetRealColumn(nameof(ClientDiscount.ClientMnem))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });
            command.Where($"{GetRealColumn(nameof(ClientDiscount.IsDeleted))} = 0");
            command.Where($"{GetRealColumn(nameof(ClientDiscount.Cdm))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

            return dbConnection.SingleOrDefault<ClientDiscount>(command);
        }

        public override bool Save(ClientDiscount table)
        {
            if (string.IsNullOrEmpty(table.EndCdmRange))
            {
                table.EndCdmRange = table.Cdm;
            }
            else
            {
                if (table.EndCdmRange != table.Cdm)
                {
                    throw new DiscountRangeNotSupportedException();
                }
            }

            var existing = GetDiscount(table.ClientMnem, table.Cdm);

            if (existing == null)
                Add(table);
            else
                Update(table);

            return true;
        }

        public bool Delete(string clientMnem)
        {
            if(string.IsNullOrEmpty(clientMnem))
            {
                throw new ArgumentNullException("clientMnem");
            }

            dbConnection.Delete<ClientDiscount>($"where {GetRealColumn(nameof(ClientDiscount.ClientMnem))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem });


            return true;
        }

        public bool SaveAll(IEnumerable<ClientDiscount> discounts)
        {
            Log.Instance.Trace("Entering");

            string clientMnem = discounts.First().ClientMnem;
            try
            {
                BeginTransaction();
                //delete all discounts and rewrite
                Delete(clientMnem);

                foreach (ClientDiscount dis in discounts)
                {
                    Add(dis);
                }


                CompleteTransaction();
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex, "Error adding client discounts");
                AbortTransaction();
                return false;
            }

            return true;
        }

        public override ClientDiscount GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
