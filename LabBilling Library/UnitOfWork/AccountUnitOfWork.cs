using LabBilling.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.UnitOfWork
{
    public class AccountUnitOfWork : UnitOfWorkMain, IAccountUnitOfWork
    {
        public AccountUnitOfWork(PetaPoco.Database context) : base(context)  {  }

        public AccountUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction)  {  }



    }
}
