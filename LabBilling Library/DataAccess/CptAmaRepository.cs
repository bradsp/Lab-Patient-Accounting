using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using PetaPoco;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public class CptAmaRepository : RepositoryBase<CptAma>
    {
        public CptAmaRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public CptAma GetCpt(string cpt)
        {
            if (string.IsNullOrEmpty(cpt))
                return new CptAma();

            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(CptAma.Cpt))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt });
            
            return Context.SingleOrDefault<CptAma>(sql);
        }
    }
}
