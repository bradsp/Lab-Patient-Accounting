using LabBilling.Core.Models;
using LabBilling.Logging;
using PetaPoco;
using System.Collections.Generic;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class PatientStatementCernerRepository : RepositoryBase<PatientStatementCerner>
    {
        public PatientStatementCernerRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context) { }

        public List<PatientStatementCerner> GetByBatch(string batch)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder;

            sql.Where($"{GetRealColumn(nameof(PatientStatementCerner.BatchId))} = @0",
                batch);

            var results = Context.Fetch<PatientStatementCerner>(sql);
            Log.Instance.Debug(Context.LastSQL);
            return results;
        }

    }
}
