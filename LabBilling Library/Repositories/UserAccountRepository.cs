﻿using LabBilling.Core.Models;
using LabBilling.Logging;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    public sealed class UserAccountRepository : RepositoryBase<UserAccount>
    {
        protected override bool RequireValidEnvironment => false;
        public UserAccountRepository(IAppEnvironment appEnvironment, PetaPoco.IDatabase context) : base(appEnvironment, context)
        {
        }

        public List<UserAccount> GetActiveUsers()
        {
            Log.Instance.Trace($"Entering");

            var sqlCmd = new PetaPoco.Sql();
            sqlCmd.Where($"{GetRealColumn(nameof(UserAccount.Access))} <> @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = UserStatus.None });
            sqlCmd.OrderBy(GetRealColumn(nameof(UserAccount.FullName)));

            var emps = Context.Fetch<UserAccount>(sqlCmd);

            return emps;
        }

        public UserAccount GetByUsername(string username)
        {
            Log.Instance.Trace($"Entering");

            UserAccount emp = null;

            emp = Context.SingleOrDefault<UserAccount>("where name = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = username });

            return emp;
        }

        public bool LoginCheck(string u, string p)
        {
            Log.Instance.Debug($"Entering");
            bool isSuccess = false;

            UserAccount user = GetByUsername(u);

            if (user.UserName != u)
            {
                isSuccess = false;
            }
            else if (user.Access == UserStatus.None)
            {
                isSuccess = false;
            }
            else if (user.Access == UserStatus.EnterEdit || user.Access == UserStatus.View)
            {
                isSuccess = true;
            }

            return isSuccess;
        }
    }

    public sealed class UserStatus
    {
        public const string View = "VIEW";
        public const string EnterEdit = "ENTER/EDIT";
        public const string None = "NONE";


    }
}
