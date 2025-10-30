using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;

namespace LabBilling.Core.DataAccess;

public sealed class FinRepository : RepositoryBase<Fin>
{
    public FinRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public List<Fin> GetActive()
    {
        var sql = PetaPoco.Sql.Builder
            .Where($"{GetRealColumn(nameof(Fin.IsDeleted))} = 0")
            .Where($"{GetRealColumn(nameof(Fin.FinCode))} <> @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, SqlValue = "CLIENT" });

        return Context.Fetch<Fin>(sql);
    }

    public Fin GetFin(string finCode)
    {
        return Context.SingleOrDefault<Fin>($"where {GetRealColumn(nameof(Fin.FinCode))} = @0",
            new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = finCode });
    }
}
