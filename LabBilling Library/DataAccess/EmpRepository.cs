using LabBilling.Core.Models;
using System;
using LabBilling.Logging;

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

        public override Emp GetById(int Id)
        {
            throw new NotImplementedException();
        }

        public Emp GetByUsername(string username)
        {
            Log.Instance.Debug("$Entering");

            Emp emp = null;

            emp = dbConnection.SingleOrDefault<Emp>("where name = @0", username);

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
