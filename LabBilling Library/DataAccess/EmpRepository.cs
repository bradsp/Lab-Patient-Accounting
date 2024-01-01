using LabBilling.Core.Models;
using System;
using LabBilling.Logging;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Data;
using Org.BouncyCastle.Crypto.Tls;

namespace LabBilling.Core.DataAccess
{
    public sealed class EmpRepository : RepositoryBase<Emp>
    {
        public EmpRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
        }

        public List<Emp> GetActiveUsers()
        {
            Log.Instance.Trace($"Entering");

            var sqlCmd = new PetaPoco.Sql();
            sqlCmd.Where($"{GetRealColumn(nameof(Emp.Access))} <> @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = UserStatus.None });
            sqlCmd.OrderBy(GetRealColumn(nameof(Emp.FullName)));

            var emps = dbConnection.Fetch<Emp>(sqlCmd);

            return emps;
        }

        public Emp GetByUsername(string username)
        {
            Log.Instance.Trace($"Entering");

            Emp emp = null;

            emp = dbConnection.SingleOrDefault<Emp>("where name = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = username });

            return emp;
        }

        public bool LoginCheck(string u, string p)
        {
            Log.Instance.Debug($"Entering");
            bool isSuccess = false;

            Emp user = new Emp();

            user = GetByUsername(u);

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
