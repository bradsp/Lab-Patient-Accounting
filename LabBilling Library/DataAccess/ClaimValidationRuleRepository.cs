using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using LabBilling.Core.BusinessLogic;

namespace LabBilling.Core.DataAccess
{
    public class ClaimValidationRuleRepository : RepositoryBase<ClaimValidationRule>
    {
        public ClaimValidationRuleRepository(string connection) : base("dict_claim_validation_rule", connection)
        {

        }

        public ClaimValidationRuleRepository(string connection, PetaPoco.Database db) : base("dict_claim_validation_rule", connection, db)
        {

        }

        public override ClaimValidationRule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<ClaimValidationRule> GetRules()
        {
            Log.Instance.Debug($"Entering");

            var sql = PetaPoco.Sql.Builder
                .Select("*")
                .From("dict_claim_validation_rules")
                .InnerJoin("dict_claim_validation_rule_criteria").On("dict_claim_validation_rules.RuleId = dict_claim_validation_rule_criteria.RuleId");

            sql.OrderBy("dict_claim_validation_rules.RuleId, dict_claim_validation_rule_criteria.GroupId ");

            var result = dbConnection.Fetch<ClaimValidationRule, ClaimValidationRuleCriterion, ClaimValidationRule>(new ClaimValidationRuleCriteriaRelator().MapIt, sql);

            return result;

        }
    }
}
