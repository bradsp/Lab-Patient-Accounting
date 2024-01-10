using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using LabBilling.Core.BusinessLogic;

namespace LabBilling.Core.DataAccess
{
    public sealed class ClaimValidationRuleRepository : RepositoryBase<ClaimValidationRule>
    {
        public ClaimValidationRuleRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
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

        public override bool Save(ClaimValidationRule table)
        {
            ClaimValidationRuleCriterionRepository criterionRepository = new ClaimValidationRuleCriterionRepository(AppEnvironment);

            //if RuleId == 0 - add new rule, otherwise update       
            if(table.RuleId == 0)
                table.RuleId = (int)Add(table);
            else
                Update(table);

            //loop through rule criteria
            foreach(var criterion in table.claimValidationRuleCriteria)
            {
                criterion.RuleId = table.RuleId;

                if (criterion.GroupId == 0)
                    criterionRepository.Add(criterion);
                else
                    criterionRepository.Update(criterion);


            }

            return base.Save(table);
        }
    }

    public class ClaimValidationRuleCriterionRepository : RepositoryBase<ClaimValidationRuleCriterion>
    {
        public ClaimValidationRuleCriterionRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

    }


}
