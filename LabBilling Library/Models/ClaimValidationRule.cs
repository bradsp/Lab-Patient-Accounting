using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PetaPoco;

namespace LabBilling.Core.Models
{
    [TableName("dict_claim_validation_rules")]
    [PrimaryKey("RuleId", AutoIncrement = true)]
    public class ClaimValidationRule : IBaseEntity
    {
        public int RuleId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public string ErrorText { get; set; }
        public DateTime EffectiveDate { get; set; }
        public DateTime EndEffectiveDate { get; set; }

        [Ignore]
        public List<ClaimValidationRuleCriterion> claimValidationRuleCriteria { get; set; }
        
        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        public Guid rowguid { get; set; }
    }

    [TableName("dict_claim_validation_rule_criteria")]
    [PrimaryKey("RuleCriteriaId", AutoIncrement = true)]
    public class ClaimValidationRuleCriterion : IBaseEntity
    {
        public int RuleId { get; set; }
        public string LineType { get; set; }
        public int GroupId { get; set; }
        public int ParentGroupId { get; set; }
        public string Class { get; set; }
        public string MemberName { get; set; }
        public string Operator { get; set; }
        public string TargetValue { get; set; }

        public DateTime mod_date { get; set; }
        public string mod_user { get; set; }
        public string mod_prg { get; set; }
        public string mod_host { get; set; }
        public Guid rowguid { get; set; }
    }

    public class ClaimValidationRuleCriteriaRelator
    {
        public ClaimValidationRule current;

        public ClaimValidationRule MapIt(ClaimValidationRule rule, ClaimValidationRuleCriterion c)
        {
            if (rule == null)
                return current;

            if (current != null && current.RuleId == rule.RuleId)
            {
                current.claimValidationRuleCriteria.Add(c);

                return null;
            }

            var prev = current;

            current = rule;
            current.claimValidationRuleCriteria = new List<ClaimValidationRuleCriterion>
            {
                c
            };

            return prev;
        }
    }

}
