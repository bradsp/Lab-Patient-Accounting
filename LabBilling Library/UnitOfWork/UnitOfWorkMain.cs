﻿using System;
using System.Data;
using LabBilling.Core.DataAccess;
using PetaPoco;
using PetaPoco.Providers;
using Utilities;

namespace LabBilling.Core.UnitOfWork
{
    public class UnitOfWorkMain : IUnitOfWork
    {
        public PetaPoco.IDatabase Context { get; private set; }
        private readonly bool _useDispose;
        private Transaction _transaction;
        public AccountRepository AccountRepository { get; private set; }
        public AccountAlertRepository AccountAlertRepository { get; private set; }
        public AccountSearchRepository AccountSearchRepository { get; private set; }
        public PatRepository PatRepository { get; private set; }
        public InsRepository InsRepository { get; private set; }
        public ChrgRepository ChrgRepository { get; private set; }
        public ChrgDetailRepository ChrgDetailRepository { get; private set; }
        public ChkRepository ChkRepository { get; private set; }
        public ClientRepository ClientRepository { get; private set; }
        public ClientDiscountRepository ClientDiscountRepository { get; private set; }
        public AccountNoteRepository AccountNoteRepository { get; private set; }
        public BillingActivityRepository BillingActivityRepository { get; private set; }
        public AccountValidationStatusRepository AccountValidationStatusRepository { get; private set; }
        public LMRPRuleRepository LmrpRuleRepository { get; private set; }
        public FinRepository FinRepository { get; private set; }
        public AccountLmrpErrorRepository AccountLmrpErrorRepository { get; private set; }
        public CdmRepository CdmRepository { get; private set; }
        public GlobalBillingCdmRepository GlobalBillingCdmRepository { get; private set; }
        public PatientStatementAccountRepository PatientStatementAccountRepository { get; private set; }
        public PatientStatementRepository PatientStatementRepository { get; private set; }
        public PatientStatementEncounterRepository PatientStatementEncounterRepository { get; private set; }
        public PatientStatementEncounterActivityRepository PatientStatementEncounterActivityRepository { get; private set; }
        public PatientStatementCernerRepository PatientStatementCernerRepository { get; private set; }
        public InvoiceSelectRepository InvoiceSelectRepository { get; private set; }
        public BillingBatchRepository BillingBatchRepository { get; private set; }
        public NumberRepository NumberRepository { get; private set; }
        public RevenueCodeRepository RevenueCodeRepository { get; private set; }
        public ChrgDiagnosisPointerRepository ChrgDiagnosisPointerRepository { get; private set; }
        public ClientTypeRepository ClientTypeRepository { get; private set; }
        public ClaimItemRepository ClaimItemRepository { get; private set; }
        public InvoiceHistoryRepository InvoiceHistoryRepository { get; private set; }
        public MessagesInboundRepository MessagesInboundRepository { get; private set; }
        public InsCompanyRepository InsCompanyRepository { get; private set; }
        public DictDxRepository DictDxRepository { get; private set; }
        public PhyRepository PhyRepository { get; private set; }
        public MappingRepository MappingRepository { get; private set; }
        public BadDebtRepository BadDebtRepository { get; private set; }
        public UserProfileRepository UserProfileRepository { get; private set; }
        public ChkBatchRepository ChkBatchRepository { get; private set; }
        public ChkBatchDetailRepository ChkBatchDetailRepository { get; private set; }
        public GLCodeRepository GLCodeRepository { get; private set; }
        public AnnouncementRepository AnnouncementRepository { get; private set; }
        public SystemParametersRepository SystemParametersRepository { get; private set; }
        public WriteOffCodeRepository WriteOffCodeRepository { get; private set; }
        public UserAccountRepository UserAccountRepository { get; private set; }
        public CptAmaRepository CptAmaRepository { get; private set; }
        public ReportingRepository ReportingRepository { get; private set; }
        public CdmDetailRepository CdmDetailRepository { get; private set; }

        /// <summary>
        /// For use with other Database instances outside of Unit Of Work
        /// </summary>
        /// <param name="context"></param>
        public UnitOfWorkMain(PetaPoco.Database context)
        {
            Context = context;
            _useDispose = false;
        }

        public UnitOfWorkMain(IAppEnvironment appEnvironment, bool useTransaction = false)
        {
            Context = Initialize(appEnvironment.ConnectionString);
            AccountRepository = new(appEnvironment, Context);
            AccountAlertRepository = new(appEnvironment, Context);
            AccountLmrpErrorRepository = new(appEnvironment, Context);
            AccountNoteRepository = new(appEnvironment, Context);
            AccountSearchRepository = new(appEnvironment, Context);
            AccountValidationStatusRepository = new(appEnvironment, Context);
            AnnouncementRepository = new(appEnvironment, Context);
            BadDebtRepository = new(appEnvironment, Context);
            BillingActivityRepository = new(appEnvironment, Context);
            BillingBatchRepository = new(appEnvironment, Context);
            CptAmaRepository = new(appEnvironment, Context);
            FinRepository = new(appEnvironment, Context);
            GLCodeRepository = new(appEnvironment, Context);
            GlobalBillingCdmRepository = new(appEnvironment, Context);
            InsCompanyRepository = new(appEnvironment, Context);
            InsRepository = new(appEnvironment, Context);
            InvoiceHistoryRepository = new(appEnvironment, Context);
            InvoiceSelectRepository = new(appEnvironment, Context);
            LmrpRuleRepository = new(appEnvironment, Context);
            MappingRepository = new(appEnvironment, Context);
            MessagesInboundRepository = new(appEnvironment, Context);
            NumberRepository = new(appEnvironment, Context);
            PatRepository = new(appEnvironment, Context);
            PatientStatementAccountRepository = new(appEnvironment, Context);
            PatientStatementCernerRepository = new(appEnvironment, Context);
            PatientStatementEncounterActivityRepository = new(appEnvironment, Context);
            PatientStatementEncounterRepository = new(appEnvironment, Context);
            PatientStatementRepository = new(appEnvironment, Context);
            PhyRepository = new(appEnvironment, Context);
            RevenueCodeRepository = new(appEnvironment, Context);
            UserAccountRepository = new(appEnvironment, Context);
            UserProfileRepository = new(appEnvironment, Context);
            WriteOffCodeRepository = new(appEnvironment, Context);
            CdmDetailRepository = new(appEnvironment, Context);
            CdmRepository = new(appEnvironment, Context);

            _useDispose = true;
            if (!useTransaction) return;

            _transaction = new Transaction(Context);
        }

        private static IDatabase Initialize(string connectionString)
        {
            return DatabaseConfiguration
                .Build()
                .UsingConnectionString(connectionString)
                .UsingProvider<CustomSqlMsDatabaseProvider>(new CustomSqlMsDatabaseProvider())
                .UsingCommandTimeout(180)
                .WithAutoSelect()
                .UsingDefaultMapper<MyMapper>(new MyMapper())
                .Create();
        }

        public void Commit()
        {
            if (_transaction == null) return;

            _transaction.Complete();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null) return;

            _transaction.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            var doThrowTransactionException = false;

            if (_transaction != null)
            {
                Context.AbortTransaction();
                _transaction.Dispose();
                doThrowTransactionException = true;
            }

            if (_useDispose && Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            GC.SuppressFinalize(this);
            if (doThrowTransactionException)
                throw new DataException("Transaction was aborted");
        }




    }
}