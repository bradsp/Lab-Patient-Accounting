using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using PetaPoco;
using LabBilling.Logging;
using RFClassLibrary;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountValidationCriteriaRepository : RepositoryBase<AccountValidationCriteria>
    {

        public AccountValidationCriteriaRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
        }

        public List<AccountValidationCriteria> GetByRuleId(int ruleId)
        {
            Log.Instance.Trace("Entering");

            var sql = PetaPoco.Sql.Builder
                .Where("rule_id = @0 ", ruleId);

            var records = dbConnection.Fetch<AccountValidationCriteria>(sql);

            return records;
        }
    }
}
