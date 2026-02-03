using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using PetaPoco;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess;
public sealed class InsCompanyRepository : RepositoryBase<InsCompany>
{

    public InsCompanyRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
    {

    }

    public InsCompany GetByCode(string code)
    {
        Log.Instance.Debug($"Entering");

        if (code == null)
        {
            Log.Instance.Error("Null value passed to InsCompanyRepository GetByCode.");
            return new InsCompany();
        }
        var record = Context.SingleOrDefault<InsCompany>("where code = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = code });

        return record;
    }

    public override List<InsCompany> GetAll()
    {
        return GetAll(true);
    }

    public List<InsCompany> GetAll(bool excludeDeleted)
    {
        Log.Instance.Debug($"Entering");

        var sql = Sql.Builder;
        sql.Where($"(@0 = 0 OR {GetRealColumn(nameof(InsCompany.IsDeleted))} = @1)",
            new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = excludeDeleted },
            new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = false });

        var queryResult = Context.Fetch<InsCompany>(sql);

        return queryResult;
    }


}
