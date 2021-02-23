using LabBilling.Logging;
using LabBilling.Models;
using PetaPoco;
using System;
using System.Data.SqlClient;

namespace LabBilling.DataAccess
{
    public class NumberRepository : RepositoryBase<Number>
    {
        public NumberRepository(string connection) : base("number", connection)
        {

        }

        public override Number GetById(int id)
        {
            throw new NotImplementedException();
        }

        public decimal GetNumber(string key)
        {
            Log.Instance.Debug($"Entering");

            //string sql = "exec GetNextNumber @key";
            //var param = new System.Data.SqlClient.SqlParameter("keyfield", keyfield);
            var number = new SqlParameter("@NextSequence", System.Data.SqlDbType.Int)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var s = Sql.Builder.Append(";EXEC GetNextNumber @0, @1 OUTPUT", key, number);

            var result = dbConnection.Execute(s);

            int NumberReturn = (int)number.Value;

            return NumberReturn;
        }
    }
}
