using System;
using System.Collections.Generic;
using System.Linq;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Data.SqlClient;
using System.Data;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public class CptAmaRepository : RepositoryBase<CptAma>
    {
        public CptAmaRepository(IAppEnvironment environment) : base(environment)
        {

        }

        public CptAma GetCpt(string cpt)
        {
            if (string.IsNullOrEmpty(cpt))
                return new CptAma();

            var sql = Sql.Builder
                .Where($"{GetRealColumn(nameof(CptAma.Cpt))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cpt });
            
            return dbConnection.SingleOrDefault<CptAma>(sql);
        }
    }
}
