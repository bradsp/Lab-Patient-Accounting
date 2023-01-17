using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Core.DataAccess
{
    public class WriteOffCodeRepository : RepositoryBase<WriteOffCode>
    {

        public WriteOffCodeRepository(string connection) : base(connection)
        {

        }

        public WriteOffCodeRepository(PetaPoco.Database db) : base(db)
        {

        }

        public WriteOffCode GetByCode(string code)
        {
            Log.Instance.Debug($"Entering");

            if (code == null)
            {
                Log.Instance.Error("Null value passed to WriteOffCodeRepository GetByCode.");
                return new WriteOffCode();
            }
            var record = dbConnection.SingleOrDefault<WriteOffCode>($"where {GetRealColumn(nameof(WriteOffCode.Code))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = code });

            if (record != null)
            {

            }

            return record;
        }


    }
}
