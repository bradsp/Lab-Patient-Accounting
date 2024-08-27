using LabBilling.Core.DataAccess;

namespace LabBilling.Core.UnitOfWork;

public class PatientStatementUnitOfWork : UnitOfWorkMain, IPatientStatementUnitOfWork
{

    public PatientStatementUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction)
    {

    }

    public PatientStatementUnitOfWork(PetaPoco.Database context) : base(context)
    {

    }
}
