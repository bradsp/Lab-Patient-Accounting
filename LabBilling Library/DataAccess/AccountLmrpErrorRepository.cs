﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;

namespace LabBilling.Core.DataAccess
{
    public class AccountLmrpErrorRepository : RepositoryBase<AccountLmrpError>
    {
        public AccountLmrpErrorRepository(string connectionString) : base(connectionString)
        {

        }

        public AccountLmrpErrorRepository(PetaPoco.Database db) : base(db)
        {

        }

        public override bool Save(AccountLmrpError table)
        {
            //lookup record by account
            var record = GetByAccount(table.AccountNo);

            if(record == null)
            {
                this.Add(table);
                return true;
            }
            else
            {
                table.uri = record.uri;
                return this.Update(table);
            }

        }

        public AccountLmrpError GetByAccount(string accountNo)
        {
            var record = dbConnection.SingleOrDefault<AccountLmrpError>($"where {this.GetRealColumn(typeof(AccountLmrpError), nameof(AccountLmrpError.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });

            return record;
        }

        public override AccountLmrpError GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}