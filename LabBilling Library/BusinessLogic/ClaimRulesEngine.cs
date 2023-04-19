using System.Collections.Generic;
using System.Text;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using MicroRuleEngine;
using System.Text.Json;
using System.IO;


namespace LabBilling.Core.BusinessLogic
{
    public sealed class ClaimRulesEngine
    {
        //public static List<MRERule> dbRules = new List<MRERule>()
        //{
        //   new MRERule()
        //   {
        //        RuleName = "CPT 80101",
        //        Description = "Do not bill 80101",
        //        ErrorText = "DO NOT BILL. Has 80101 cpt4 (dictionary edit)",
        //        Rule = ClaimValidationRules.GetRule()
        //   }
        //};

        private List<ClaimValidationRule> _rules;

        public ClaimRulesEngine(string connectionstring)
        {
            ClaimValidationRuleRepository claimRuleRepository = new ClaimValidationRuleRepository(connectionstring);

            _rules = claimRuleRepository.GetRules();
            
        }

        public ClaimRulesEngine(PetaPoco.Database db)
        {
            ClaimValidationRuleRepository claimRuleRepository = new ClaimValidationRuleRepository(db);

            _rules = claimRuleRepository.GetRules();

        }

        #region Testdata
        //public static List<ClaimValidationRule> claimValidationRules = new List<ClaimValidationRule>()
        //{
        //    new ClaimValidationRule()
        //    {
        //        RuleId = 1,
        //        RuleName = "CPT 80101",
        //        Description = "Do not bill 80101",
        //        ErrorText = "DO NOT BILL. Has 80101 cpt4 (dictionary edit)",
        //        claimValidationRuleCriteria = new List<ClaimValidationRuleCriterion>()
        //        {
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Header",
        //                GroupId = 0,
        //                ParentGroupId = 0,
        //                Operator = "AndAlso"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Group",
        //                GroupId = 1,
        //                ParentGroupId = 0,
        //                MemberName = "",
        //                Operator = "Or",
        //                TargetValue = ""
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Detail",
        //                GroupId = 1,
        //                ParentGroupId = 0,
        //                MemberName = "fin_code",
        //                Operator = "Equal",
        //                TargetValue = "A"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Detail",
        //                GroupId = 1,
        //                ParentGroupId = 0,
        //                MemberName = "fin_code",
        //                Operator = "Equal",
        //                TargetValue = "M"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Group",
        //                GroupId = 2,
        //                ParentGroupId = 0,
        //                Operator = "Or"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Detail",
        //                GroupId = 2,
        //                ParentGroupId = 0,
        //                MemberName = "Insurances[0].InsCode",
        //                Operator = "Equal",
        //                TargetValue = "AM"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Detail",
        //                GroupId = 2,
        //                ParentGroupId = 0,
        //                MemberName = "Insurances[0].InsCode",
        //                Operator = "Equal",
        //                TargetValue = "WIN"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                GroupId = 3,
        //                LineType = "Group",
        //                ParentGroupId = 0,
        //                Operator = "Any",
        //                MemberName = "Charges",
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "SubGroup",
        //                GroupId = 4,
        //                ParentGroupId = 3,
        //                Operator = "Any",
        //                MemberName = "ChrgDetails"
        //            },
        //            new ClaimValidationRuleCriterion()
        //            {
        //                RuleId = 1,
        //                LineType = "Detail",
        //                GroupId = 4,
        //                ParentGroupId = 3,
        //                MemberName = "cpt4",
        //                Operator = "Equal",
        //                TargetValue = "80101"
        //            }
        //        }
        //    }
        //};
        #endregion

        /// <summary>
        /// Runs claim validation rules against account.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="errorList"></param>
        /// <returns>True if no validation errors; False if errors. Errors will be in errorList.</returns>        
        public bool ValidateAccount(Account account, out string errorList)
        {            
            

            var lRules = PrepareClaimRules.PrepareRules(_rules);

            errorList = null;
            StringBuilder sbErrors = new StringBuilder();

            MRE engine = new MRE();


            foreach (var rule in lRules)
            {
                var compiledRule = engine.CompileRule<Account>(rule.Rule);
                if (compiledRule(account))
                {
                    sbErrors.AppendLine(rule.ErrorText);
                }
            }

            if (sbErrors.Length > 0)
            {
                errorList = sbErrors.ToString();
                return false;
            }
            return true;

        }


    }

    //public static class ClaimValidationRules
    //{

    //    public static MicroRuleEngine.Rule GetRule()
    //    {
    //        return rule;
    //    }

    //    static MicroRuleEngine.Rule rule = new MicroRuleEngine.Rule()
    //    {
    //        Operator = "AndAlso",
    //        Rules = new List<MicroRuleEngine.Rule>()
    //        {
    //            new MicroRuleEngine.Rule()
    //            {
    //                Operator = "Or",
    //                Rules = new []
    //                {
    //                    new MicroRuleEngine.Rule()
    //                    {
    //                        MemberName = "fin_code",
    //                        Operator = System.Linq.Expressions.ExpressionType.Equal.ToString(),
    //                        TargetValue = "A"
    //                    },
    //                    new MicroRuleEngine.Rule()
    //                    {
    //                        MemberName = "fin_code",
    //                        Operator = System.Linq.Expressions.ExpressionType.Equal.ToString(),
    //                        TargetValue = "M"
    //                    }
    //                }
    //            },
    //            new MicroRuleEngine.Rule()
    //            {
    //                Operator = "Or",
    //                Rules = new []
    //                {
    //                    new MicroRuleEngine.Rule()
    //                    {
    //                        MemberName = "Insurances[0].InsCode",
    //                        Operator = System.Linq.Expressions.ExpressionType.Equal.ToString(),
    //                        TargetValue = "AM"
    //                    },
    //                    new MicroRuleEngine.Rule()
    //                    {
    //                        MemberName = "Insurances[0].InsCode",
    //                        Operator = System.Linq.Expressions.ExpressionType.Equal.ToString(),
    //                        TargetValue = "WIN"
    //                    }
    //                }
    //            },
    //            new MicroRuleEngine.Rule()
    //            {
    //                MemberName = "Charges",
    //                Operator = "Any",
    //                Rules = new []
    //                {
    //                    new MicroRuleEngine.Rule()
    //                    {
    //                        MemberName = "ChrgDetails",
    //                        Operator = "Any",
    //                        Rules = new []
    //                        {
    //                            new MicroRuleEngine.Rule()
    //                            {
    //                                MemberName = "cpt4",
    //                                Operator = "Equal",
    //                                TargetValue = "80101"
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    };
    //}


    public class MRERule
    {
        public string RuleId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public string ErrorText { get; set; }
        public string Trigger { get; set; }
        public string Action { get; set; }
        public MicroRuleEngine.Rule Rule { get; set; }

    }


}
