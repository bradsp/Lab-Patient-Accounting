using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using PetaPoco;

namespace LabBilling.Core.UnitOfWork
{
    public class PatientStatementUnitOfWork : UnitOfWorkMain, IPatientStatementUnitOfWork
    {

        public PatientStatementUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction)
        {

        }

        public PatientStatementUnitOfWork(PetaPoco.Database context) : base(context)
        {

        }
    }
}
