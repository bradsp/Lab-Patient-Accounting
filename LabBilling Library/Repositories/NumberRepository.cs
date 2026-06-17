using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class NumberRepository : RepositoryBase<Number>
    {
        public NumberRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public decimal GetNumber(string key)
        {
            Log.Instance.Debug($"Entering");

            //string sql = "exec GetNextNumber @key";
            // NOTE Phase 23-03: ";EXEC ... @1 OUTPUT" is T-SQL stored-proc syntax; PG uses a function call. Rewrite the SQL text + output-param mechanics there.
            var number = new NpgsqlParameter("@NextSequence", NpgsqlDbType.Integer)
            {
                Direction = System.Data.ParameterDirection.Output
            };
            var s = Sql.Builder.Append(";EXEC GetNextNumber @0, @1 OUTPUT",
                key,
                number);

            var result = Context.Execute(s);

            int NumberReturn = (int)number.Value;

            return NumberReturn;
        }
    }
}
