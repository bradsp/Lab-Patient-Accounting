using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using PetaPoco;
using RulesEngine;
using RulesEngine.Models;
using RulesEngine.Actions;
using LabBilling.Logging;
using LabBilling.Core.DataAccess;

namespace LabBilling.Core.BusinessLogic
{
    public class ClaimRulesEngine
    {
        private List<Rule> dbRules;
        private string _connection;

        public ClaimRulesEngine(string connection)
        {
            dbRules = new List<Rule>
            {
                new Rule()
                {
                    RuleName = "80101",
                    Description = "Do not bill 80101",
                    ErrorText = "DO NOT BILL. Has 80101 cpt4 (dictionary edit)",
                    Trigger = "",
                    ruleConditions = new List<RuleCondition>
                    {
                        new RuleCondition() { PropertyName = "Charges", Operation = "Lambda", Value = "input1.cpt4List.Contains(\"80101\")", ValueType = "string" },
                        new RuleCondition() { PropertyName = "PrimaryInsuranceCode", Operation = "Lambda", Value = "input1.PrimaryInsuranceCode == \"AM\"", ValueType = "string" },
                        new RuleCondition() { PropertyName = "fin_code", Operation = "Lambda", Value = "input1.fin_code == \"A\" || input1.fin_code == \"M\"", ValueType = "string" },
                        //new RuleCondition() { PropertyName = "Insurances[0].InsCode", Operation = "Equals", Value = "HUM", ValueType = "string" },
                    },
                }
            };
            _connection = connection;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="sb">Contains a list of all validation errors.</param>
        /// <returns></returns>
        /// <exception cref="RuleProcessException"></exception>        
        public bool Evaluate(Account account, out StringBuilder sb)
        {
            //var insCodeMatch = account.Insurances.All(x => x.InsCode == "WIN");
            //var cptMatch = account.Charges.All(x => x.ChrgDetails.All(y => y.cpt4 == "80101"));
            sb = new StringBuilder();
            List<RulesEngine.Models.Rule> rules = new List<RulesEngine.Models.Rule>();

            foreach (var rule in this.dbRules)
            {
                foreach(var condition in rule.ruleConditions)
                {
                    rules.Add(new RulesEngine.Models.Rule()
                    {
                        RuleName = rule.Description + condition.PropertyName,
                        SuccessEvent = condition.PropertyName + " meets criteria.",
                        ErrorMessage = condition.PropertyName + " does not meet criteria.",
                        ErrorType = ErrorType.Error,
                        RuleExpressionType = RuleExpressionType.LambdaExpression,
                        Expression = condition.Value
                    });
                }

                var workflows = new List<Workflow>();

                Workflow workflow = new Workflow();
                workflow.WorkflowName = rule.Description;
                workflow.Rules = rules;

                workflows.Add(workflow);

                var bre = new RulesEngine.RulesEngine(workflows.ToArray(), null);

                var resultList = bre.ExecuteAllRulesAsync(rule.Description, account);
                bool meetsAllCriteria = true;
                foreach (var result in resultList.Result)
                {
                    if(!result.IsSuccess)
                    {
                        meetsAllCriteria = false;
                    }
                }

                if(resultList.IsFaulted)
                {
                    //this is a rule processing error
                    Log.Instance.Error("Error processing claim validation rule.");
                    throw new RuleProcessException("Error processing claim validation rule.", rule.RuleName);
                }

                if (meetsAllCriteria)
                    sb.AppendLine(rule.ErrorText);
            }

            //add/update validation status in database

            AccountValidationStatusRepository accountValidationStatusRepository = new AccountValidationStatusRepository(_connection);
            if(sb.Length > 0)
            {
                account.AccountValidationStatus.account = account.account;
                account.AccountValidationStatus.mod_date = DateTime.Now;
                account.AccountValidationStatus.validation_text = sb.ToString();
                try
                {
                    accountValidationStatusRepository.Save(account.AccountValidationStatus);
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex.ToString(), ex);
                    throw new RuleProcessException("Error writing account validation status.", ex);
                }
                return false;
            }
            return true;
        }
    }

    public class Rule
    {
        public string RuleId { get; set; }
        public string RuleName { get; set; }
        public string Description { get; set; }
        public string ErrorText { get; set; }
        public string Trigger { get; set; }
        public string Action { get; set; }

        public List<RuleCondition> ruleConditions = new List<RuleCondition>();

    }

    public class RuleCondition
    {
        public string RuleId { get; set; }
        public string PropertyName { get; set; }
        public string Operation { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
    }


}
