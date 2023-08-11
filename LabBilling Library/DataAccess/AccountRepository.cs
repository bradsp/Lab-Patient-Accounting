using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Log = LabBilling.Logging.Log;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using System.ServiceProcess;
using System.ComponentModel.DataAnnotations;

namespace LabBilling.Core.DataAccess
{
    public sealed class AccountRepository : RepositoryBase<Account>, IRepositoryBase<Account>
    {

        private const string invoicedCdm = "CBILL";
        private const string cbillStatus = "CBILL";
        private const string capStatus = "CAP";
        private const string naStatus = "N/A";
        private const string newChrgStatus = "NEW";

        private const string clientFinType = "C";
        private const string patientFinType = "M";
        private const string zFinType = "Z";
        private const string invalidFinCode = "K";
        private const string clientFinCode = "CLIENT";
        private const string pthExceptionClient = "HC";

        public AccountRepository(IAppEnvironment appEnvironment) : base(appEnvironment)
        {

        }

        public async Task<Account> GetByAccountAsync(string account, bool demographicsOnly = false) => await Task.Run(() => GetByAccount(account, demographicsOnly));        

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            var record = dbConnection.SingleOrDefault<Account>($"where {this.GetRealColumn(nameof(Account.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });


            if (record == null)
                return null;

            if (!string.IsNullOrEmpty(record.ClientMnem))
            {
                if (record.ClientMnem != invalidFinCode)
                {
                    record.Client = AppEnvironment.Context.ClientRepository.GetClient(record.ClientMnem);
                    if (record.Client != null)
                        record.ClientName = record.Client.Name;
                }
            }
            record.Pat = AppEnvironment.Context.PatRepository.GetByAccount(record);

            if (!demographicsOnly)
            {
                record.Charges = AppEnvironment.Context.ChrgRepository.GetByAccount(account, true, true, null, false);
                record.ChargeDetails = AppEnvironment.Context.ChrgDetailRepository.GetByAccount(account);
                record.Payments = AppEnvironment.Context.ChkRepository.GetByAccount(account);
                record.Insurances = AppEnvironment.Context.InsRepository.GetByAccount(account);
                record.Notes = AppEnvironment.Context.AccountNoteRepository.GetByAccount(account);
                record.BillingActivities = AppEnvironment.Context.BillingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = AppEnvironment.Context.AccountValidationStatusRepository.GetByAccount(account);
                record.Fin = AppEnvironment.Context.FinRepository.GetFin(record.FinCode);
                record.AccountAlert = dbConnection.SingleOrDefault<AccountAlert>($"where {this.GetRealColumn(nameof(AccountAlert.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });

                DateTime outpBillStartDate;
                DateTime questStartDate = new DateTime(2012, 10, 1);
                DateTime questEndDate = new DateTime(2020, 5, 31);
                DateTime arbitraryEndDate = new DateTime(2016, 12, 31);

                outpBillStartDate = AppEnvironment.ApplicationParameters.OutpatientBillStart;

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

                            if(string.IsNullOrEmpty(record.BillForm))
                            {
                                record.BillForm = record.Fin.ClaimType;
                            }
                        }
                        else
                        {
                            record.BillForm = record.Fin.ClaimType;
                        }
                    }
                }
            }

            record.TotalBadDebt = record.Payments.Where(x => x.IsCollectionPmt).Sum(x => x.WriteOffAmount);

            record.BillableCharges = record.ChargeDetails.Where(x => x.Type != naStatus).ToList();

            if (record.FinCode == clientFinCode)
            {
                record.TotalCharges = record.ChargeDetails.Where(x => x.Type != cbillStatus).Sum(x => x.Quantity * x.Amount);
            }
            else
            {
                record.TotalCharges = record.ChargeDetails.Where(x => x.Type != naStatus)
                    .Sum(x => x.Quantity * x.Amount);
            }

            record.TotalWriteOff = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.WriteOffAmount);
            record.TotalPayments = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.PaidAmount);
            record.TotalContractual = record.Payments.Where(x => x.Status != cbillStatus).Sum(x => x.ContractualAmount);

            record.ClaimBalance = record.BillableCharges.Where(x => x.FinancialType == patientFinType).Sum(x => x.Quantity * x.Amount)
                - (record.TotalPayments + record.TotalWriteOff + record.TotalContractual);

            var results = record.BillableCharges.Where(x => x.FinancialType == clientFinType)
                .GroupBy(x => x.ClientMnem, (client, balance) => new { Client = client, Balance = balance.Sum(c => c.Quantity * c.Amount) });

            record.ClientBalance = new List<(string client, double balance)>();

            foreach (var result in results)
            {
                record.ClientBalance.Add((result.Client, result.Balance));
            }

            record.Balance = record.TotalCharges - (record.TotalPayments + record.TotalContractual + record.TotalWriteOff);

            return record;
        }

        public async Task<object> AddAsync(Account table) => await Task.Run(() => Add(table));        

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

            var pat = AppEnvironment.Context.PatRepository.GetByAccount(table);
            if (pat == null)
                AppEnvironment.Context.PatRepository.Add(table.Pat);
            else
                AppEnvironment.Context.PatRepository.Update(table.Pat);

            foreach (Ins ins in table.Insurances)
            {
                if (ins.Account != table.AccountNo)
                    ins.Account = table.AccountNo;

                AppEnvironment.Context.InsRepository.Save(ins);
            }
            return table;
        }

        public async Task AddAccountAsync(Account acc) => await Task.Run(() => AddAccount(acc));

        public void AddAccount(Account acc)
        {
            Log.Instance.Trace($"Entering - account {acc.AccountNo}");
            if (string.IsNullOrEmpty(acc.Status))
                acc.Status = AccountStatus.New;

            acc.PatFullName = acc.PatNameDisplay;

            this.Add(acc);
        }

        public async Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate) => await Task.Run(() => GetInvoiceAccounts(clientMnem, thruDate));

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");

            return dbConnection.Fetch<InvoiceSelect>($"where {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.ClientMnem))} = @0 " +
                $"and {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.TransactionDate))} <= @1",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });
        }

        public async Task<IEnumerable<ClaimItem>> GetAccountsForClaimsAsync(ClaimType claimType, int maxClaims = 0) => await Task.Run(() => GetAccountsForClaims(claimType, maxClaims));
        

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
                            .InnerJoin("ins").On($"ins.account = acc.account and ins_a_b_c = '{InsCoverage.Primary}'")
                            .Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Institutional })
                            .OrderBy($"{GetRealColumn(nameof(Account.TransactionDate))}");
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select($"{selMaxRecords}status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On($"ins.account = acc.account and ins_a_b_c = '{InsCoverage.Primary}'")
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

        public async Task<bool> UpdateAsync(Account table) => await Task.Run(() => Update(table));
        
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

        public async Task<bool> UpdateAsync(Account table, IEnumerable<string> columns) => await Task.Run(() => Update(table, columns));

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

        public async Task<bool> SetNoteAlertAsync(string account, bool showAlert) => await Task.Run(() => SetNoteAlert(account, showAlert));

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
                    record = new AccountAlert
                    {
                        AccountNo = account,
                        Alert = showAlert
                    };

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

        public async Task<bool> UpdateDiagnosesAsync(Account acc) => await Task.Run(() => UpdateDiagnoses(acc));

        public bool UpdateDiagnoses(Account acc)
        {
            Log.Instance.Trace("Entering");

            if (acc == null)
                throw new ArgumentNullException(nameof(acc));

            if (AppEnvironment.Context.PatRepository.SaveDiagnoses(acc.Pat))
            {
                acc.Pat = AppEnvironment.Context.PatRepository.GetByAccount(acc);
                return true;
            }
            return false;
        }

        public async Task<int> UpdateStatusAsync(string accountNo, string status) => await Task.Run(() => UpdateStatus(accountNo, status));

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
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = RFClassLibrary.OS.GetAppName() },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });
        }

        public async Task<bool> InsuranceSwapAsync(string accountNo, InsCoverage swap1, InsCoverage swap2) => await Task.Run(() => InsuranceSwap(accountNo, swap1, swap2));

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

                AppEnvironment.Context.InsRepository.Update(insA);
                AppEnvironment.Context.InsRepository.Update(insB);

                insA.Coverage = swap2;
                AppEnvironment.Context.InsRepository.Update(insA);

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

        public async Task<bool> ChangeDateOfServiceAsync(Account table, DateTime newDate, string reason_comment) => await Task.Run(() => ChangeDateOfService(table, newDate, reason_comment));

        public bool ChangeDateOfService(Account table, DateTime newDate, string reason_comment)
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
                    table.Notes = AppEnvironment.Context.AccountNoteRepository.GetByAccount(table.AccountNo);
                }

                //TODO: is there any reason a date of service change should result in changing all charges --
                // except: the date of service on charges will not match new date.

                // option: reprocess all charges, or update service date on charge records
                foreach (var chrg in table.Charges)
                {
                    chrg.ServiceDate = newDate;
                    AppEnvironment.Context.ChrgRepository.Update(chrg);
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

        public async Task<bool> AddNoteAsync(string account, string noteText) => await Task.Run(() => AddNote(account, noteText));

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
                AppEnvironment.Context.AccountNoteRepository.Add(accountNote);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error adding account note.");
                throw new ApplicationException("Error adding account note.", ex);
            }

            Log.Instance.Trace($"Exiting");
            return addSuccess;
        }

        public async Task<bool> ChangeFinancialClassAsync(string account, string newFinCode) => await Task.Run(() =>  ChangeFinancialClass(account, newFinCode));

        public bool ChangeFinancialClass(string account, string newFinCode)
        {
            Log.Instance.Trace($"Entering - Account {account} New Fin {newFinCode}");

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));
            if (string.IsNullOrEmpty(newFinCode))
                throw new ArgumentNullException(nameof(newFinCode));

            var record = GetByAccount(account);

            if (record != null)
                return ChangeFinancialClass(record, newFinCode);
            else
                return false;
        }

        public async Task<bool> ChangeFinancialClassAsync(Account table, string newFinCode) => await Task.Run(() => ChangeFinancialClass(table, newFinCode));

        public bool ChangeFinancialClass(Account table, string newFinCode)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new fin {newFinCode}");
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            else if (newFinCode == null)
                throw new ArgumentNullException(nameof(newFinCode));

            bool updateSuccess = true;
            string oldFinCode = table.FinCode;

            //check that newFincode is a valid fincode

            Fin newFin = AppEnvironment.Context.FinRepository.GetFin(newFinCode);
            Fin oldFin = AppEnvironment.Context.FinRepository.GetFin(oldFinCode);

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

        public async Task<bool> ChangeClientAsync(Account table, string newClientMnem) => await Task.Run(() => ChangeClient(table, newClientMnem));

        public bool ChangeClient(Account table, string newClientMnem)
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
                Client oldClient = AppEnvironment.Context.ClientRepository.GetClient(oldClientMnem);
                Client newClient = AppEnvironment.Context.ClientRepository.GetClient(newClientMnem);

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
                    if (table.Fin.FinClass == clientFinType)
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

                    if (table.Fin.FinClass == patientFinType)
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
                        else if (oldClient.ClientMnem == pthExceptionClient || newClient.ClientMnem == pthExceptionClient)
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

        public async Task<int> ReprocessChargesAsync(Account account, string comment) => await Task.Run(() => ReprocessCharges(account, comment));

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
                foreach (var chrg in account.Charges)
                {
                    foreach (var chrgdetail in chrg.ChrgDetails.Where(x => x.IsCredited == false))
                    {

                        if (chrgdetail.Type == ChrgDetailStatus.Invoice)  //do not reprocess CBILL charge records
                            continue;

                        ChrgDetail creditChrgDetail = chrgdetail;
                        creditChrgDetail.Quantity = creditChrgDetail.Quantity * -1;
                        creditChrgDetail.ChrgDetailId = 0;
                        creditChrgDetail.IsCredited = true;
                        creditChrgDetail.PostedDate = DateTime.Now;

                        AppEnvironment.Context.ChrgDetailRepository.Add(creditChrgDetail);

                        AppEnvironment.Context.ChrgDetailRepository.UpdateCredited(chrgdetail.ChrgDetailId, true);

                        //insert new charge and detail
                        if (chrg.Status != ChargeStatus.Invoice)
                            AddChargeDetail(account, chrg);
                    }
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

        public async Task<int> ReprocessChargesAsync(string account, string comment) => await Task.Run(() => ReprocessCharges(account, comment));

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

        public async Task<bool> UpdateChargesFinCodeAsync(string account, string finCode) => await Task.Run(() => UpdateChargesFinCode(account, finCode));

        public bool UpdateChargesFinCode(string account, string finCode)
        {
            Log.Instance.Trace("Entering");

            List<Chrg> charges = AppEnvironment.Context.ChrgRepository.GetByAccount(account);
            List<ChrgDetail> chargeDetails = AppEnvironment.Context.ChrgDetailRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
                return false;

            if(chargeDetails == null || chargeDetails.Count == 0)
                return false;

            var fin = AppEnvironment.Context.FinRepository.GetFin(finCode);

            if (fin == null)
                return false;

            var chrgsToUpdate = chargeDetails.Where(x => x.IsCredited == false &&
                (x.ClientMnem != AppEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
                string.IsNullOrEmpty(AppEnvironment.ApplicationParameters.PathologyGroupClientMnem))
                && x.FinancialType == fin.FinClass).ToList();

            foreach (var chrg in chrgsToUpdate)
            {
                chrg.FinCode = finCode;
                chrg.FinancialType = fin.FinClass;
                AppEnvironment.Context.ChrgDetailRepository.Update(chrg, new[] { nameof(ChrgDetail.FinancialType), nameof(ChrgDetail.FinCode) });
            }

            return true;
        }

        public async Task<bool> UpdateChargesClientAsync(string account, string clientMnem) => await Task.Run(() => UpdateChargesClient(account, clientMnem));

        public bool UpdateChargesClient(string account, string clientMnem)
        {
            Log.Instance.Trace("Entering");

            List<Chrg> charges = AppEnvironment.Context.ChrgRepository.GetByAccount(account);
            List<ChrgDetail> chrgDetails = AppEnvironment.Context.ChrgDetailRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
                return false;

            if (chrgDetails == null || chrgDetails.Count == 0)
                return false;

            var client = AppEnvironment.Context.ClientRepository.GetClient(clientMnem);
            if (client == null)
                return false;

            foreach (var chrg in chrgDetails)
            {
                chrg.ClientMnem = clientMnem;
                AppEnvironment.Context.ChrgDetailRepository.Update(chrg, new[] { nameof(ChrgDetail.ClientMnem) });
            }

            return true;
        }

        public async Task<List<ChrgDetail>> AddChargeAsync(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
            => await Task.Run(() => AddCharge(account, cdm, serviceDate, comment, refNumber));

        /// <summary>
        /// Add a charge to an account. The account must exist.
        /// </summary>
        /// <param name="account">The account number.</param>
        /// <param name="cdm">Cdm of the charge to add.</param>
        /// <param name="serviceDate"></param>
        /// <param name="comment"></param>
        /// <param name="refNumber"></param>
        /// <returns></returns>
        /// <exception cref="AccountNotFoundException"></exception>
        public List<ChrgDetail> AddCharge(string account, string cdm, DateTime? serviceDate = null, string comment = null, string refNumber = null)
        {
            Log.Instance.Trace($"Entering - account {account} cdm {cdm}");

            //verify the account exists
            Account accData = GetByAccount(account);
            if (accData == null)
            {
                Log.Instance.Error($"Account {account} not found");
                throw new AccountNotFoundException("Account is not a valid account.", account);
            }

            return AddCharge(accData, cdm, serviceDate, comment, refNumber);
        }

        public async Task<List<ChrgDetail>> AddChargeAsync(Account accData, string cdm, DateTime serviceDate, string comment = null, string refNumber = null)
            => await Task.Run(() => AddCharge(accData, cdm, serviceDate, comment, refNumber));

        /// <summary>
        /// Add a new charge to an account. The account must exist.
        /// </summary>
        /// <param name="accData"></param>
        /// <param name="cdm"></param>
        /// <param name="serviceDate"></param>
        /// <param name="comment"></param>
        /// <param name="refNumber"></param>
        /// <returns>A list of the charge details created</returns>
        /// <exception cref="InvalidClientException"></exception>
        /// <exception cref="CdmNotFoundException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public List<ChrgDetail> AddCharge(Account accData, string cdm, DateTime? serviceDate = null, string comment = null, string refNumber = null)
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

            if (accData.Client == null)
            {
                throw new InvalidClientException("Client not valid", accData.ClientMnem);
            }

            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = AppEnvironment.Context.CdmRepository.GetCdm(cdm);
            if (cdmData == null)
            {
                Log.Instance.Error($"CDM {cdm} not found.");
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = AppEnvironment.Context.FinRepository.GetFin(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");
            try
            {
                BeginTransaction();

                Chrg chrg = new Chrg();

                //now build the charge & detail records
                chrg.AccountNo = accData.AccountNo;
                chrg.CDMCode = cdm;
                chrg.Cdm = cdmData;
                chrg.Comment = comment;
                chrg.IsCredited = false;
                chrg.OrderMnem = cdmData.Mnem;
                chrg.LISReqNo = refNumber;
                chrg.PostingDate = DateTime.Today;
                chrg.ServiceDate = serviceDate ?? accData.TransactionDate;

                var chrgNo = AppEnvironment.Context.ChrgRepository.AddCharge(chrg);
                chrg.ChrgNo = chrgNo;

                Log.Instance.Trace($"Exiting");

                var result = AddChargeDetail(accData, chrg);

                CompleteTransaction();

                return result;
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex, $"Error during Add Charge. Account {accData.AccountNo} CDM {cdm}");
                AbortTransaction();
                throw new ApplicationException($"Error during Add Charge. Account {accData.AccountNo} CDM {cdm}", ex);
            }
        }

        /// <summary>
        /// Update a charge. The charge must exist.
        /// </summary>
        /// <param name="chrgId"></param>
        /// <returns>A list of the charge detail records created.</returns>
        /// <exception cref="ApplicationException"></exception>
        public List<ChrgDetail> UpdateCharge(int chrgId)
        {
            Log.Instance.Trace($"Entering - charge {chrgId}");

            var chrg = AppEnvironment.Context.ChrgRepository.GetById(chrgId);
            var acc = GetByAccount(chrg.AccountNo);

            if (chrg == null)
            {
                Log.Instance.Error($"Charge {chrgId} not found.");
                throw new ApplicationException($"Charge {chrgId} not found.");
            }

            Log.Instance.Trace($"Exiting");

            return AddChargeDetail(acc, chrg);
        }

        /// <summary>
        /// Add a charge to an account. The account and parent charge must exist.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="comment"></param>
        /// <returns>Charge number of newly entered charge or < 0 if an error occurs.</returns>
        private List<ChrgDetail> AddChargeDetail(Account accData, Chrg chrg)
        {
            Log.Instance.Trace($"Entering - account {chrg.AccountNo} cdm {chrg.CDMCode}");
            List<ChrgDetail> chrgDetails = new List<ChrgDetail>();
            Fin fin = AppEnvironment.Context.FinRepository.GetFin(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");
            Client chargeClient = accData.Client;

            if (AppEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
            {
                //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
                var gb = AppEnvironment.Context.GlobalBillingCdmRepository.GetCdm(chrg.CDMCode);
                //hard coding exception for Hardin County for now - 05/09/2023 BSP
                if (gb != null && accData.ClientMnem != pthExceptionClient)
                {
                    fin = AppEnvironment.Context.FinRepository.GetFin("Y") ?? throw new ApplicationException($"Fin code Y not found error {accData.AccountNo}");
                    chargeClient = AppEnvironment.Context.ClientRepository.GetClient(AppEnvironment.ApplicationParameters.PathologyGroupClientMnem);
                }
            }

            List<CdmDetail> feeSched = null;

            switch (chargeClient.FeeSchedule)
            {
                case "1":
                    feeSched = chrg.Cdm.CdmFeeSchedule1;
                    break;
                case "2":
                    feeSched = chrg.Cdm.CdmFeeSchedule2;
                    break;
                case "3":
                    feeSched = chrg.Cdm.CdmFeeSchedule3;
                    break;
                case "4":
                    feeSched = chrg.Cdm.CdmFeeSchedule4;
                    break;
                case "5":
                    feeSched = chrg.Cdm.CdmFeeSchedule5;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("FeeSchedule");
                    //break;
            }

            foreach (CdmDetail fee in feeSched)
            {
                ChrgDetail chrgDetail = new ChrgDetail
                {
                    //DiagnosisPointer = new ChrgDiagnosisPointer(),
                    BillingCode = fee.BillCode,
                    Cpt4 = fee.Cpt4,
                    Type = fee.Type,
                    AccountNo = chrg.AccountNo,
                    ChrgNo = chrg.ChrgNo,
                    Modifier = fee.Modifier,
                    RevenueCode = fee.RevenueCode,
                    ClientMnem = chargeClient.ClientMnem,
                    FinCode = fin.FinCode,
                    FinancialType = fin.FinClass,
                    ServiceDate = chrg.ServiceDate,
                    PostedDate = DateTime.Today,
                    Quantity = 1,
                    FeeSchedule = fee.FeeSchedule,
                };
                switch (fin.FinClass)
                {
                    case patientFinType:
                        chrgDetail.Amount = fee.MClassPrice;
                        chrgDetail.DiscountAmount = 0.00;
                        break;
                    case clientFinType:
                        //todo: calculate client discount
                        var cliDiscount = chargeClient.Discounts.Find(c => c.Cdm == chrg.CDMCode);
                        double discountPercentage = chargeClient.DefaultDiscount;
                        if (cliDiscount != null)
                        {
                            discountPercentage = cliDiscount.PercentDiscount;
                        }
                        chrgDetail.DiscountAmount = fee.CClassPrice * (discountPercentage / 100);
                        chrgDetail.Amount = fee.CClassPrice - chrgDetail.DiscountAmount;
                        break;
                    case "Z":
                        chrgDetail.Amount = fee.ZClassPrice;
                        chrgDetail.DiscountAmount = 0.00;
                        break;
                    default:
                        chrgDetail.Amount = fee.MClassPrice;
                        chrgDetail.DiscountAmount = 0.00;
                        break;
                }

                chrgDetails.Add(chrgDetail);
            }

            Log.Instance.Trace($"Exiting");
            return AppEnvironment.Context.ChrgDetailRepository.Add(chrgDetails);
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

        public async Task UnbundlePanelsAsync(Account account) => await Task.Run(() => UnbundlePanels(account));

        public void UnbundlePanels(Account account)
        {
            var bundledProfiles = account.Charges.Where(x => x.CDMCode == "MCL0029" && x.IsCredited == false);
            try
            {
                BeginTransaction();

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    AppEnvironment.Context.ChrgRepository.CreditCharge(bundledProfile.ChrgNo, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", account.TransactionDate);
                    AddCharge(account, "5382522", account.TransactionDate);
                    AddCharge(account, "5646008", account.TransactionDate);
                }

                bundledProfiles = account.Charges.Where(x => x.CDMCode == "MCL0021" && x.IsCredited == false);

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    AppEnvironment.Context.ChrgRepository.CreditCharge(bundledProfile.ChrgNo, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", account.TransactionDate);
                    AddCharge(account, "5646012", account.TransactionDate);
                    AddCharge(account, "5646086", account.TransactionDate);
                    AddCharge(account, "5646054", account.TransactionDate);
                    AddCharge(account, "5728026", account.TransactionDate);
                    AddCharge(account, "5728190", account.TransactionDate);
                }

                CompleteTransaction();
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                AbortTransaction();
            }

        }

        public async Task BundlePanelsAsync(Account account) => await Task.Run(() => BundlePanels(account));

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
                            bundledProfiles[x].ComponentCpt[i].ChrgId = chrg.ChrgNo;
                        }
                    }
                }

                if (bundledProfiles[x].BundleQualfies)
                {
                    //credit components and charge profile cdm

                    for (int i = 0; i < bundledProfiles[x].ComponentCpt.Count; i++)
                    {
                        AppEnvironment.Context.ChrgRepository.CreditCharge(bundledProfiles[x].ComponentCpt[i].ChrgId, $"Bundling to {bundledProfiles[x].ProfileCdm}");
                    }

                    this.AddCharge(account, bundledProfiles[x].ProfileCdm, (DateTime)account.TransactionDate, $"Bundled by Rule");
                    this.AddNote(account.AccountNo, $"Bundled charges into {bundledProfiles[x].ProfileCdm}");
                }
            }
        }

        public async Task<bool> ValidateAsync(Account account, bool reprint = false) => await Task.Run(() => Validate(account, reprint));

        /// <summary>
        /// Runs all validation routines on account. Updates validation status and account flags. Errors are stored in the validation status table.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="reprint">Set true if validating account to resubmit the claim with no changes.</param>
        /// <returns>True if account is valid for billing, false if there are validation errors.</returns>
        public bool Validate(Account account, bool reprint = false)
        {
            Log.Instance.Trace($"Entering - account {account}");

            try
            {
                if ((account.Status == AccountStatus.InstSubmitted || account.Status == AccountStatus.ProfSubmitted 
                    || account.Status == AccountStatus.ClaimSubmitted || account.Status == AccountStatus.Statements
                    || account.Status == AccountStatus.Closed || account.Status == AccountStatus.PaidOut) && !reprint)
                {
                    //account has been billed, do not validate
                    account.AccountValidationStatus.account = account.AccountNo;
                    account.AccountValidationStatus.mod_date = DateTime.Now;
                    account.AccountValidationStatus.validation_text = "Account has already been billed. Did not validate.";
                    AppEnvironment.Context.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                    return false;
                }
                else
                {
                    BeginTransaction();
                    if (account.Fin.FinClass == patientFinType && account.InsurancePrimary != null)
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
                        if (account.Status == AccountStatus.ReadyToBill)
                        {   
                            UpdateStatus(account.AccountNo, account.BillForm);
                            //if this is a self-pay, set the statement flag
                            if(account.BillForm == AccountStatus.Statements)
                            {
                                AppEnvironment.Context.PatRepository.SetStatementFlag(account.AccountNo, "Y");
                            }
                        }
                    }

                    AppEnvironment.Context.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                    if (!string.IsNullOrEmpty(lmrperrors))
                    {
                        AccountLmrpError record = new AccountLmrpError
                        {
                            AccountNo = account.AccountNo,
                            DateOfService = (DateTime)account.TransactionDate,
                            ClientMnem = account.ClientMnem,
                            FinancialCode = account.FinCode,
                            Error = lmrperrors
                        };

                        AppEnvironment.Context.AccountLmrpErrorRepository.Save(record);
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
                AppEnvironment.Context.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
            }

            return false;
        }

        private async Task<List<string>> ValidateLMRPAsync(Account account) => await Task.Run(() => ValidateLMRP(account));

        private List<string> ValidateLMRP(Account account)
        {
            Log.Instance.Trace($"Entering - account {account}");
            List<string> errorList = new List<string>();

            //determine if there are any rules for ama_year
            if (AppEnvironment.Context.LMRPRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
            {
                // no lmrp rules loaded for this ama year. 
                errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
                return errorList;
            }

            foreach (var cpt4 in account.Cpt4List.Distinct())
            {
                if (cpt4 == null)
                    continue;
                var ruleDef = AppEnvironment.Context.LMRPRuleRepository.GetRuleDefinition(cpt4, (DateTime)account.TransactionDate);
                if (ruleDef == null)
                    continue;

                bool dxIsValid = ruleDef.DxIsValid != 0;
                bool dxSupported = false;

                foreach (var dx in account.Pat.Diagnoses)
                {
                    var rule = AppEnvironment.Context.LMRPRuleRepository.GetRule(cpt4, dx.Code, (DateTime)account.TransactionDate);
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

            DateTime thruDate = AppEnvironment.ApplicationParameters.SSIBillThruDate;

            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.PaidOut),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Closed),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.ProfSubmitted),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.InstSubmitted),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, AccountStatus.Client),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, AccountStatus.Hold),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "Y"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, clientFinCode),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "E")
            };

            var accounts = AppEnvironment.Context.AccountSearchRepository.GetBySearch(parameters).ToList();

            foreach (var account in accounts)
            {
                try
                {
                    var accountRecord = this.GetByAccount(account.Account);
                    this.Validate(accountRecord);
                }
                catch (Exception e)
                {
                    Log.Instance.Error(e, "Error during account validation job.");
                }
            }
        }

        public async Task ValidateUnbilledAccountsAsync() => await Task.Run(() => ValidateUnbilledAccounts());

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
                AppEnvironment.Context.ChrgRepository.CreditCharge(charge.ChrgNo, $"Move to {destinationAccount}");
                AddCharge(destination, charge.CDMCode, (DateTime)destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
            }
            return (true, string.Empty);
        }

        public async Task MoveChargeAsync(string sourceAccount, string destinationAccount, int chrgId) => await Task.Run(() => MoveCharge(sourceAccount, destinationAccount, chrgId));

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

            var charge = source.Charges.SingleOrDefault(c => c.ChrgNo == chrgId);

            if (charge.IsCredited)
            {
                throw new ApplicationException("Charge is already credited.");
            }

            AppEnvironment.Context.ChrgRepository.CreditCharge(charge.ChrgNo, $"Move to {destinationAccount}");
            AddCharge(destinationAccount, charge.CDMCode, destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);

        }


        public async Task ClearClaimStatusAsync(Account account) => await Task.Run(() => ClearClaimStatus(account));

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

                AppEnvironment.Context.PatRepository.Update(account.Pat, columns);
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
