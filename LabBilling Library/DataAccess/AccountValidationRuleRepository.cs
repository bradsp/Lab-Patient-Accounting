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
    public sealed class AccountValidationRuleRepository : RepositoryBase<AccountValidationRule>
    {
        private AccountValidationCriteriaRepository accountValidationCriteriaRepository;

        public AccountValidationRuleRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(appEnvironment);
        }

        public override List<AccountValidationRule> GetAll()
        {
            Log.Instance.Trace($"Entering");
            var results = base.GetAll();

            foreach(AccountValidationRule rule in results)
            {
                rule.validationCriterion = accountValidationCriteriaRepository.GetByRuleId(rule.rule_id);
            }

            return results;
        }

    }
}
