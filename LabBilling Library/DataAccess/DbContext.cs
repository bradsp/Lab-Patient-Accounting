using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class DbContext
    {
        protected IAppEnvironment AppEnvironment { get; set; }
        public DbContext(IAppEnvironment appEnvironment)
        {
            AppEnvironment = appEnvironment;

            AccountLmrpErrorRepository = new AccountLmrpErrorRepository(appEnvironment);
            AccountNoteRepository = new AccountNoteRepository(appEnvironment);
            AccountRepository = new AccountRepository(appEnvironment);
            AccountSearchRepository = new AccountSearchRepository(appEnvironment);
            AccountValidationCriteriaRepository = new AccountValidationCriteriaRepository(appEnvironment);
            AccountValidationRuleRepository = new AccountValidationRuleRepository(appEnvironment);
            AccountValidationStatusRepository = new AccountValidationStatusRepository(appEnvironment);
            AnnouncementRepository = new AnnouncementRepository(appEnvironment);
            BadDebtRepository = new BadDebtRepository(appEnvironment);
            BillingActivityRepository = new BillingActivityRepository(appEnvironment);
            BillingBatchRepository = new BillingBatchRepository(appEnvironment);
            CdmDetailRepository = new CdmDetailRepository(appEnvironment);
            CdmRepository = new CdmRepository(appEnvironment);
            ChkBatchDetailRepository = new ChkBatchDetailRepository(appEnvironment);
            ChkBatchRepository = new ChkBatchRepository(appEnvironment);
            ChkRepository = new ChkRepository(appEnvironment);
            ChrgDetailRepository = new ChrgDetailRepository(appEnvironment);
            ChrgRepository = new ChrgRepository(appEnvironment);
            ClientDiscountRepository = new ClientDiscountRepository(appEnvironment);
            ClientRepository = new ClientRepository(appEnvironment);
            ClientTypeRepository = new ClientTypeRepository(appEnvironment);
            DictDxRepository = new DictDxRepository(appEnvironment);
            EmpRepository = new EmpRepository(appEnvironment);
            FinRepository = new FinRepository(appEnvironment);
            GLCodeRepository = new GLCodeRepository(appEnvironment);
            GlobalBillingCdmRepository = new GlobalBillingCdmRepository(appEnvironment);
            InsCompanyRepository = new InsCompanyRepository(appEnvironment);
            InsRepository = new InsRepository(appEnvironment);
            InvoiceHistoryRepository = new InvoiceHistoryRepository(appEnvironment);
            LMRPRuleRepository = new LMRPRuleRepository(appEnvironment);
            MappingRepository = new MappingRepository(appEnvironment);
            MessagesInboundRepository = new MessagesInboundRepository(appEnvironment);
            MutuallyExclusiveEditRepository = new MutuallyExclusiveEditRepository(appEnvironment);
            PatDxRepository = new PatDxRepository(appEnvironment);
            NumberRepository = new NumberRepository(appEnvironment);
            PatientStatementAccountRepository = new PatientStatementAccountRepository(appEnvironment);
            PatientStatementCernerRepository = new PatientStatementCernerRepository(appEnvironment);
            PatientStatementEncounterActivityRepository = new PatientStatementEncounterActivityRepository(appEnvironment);
            PatientStatementEncounterRepository = new PatientStatementEncounterRepository(appEnvironment);
            PatientStatementRepository = new PatientStatementRepository(appEnvironment);
            PatRepository = new PatRepository(appEnvironment);
            PhyRepository = new PhyRepository(appEnvironment);
            RevenueCodeRepository = new RevenueCodeRepository(appEnvironment);
            SystemParametersRepository = new SystemParametersRepository(appEnvironment);
            UserProfileRepository = new UserProfileRepository(appEnvironment);
            WriteOffCodeRepository = new WriteOffCodeRepository(appEnvironment);
        }

        public AccountLmrpErrorRepository AccountLmrpErrorRepository { get; private set; } 
        public AccountNoteRepository AccountNoteRepository { get; private set; }
        public AccountRepository AccountRepository { get; private set; }
        public AccountSearchRepository AccountSearchRepository { get; private set; }
        public AccountValidationCriteriaRepository AccountValidationCriteriaRepository { get; private set; }
        public AccountValidationRuleRepository AccountValidationRuleRepository { get; private set; }
        public AccountValidationStatusRepository AccountValidationStatusRepository { get; private set; }
        public AnnouncementRepository AnnouncementRepository { get; private set; }
        public BadDebtRepository BadDebtRepository { get; private set; }
        public BillingActivityRepository BillingActivityRepository { get; private set; }
        public BillingBatchRepository BillingBatchRepository { get; private set; }
        public CdmDetailRepository CdmDetailRepository { get; private set; }
        public CdmRepository CdmRepository { get; private set; }
        public ChkBatchDetailRepository ChkBatchDetailRepository { get; private set; }
        public ChkBatchRepository ChkBatchRepository { get; private set; }
        public ChkRepository ChkRepository { get; private set; }
        public ChrgDetailRepository ChrgDetailRepository { get; private set; }
        public ChrgRepository ChrgRepository { get; private set; }
        public ClientDiscountRepository ClientDiscountRepository { get; private set; }
        public ClientRepository ClientRepository { get; private set; }
        public ClientTypeRepository ClientTypeRepository { get; private set; }
        public DictDxRepository DictDxRepository { get; private set; }
        public EmpRepository EmpRepository { get; private set; }
        public FinRepository FinRepository { get; private set; }
        public GLCodeRepository GLCodeRepository { get; private set; }
        public GlobalBillingCdmRepository GlobalBillingCdmRepository { get; private set; }
        public InsCompanyRepository InsCompanyRepository { get; private set; }
        public InsRepository InsRepository { get; private set; }
        public InvoiceHistoryRepository InvoiceHistoryRepository { get; private set; }
        public LMRPRuleRepository LMRPRuleRepository { get; private set; }
        public MappingRepository MappingRepository { get; private set; }
        public MessagesInboundRepository MessagesInboundRepository { get; private set; }
        public MutuallyExclusiveEditRepository MutuallyExclusiveEditRepository { get; private set; }
        public NumberRepository NumberRepository { get; private set; }
        public PatDxRepository PatDxRepository { get; private set; }
        public PatientStatementAccountRepository PatientStatementAccountRepository { get; private set; }
        public PatientStatementCernerRepository PatientStatementCernerRepository { get; private set; }
        public PatientStatementEncounterActivityRepository PatientStatementEncounterActivityRepository { get; private set; }
        public PatientStatementEncounterRepository PatientStatementEncounterRepository { get; private set; }
        public PatientStatementRepository PatientStatementRepository { get; private set; }
        public PatRepository PatRepository { get; private set; }
        public PhyRepository PhyRepository { get; private set; }
        public RevenueCodeRepository RevenueCodeRepository { get; private set; }
        public SystemParametersRepository SystemParametersRepository { get; private set; }
        public UserProfileRepository UserProfileRepository { get; private set; }
        public WriteOffCodeRepository WriteOffCodeRepository { get; private set; }
        public ReportingRepository ReportingRepository { get; private set; }


    }
}
