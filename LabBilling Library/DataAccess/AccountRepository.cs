using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Log = LabBilling.Logging.Log;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountRepository : RepositoryBase<Account>, IRepositoryBase<Account>, IAccountRepository
    {
        private readonly PatRepository patRepository;
        private readonly InsRepository insRepository;
        private readonly ChrgRepository chrgRepository;
        private readonly ChkRepository chkRepository;
        private readonly ClientRepository clientRepository;
        private readonly ClientDiscountRepository clientDiscountRepository;
        private readonly AccountNoteRepository accountNoteRepository;
        private readonly BillingActivityRepository billingActivityRepository;
        private readonly AccountValidationRuleRepository accountValidationRuleRepository;
        private readonly AccountValidationCriteriaRepository accountValidationCriteriaRepository;
        private readonly AccountValidationStatusRepository accountValidationStatusRepository;
        private readonly LMRPRuleRepository lmrpRuleRepository;
        private readonly FinRepository finRepository;
        private readonly SystemParametersRepository systemParametersRepository;
        private readonly AccountLmrpErrorRepository accountLmrpErrorRepository;
        private readonly CdmRepository cdmRepository;
        private readonly GlobalBillingCdmRepository globalBillingCdmRepository;

        private const string invoicedCdm = "CBILL";
        private const string cbillStatus = "CBILL";
        private const string capStatus = "CAP";
        private const string naStatus = "N/A";
        private const string newChrgStatus = "NEW";

        public AccountRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            patRepository = new PatRepository(appEnvironment);
            insRepository = new InsRepository(appEnvironment);
            chrgRepository = new ChrgRepository(appEnvironment);
            chkRepository = new ChkRepository(appEnvironment);
            clientRepository = new ClientRepository(appEnvironment);
            clientDiscountRepository = new ClientDiscountRepository(appEnvironment);
            accountNoteRepository = new AccountNoteRepository(appEnvironment);
            billingActivityRepository = new BillingActivityRepository(appEnvironment);
            accountValidationRuleRepository = new AccountValidationRuleRepository(appEnvironment);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(appEnvironment);
            accountValidationStatusRepository = new AccountValidationStatusRepository(appEnvironment);
            accountLmrpErrorRepository = new AccountLmrpErrorRepository(appEnvironment);
            lmrpRuleRepository = new LMRPRuleRepository(appEnvironment);
            finRepository = new FinRepository(appEnvironment);
            systemParametersRepository = new SystemParametersRepository(appEnvironment);
            cdmRepository = new CdmRepository(appEnvironment);
            globalBillingCdmRepository = new GlobalBillingCdmRepository(appEnvironment);
        }

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Logging.Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            var record = dbConnection.SingleOrDefault<Account>($"where {this.GetRealColumn(nameof(Account.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });


            if (record == null)
                return null;

            if (!string.IsNullOrEmpty(record.ClientMnem))
            {
                if (record.ClientMnem != "K")
                {
                    record.Client = clientRepository.GetClient(record.ClientMnem);
                    if (record.Client != null)
                        record.ClientName = record.Client.Name;
                }
            }
            record.Pat = patRepository.GetByAccount(record);

            if (!demographicsOnly)
            {
                record.Charges = chrgRepository.GetByAccount(account, true, true, null, false);
                record.Payments = chkRepository.GetByAccount(account);
                record.Insurances = insRepository.GetByAccount(account);
                record.Notes = accountNoteRepository.GetByAccount(account);
                record.BillingActivities = billingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = accountValidationStatusRepository.GetByAccount(account);
                record.Fin = finRepository.GetFin(record.FinCode);
                record.AccountAlert = dbConnection.SingleOrDefault<AccountAlert>($"where {this.GetRealColumn(nameof(AccountAlert.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

                DateTime outpBillStartDate;
                DateTime questStartDate = new DateTime(2012, 10, 1);
                DateTime questEndDate = new DateTime(2020, 5, 31);
                DateTime arbitraryEndDate = new DateTime(2016, 12, 31);

                outpBillStartDate = _appEnvironment.ApplicationParameters.OutpatientBillStart;

                if (outpBillStartDate == DateTime.MinValue)
                {
                    //set default date
                    outpBillStartDate = new DateTime(2012, 4, 1);
                }

                if (record.Client != null)
                {
                    if (record.Client.OutpatientBilling)
                    {
                        record.BillingType = "REF LAB";

                        if (record.TransactionDate.IsBetween(outpBillStartDate, arbitraryEndDate)
                            && record.TransactionDate.IsBetween(questStartDate, questEndDate))
                        {
                            if (record.PrimaryInsuranceCode.In("BC", "UMR", "GOLDEN1", "HP", "AETNA", "AG", "HUM", "SECP", "BCA"))
                                record.BillForm = "UBOP";
                            else if (record.FinCode == "D")
                                record.BillForm = "QUEST";
                            else
                                record.BillForm = "UNDEFINED";
                        }
                    }
                    else
                    {
                        if (record.InsurancePrimary != null)
                        {
                            record.BillingType = "REF LAB";
                            record.BillForm = record.InsurancePrimary.InsCompany.BillForm;
                        }
                        else
                        {
                            record.BillForm = "UNDEFINED";
                        }
                    }
                }
            }

            record.TotalBadDebt = record.Payments.Where(x => x.IsCollectionPmt).Sum(x => x.WriteOffAmount);

            record.BillableCharges = record.Charges.Where(x => x.Status != cbillStatus && x.Status != capStatus && x.Status != naStatus).ToList();

            if (record.FinCode == "CLIENT")
            {
                record.TotalCharges = record.Charges.Where(x => x.Status != cbillStatus).Sum(x => x.Quantity * x.NetAmount);
            }
            else
            {
                record.TotalCharges = record.Charges.Where(x => x.Status != cbillStatus && x.Status != capStatus && x.Status != naStatus)
                    .Sum(x => x.Quantity * x.NetAmount);
            }

            record.TotalWriteOff = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.WriteOffAmount);
            record.TotalPayments = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.PaidAmount);
            record.TotalContractual = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.ContractualAmount);

            record.ClaimBalance = record.BillableCharges.Where(x => x.FinancialType == "M").Sum(x => x.Quantity * x.NetAmount)
                - (record.TotalPayments + record.TotalWriteOff + record.TotalContractual);

            var results = record.BillableCharges.Where(x => x.FinancialType == "C")
                .GroupBy(x => x.ClientMnem, (client, balance) => new { Client = client, Balance = balance.Sum(c => c.Quantity * c.NetAmount) });

            record.ClientBalance = new List<(string client, double balance)>();

            foreach (var result in results)
            {
                record.ClientBalance.Add((result.Client, result.Balance));
            }

            record.Balance = record.TotalCharges - (record.TotalPayments + record.TotalContractual + record.TotalWriteOff);

            return record;
        }

        public override object Add(Account table)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");

            if (table.FinCode != "CLIENT")
                table.PatFullName = table.PatNameDisplay;

            table.Status = AccountStatus.New;

            table.TransactionDate = table.TransactionDate.Date;

            base.Add(table);

            //make sure Pat record has an account number
            if (table.Pat.AccountNo != table.AccountNo)
                table.Pat.AccountNo = table.AccountNo;

            var pat = patRepository.GetByAccount(table);
            if (pat == null)
                patRepository.Add(table.Pat);
            else
                patRepository.Update(table.Pat);

            foreach (Ins ins in table.Insurances)
            {
                if (ins.Account != table.AccountNo)
                    ins.Account = table.AccountNo;

                insRepository.Save(ins);
            }
            return table;
        }

        public void AddAccount(Account acc)
        {
            Log.Instance.Trace($"Entering - account {acc.AccountNo}");
            if (string.IsNullOrEmpty(acc.Status))
                acc.Status = AccountStatus.New;

            acc.PatFullName = acc.PatNameDisplay;

            this.Add(acc);
        }

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");

            return dbConnection.Fetch<InvoiceSelect>($"where {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.ClientMnem))} = @0 " +
                $"and {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.TransactionDate))} <= @1",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });
        }

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType, int maxClaims = 0)
        {
            Log.Instance.Trace($"Entering - claimType {claimType}");

            PetaPoco.Sql command;

            string selMaxRecords = string.Empty;

            if (maxClaims > 0)
            {
                selMaxRecords = $"TOP {maxClaims} ";
            }

            try
            {
                switch (claimType)
                {
                    case ClaimType.Institutional:
                        command = PetaPoco.Sql.Builder
                            .Select($"{selMaxRecords}status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'A'")
                            .Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Institutional })
                            .OrderBy($"{GetRealColumn(nameof(Account.TransactionDate))}");
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select($"{selMaxRecords}status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'A'")
                            .Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Professional })
                            .OrderBy($"{GetRealColumn(nameof(Account.TransactionDate))}");
                        break;
                    default:
                        command = PetaPoco.Sql.Builder;
                        break;
                }

                var queryResult = dbConnection.Fetch<ClaimItem>(command);

                return queryResult;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }
            Log.Instance.Trace("Exiting");

            return new List<ClaimItem>();
        }

        public enum ClaimType
        {
            Institutional,
            Professional
        }

        public override bool Update(Account table)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");

            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (string.IsNullOrEmpty(table.Status))
                table.Status = AccountStatus.New;
            table.PatFullName = table.PatNameDisplay;

            Log.Instance.Trace("Exiting");
            return base.Update(table);
        }

        public override bool Update(Account table, IEnumerable<string> columns)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");
            //generate full name field from name parts
            if (string.IsNullOrEmpty(table.Status))
                table.Status = AccountStatus.New;
            table.PatFullName = table.PatNameDisplay;

            Log.Instance.Trace($"Exiting");
            return base.Update(table, columns);
        }

        public bool SetNoteAlert(string account, bool showAlert)
        {
            Log.Instance.Trace($"Entering - account {account} showAlert {showAlert}");

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            try
            {
                var record = dbConnection.SingleOrDefault<AccountAlert>($"where {GetRealColumn(nameof(AccountAlert.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

                if (record == null)
                {
                    record = new AccountAlert();
                    record.AccountNo = account;
                    record.Alert = showAlert;

                    dbConnection.Insert(record);
                }
                else
                {
                    record.Alert = showAlert;
                    dbConnection.Update(record);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error updating NoteAlert.");
                return false;
            }

            return true;
        }

        public bool UpdateDiagnoses(Account acc)
        {
            Log.Instance.Trace("Entering");

            if (acc == null)
                throw new ArgumentNullException(nameof(acc));

            if (patRepository.SaveDiagnoses(acc.Pat))
            {
                acc.Pat = patRepository.GetByAccount(acc);
                return true;
            }
            return false;
        }

        public int UpdateStatus(string accountNo, string status)
        {
            Log.Instance.Trace($"Entering - account {accountNo} status {status}");

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));
            if (string.IsNullOrEmpty(status))
                throw new ArgumentNullException(nameof(status));
            if (!AccountStatus.IsValid(status))
                throw new ArgumentOutOfRangeException("Invalid status", nameof(status));

            return dbConnection.Update<Account>($"set status = @0, mod_date = @1, mod_user = @2, mod_prg = @3, mod_host = @4 where account = @5",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = status },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName.ToString() },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = System.AppDomain.CurrentDomain.FriendlyName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });
        }

        public bool InsuranceSwap(string accountNo, InsCoverage swap1, InsCoverage swap2)
        {
            Log.Instance.Trace($"Entering - Account {accountNo} Ins1 {swap1} Ins2 {swap2}");

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));

            BeginTransaction();

            Account account = GetByAccount(accountNo);

            if (account == null)
            {
                AbortTransaction();
                return false;
            }

            Ins insA = account.Insurances.Where(x => x.Coverage == swap1).FirstOrDefault();
            Ins insB = account.Insurances.Where(x => x.Coverage == swap2).FirstOrDefault();

            if (insA == null || insB == null)
            {
                AbortTransaction();
                return false;
            }
            try
            {

                insA.Coverage = InsCoverage.Temporary;
                insB.Coverage = swap1;

                insRepository.Update(insA);
                insRepository.Update(insB);

                insA.Coverage = swap2;
                insRepository.Update(insA);

                AddNote(accountNo, $"Insurance swap: {swap1} {insA.PlanName} and {swap2} {insB.PlanName}");
                CompleteTransaction();
                return false;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error swapping insurances.");
                AbortTransaction();
                return false;
            }

        }

        public bool ChangeDateOfService(ref Account table, DateTime newDate, string reason_comment)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new date {newDate} reason {reason_comment}");

            if (table == null)
                throw new ArgumentNullException(nameof(table));
            if (newDate == null)
                throw new ArgumentNullException(nameof(newDate));
            if (reason_comment == null)
                throw new ArgumentNullException(nameof(reason_comment));

            bool updateSuccess = false;
            DateTime oldServiceDate = DateTime.MinValue;

            // update trans_date on acc table
            if (table.TransactionDate != newDate)
            {
                oldServiceDate = (DateTime)table.TransactionDate;
                table.TransactionDate = newDate;
                Update(table, new[] { nameof(Account.TransactionDate) });

                if (AddNote(table.AccountNo, $"Service Date changed from {oldServiceDate} to {newDate}"))
                {
                    table.Notes = accountNoteRepository.GetByAccount(table.AccountNo);
                }

                //determine if charges need to be reprocessed.

                //TODO: is there any reason a date of service change should result in changing all charges --
                // except: the date of service on charges will not match new date.

                // option: reprocess all charges, or update service date on charge records
                foreach (var chrg in table.Charges)
                {
                    chrg.ServiceDate = newDate;
                    chrgRepository.Update(chrg);
                }
            }
            else
            {
                // error - current date is same as new date
                updateSuccess = false;
            }

            Log.Instance.Trace($"Exiting - return value {updateSuccess}");
            //return true if successful - false if error
            return updateSuccess;

        }

        public bool AddNote(string account, string noteText)
        {
            Log.Instance.Trace($"Entering - account {account} note {noteText}");
            bool addSuccess = true;

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            if (string.IsNullOrEmpty(noteText))
            {
                //there is no note to add
                return false;
            }

            AccountNote accountNote = new AccountNote()
            {
                Account = account,
                Comment = noteText
            };
            try
            {
                accountNoteRepository.Add(accountNote);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error adding account note.");
                throw new ApplicationException("Error adding account note.", ex);
            }

            Log.Instance.Trace($"Exiting");
            return addSuccess;
        }

        public bool ChangeFinancialClass(string account, string newFinCode)
        {
            Log.Instance.Trace($"Entering - Account {account} New Fin {newFinCode}");

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));
            if (string.IsNullOrEmpty(newFinCode))
                throw new ArgumentNullException(nameof(newFinCode));

            var record = GetByAccount(account);

            if (record != null)
                return ChangeFinancialClass(ref record, newFinCode);
            else
                return false;
        }

        public bool ChangeFinancialClass(ref Account table, string newFinCode)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new fin {newFinCode}");
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            else if (newFinCode == null)
                throw new ArgumentNullException(nameof(newFinCode));

            bool updateSuccess = true;
            string oldFinCode = table.FinCode;

            //check that newFincode is a valid fincode
            FinRepository finRepository = new FinRepository(_appEnvironment);

            Fin newFin = finRepository.GetFin(newFinCode);
            Fin oldFin = finRepository.GetFin(oldFinCode);

            if (newFin == null)
            {
                throw new ArgumentException($"Financial code {newFinCode} is not valid code.", "newFinCode");
            }

            BeginTransaction();

            if (oldFinCode != newFinCode)
            {
                table.FinCode = newFinCode;
                try
                {
                    Update(table, new[] { nameof(Account.FinCode) });
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    throw new ApplicationException($"Exception updating fin code for {table.AccountNo}.", ex);
                }
                AddNote(table.AccountNo, $"Financial code updated from {oldFinCode} to {newFinCode}.");

                //reprocess charges if needed due to financial code change.
                if (newFin.FinClass != oldFin.FinClass)
                {
                    try
                    {
                        ReprocessCharges(table, $"Fin Code changed from {oldFinCode} to {newFinCode}");
                    }
                    catch (Exception ex)
                    {
                        AbortTransaction();
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }
                else
                {
                    UpdateChargesFinCode(table.AccountNo, newFinCode);
                }
            }
            else
            {
                updateSuccess = false;
            }

            CompleteTransaction();
            Log.Instance.Trace($"Exiting");
            return updateSuccess;
        }

        public bool ChangeClient(ref Account table, string newClientMnem)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new client {newClientMnem}");
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            else if (newClientMnem == null)
                throw new ArgumentNullException(nameof(newClientMnem));

            bool updateSuccess = true;
            string oldClientMnem = table.ClientMnem;

            try
            {
                ClientRepository clientRepository = new ClientRepository(_appEnvironment);
                Client oldClient = clientRepository.GetClient(oldClientMnem);
                Client newClient = clientRepository.GetClient(newClientMnem);

                dbConnection.BeginTransaction();

                if (newClient == null)
                    throw new ArgumentException($"Client mnem {newClientMnem} is not valid.", "newClientMnem");

                if (oldClientMnem != newClientMnem)
                {

                    table.ClientMnem = newClientMnem;
                    table.Client = newClient;
                    table.ClientName = newClient.Name;
                    try
                    {
                        Update(table, new[] { nameof(Account.ClientMnem) });
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Error(ex);
                        throw new ApplicationException($"Exception updating client for {table.AccountNo}.", ex);
                    }
                    AddNote(table.AccountNo, $"Client updated from {oldClientMnem} to {newClientMnem}.");

                    //reprocess charges if fin class is client bill (C) to pick up proper discounts.
                    if (table.Fin.FinClass == "C")
                    {
                        try
                        {
                            ReprocessCharges(table.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("Error reprocessing charges.", ex);
                        }
                    }

                    if (table.Fin.FinClass == "M")
                    {
                        //reprocess charges if fee schedule is different to pick up correct charge amounts
                        if (oldClient.FeeSchedule != newClient.FeeSchedule)
                        {
                            try
                            {
                                ReprocessCharges(table.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException("Error reprocessing charges.", ex);
                            }
                        }
                        else if (oldClient.ClientMnem == "HC" || newClient.ClientMnem == "HC")
                        {
                            try
                            {
                                ReprocessCharges(table.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException("Error reprocessing charges.", ex);
                            }
                        }
                        else
                        {
                            try
                            {
                                UpdateChargesClient(table.AccountNo, newClientMnem);
                            }
                            catch (Exception ex)
                            {
                                throw new ApplicationException("Error updating charge client.", ex);
                            }
                        }
                    }

                    CompleteTransaction();
                }
                else
                {
                    // old client is same as new client - no change
                    updateSuccess = false;
                    CompleteTransaction();
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error during Change Client", ex);
                AbortTransaction();
                updateSuccess = false;
            }
            Log.Instance.Trace($"Exiting");
            return updateSuccess;
        }

        private class ChargeInfo
        {
            public string CdmCode { get; set; }
            public string FinCode { get; set; }
            public string ClientMnem { get; set; }
            public decimal NetAmount { get; set; }
            public decimal Quantity { get; set; }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int ReprocessCharges(Account account, string comment)
        {
            Log.Instance.Trace($"Entering {account}");

            if (account == null)
                throw new ArgumentNullException(nameof(account));

            BeginTransaction();
            try
            {
                var chargesToCredit = account.Charges.Where(x => x.IsCredited == false).ToList();

                //foreach (var chrg in account.Charges.Where(x => x.IsCredited == false))
                foreach(var chrg in chargesToCredit)
                {
                    if (chrg.CDMCode == invoicedCdm)  //do not reprocess CBILL charge records
                        continue;

                    chrgRepository.CreditCharge(chrg.ChrgId, comment);

                    //insert new charge and detail
                    if (chrg.CDMCode != invoicedCdm)
                        AddCharge(account, chrg.CDMCode, chrg.Quantity, account.TransactionDate);
                }

                CompleteTransaction();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                AbortTransaction();
                throw new ApplicationException("Error reprocessing charges", ex);
            }
            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int ReprocessCharges(string account, string comment)
        {
            Log.Instance.Trace($"Entering {account}");

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            var acc = GetByAccount(account);

            if (acc == null)
            {
                Log.Instance.Error($"Account {account} is not a valid account.");
                throw new AccountNotFoundException($"Account is not a valid account.", account);
            }

            return ReprocessCharges(acc, comment);
        }

        public bool UpdateChargesFinCode(string account, string finCode)
        {
            Log.Instance.Trace("Entering");

            List<Chrg> charges = chrgRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
            {
                return false;
            }

            var fin = finRepository.GetFin(finCode);

            if (fin == null)
            {
                return false;
            }

            var chrgsToUpdate = charges.Where(x => x.IsCredited == false &&
                (x.ClientMnem != _appEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
                string.IsNullOrEmpty(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem))
                && x.FinancialType == fin.FinClass).ToList();

            foreach (var chrg in chrgsToUpdate)
            {
                chrg.FinCode = finCode;
                chrg.FinancialType = fin.FinClass;
                chrgRepository.Update(chrg, new[] { nameof(Chrg.FinancialType), nameof(Chrg.FinCode) });
            }

            return true;
        }

        public bool UpdateChargesClient(string account, string clientMnem)
        {
            Log.Instance.Trace("Entering");

            List<Chrg> charges = chrgRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
                return false;

            var client = clientRepository.GetClient(clientMnem);
            if (client == null)
                return false;

            //var chrgsToUpdate = charges.Where(x => x.IsCredited == false).ToList();

            foreach (var chrg in charges)
            {
                chrg.ClientMnem = clientMnem;
                chrgRepository.Update(chrg, new[] { nameof(Chrg.ClientMnem) });
            }

            return true;
        }

        public int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
        {
            Log.Instance.Trace($"Entering - account {account} cdm {cdm}");

            //verify the account exists
            Account accData = GetByAccount(account);
            if (accData == null)
            {
                Log.Instance.Error($"Account {account} not found");
                throw new AccountNotFoundException("Account is not a valid account.", account);
            }

            return AddCharge(accData, cdm, qty, serviceDate, comment, refNumber);
        }

        /// <summary>
        /// Add a charge to an account. The account must exist.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="comment"></param>
        /// <returns>Charge number of newly entered charge or < 0 if an error occurs.</returns>
        public int AddCharge(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
        {
            Log.Instance.Trace($"Entering - account {accData.AccountNo} cdm {cdm}");

            if (accData.Client == null)
            {
                throw new InvalidClientException("Client not valid", accData.ClientMnem);
            }

            //check account status, change to NEW if it is paid out.
            if (accData.Status == AccountStatus.PaidOut)
            {
                UpdateStatus(accData.AccountNo, AccountStatus.New);
                accData.Status = AccountStatus.New;
            }

            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = cdmRepository.GetCdm(cdm);
            if (cdmData == null)
            {
                Log.Instance.Error($"CDM {cdm} not found.");
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = finRepository.GetFin(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");
            Client chargeClient = accData.Client;

            if (_appEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
            {
                //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
                var gb = globalBillingCdmRepository.GetCdm(cdm);
                //hard coding exception for Hardin County for now - 05/09/2023 BSP
                if (gb != null && accData.ClientMnem != "HC")
                {
                    fin = finRepository.GetFin("Y") ?? throw new ApplicationException($"Fin code Y not found error {accData.AccountNo}");
                    chargeClient = clientRepository.GetClient(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem);
                }
            }

            Chrg chrg = new Chrg();

            switch (fin.FinClass)
            {
                case "M":
                    chrg.Status = cdmData.MClassType == naStatus ? naStatus : newChrgStatus;
                    break;
                case "C":
                    chrg.Status = cdmData.CClassType == naStatus ? naStatus : newChrgStatus;
                    break;
                case "Z":
                    chrg.Status = cdmData.ZClassType == naStatus ? naStatus : newChrgStatus;
                    break;
                default:
                    chrg.Status = newChrgStatus;
                    break;
            }

            //now build the charge & detail records
            chrg.AccountNo = accData.AccountNo;
            chrg.Action = "";
            chrg.BillMethod = fin.ClaimType;
            chrg.CDMCode = cdm;
            chrg.Comment = comment;
            chrg.IsCredited = false;
            chrg.Facility = "";
            chrg.FinCode = fin.FinCode;
            chrg.ClientMnem = chargeClient.ClientMnem;
            chrg.FinancialType = fin.FinClass;
            chrg.OrderMnem = cdmData.Mnem;
            chrg.LISReqNo = refNumber;
            chrg.OrderingSite = "";
            chrg.PerformingSite = "";
            chrg.PostingDate = DateTime.Today;
            chrg.Quantity = qty;
            chrg.ServiceDate = serviceDate;
            chrg.ResponsibleProvider = "";

            List<CdmDetail> feeSched = null;

            switch (chargeClient.FeeSchedule)
            {
                case "1":
                    feeSched = cdmData.CdmFeeSchedule1;
                    break;
                case "2":
                    feeSched = cdmData.CdmFeeSchedule2;
                    break;
                case "3":
                    feeSched = cdmData.CdmFeeSchedule3;
                    break;
                case "4":
                    feeSched = cdmData.CdmFeeSchedule4;
                    break;
                case "5":
                    feeSched = cdmData.CdmFeeSchedule5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("FeeSchedule");
                    //break;
            }


            //need to determine the correct fee schedule - for now default to 1
            double ztotal = 0.0;
            double amtTotal = 0.0;
            double retailTotal = 0.0;

            foreach (CdmDetail fee in feeSched)
            {
                ChrgDetail chrgDetail = new ChrgDetail
                {
                    DiagnosisPointer = new ChrgDiagnosisPointer(),
                    Cpt4 = fee.Cpt4,
                    Type = fee.Type
                };
                switch (fin.FinClass)
                {
                    case "M":
                        chrgDetail.Amount = fee.MClassPrice;
                        retailTotal += fee.MClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                    case "C":
                        //todo: calculate client discount
                        var cliDiscount = chargeClient.Discounts.Find(c => c.Cdm == cdm);
                        double discountPercentage = chargeClient.DefaultDiscount;
                        if (cliDiscount != null)
                        {
                            discountPercentage = cliDiscount.PercentDiscount;
                        }
                        chrgDetail.Amount = fee.CClassPrice - (fee.CClassPrice * (discountPercentage / 100));
                        retailTotal += fee.CClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                    case "Z":
                        chrgDetail.Amount = fee.ZClassPrice;
                        retailTotal += fee.ZClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                    default:
                        chrgDetail.Amount = fee.MClassPrice;
                        retailTotal += fee.MClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                }

                amtTotal += chrgDetail.Amount;

                chrgDetail.Modifier = fee.Modifier;
                chrgDetail.RevenueCode = fee.RevenueCode;
                chrgDetail.LISReqNo = "";
                chrgDetail.OrderCode = fee.BillCode;
                chrgDetail.BillType = "";
                chrgDetail.BillMethod = "";
                chrgDetail.DiagnosisPointer.DiagnosisPointer = "1:";

                chrg.ChrgDetails.Add(chrgDetail);
            }

            chrg.NetAmount = amtTotal;
            chrg.HospAmount = ztotal;
            chrg.RetailAmount = retailTotal;

            Log.Instance.Trace($"Exiting");
            return chrgRepository.AddCharge(chrg);
        }

        private class BundledProfile
        {
            public string ProfileCdm { get; set; }

            public List<BundledProfileComponent> ComponentCpt { get; set; }

            public bool BundleQualfies
            {
                get
                {
                    return !ComponentCpt.Any(x => x.IsPresent == false);
                }
            }
        }

        private class BundledProfileComponent
        {
            public string Cdm { get; set; }
            public string Cpt { get; set; }
            public bool IsPresent { get; set; }
            public int ChrgId { get; set; }

            public BundledProfileComponent()
            {

            }

            public BundledProfileComponent(string cpt, bool isPresent = false)
            {
                Cpt = cpt;
                IsPresent = isPresent;
            }

            public BundledProfileComponent(string cpt, string cdm, bool isPresent = false)
            {
                Cpt = cpt;
                Cdm = cdm;
                IsPresent = isPresent;
            }
        }

        public void BundlePanels(string accountNo)
        {

        }

        public void UnbundlePanels(Account account)
        {
            var bundledProfiles = account.Charges.Where(x => x.CDMCode == "MCL0029" && x.IsCredited == false);
            try
            {
                BeginTransaction();

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    chrgRepository.CreditCharge(bundledProfile.ChrgId, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", 1, account.TransactionDate);
                    AddCharge(account, "5382522", 1, account.TransactionDate);
                    AddCharge(account, "5646008", 1, account.TransactionDate);
                }

                bundledProfiles = account.Charges.Where(x => x.CDMCode == "MCL0021" && x.IsCredited == false);

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    chrgRepository.CreditCharge(bundledProfile.ChrgId, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", 1, account.TransactionDate);
                    AddCharge(account, "5646012", 1, account.TransactionDate);
                    AddCharge(account, "5646086", 1, account.TransactionDate);
                    AddCharge(account, "5646054", 1, account.TransactionDate);
                    AddCharge(account, "5728026", 1, account.TransactionDate);
                    AddCharge(account, "5728190", 1, account.TransactionDate);
                }

                CompleteTransaction();
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AbortTransaction();
            }

        }

        public void BundlePanels(Account account)
        {
            List<BundledProfile> bundledProfiles = new List<BundledProfile>();
            bundledProfiles.Add(new BundledProfile()
            {
                ProfileCdm = "MCL0029",
                ComponentCpt = new List<BundledProfileComponent>()
                {
                    new BundledProfileComponent("85025", "5545154"),
                    new BundledProfileComponent("80053", "5382522"),
                    new BundledProfileComponent("84443", "5646008")
                }
            });

            bundledProfiles.Add(new BundledProfile()
            {
                ProfileCdm = "MCL0021",
                ComponentCpt = new List<BundledProfileComponent>()
                {
                    new BundledProfileComponent("85025", "5545154"),
                    new BundledProfileComponent("87340", "5646012"),
                    new BundledProfileComponent("86762", "5646086"),
                    new BundledProfileComponent("86592", "5686054"),
                    new BundledProfileComponent("86850", "5728026"),
                    new BundledProfileComponent("86900", "5728190"),
                    new BundledProfileComponent("86901")
                }
            });

            for (int x = 0; x < bundledProfiles.Count; x++)
            {
                for (int i = 0; i < bundledProfiles[x].ComponentCpt.Count; i++)
                {
                    foreach (var chrg in account.Charges.Where(c => !c.IsCredited))
                    {
                        if (chrg.ChrgDetails.Any(d => d.Cpt4 == bundledProfiles[x].ComponentCpt[i].Cpt))
                        {
                            bundledProfiles[x].ComponentCpt[i].IsPresent = true;
                            bundledProfiles[x].ComponentCpt[i].ChrgId = chrg.ChrgId;
                        }
                    }
                }

                if (bundledProfiles[x].BundleQualfies)
                {
                    //credit components and charge profile cdm

                    for (int i = 0; i < bundledProfiles[x].ComponentCpt.Count; i++)
                    {
                        chrgRepository.CreditCharge(bundledProfiles[x].ComponentCpt[i].ChrgId, $"Bundling to {bundledProfiles[x].ProfileCdm}");
                    }

                    this.AddCharge(account, bundledProfiles[x].ProfileCdm, 1, (DateTime)account.TransactionDate, $"Bundled by Rule");
                    this.AddNote(account.AccountNo, $"Bundled charges into {bundledProfiles[x].ProfileCdm}");
                }
            }
        }

        /// <summary>
        /// Runs all validation routines on account. Updates validation status and account flags. Errors are stored in the validation status table.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="reprint">Set true if validating account to resubmit the claim with no changes.</param>
        /// <returns>True if account is valid for billing, false if there are validation errors.</returns>
        public bool Validate(ref Account account, bool reprint = false)
        {
            Log.Instance.Trace($"Entering - account {account}");

            try
            {
                BeginTransaction();
                if ((account.Status == "SSIUB" || account.Status == "SSI1500" || account.Status == "CLAIM" || account.Status == "STMT"
                    || account.Status == "CLOSED" || account.Status == "PAID_OUT") && !reprint)
                {
                    //account has been billed, do not validate
                    account.AccountValidationStatus.account = account.AccountNo;
                    account.AccountValidationStatus.mod_date = DateTime.Now;
                    account.AccountValidationStatus.validation_text = "Account has already been billed. Did not validate.";
                    accountValidationStatusRepository.Save(account.AccountValidationStatus);
                    return false;
                }
                else
                {
                    if (account.InsurancePrimary != null)
                    {
                        if (account.InsurancePrimary.InsCompany != null)
                        {
                            if(account.InsurancePrimary.InsCompany.IsMedicareHmo)
                                UnbundlePanels(account);
                            if (!account.InsurancePrimary.InsCompany.IsMedicareHmo)
                                BundlePanels(account);
                        }
                    }

                    BusinessLogic.Validators.ClaimValidator claimValidator = new BusinessLogic.Validators.ClaimValidator();
                    account.LmrpErrors = ValidateLMRP(account);
                    var validationResult = claimValidator.Validate(account);

                    bool isAccountValid = true;

                    account.AccountValidationStatus.account = account.AccountNo;
                    account.AccountValidationStatus.mod_date = DateTime.Now;

                    string lmrperrors = null;

                    if (!validationResult.IsValid)
                    {
                        isAccountValid = false;
                        account.AccountValidationStatus.validation_text = validationResult.ToString() + Environment.NewLine;
                        //update account status back to new
                        if (!reprint)
                            UpdateStatus(account.AccountNo, AccountStatus.New);
                    }

                    //only run LMRP on A fin_code
                    if (account.LmrpErrors.Count > 0 && account.FinCode == "A")
                    {
                        isAccountValid = false;
                        foreach (var error in account.LmrpErrors)
                        {
                            account.AccountValidationStatus.validation_text += error + Environment.NewLine;
                            lmrperrors += error + "\n";
                        }
                        if (!reprint)
                            UpdateStatus(account.AccountNo, AccountStatus.New);
                    }

                    if (isAccountValid)
                    {
                        account.AccountValidationStatus.validation_text = "No validation errors.";
                        //update account status if this account has been flagged to bill
                        if (account.Status == "RTB")
                            UpdateStatus(account.AccountNo, account.BillForm);
                    }

                    accountValidationStatusRepository.Save(account.AccountValidationStatus);
                    if (!string.IsNullOrEmpty(lmrperrors))
                    {
                        AccountLmrpError record = new AccountLmrpError();
                        record.AccountNo = account.AccountNo;
                        record.DateOfService = (DateTime)account.TransactionDate;
                        record.ClientMnem = account.ClientMnem;
                        record.FinancialCode = account.FinCode;
                        record.Error = lmrperrors;

                        accountLmrpErrorRepository.Save(record);
                    }
                    CompleteTransaction();
                    return isAccountValid;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                AbortTransaction();

                account.AccountValidationStatus.account = account.AccountNo;
                account.AccountValidationStatus.mod_date = DateTime.Now;
                account.AccountValidationStatus.validation_text = "Exception during Validation. Unable to validate.";
                accountValidationStatusRepository.Save(account.AccountValidationStatus);
            }

            return false;
        }

        private List<string> ValidateLMRP(Account account)
        {
            Log.Instance.Trace($"Entering - account {account}");
            List<string> errorList = new List<string>();

            //determine if there are any rules for ama_year
            if (lmrpRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
            {
                // no lmrp rules loaded for this ama year. 
                errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
                return errorList;
            }

            foreach (var cpt4 in account.cpt4List.Distinct())
            {
                if (cpt4 == null)
                    continue;
                var ruleDef = lmrpRuleRepository.GetRuleDefinition(cpt4, (DateTime)account.TransactionDate);
                if (ruleDef == null)
                    continue;

                bool dxIsValid = ruleDef.DxIsValid == 0 ? false : true;
                bool dxSupported = false;

                foreach (var dx in account.Pat.Diagnoses)
                {
                    var rule = lmrpRuleRepository.GetRule(cpt4, dx.Code, (DateTime)account.TransactionDate);
                    if (dxIsValid && rule != null)
                        dxSupported = true;

                    if (!dxIsValid && rule == null)
                        dxSupported = true;

                }

                if (!dxSupported)
                {
                    errorList.Add($"LMRP Violation - No dx codes support medical necessity for cpt {cpt4}.");
                }
            }
            return errorList;
        }

        public void ValidateUnbilledAccounts()
        {
            Log.Instance.Trace($"Entering");

            //DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);

            DateTime thruDate = _appEnvironment.ApplicationParameters.SSIBillThruDate;

            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "Y"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "E")
            };

            AccountSearchRepository accountSearchRepository = new AccountSearchRepository(_appEnvironment);

            var accounts = accountSearchRepository.GetBySearch(parameters).ToList();

            foreach (var account in accounts)
            {
                try
                {
                    var accountRecord = this.GetByAccount(account.Account);
                    this.Validate(ref accountRecord);
                }
                catch (Exception e)
                {
                    Log.Instance.Error(e, "Error during account validation job.");
                }
            }
        }

        public async Task ValidateUnbilledAccountsAsync()
        {
            Log.Instance.Trace($"Entering");

            DateTime thruDate = _appEnvironment.ApplicationParameters.SSIBillThruDate;

            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")
            };

            AccountSearchRepository accountSearchRepository = new AccountSearchRepository(_appEnvironment);

            var accounts = await Task.Run(() => accountSearchRepository.GetBySearch(parameters).ToList());

            foreach (var account in accounts)
            {
                try
                {
                    var accountRecord = this.GetByAccount(account.Account);
                    this.Validate(ref accountRecord);
                }
                catch (Exception e)
                {
                    Log.Instance.Error(e, "Error during account validation job.");
                }
            }

        }

        /// <summary>
        /// Move all charges between accounts. Credits the charges from sourceAccount and charges them on destinationAccount.
        /// SourceAccount will have a zero total charge amount.
        /// </summary>
        /// <param name="sourceAccount"></param>
        /// <param name="destinationAccount"></param>
        public (bool isSuccess, string error) MoveCharges(string sourceAccount, string destinationAccount)
        {
            if (string.IsNullOrEmpty(sourceAccount) || string.IsNullOrEmpty(destinationAccount))
            {
                throw new ArgumentException("One or both arguments are null or empty.");
            }

            var source = GetByAccount(sourceAccount);
            var destination = GetByAccount(destinationAccount);
            if (source == null || destination == null)
            {
                Log.Instance.Error($"Either source or destination account was not valid. {sourceAccount} {destinationAccount}");
                return (false, "Either source or destination account was not valid.");
            }

            var charges = source.Charges.Where(c => c.IsCredited == false);

            foreach (var charge in charges)
            {
                chrgRepository.CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
                AddCharge(destination, charge.CDMCode, charge.Quantity, (DateTime)destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
            }
            return (true, string.Empty);
        }

        /// <summary>
        /// Moves a single charge from sourceAccount to destinationAccount
        /// </summary>
        /// <param name="sourceAccount"></param>
        /// <param name="destinationAccount"></param>
        /// <param name="chrgId"></param>
        public void MoveCharge(string sourceAccount, string destinationAccount, int chrgId)
        {
            if (string.IsNullOrEmpty(sourceAccount) || string.IsNullOrEmpty(destinationAccount))
            {
                throw new ArgumentException("One or both arguments are null or empty.");
            }

            var source = GetByAccount(sourceAccount);
            var destination = GetByAccount(destinationAccount);

            var charge = source.Charges.SingleOrDefault(c => c.ChrgId == chrgId);

            if (charge.IsCredited)
            {
                throw new ApplicationException("Charge is already credited.");
            }

            chrgRepository.CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
            AddCharge(destinationAccount, charge.CDMCode, charge.Quantity, destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);

        }

        /// <summary>
        /// Clears all claim flags so account will be picked up in next claim batch
        /// </summary>
        /// <param name="account"></param>
        public void ClearClaimStatus(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            List<string> columns = new List<string>();
            try
            {
                //clear pat 1500 & ub date flags
                account.Pat.ProfessionalClaimDate = null;
                columns.Add(nameof(Pat.ProfessionalClaimDate));
                account.Pat.InstitutionalClaimDate = null;
                columns.Add(nameof(Pat.InstitutionalClaimDate));
                account.Pat.EBillBatchDate = null;
                columns.Add(nameof(Pat.EBillBatchDate));
                account.Pat.SSIBatch = null;
                columns.Add(nameof(Pat.SSIBatch));
                //set account status to "NEW"
                account.Status = AccountStatus.New;

                patRepository.Update(account.Pat, columns);
                UpdateStatus(account.AccountNo, AccountStatus.New);

                AddNote(account.AccountNo, "Claim status cleared.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
            }
        }

    }
}
