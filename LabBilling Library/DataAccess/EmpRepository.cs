using LabBilling.Core.Models;
using System;
using LabBilling.Logging;
using iText.Layout.Element;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class EmpRepository : RepositoryBase<Emp>
    {
        public EmpRepository(string connection) : base(connection)
        {
        }

        public EmpRepository(PetaPoco.Database db) : base(db)
        {

        }

        public List<Emp> GetActiveUsers()
        {
            Log.Instance.Trace($"Entering");

            var sqlCmd = new PetaPoco.Sql();
            sqlCmd.Where($"{GetRealColumn(nameof(Emp.Access))} <> 'NONE'");
            sqlCmd.OrderBy(GetRealColumn(nameof(Emp.FullName)));

            var emps = dbConnection.Fetch<Emp>(sqlCmd);

            return emps;
        }

        public override Emp GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Emp GetByUsername(string username)
        {
            Log.Instance.Trace($"Entering");

            Emp emp = null;

            emp = dbConnection.SingleOrDefault<Emp>("where name = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = username });

            return emp;
        }

        public bool LoginCheck(string u, string p)
        {
            Log.Instance.Debug($"Entering");
            bool isSuccess = false;

            Emp user = new Emp();

            user = GetByUsername(u);

            if (user.UserName != u || user.Password != p)
            {
                isSuccess = false;
            }
            else if (user.Access == "NONE")
            {
                isSuccess = false;
            }
            else if (user.Access == "VIEW" || user.Access == "ENTER/EDIT")
            {
                isSuccess = true;
            }

            return isSuccess;
        }

    }
}
