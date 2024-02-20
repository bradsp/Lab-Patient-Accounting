using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class ChrgDetailRepository : RepositoryBase<ChrgDetail>
    {
        public ChrgDetailRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {

        }

        public int AddModifier(int uri, string modifier)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .From($"{_tableName}")
                .Where($"{this.GetRealColumn(nameof(ChrgDetail.uri))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal,Value = uri });

            var result = Context.SingleOrDefault<ChrgDetail>(sql);

            if(result != null)
            {
                result.Modifier = modifier;

                return Context.Update(result);
            }

            return 0;
        }

        public int RemoveModifier(int uri)
        {
            return AddModifier(uri, string.Empty);
        }

        public IList<ChrgDetail> GetByChrgId(int chrgId)
        {
            Log.Instance.Trace($"Entering chrgId = {chrgId}");

            var sql = PetaPoco.Sql.Builder
                .Where($"{GetRealColumn(nameof(ChrgDetail.ChrgNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.Decimal, Value = chrgId });

            return Context.Fetch<ChrgDetail>(sql);

        }

    }

}
