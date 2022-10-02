using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using PetaPoco.Core;

namespace LabBilling.Core.DataAccess
{
    public class AccountValidationStatusRepository : RepositoryBase<AccountValidationStatus>
    {

        public AccountValidationStatusRepository(string connectionString) : base(connectionString)
        {
        }

        public AccountValidationStatusRepository(PetaPoco.Database db) : base(db)
        {
        }

        public AccountValidationStatus GetByAccount(string account)
        {
            var record = dbConnection.SingleOrDefault<AccountValidationStatus>("where account = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
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
