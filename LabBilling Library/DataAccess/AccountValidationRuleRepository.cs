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
    public class AccountValidationRuleRepository : RepositoryBase<AccountValidationRule>
    {
        private string _connection;
        private AccountValidationCriteriaRepository accountValidationCriteriaRepository;

        public AccountValidationRuleRepository(string connection) : base("dict_acc_validation", connection)
        {
            _connection = connection;
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(_connection, dbConnection);
        }

        public AccountValidationRuleRepository(string connection, PetaPoco.Database db) : base("dict_acc_valiation", connection, db)
        {
            _connection = connection;
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(_connection, dbConnection);
        }

        public override AccountValidationRule GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<AccountValidationRule> GetAll()
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
