using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.CodeDom.Compiler;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class CdmRepository : RepositoryBase<Cdm>
    {
        public CdmRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public override List<Cdm> GetAll()
        {
            return GetAll(false);
        }

        public List<Cdm> GetAll(bool includeDeleted = false)
        {
            Log.Instance.Debug($"Entering");
            Sql sql = Sql.Builder
                .From(_tableName);

            if (includeDeleted == false)
                sql.Where($"{this.GetRealColumn(nameof(Cdm.IsDeleted))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.Bit, Value = 0 });

            sql.Append($"order by {_tableName}.{this.GetRealColumn(nameof(Cdm.Description))}");

            var queryResult = Context.Fetch<Cdm>(sql);

            return queryResult;
        }

        public override bool Save(Cdm table)
        {
            var record = GetCdm(table.ChargeId, true);

            if (record != null)
                return Update(table);
            else
            {
                Add(table);
                return true;
            }
        }

        public override bool Update(Cdm table)
        {
            return base.Update(table);
        }

        public Cdm GetCdm(string cdm, bool includeDeleted = false)
        {

            string cdmRealName = this.GetRealColumn(nameof(Cdm.ChargeId));
            string isDeletedRealName = this.GetRealColumn(nameof(Cdm.IsDeleted));

            var cmd = PetaPoco.Sql.Builder;
            cmd.Where($"{cdmRealName} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cdm });

            if (!includeDeleted)
                cmd.Where($"{isDeletedRealName} = 0");

            var result = Context.SingleOrDefault<Cdm>(cmd);
            return result;
        }

        public List<Cdm> GetCdm(IList<string> cdms)
        {
            if (cdms.Count > 0)
            {
                Sql cmd = Sql.Builder;
                cmd.Where($"{GetRealColumn(nameof(Cdm.ChargeId))} in (@cdms)", new { cdms = cdms });
                List<Cdm> results = Context.Fetch<Cdm>(cmd);

                return results;
            }

            return new List<Cdm>();
        }


    }
}
