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
    public class AccountValidationCriteriaRepository : RepositoryBase<AccountValidationCriteria>
    {
        private string _connectionString;

        public AccountValidationCriteriaRepository(string connectionString) : base("dict_acc_validation_criteria", connectionString)
        {
            _connectionString = connectionString;
        }

        public AccountValidationCriteriaRepository(string connectionString, PetaPoco.Database db) : base("dict_acc_validation_criteria", connectionString, db)
        {
            _connectionString=connectionString;
        }

        public override AccountValidationCriteria GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<AccountValidationCriteria> GetByRuleId(int ruleId)
        {
            Log.Instance.Trace("$Entering");

            var sql = PetaPoco.Sql.Builder
                .Where("rule_id = @0 ", ruleId);

            var records = dbConnection.Fetch<AccountValidationCriteria>(sql);

            return records;
        }
    }
}
