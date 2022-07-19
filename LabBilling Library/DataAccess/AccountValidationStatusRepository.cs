using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using PetaPoco.Core;

namespace LabBilling.Core.DataAccess
{
    public class AccountValidationStatusRepository : RepositoryBase<AccountValidationStatus>
    {
        private string _connection;

        public AccountValidationStatusRepository(string connectionString) : base("acc_validation_status", connectionString)
        {
            _connection = connectionString;
        }

        public AccountValidationStatusRepository(string connectionString, PetaPoco.Database db) : base("acc_validation_status", connectionString, db)
        {
            _connection = connectionString;
        }

        public AccountValidationStatus GetByAccount(string account)
        {
            var record = dbConnection.SingleOrDefault<AccountValidationStatus>("where account = @0", account);
            if (record == null)
                record = new AccountValidationStatus();
            return record;
        }

        public override bool Save(AccountValidationStatus table)
        {
            //TODO: error catching
            bool retVal = true;
            if(dbConnection.Exists<AccountValidationStatus>((object)table.account))
            {
                this.Update(table);
            }
            else
            {
                this.Add(table);
            }

            return retVal;

        }

        public override AccountValidationStatus GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
