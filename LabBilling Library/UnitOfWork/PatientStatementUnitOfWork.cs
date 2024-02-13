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

        public PatRepository PatRepository { get; private set; }
        public AccountRepository AccountRepository { get; private set; }
        public BadDebtRepository BadDebtRepository { get; private set; }
        public ChkRepository ChkRepository { get; private set; }
        public PatientStatementRepository PatientStatementRepository { get; private set; }
        public PatientStatementAccountRepository PatientStatementAccountRepository { get; private set; }
        public PatientStatementCernerRepository PatientStatementCernerRepository { get; private set; }
        public PatientStatementEncounterActivityRepository PatientStatementEncounterActivityRepository { get; private set; }
        public PatientStatementEncounterRepository PatientStatementEncounterRepository { get; private set; }
        public AccountNoteRepository AccountNoteRepository { get; private set; }

        public PatientStatementUnitOfWork(IAppEnvironment appEnvironment, bool useTransaction = false) : base(appEnvironment, useTransaction)
        {

        }

        public PatientStatementUnitOfWork(PetaPoco.Database context) : base(context)
        {

        }
    }
}
