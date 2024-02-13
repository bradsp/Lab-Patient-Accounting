using System;
using LabBilling.Core.DataAccess;
using PetaPoco;

namespace LabBilling.Core.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDatabase Context { get; }
        void Commit();
        void Rollback();
        AccountRepository AccountRepository { get; }
        AccountAlertRepository AccountAlertRepository { get; }
        PatRepository PatRepository { get; }
        InsRepository InsRepository { get; }
        ChrgRepository ChrgRepository { get; }
        ChkRepository ChkRepository { get; }
        ClientRepository ClientRepository { get; }
        ClientDiscountRepository ClientDiscountRepository { get; }
        AccountNoteRepository AccountNoteRepository { get; }
        BillingActivityRepository BillingActivityRepository { get; }
        AccountValidationStatusRepository AccountValidationStatusRepository { get; }
        LMRPRuleRepository LmrpRuleRepository { get; }
        FinRepository FinRepository { get; }
        AccountLmrpErrorRepository AccountLmrpErrorRepository { get; }
        CdmRepository CdmRepository { get; }
        GlobalBillingCdmRepository GlobalBillingCdmRepository { get; }
        PatientStatementAccountRepository PatientStatementAccountRepository { get; }
        InvoiceSelectRepository InvoiceSelectRepository { get; }
        ClaimItemRepository ClaimItemRepository { get; }
        BillingBatchRepository BillingBatchRepository { get; }
        NumberRepository NumberRepository { get; }
        RevenueCodeRepository RevenueCodeRepository { get; }
        ChrgDiagnosisPointerRepository ChrgDiagnosisPointerRepository { get; }
        ClientTypeRepository ClientTypeRepository { get; }
        InvoiceHistoryRepository InvoiceHistoryRepository { get; }
        MessagesInboundRepository MessagesInboundRepository { get; }
        InsCompanyRepository InsCompanyRepository { get; }
        DictDxRepository DictDxRepository { get; }
        PhyRepository PhyRepository { get; }
        MappingRepository MappingRepository { get; }
        BadDebtRepository BadDebtRepository { get; }
        PatientStatementRepository PatientStatementRepository { get; }
        PatientStatementEncounterRepository PatientStatementEncounterRepository { get; }
        PatientStatementEncounterActivityRepository PatientStatementEncounterActivityRepository { get; }
        PatientStatementCernerRepository PatientStatementCernerRepository { get; }
        AccountSearchRepository AccountSearchRepository { get; }
        UserProfileRepository UserProfileRepository { get; }
        ChkBatchRepository ChkBatchRepository { get; }
        ChkBatchDetailRepository ChkBatchDetailRepository { get; }
        ChrgDetailRepository ChrgDetailRepository { get; }
        GLCodeRepository GLCodeRepository { get; }
        AnnouncementRepository AnnouncementRepository { get; }
        SystemParametersRepository SystemParametersRepository { get; }
        WriteOffCodeRepository WriteOffCodeRepository { get; }
        UserAccountRepository UserAccountRepository { get; }
        CptAmaRepository CptAmaRepository { get; }
        ReportingRepository ReportingRepository { get; }
        CdmDetailRepository CdmDetailRepository { get; }
    }
}
