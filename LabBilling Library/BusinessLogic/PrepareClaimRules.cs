using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using MicroRuleEngine;

namespace LabBilling.Core.BusinessLogic
{
    public class PrepareClaimRules
    {

        private static List<ClaimValidationRule> _claimRules;


        public static List<MRERule> PrepareRules(List<ClaimValidationRule> claimRules)
        {
            _claimRules = claimRules;

            List<MRERule> mreRules = new List<MRERule>();

            foreach (var rule in _claimRules)
            {
                int groups = rule.claimValidationRuleCriteria.Count(x => x.LineType == "Group");
                int details = rule.claimValidationRuleCriteria.Count(x => x.LineType == "Detail");

                MRERule mreRule = new MRERule();
                mreRule.RuleName = rule.RuleName;
                mreRule.Description = rule.Description;
                mreRule.ErrorText = rule.ErrorText;

                MicroRuleEngine.Rule lRule = new Rule();

                var header = rule.claimValidationRuleCriteria.Where(x => x.LineType == "Header").Single();
                lRule = new Rule()
                {
                    MemberName = header.MemberName,
                    Operator = header.Operator,
                    TargetValue = header.TargetValue,
                    Rules = new List<Rule>()
                };

                //loop through rules by group
                for (int g = 1; g <= groups; g++)
                {
                    var groupRules = rule.claimValidationRuleCriteria.Where(x => x.GroupId == g && x.LineType == "Group");
                    MicroRuleEngine.Rule gRule = new Rule();

                    gRule.Rules = new List<Rule>();
                    var gr = rule.claimValidationRuleCriteria.Where(x => x.GroupId == g && x.LineType == "Group").FirstOrDefault();
                    var grRule = rule.claimValidationRuleCriteria.Where(x => x.GroupId == g);
                    gRule.MemberName = gr.MemberName;
                    gRule.Operator = gr.Operator;
                    gRule.TargetValue = gr.TargetValue;
                    gRule.Rules.AddRange(GetDetailLines(gr.GroupId, grRule).ToList());
                    if(rule.claimValidationRuleCriteria.Where(x=>x.ParentGroupId == g && x.LineType == "SubGroup").Count() > 0)
                        gRule.Rules.AddRange(GetSubGroup(gr.GroupId, rule.claimValidationRuleCriteria).ToList());

                    lRule.Rules.Add(gRule);
                }
                mreRule.Rule = lRule;
                mreRules.Add(mreRule);
            }

            return mreRules;
        }

        private static IEnumerable<MicroRuleEngine.Rule> GetSubGroup(int parentGroup, IEnumerable<ClaimValidationRuleCriterion> groupRule)
        {
            var groupId = groupRule
                .Where(y => y.ParentGroupId == parentGroup && y.LineType == "SubGroup")
                .Select(x => x.GroupId)
                .FirstOrDefault();

            if (groupId > 0)
            {
                var subGroupRule = groupRule.Where(x => x.GroupId == groupId && x.LineType == "SubGroup");

                var rules = new List<Rule>();

                foreach (var rule in subGroupRule)
                {
                    rules.Add(new Rule()
                    {
                        MemberName = rule.MemberName,
                        Operator = rule.Operator,
                        TargetValue = rule.TargetValue,
                        Rules = GetDetailLines(rule.GroupId, groupRule).ToList()
                    });
                }
                return rules;
            }

            return null;
        }

        private static IEnumerable<MicroRuleEngine.Rule> GetDetailLines(int groupId, IEnumerable<ClaimValidationRuleCriterion> groupRule)
        {
            var groupDetailRule = groupRule.Where(x => x.GroupId == groupId && x.LineType == "Detail");

            List<MicroRuleEngine.Rule> rules = new List<Rule>();
            foreach(var rule in groupDetailRule)
            {
                rules.Add(new Rule()
                {
                    MemberName = rule.MemberName,
                    Operator = rule.Operator,
                    TargetValue = rule.TargetValue
                });
            }

            return rules;
        }
    }
}
