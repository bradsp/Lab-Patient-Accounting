using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class WriteOffCodeRepository : RepositoryBase<WriteOffCode>
    {

        public WriteOffCodeRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
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
            var record = Context.SingleOrDefault<WriteOffCode>($"where {GetRealColumn(nameof(WriteOffCode.Code))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = code });

            if (record != null)
            {

            }

            return record;
        }


    }
}
