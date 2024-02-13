using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using Utilities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data;
using Log = LabBilling.Logging.Log;
using PetaPoco;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.DataAccess;
using Microsoft.AspNetCore.Http;

namespace LabBilling.Core.Services
{
    public sealed class AccountService
    {
        private IAppEnvironment appEnvironment;

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

        private DictionaryService dictionaryService;

        public AccountService(IAppEnvironment appEnvironment)
        {
            this.appEnvironment = appEnvironment;
            dictionaryService = new(appEnvironment);
        }

        public decimal GetNextAccountNumber()
        {
            using AccountUnitOfWork uow = new(appEnvironment);

            return uow.NumberRepository.GetNumber("account");
        }

        public async Task<Account> GetAccountAsync(string account, bool demographicsOnly = false) => await Task.Run(() => GetAccount(account, demographicsOnly));

        public Account GetAccount(string account, bool demographicsOnly = false)
        {
            Logging.Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            using AccountUnitOfWork unitOfWork = new AccountUnitOfWork(appEnvironment);

            var record = unitOfWork.AccountRepository.GetByAccount(account, demographicsOnly);

            if (record == null)
                return null;

            if (!string.IsNullOrEmpty(record.ClientMnem))
            {
                if (record.ClientMnem != invalidFinCode)
                {
                    record.Client = unitOfWork.ClientRepository.GetClient(record.ClientMnem);
                    if (record.Client != null)
                        record.ClientName = record.Client.Name;
                }
            }
            record.Pat = unitOfWork.PatRepository.GetByAccount(record);

            if (!demographicsOnly)
            {
                record.Charges = unitOfWork.ChrgRepository.GetByAccount(account, true, true, null, false);
                record.Payments = unitOfWork.ChkRepository.GetByAccount(account);
                record.Insurances = unitOfWork.InsRepository.GetByAccount(account);
                record.Notes = unitOfWork.AccountNoteRepository.GetByAccount(account);
                record.BillingActivities = unitOfWork.BillingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = unitOfWork.AccountValidationStatusRepository.GetByAccount(account);
                record.Fin = unitOfWork.FinRepository.GetFin(record.FinCode);
                record.AccountAlert = unitOfWork.AccountAlertRepository.GetByAccount(account);
                record.PatientStatements = unitOfWork.PatientStatementAccountRepository.GetByAccount(account);

                DateTime outpBillStartDate;
                DateTime questStartDate = new(2012, 10, 1);
                DateTime questEndDate = new(2020, 5, 31);
                DateTime arbitraryEndDate = new(2016, 12, 31);

                outpBillStartDate = appEnvironment.ApplicationParameters.OutpatientBillStart;

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

                            if (string.IsNullOrEmpty(record.BillForm))
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

            record.BillableCharges = record.Charges.Where(x => x.Status != cbillStatus && x.Status != capStatus && x.Status != naStatus).ToList();

            if (record.FinCode == clientFinCode)
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

            if (record.FinCode == clientFinCode)
            {
                record.ClaimBalance = 0.00;

                record.ClientBalance = new List<(string client, double balance)>();

                var balance = record.BillableCharges.Sum(y => y.Quantity * y.NetAmount)
                     - (record.TotalPayments + record.TotalContractual + record.TotalWriteOff);

                record.ClientBalance.Add((record.ClientMnem, balance));
            }
            else
            {
                record.ClaimBalance = record.BillableCharges.Where(x => x.FinancialType == patientFinType).Sum(x => x.Quantity * x.NetAmount)
                    - (record.TotalPayments + record.TotalWriteOff + record.TotalContractual);

                var results = record.BillableCharges.Where(x => x.FinancialType == clientFinType)
                    .GroupBy(x => x.ClientMnem, (client, balance) => new { Client = client, Balance = balance.Sum(c => c.Quantity * c.NetAmount) });

                record.ClientBalance = new List<(string client, double balance)>();

                foreach (var result in results)
                {
                    record.ClientBalance.Add((result.Client, result.Balance));
                }
            }
            record.Balance = record.TotalCharges - (record.TotalPayments + record.TotalContractual + record.TotalWriteOff);

            return record;
        }

        public double GetBalance(string accountNo)
        {
            using AccountUnitOfWork unitOfWork = new(appEnvironment);

            string chrgTableName = unitOfWork.ChrgRepository.TableInfo.TableName;
            string accTableName = unitOfWork.AccountRepository.TableInfo.TableName;
            string chkTableName = unitOfWork.ChkRepository.TableInfo.TableName;

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));
            try
            {
                var sql = Sql.Builder
                    .Select(new[]
                    {
                        $"coalesce(sum({unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.Quantity))} * {unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.NetAmount))}), 0.00) as 'Total'"
                    })
                    .From(chrgTableName)
                    .InnerJoin(accTableName)
                    .On($"{accTableName}.{unitOfWork.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} = {chrgTableName}.{unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))}")
                    .Where($"{chrgTableName}.{unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo })
                    .Where($"(({accTableName}.{unitOfWork.AccountRepository.GetRealColumn(nameof(Account.FinCode))} = @0 and {chrgTableName}.{unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} <> @1)",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientFinCode },
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cbillStatus })
                    .Append($"or ({accTableName}.{unitOfWork.AccountRepository.GetRealColumn(nameof(Account.FinCode))} <> @0 and {chrgTableName}.{unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} not in (@1, @2, @3)))",
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientFinCode },
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cbillStatus },
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = capStatus },
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = naStatus })
                    .GroupBy(chrgTableName + "." + unitOfWork.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo)));

                double charges = unitOfWork.Context.SingleOrDefault<double>(sql);

                sql = Sql.Builder
                    .Select(new[]
                    {
                        $"coalesce(sum({unitOfWork.ChkRepository.GetRealColumn(nameof(Chk.PaidAmount))} + {unitOfWork.ChkRepository.GetRealColumn(nameof(Chk.ContractualAmount))} + {unitOfWork.ChkRepository.GetRealColumn(nameof(Chk.WriteOffAmount))}), 0.00) as 'Total'"
                    })
                    .From(unitOfWork.ChkRepository.TableInfo.TableName)
                    .Where($"{chkTableName}.{unitOfWork.ChkRepository.GetRealColumn(nameof(Chk.AccountNo))} = @0", 
                        new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo})
                    .GroupBy(unitOfWork.ChkRepository.GetRealColumn(nameof(Chk.AccountNo)));

                double adj = (double)unitOfWork.Context.SingleOrDefault<double>(sql);

                return charges - adj;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, $"Error in AccountRepository.GetBalance({accountNo}");
                throw new ApplicationException($"Error in AccountRepository.GetBalance({accountNo}", ex);
            }
        }

        public async Task<object> AddAsync(Account table) => await Task.Run(() => Add(table));

        public Account Add(Account table)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            if (table.FinCode != "CLIENT")
                table.PatFullName = table.PatNameDisplay;

            table.Status = AccountStatus.New;

            table.TransactionDate = table.TransactionDate.Date;

            unitOfWork.AccountRepository.Add(table);

            //make sure Pat record has an account number
            if (table.Pat.AccountNo != table.AccountNo)
                table.Pat.AccountNo = table.AccountNo;

            var pat = unitOfWork.PatRepository.GetByAccount(table);
            if (pat == null)
                unitOfWork.PatRepository.Add(table.Pat);
            else
                unitOfWork.PatRepository.Update(table.Pat);

            foreach (Ins ins in table.Insurances)
            {
                if (ins.Account != table.AccountNo)
                    ins.Account = table.AccountNo;

                unitOfWork.InsRepository.Save(ins);
            }

            unitOfWork.Commit();
            return table;
        }

        public async Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate) => await Task.Run(() => GetInvoiceAccounts(clientMnem, thruDate));

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment);
            return unitOfWork.InvoiceSelectRepository.GetByClientAndDate(clientMnem, thruDate);
        }

        public async Task<IEnumerable<ClaimItem>> GetAccountsForClaimsAsync(ClaimType claimType, int maxClaims = 0) => await Task.Run(() => GetAccountsForClaims(claimType, maxClaims));

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType, int maxClaims = 0)
        {
            Log.Instance.Trace($"Entering - claimType {claimType}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment);
            try
            {
                var queryResult = unitOfWork.ClaimItemRepository.GetClaimItems(claimType);

                return queryResult;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }
            Log.Instance.Trace("Exiting");

            return new List<ClaimItem>();
        }

        public List<PatientStatementAccount> GetPatientStatements(string accountNo)
        {
            using AccountUnitOfWork unitOfWork = new(appEnvironment);

            var statements = unitOfWork.PatientStatementAccountRepository.GetByAccount(accountNo);

            return statements;
        }

        public async Task<bool> SetNoteAlertAsync(string account, bool showAlert) => await Task.Run(() => SetNoteAlert(account, showAlert));

        public bool SetNoteAlert(string account, bool showAlert)
        {
            Log.Instance.Trace($"Entering - account {account} showAlert {showAlert}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            try
            {
                var record = unitOfWork.AccountAlertRepository.GetByAccount(account);

                if (record == null)
                {
                    record = new AccountAlert
                    {
                        AccountNo = account,
                        Alert = showAlert
                    };

                    unitOfWork.AccountAlertRepository.Add(record);
                }
                else
                {
                    record.Alert = showAlert;
                    unitOfWork.AccountAlertRepository.Update(record);
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error updating NoteAlert.");
                return false;
            }

            unitOfWork.Commit();
            return true;
        }


        public bool UpdateAccountDemographics(Account acc)
        {
            Log.Instance.Trace("Entering");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            try
            {
                bool accUpdateStatus = unitOfWork.AccountRepository.Update(acc);
                unitOfWork.PatRepository.SaveAll(acc.Pat);
                unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                return false;
            }
        }

        public async Task<bool> UpdateDiagnosesAsync(Account acc) => await Task.Run(() => UpdateDiagnoses(acc));

        public bool UpdateDiagnoses(Account acc)
        {
            Log.Instance.Trace("Entering");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            if (acc == null)
                throw new ArgumentNullException(nameof(acc));

            if (unitOfWork.PatRepository.SaveDiagnoses(acc.Pat))
            {
                acc.Pat = unitOfWork.PatRepository.GetByAccount(acc);
                unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public async Task<int> UpdateStatusAsync(string accountNo, string status) => await Task.Run(() => UpdateStatus(accountNo, status));

        public int UpdateStatus(string accountNo, string status)
        {
            Log.Instance.Trace($"Entering - account {accountNo} status {status}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));
            if (string.IsNullOrEmpty(status))
                throw new ArgumentNullException(nameof(status));
            if (!AccountStatus.IsValid(status))
                throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

            var returnval = unitOfWork.AccountRepository.UpdateStatus(accountNo, status);
            unitOfWork.Commit();

            return returnval;
        }

        public bool UpdateStatementFlag(Account acc, string flag)
        {
            using AccountUnitOfWork unitOfWork=new(appEnvironment, true);
            AddNote(acc.AccountNo, $"Statement flag changed from {acc.Pat.StatementFlag} to {flag}");

            acc.Pat.StatementFlag = flag;
            if(flag != "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.Statements);
                acc.Status = AccountStatus.Statements;
            }
            
            if (unitOfWork.PatRepository.Update(acc.Pat, new[] { nameof(Pat.StatementFlag) }))
            {
                unitOfWork.Commit();
                return true;
            }
            return false;
        }

        public void SaveInsurance(Ins ins)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            if (ins.rowguid == Guid.Empty)
                uow.InsRepository.Add(ins);
            else
                uow.InsRepository.Update(ins);

            uow.Commit();
        }

        public bool DeleteInsurance(Ins ins)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            var retval = uow.InsRepository.Delete(ins);
            uow.Commit();
            return retval;
        }

        public async Task<bool> InsuranceSwapAsync(string accountNo, InsCoverage swap1, InsCoverage swap2) => await Task.Run(() => InsuranceSwap(accountNo, swap1, swap2));

        public bool InsuranceSwap(string accountNo, InsCoverage swap1, InsCoverage swap2)
        {
            Log.Instance.Trace($"Entering - Account {accountNo} Ins1 {swap1} Ins2 {swap2}");

            using AccountUnitOfWork unitOfWork = new AccountUnitOfWork(appEnvironment, true);

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));

            Account account = GetAccount(accountNo);

            if (account == null)
            {
                return false;
            }

            Ins insA = account.Insurances.Where(x => x.Coverage == swap1).FirstOrDefault();
            Ins insB = account.Insurances.Where(x => x.Coverage == swap2).FirstOrDefault();

            if (insA == null || insB == null)
            {
                return false;
            }
            try
            {

                insA.Coverage = InsCoverage.Temporary;
                insB.Coverage = swap1;

                unitOfWork.InsRepository.Update(insA);
                unitOfWork.InsRepository.Update(insB);

                insA.Coverage = swap2;
                unitOfWork.InsRepository.Update(insA);

                AddNote(accountNo, $"Insurance swap: {swap1} {insA.PlanName} and {swap2} {insB.PlanName}");
                unitOfWork.Commit();
                return false;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error swapping insurances.");
                return false;
            }

        }

        public async Task<bool> ChangeDateOfServiceAsync(Account table, DateTime newDate, string reason_comment) => await Task.Run(() => ChangeDateOfService(table, newDate, reason_comment));

        public bool ChangeDateOfService(Account table, DateTime newDate, string reason_comment)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new date {newDate} reason {reason_comment}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            if (table == null)
                throw new ArgumentNullException(nameof(table));
            if (reason_comment == null)
                throw new ArgumentNullException(nameof(reason_comment));

            bool updateSuccess = false;
            DateTime oldServiceDate = DateTime.MinValue;

            // update trans_date on acc table
            if (table.TransactionDate != newDate)
            {
                oldServiceDate = (DateTime)table.TransactionDate;
                table.TransactionDate = newDate;
                unitOfWork.AccountRepository.Update(table, new[] { nameof(Account.TransactionDate) });

                if (AddNote(table.AccountNo, $"Service Date changed from {oldServiceDate} to {newDate}"))
                {
                    table.Notes = unitOfWork.AccountNoteRepository.GetByAccount(table.AccountNo);
                }

                //determine if charges need to be reprocessed.

                //TODO: is there any reason a date of service change should result in changing all charges --
                // except: the date of service on charges will not match new date.

                // option: reprocess all charges, or update service date on charge records
                foreach (var chrg in table.Charges)
                {
                    chrg.ServiceDate = newDate;
                    unitOfWork.ChrgRepository.Update(chrg);
                }
                unitOfWork.Commit();
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
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            bool addSuccess = true;

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));

            if (string.IsNullOrEmpty(noteText))
            {
                //there is no note to add
                return false;
            }

            AccountNote accountNote = new()
            {
                Account = account,
                Comment = noteText
            };
            try
            {
                unitOfWork.AccountNoteRepository.Add(accountNote);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Error adding account note.");
                throw new ApplicationException("Error adding account note.", ex);
            }

            Log.Instance.Trace($"Exiting");
            return addSuccess;
        }

        public List<AccountNote> GetNotes(string accountNo)
        {
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            var notes = unitOfWork.AccountNoteRepository.GetByAccount(accountNo);

            return notes;
        }

        public async Task<bool> ChangeFinancialClassAsync(string account, string newFinCode) => await Task.Run(() => ChangeFinancialClass(account, newFinCode));

        public bool ChangeFinancialClass(string account, string newFinCode)
        {
            Log.Instance.Trace($"Entering - Account {account} New Fin {newFinCode}");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            if (string.IsNullOrEmpty(account))
                throw new ArgumentNullException(nameof(account));
            if (string.IsNullOrEmpty(newFinCode))
                throw new ArgumentNullException(nameof(newFinCode));

            var record = GetAccount(account);

            if (record != null)
            {
                var retval = ChangeFinancialClass(record, newFinCode);
                unitOfWork.Commit();
                return retval;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ChangeFinancialClassAsync(Account table, string newFinCode) => await Task.Run(() => ChangeFinancialClass(table, newFinCode));

        public bool ChangeFinancialClass(Account table, string newFinCode)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new fin {newFinCode}");
            if (table == null)
                throw new ArgumentNullException(nameof(table));
            else if (newFinCode == null)
                throw new ArgumentNullException(nameof(newFinCode));

            using AccountUnitOfWork uow = new(appEnvironment, true);

            bool updateSuccess = true;
            string oldFinCode = table.FinCode;

            //check that newFincode is a valid fincode
            Fin newFin = uow.FinRepository.GetFin(newFinCode);
            Fin oldFin = uow.FinRepository.GetFin(oldFinCode);

            if (newFin == null)
            {
                throw new ArgumentException($"Financial code {newFinCode} is not valid code.", "newFinCode");
            }

            if (oldFinCode != newFinCode)
            {
                table.FinCode = newFinCode;
                try
                {
                    uow.AccountRepository.Update(table, new[] { nameof(Account.FinCode) });
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
                        //AbortTransaction();
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }
                else
                {
                    UpdateChargesFinCode(table.AccountNo, newFinCode);
                }
                uow.Commit();
            }
            else
            {
                updateSuccess = false;
            }


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

            using AccountUnitOfWork uow = new(appEnvironment, true);

            bool updateSuccess = true;
            string oldClientMnem = table.ClientMnem;

            if (oldClientMnem == null)
            {
                //account does not have a valid current client defined

            }

            try
            {
                Client oldClient = uow.ClientRepository.GetClient(oldClientMnem);
                Client newClient = uow.ClientRepository.GetClient(newClientMnem);

                //Context.BeginTransaction();

                if (oldClient == null)
                    throw new ApplicationException($"Client mnem {oldClientMnem} is not valid.");

                if (newClient == null)
                    throw new ArgumentException($"Client mnem {newClientMnem} is not valid.", nameof(newClientMnem));

                if (oldClientMnem != newClientMnem)
                {

                    table.ClientMnem = newClientMnem;
                    table.Client = newClient;
                    table.ClientName = newClient.Name;
                    try
                    {
                        uow.AccountRepository.Update(table, new[] { nameof(Account.ClientMnem) });
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

                    uow.Commit();
                }
                else
                {
                    // old client is same as new client - no change
                    updateSuccess = false;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error during Change Client", ex);
                //AbortTransaction();
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

        public Chrg GetCharge(int chrgNo)
        {
            using AccountUnitOfWork uow = new(appEnvironment);

            var charge = uow.ChrgRepository.GetById(chrgNo);

            charge.Cdm = uow.CdmRepository.GetCdm(charge.CDMCode);
            charge.ChrgDetails = uow.ChrgDetailRepository.GetByChrgId(chrgNo).ToList();

            return charge;
        }

        public IList<ClaimChargeView> GetClaimCharges(string accountNo)
        {
            using AccountUnitOfWork uow = new(appEnvironment);

            var charges = uow.ChrgRepository.GetClaimCharges(accountNo);

            foreach (var chrg in charges)
            {
                chrg.RevenueCodeDetail = uow.RevenueCodeRepository.GetByCode(chrg.RevenueCode);
                chrg.Cdm = uow.CdmRepository.GetCdm(chrg.ChargeId, true);
            }

            return charges;
        }

        public IList<Chrg> GetCharges(string accountNo, bool showCredited = true, bool includeInvoiced = true, DateTime? asOfDate = null, bool excludeCBill = true)
        {
            Log.Instance.Debug($"Entering - account {accountNo}");

            using AccountUnitOfWork uow = new(appEnvironment);

            if (string.IsNullOrEmpty(accountNo))
                throw new ArgumentNullException(nameof(accountNo));

            List<Chrg> charges = uow.ChrgRepository.GetByAccount(accountNo, showCredited, includeInvoiced, asOfDate, excludeCBill);

            foreach (Chrg chrg in charges)
            {
                chrg.Cdm = dictionaryService.GetCdm(chrg.CDMCode, true);
                chrg.ChrgDetails = uow.ChrgDetailRepository.GetByChrgId(chrg.ChrgId).ToList();
                foreach (ChrgDetail detail in chrg.ChrgDetails)
                {
                    detail.RevenueCodeDetail = dictionaryService.GetRevenueCode(detail.RevenueCode);
                    detail.DiagnosisPointer = uow.ChrgDiagnosisPointerRepository.GetById(detail.uri);
                    var cpt = uow.CptAmaRepository.GetCpt(detail.Cpt4);
                    if (cpt != null)
                        detail.CptDescription = cpt.ShortDescription;
                }
            }

            return charges;
        }

        public bool UpdateDxPointers(IEnumerable<Chrg> chrgs)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            bool returnVal = true;

            foreach (var chrg in chrgs)
            {
                foreach (var detail in chrg.ChrgDetails)
                {
                    returnVal = uow.ChrgDiagnosisPointerRepository.Save(detail.DiagnosisPointer) && returnVal;
                }
            }
            uow.Commit();
            return returnVal;
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

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            try
            {
                var chargesToCredit = account.Charges.Where(x => x.IsCredited == false).ToList();

                foreach (var chrg in chargesToCredit)
                {
                    if (chrg.CDMCode == invoicedCdm)  //do not reprocess CBILL charge records
                        continue;

                    // need to determine the correct logic for dealing with client charges

                    CreditCharge(chrg.ChrgId, comment);

                    //insert new charge and detail
                    if (chrg.CDMCode != invoicedCdm)
                        AddCharge(account, chrg.CDMCode, chrg.Quantity, account.TransactionDate);
                }

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                //AbortTransaction();
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

            var acc = GetAccount(account);

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

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            List<Chrg> charges = unitOfWork.ChrgRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
            {
                return false;
            }

            var fin = unitOfWork.FinRepository.GetFin(finCode);

            if (fin == null)
            {
                return false;
            }

            var chrgsToUpdate = charges.Where(x => x.IsCredited == false &&
                (x.ClientMnem != appEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
                string.IsNullOrEmpty(appEnvironment.ApplicationParameters.PathologyGroupClientMnem))
                && x.FinancialType == fin.FinClass).ToList();

            foreach (var chrg in chrgsToUpdate)
            {
                chrg.FinCode = finCode;
                chrg.FinancialType = fin.FinClass;
                unitOfWork.ChrgRepository.Update(chrg, new[] { nameof(Chrg.FinancialType), nameof(Chrg.FinCode) });
            }
            unitOfWork.Commit();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="clientMnem"></param>
        /// <returns></returns>
        public async Task<bool> UpdateChargesClientAsync(string account, string clientMnem) => await Task.Run(() => UpdateChargesClient(account, clientMnem));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="clientMnem"></param>
        /// <returns></returns>
        public bool UpdateChargesClient(string account, string clientMnem)
        {
            Log.Instance.Trace("Entering");

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            List<Chrg> charges = unitOfWork.ChrgRepository.GetByAccount(account);

            if (charges == null || charges.Count == 0)
                return false;

            var client = unitOfWork.ClientRepository.GetClient(clientMnem);
            if (client == null)
                return false;

            foreach (var chrg in charges)
            {
                chrg.ClientMnem = clientMnem;
                unitOfWork.ChrgRepository.Update(chrg, new[] { nameof(Chrg.ClientMnem) });
            }
            unitOfWork.Commit();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="serviceDate"></param>
        /// <param name="comment"></param>
        /// <param name="refNumber"></param>
        /// <param name="miscAmount"></param>
        /// <returns></returns>
        public async Task<int> AddChargeAsync(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
            => await Task.Run(() => AddCharge(account, cdm, qty, serviceDate, comment, refNumber, miscAmount));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="serviceDate"></param>
        /// <param name="comment"></param>
        /// <param name="refNumber"></param>
        /// <param name="miscAmount"></param>
        /// <returns></returns>
        /// <exception cref="AccountNotFoundException"></exception>
        public int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
        {
            Log.Instance.Trace($"Entering - account {account} cdm {cdm}");

            //verify the account exists
            Account accData = GetAccount(account);
            if (accData == null)
            {
                Log.Instance.Error($"Account {account} not found");
                throw new AccountNotFoundException("Account is not a valid account.", account);
            }

            return AddCharge(accData, cdm, qty, serviceDate, comment, refNumber, miscAmount);
        }


        public async Task<int> AddChargeAsync(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
            => await Task.Run(() => AddCharge(accData, cdm, qty, serviceDate, comment, refNumber, miscAmount));

        /// <summary>
        /// Add a charge to an account. The account must exist.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="comment"></param>
        /// <param name="refNumber"></param>
        /// <param name="miscAmount"></param>
        /// <returns>Charge number of newly entered charge or < 0 if an error occurs.</returns>
        public int AddCharge(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
        {
            Log.Instance.Trace($"Entering - account {accData.AccountNo} cdm {cdm}");

            if (accData.Client == null)
            {
                throw new InvalidClientException("Client not valid", accData.ClientMnem);
            }

            if (string.IsNullOrEmpty(accData.Client.FeeSchedule))
            {
                throw new ApplicationException($"Fee Schedule not defined on client. Cannot post charge. Client {accData.ClientMnem}");
            }

            using AccountUnitOfWork uow = new(appEnvironment, true);

            //check account status, change to NEW if it is paid out.
            if (accData.Status == AccountStatus.PaidOut)
            {
                UpdateStatus(accData.AccountNo, AccountStatus.New);
                accData.Status = AccountStatus.New;
            }

            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = uow.CdmRepository.GetCdm(cdm);
            if (cdmData == null)
            {
                Log.Instance.Error($"CDM {cdm} not found.");
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = uow.FinRepository.GetFin(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");
            Client chargeClient = accData.Client;

            if (appEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
            {
                //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
                var gb = uow.GlobalBillingCdmRepository.GetCdm(cdm);
                //hard coding exception for Hardin County for now - 05/09/2023 BSP
                if (gb != null && accData.ClientMnem != pthExceptionClient)
                {
                    fin = uow.FinRepository.GetFin("Y") ?? throw new ApplicationException($"Fin code Y not found error {accData.AccountNo}");
                    chargeClient = uow.ClientRepository.GetClient(appEnvironment.ApplicationParameters.PathologyGroupClientMnem);
                }
            }

            Chrg chrg = new()
            {
                Status = fin.FinClass switch
                {
                    patientFinType => cdmData.MClassType == naStatus ? naStatus : newChrgStatus,
                    clientFinType => cdmData.CClassType == naStatus ? naStatus : newChrgStatus,
                    "Z" => cdmData.ZClassType == naStatus ? naStatus : newChrgStatus,
                    _ => newChrgStatus,
                },

                //now build the charge & detail records
                AccountNo = accData.AccountNo,
                BillMethod = fin.ClaimType,
                CDMCode = cdm,
                Comment = comment,
                IsCredited = false,
                FinCode = fin.FinCode,
                ClientMnem = chargeClient.ClientMnem,
                FinancialType = fin.FinClass,
                OrderMnem = cdmData.Mnem,
                LISReqNo = refNumber,
                PostingDate = DateTime.Today,
                Quantity = qty,
                ServiceDate = serviceDate
            };

            List<CdmDetail> feeSched = null;

            feeSched = chargeClient.FeeSchedule switch
            {
                "1" => cdmData.CdmFeeSchedule1,
                "2" => cdmData.CdmFeeSchedule2,
                "3" => cdmData.CdmFeeSchedule3,
                "4" => cdmData.CdmFeeSchedule4,
                "5" => cdmData.CdmFeeSchedule5,
                _ => throw new ArgumentOutOfRangeException("FeeSchedule"),
            };


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
                    case patientFinType:
                        chrgDetail.Amount = fee.MClassPrice;
                        retailTotal += fee.MClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                    case clientFinType:
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

                if (cdmData.Variable)
                {
                    chrgDetail.Amount = miscAmount;
                }

                amtTotal += chrgDetail.Amount;

                chrgDetail.Modifier = fee.Modifier;
                chrgDetail.RevenueCode = fee.RevenueCode;
                chrgDetail.OrderCode = fee.BillCode;
                chrgDetail.DiagnosisPointer.DiagnosisPointer = "1:";

                chrg.ChrgDetails.Add(chrgDetail);
            }

            chrg.NetAmount = amtTotal;
            chrg.HospAmount = ztotal;
            chrg.RetailAmount = retailTotal;

            var retval = uow.ChrgRepository.AddCharge(chrg);
            chrg.ChrgId = Convert.ToInt32(retval);
            chrg.ChrgDetails.ForEach(d =>
            {
                d.ChrgNo = chrg.ChrgId;
                var detUri = uow.ChrgDetailRepository.Add(d);
                if (d.DiagnosisPointer != null)
                {
                    d.DiagnosisPointer.ChrgDetailUri = Convert.ToDouble(detUri);
                    uow.ChrgDiagnosisPointerRepository.Save(d.DiagnosisPointer);
                }
            });

            uow.Commit();
            Log.Instance.Trace($"Exiting");
            return retval;
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
                using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    CreditCharge(bundledProfile.ChrgId, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", 1, account.TransactionDate);
                    AddCharge(account, "5382522", 1, account.TransactionDate);
                    AddCharge(account, "5646008", 1, account.TransactionDate);
                }

                bundledProfiles = account.Charges.Where(x => x.CDMCode == "MCL0021" && x.IsCredited == false);

                foreach (var bundledProfile in bundledProfiles)
                {
                    //credit the profile charge
                    CreditCharge(bundledProfile.ChrgId, "Unbundling charge");

                    //enter charges for each component
                    AddCharge(account, "5545154", 1, account.TransactionDate);
                    AddCharge(account, "5646012", 1, account.TransactionDate);
                    AddCharge(account, "5646086", 1, account.TransactionDate);
                    AddCharge(account, "5646054", 1, account.TransactionDate);
                    AddCharge(account, "5728026", 1, account.TransactionDate);
                    AddCharge(account, "5728190", 1, account.TransactionDate);
                }

                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                //AbortTransaction();
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
                    new("85025", "5545154"),
                    new("80053", "5382522"),
                    new("84443", "5646008")
                }
            });

            bundledProfiles.Add(new BundledProfile()
            {
                ProfileCdm = "MCL0021",
                ComponentCpt = new List<BundledProfileComponent>()
                {
                    new("85025", "5545154"),
                    new("87340", "5646012"),
                    new("86762", "5646086"),
                    new("86592", "5686054"),
                    new("86850", "5728026"),
                    new("86900", "5728190"),
                    new("86901")
                }
            });

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

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
                        CreditCharge(bundledProfiles[x].ComponentCpt[i].ChrgId, $"Bundling to {bundledProfiles[x].ProfileCdm}");
                    }

                    this.AddCharge(account, bundledProfiles[x].ProfileCdm, 1, (DateTime)account.TransactionDate, $"Bundled by Rule");
                    this.AddNote(account.AccountNo, $"Bundled charges into {bundledProfiles[x].ProfileCdm}");
                }
            }

            unitOfWork.Commit();
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
                using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

                if ((account.Status == AccountStatus.InstSubmitted || account.Status == AccountStatus.ProfSubmitted
                    || account.Status == AccountStatus.ClaimSubmitted || account.Status == AccountStatus.Statements
                    || account.Status == AccountStatus.Closed || account.Status == AccountStatus.PaidOut) && !reprint)
                {
                    //account has been billed, do not validate
                    account.AccountValidationStatus.Account = account.AccountNo;
                    account.AccountValidationStatus.UpdatedDate = DateTime.Now;
                    account.AccountValidationStatus.ValidationText = "Account has already been billed. Did not validate.";
                    unitOfWork.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                    return false;
                }
                else
                {
                    if (account.Fin.FinClass == patientFinType && account.InsurancePrimary != null)
                    {
                        if (account.InsurancePrimary.InsCompany != null)
                        {
                            if (account.InsurancePrimary.InsCompany.IsMedicareHmo)
                                UnbundlePanels(account);
                            if (!account.InsurancePrimary.InsCompany.IsMedicareHmo)
                                BundlePanels(account);
                        }
                    }

                    Services.Validators.ClaimValidator claimValidator = new();
                    account.LmrpErrors = ValidateLMRP(account);
                    var validationResult = claimValidator.Validate(account);

                    bool isAccountValid = true;

                    account.AccountValidationStatus.Account = account.AccountNo;
                    account.AccountValidationStatus.UpdatedDate = DateTime.Now;

                    string lmrperrors = null;

                    if (!validationResult.IsValid)
                    {
                        isAccountValid = false;
                        account.AccountValidationStatus.ValidationText = validationResult.ToString() + Environment.NewLine;
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
                            account.AccountValidationStatus.ValidationText += error + Environment.NewLine;
                            lmrperrors += error + "\n";
                        }
                        if (!reprint)
                            UpdateStatus(account.AccountNo, AccountStatus.New);
                    }

                    if (isAccountValid)
                    {
                        account.AccountValidationStatus.ValidationText = "No validation errors.";
                        //update account status if this account has been flagged to bill
                        if (account.Status == AccountStatus.ReadyToBill)
                        {
                            UpdateStatus(account.AccountNo, account.BillForm);
                            //if this is a self-pay, set the statement flag
                            if (account.BillForm == AccountStatus.Statements)
                            {
                                unitOfWork.PatRepository.SetStatementFlag(account.AccountNo, "Y");
                            }
                        }
                    }

                    unitOfWork.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                    if (!string.IsNullOrEmpty(lmrperrors))
                    {
                        AccountLmrpError record = new()
                        {
                            AccountNo = account.AccountNo,
                            DateOfService = (DateTime)account.TransactionDate,
                            ClientMnem = account.ClientMnem,
                            FinancialCode = account.FinCode,
                            Error = lmrperrors
                        };

                        unitOfWork.AccountLmrpErrorRepository.Save(record);
                    }
                    unitOfWork.Commit();
                    return isAccountValid;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                using AccountUnitOfWork unitOfWork = new(appEnvironment);
                account.AccountValidationStatus.Account = account.AccountNo;
                account.AccountValidationStatus.UpdatedDate = DateTime.Now;
                account.AccountValidationStatus.ValidationText = "Exception during Validation. Unable to validate.";
                unitOfWork.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
            }

            return false;
        }

        private async Task<List<string>> ValidateLMRPAsync(Account account) => await Task.Run(() => ValidateLMRP(account));

        private List<string> ValidateLMRP(Account account)
        {
            Log.Instance.Trace($"Entering - account {account}");
            List<string> errorList = new List<string>();

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            //determine if there are any rules for ama_year
            if (unitOfWork.LmrpRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
            {
                // no lmrp rules loaded for this ama year. 
                errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
                return errorList;
            }

            foreach (var cpt4 in account.cpt4List.Distinct())
            {
                if (cpt4 == null)
                    continue;
                var ruleDef = unitOfWork.LmrpRuleRepository.GetRuleDefinition(cpt4, (DateTime)account.TransactionDate);
                if (ruleDef == null)
                    continue;

                bool dxIsValid = ruleDef.DxIsValid == 0 ? false : true;
                bool dxSupported = false;

                foreach (var dx in account.Pat.Diagnoses)
                {
                    var rule = unitOfWork.LmrpRuleRepository.GetRule(cpt4, dx.Code, (DateTime)account.TransactionDate);
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
            unitOfWork.Commit();
            return errorList;
        }

        public void ValidateUnbilledAccounts()
        {
            Log.Instance.Trace($"Entering");

            using AccountUnitOfWork uow = new(appEnvironment);

            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, DateTime.Today.AddDays((appEnvironment.ApplicationParameters.BillingInitialHoldDays)*-1).ToShortDateString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.New),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "Y"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, clientFinCode),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "E")
            };


            var accounts = uow.AccountSearchRepository.GetBySearch(parameters).ToList();

            foreach (var account in accounts)
            {
                try
                {
                    var accountRecord = this.GetAccount(account.Account);
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

            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            var source = GetAccount(sourceAccount);
            var destination = GetAccount(destinationAccount);
            if (source == null || destination == null)
            {
                Log.Instance.Error($"Either source or destination account was not valid. {sourceAccount} {destinationAccount}");
                return (false, "Either source or destination account was not valid.");
            }

            var charges = source.Charges.Where(c => c.IsCredited == false);

            foreach (var charge in charges)
            {
                CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
                AddCharge(destination, charge.CDMCode, charge.Quantity, (DateTime)destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
            }
            unitOfWork.Commit();
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
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            var source = GetAccount(sourceAccount);
            var destination = GetAccount(destinationAccount);

            var charge = source.Charges.SingleOrDefault(c => c.ChrgId == chrgId);

            if (charge.IsCredited)
            {
                throw new ApplicationException("Charge is already credited.");
            }

            CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
            AddCharge(destinationAccount, charge.CDMCode, charge.Quantity, destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
            unitOfWork.Commit();
        }

        public void AddRecentlyAccessedAccount(string accountNo, string userName)
        {
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            unitOfWork.UserProfileRepository.InsertRecentAccount(accountNo, userName);
        }

        public void AddPayment(Chk chk)
        {
            Log.Instance.Trace("Entering");
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            try
            {
                unitOfWork.ChkRepository.Add(chk);
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException("Error occurred in Add Payment", ex);
            }
        }

        public bool UpdateDiagnosisPointers(IEnumerable<Chrg> chrgs)
        {
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);

            if(UpdateDxPointers(chrgs))
            {
                unitOfWork.Commit();
                return true;
            }
            return false;
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
            using AccountUnitOfWork unitOfWork = new(appEnvironment, true);
            List<string> columns = new();
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

                unitOfWork.PatRepository.Update(account.Pat, columns);
                UpdateStatus(account.AccountNo, AccountStatus.New);

                AddNote(account.AccountNo, "Claim status cleared.");
                unitOfWork.Commit();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
            }
        }

        public int RemoveChargeModifier(int chrgDetailId)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);
            var retval = uow.ChrgDetailRepository.RemoveModifier(chrgDetailId);
            uow.Commit();
            return retval;
        }

        public int AddChargeModifier(int chrgDetailId, string modifier)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);
            var retval = uow.ChrgDetailRepository.AddModifier(chrgDetailId, modifier);
            uow.Commit();

            return retval;
        }

        public int CreditCharge(int chrgId, string comment = "")
        {
            using AccountUnitOfWork uow = new(appEnvironment);

            Log.Instance.Trace($"Entering - chrg number {chrgId} comment {comment}");

            bool setCreditFlag = false;
            bool setOldChrgCreditFlag = false;

            if (chrgId <= 0)
                throw new ArgumentOutOfRangeException(nameof(chrgId));

            var chrg = uow.ChrgRepository.GetById(chrgId) ?? throw new ApplicationException($"Charge number {chrgId} not found.");

            if (chrg.FinancialType == "M")
            {
                setCreditFlag = true;
                setOldChrgCreditFlag = true;
            }
            if (chrg.FinancialType == "C")
            {
                setCreditFlag = true;
                setOldChrgCreditFlag = true;
            }

            chrg.IsCredited = setCreditFlag;
            chrg.ChrgId = 0;
            chrg.Quantity *= -1;
            chrg.Comment = comment;
            chrg.Invoice = null;
            chrg.PostingDate = DateTime.Today;
            chrg.ChrgDetails.ForEach(x => x.ChrgNo = 0);

            int retVal = uow.ChrgRepository.AddCharge(chrg);
            if (setOldChrgCreditFlag)
                uow.ChrgRepository.SetCredited(chrgId);
            Log.Instance.Trace($"Credit charge number {chrgId} comment {comment} returned {retVal}");

            uow.Commit();
            return retVal;
        }

        public bool SetChargeCreditFlag(int chrgId, bool flag) 
        {
            using AccountUnitOfWork uow = new(appEnvironment);

            var retval = uow.ChrgRepository.SetCredited(chrgId, flag);
            return retval;
        }

        public bool SetCollectionsDate(Pat pat)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            var retval = uow.PatRepository.Update(pat, new[] { nameof(Pat.BadDebtListDate) });
            uow.Commit();

            return retval;
        }

        public IList<AccountSearch> SearchAccounts(string lastName, string firstName, string mrn, string ssn, string dob,
                string sex, string accountSearch)
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);
            var results = uow.AccountSearchRepository.GetBySearch(lastName, firstName, mrn, ssn, dob, sex, accountSearch).ToList();

            return results;
        }
    }
}
