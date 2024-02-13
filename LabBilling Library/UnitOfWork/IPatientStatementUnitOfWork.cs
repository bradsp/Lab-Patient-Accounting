using LabBilling.Core.DataAccess;

namespace LabBilling.Core.UnitOfWork
{
    public interface IPatientStatementUnitOfWork
    {
        AccountRepository AccountRepository { get; }
        BadDebtRepository BadDebtRepository { get; }
        ChkRepository ChkRepository { get; }
        PatientStatementAccountRepository PatientStatementAccountRepository { get; }
        PatientStatementCernerRepository PatientStatementCernerRepository { get; }
        PatientStatementEncounterActivityRepository PatientStatementEncounterActivityRepository { get; }
        PatientStatementEncounterRepository PatientStatementEncounterRepository { get; }
        PatientStatementRepository PatientStatementRepository { get; }
        PatRepository PatRepository { get; }
        AccountNoteRepository AccountNoteRepository { get; }
    }
}