using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public sealed class FinRepository : RepositoryBase<Fin>
    {
        public FinRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public List<Fin> GetActive()
        {
            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(Fin.IsDeleted))} = 0")
                .Where($"{GetRealColumn(nameof(Fin.FinCode))} <> @0", 
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, SqlValue = "CLIENT" });

            return dbConnection.Fetch<Fin>(sql);
        }

        public Fin GetFin(string finCode)
        {
            return dbConnection.SingleOrDefault<Fin>($"where {GetRealColumn(nameof(Fin.FinCode))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = finCode });
        }
    }
}
