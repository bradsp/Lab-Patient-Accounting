using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using Microsoft.Data.SqlClient;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Log = LabBilling.Logging.Log;
using Utilities;

namespace LabBilling.Core.Services;

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
    private const string billClientFinCode = "Y";
    private const string invalidFinCode = "K";
    private const string clientFinCode = "CLIENT";
    private const string pthExceptionClient = "HC";

    private readonly DictionaryService dictionaryService;

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

    /// <summary>
    /// Get only minimal account info. Only pulls data from acc table. Does not load charges or other detail info.
    /// </summary>
    /// <param name="accountNo"></param>
    /// <returns></returns>
    public Account GetAccountMinimal(string accountNo)
    {
        using AccountUnitOfWork uow = new(appEnvironment);
        var acc = uow.AccountRepository.GetByAccount(accountNo);
        return acc;
    }

    public async Task<Account> GetAccountAsync(string account, bool demographicsOnly = false) => await Task.Run(() => GetAccount(account, demographicsOnly));

    public (bool locksuccessful, AccountLock lockInfo) GetAccountLock(string account)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);

        var alock = uow.AccountLockRepository.GetLock(account);

        if (alock == null || string.IsNullOrEmpty(alock?.AccountNo))
        {
            //there is no lock on this account - establish one
            alock = uow.AccountLockRepository.Add(account);
            uow.Commit();
            return (true, alock);
        }
        else
        {
            // check to see if the lock is this user and machine
            if (alock.UpdatedUser == appEnvironment.User && alock.UpdatedHost == Environment.MachineName && alock.LockDateTime >= DateTime.Today)
            {
                uow.Commit();
                return (true, alock);
            }
        }
        uow.Commit();
        return (false, alock);
    }

    public bool ClearAccountLock(Account account)
    {
        UnitOfWorkMain uow = new(appEnvironment);
        if (account == null)
            throw new ArgumentNullException(nameof(account));
        if (account.AccountLockInfo == null)
        {
            account.AccountLockInfo = uow.AccountLockRepository.GetLock(account.AccountNo);
            if (account.AccountLockInfo == null)
            {
                Log.Instance.Warn($"Account Lock not found for {account.AccountNo}");
                return false;
            }
        }
        return ClearAccountLock(account.AccountLockInfo.id);
    }

    public bool ClearAccountLock(int id)
    {
        using AccountUnitOfWork uow = new(appEnvironment);
        var retval = uow.AccountLockRepository.Delete(id);
        return retval;
    }

    public Account GetAccount(string account, bool demographicsOnly = false)
    {
        Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

        using AccountUnitOfWork uow = new(appEnvironment);

        //get lock before loading account
        var (locksuccessful, alock) = GetAccountLock(account);
        if (!locksuccessful)
        {
            throw new AccountLockException(alock);
        }

        Account record = null;
        try
        {
            record = uow.AccountRepository.GetByAccount(account);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
        }

        if (record == null)
        {
            ClearAccountLock(alock.id);
            return null;
        }

        record.AccountLockInfo = alock;
        if (record == null)
            return null;

        if (!string.IsNullOrEmpty(record.ClientMnem))
        {
            if (record.ClientMnem != invalidFinCode)
            {
                record.Client = uow.ClientRepository.GetClient(record.ClientMnem);
            }
        }
        record.Pat = uow.PatRepository.GetByAccount(record);

        if (!demographicsOnly)
        {
            record.Charges = GetCharges(account, true, true, null, false).ToList();
            record.Payments = uow.ChkRepository.GetByAccount(account);
            record.Insurances = uow.InsRepository.GetByAccount(account);
            record.Notes = uow.AccountNoteRepository.GetByAccount(account);
            record.BillingActivities = uow.BillingActivityRepository.GetByAccount(account);
            record.AccountValidationStatus = uow.AccountValidationStatusRepository.GetByAccount(account);
            record.Fin = uow.FinRepository.GetFin(record.FinCode);
            record.AccountAlert = uow.AccountAlertRepository.GetByAccount(account);
            record.PatientStatements = uow.PatientStatementAccountRepository.GetByAccount(account);

            DateTime outpBillStartDate;
            DateTime questStartDate = new(2012, 10, 1);
            DateTime questEndDate = new(2020, 5, 31);
            DateTime arbitraryEndDate = new(2016, 12, 31);

            if (record.Client != null)
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

        return record;
    }

    public double GetBalance(string accountNo)
    {
        using AccountUnitOfWork uow = new(appEnvironment);

        string chrgTableName = uow.ChrgRepository.TableInfo.TableName;
        string accTableName = uow.AccountRepository.TableInfo.TableName;
        string chkTableName = uow.ChkRepository.TableInfo.TableName;

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        try
        {
            var sql = Sql.Builder
                .Select(new[]
                {
                    $"coalesce(sum({uow.ChrgRepository.GetRealColumn(nameof(Chrg.Quantity))} * {uow.ChrgRepository.GetRealColumn(nameof(Chrg.NetAmount))}), 0.00) as 'Total'"
                })
                .From(chrgTableName)
                .InnerJoin(accTableName)
                .On($"{accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} = {chrgTableName}.{uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))}")
                .Where($"{chrgTableName}.{uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo })
                .Where($"(({accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.FinCode))} = @0 and {chrgTableName}.{uow.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} <> @1)",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cbillStatus })
                .Append($"or ({accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.FinCode))} <> @0 and {chrgTableName}.{uow.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} not in (@1, @2, @3)))",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = cbillStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = capStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = naStatus })
                .GroupBy(chrgTableName + "." + uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo)));

            double charges = uow.Context.SingleOrDefault<double>(sql);

            sql = Sql.Builder
                .Select(new[]
                {
                    $"coalesce(sum({uow.ChkRepository.GetRealColumn(nameof(Chk.PaidAmount))} + {uow.ChkRepository.GetRealColumn(nameof(Chk.ContractualAmount))} + {uow.ChkRepository.GetRealColumn(nameof(Chk.WriteOffAmount))}), 0.00) as 'Total'"
                })
                .From(uow.ChkRepository.TableInfo.TableName)
                .Where($"{chkTableName}.{uow.ChkRepository.GetRealColumn(nameof(Chk.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo })
                .GroupBy(uow.ChkRepository.GetRealColumn(nameof(Chk.AccountNo)));

            double adj = (double)uow.Context.SingleOrDefault<double>(sql);

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

        using AccountUnitOfWork uow = new(appEnvironment, true);

        if (table.FinCode != "CLIENT")
            table.PatFullName = table.PatNameDisplay;

        table.Status = AccountStatus.New;

        table.TransactionDate = table.TransactionDate.Date;

        uow.AccountRepository.Add(table);

        //make sure Pat record has an account number
        if (table.Pat.AccountNo != table.AccountNo)
            table.Pat.AccountNo = table.AccountNo;

        var pat = uow.PatRepository.GetByAccount(table);
        if (pat == null)
            uow.PatRepository.Add(table.Pat);
        else
            uow.PatRepository.Update(table.Pat);

        table.Insurances.ForEach(ins =>
        {
            if (ins.Account != table.AccountNo)
                ins.Account = table.AccountNo;

            uow.InsRepository.Save(ins);
        });
        var result = GetAccountLock(table.AccountNo);
        if (result.locksuccessful)
            table.AccountLockInfo = result.lockInfo;
        uow.Commit();
        return table;
    }

    public async Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate) => await Task.Run(() => GetInvoiceAccounts(clientMnem, thruDate));

    public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
    {
        Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");
        using AccountUnitOfWork uow = new(appEnvironment);
        return uow.InvoiceSelectRepository.GetByClientAndDate(clientMnem, thruDate);
    }

    public async Task<IEnumerable<ClaimItem>> GetAccountsForClaimsAsync(ClaimType claimType, int maxClaims = 0) => await Task.Run(() => GetAccountsForClaims(claimType, maxClaims));

    public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType, int maxClaims = 0)
    {
        Log.Instance.Trace($"Entering - claimType {claimType}");
        try
        {
            var queryResult = GetClaimItems(claimType);
            return queryResult;
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex, $"Exception in");
        }
        Log.Instance.Trace("Exiting");

        return new List<ClaimItem>();
    }

    public List<ClaimItem> GetClaimItems(ClaimType claimType)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        PetaPoco.Sql command;

        string selMaxRecords = string.Empty;
        string accTableName = uow.AccountRepository.TableName;
        string insTableName = uow.InsRepository.TableName;

        var selectCols = new[]
        {
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.Status)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.AccountNo)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.PatName)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.SocSecNum)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.ClientMnem)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.FinCode)),
            accTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.TransactionDate)),
            insTableName + "." + uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.InsPlanName))
        };

        command = Sql.Builder
            .Select(selectCols)
            .From(accTableName)
            .InnerJoin(insTableName)
            .On($"{insTableName}.{uow.InsRepository.GetRealColumn(nameof(Ins.Account))} = {accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} and {uow.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = '{InsCoverage.Primary}'");

        try
        {
            switch (claimType)
            {
                case ClaimType.Institutional:
                    command.Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Institutional });
                    break;
                case ClaimType.Professional:
                    command.Where("status = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = AccountStatus.Professional });
                    break;
                default:
                    command = PetaPoco.Sql.Builder;
                    break;
            }

            command.OrderBy($"{uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.TransactionDate))}");

            var queryResult = uow.ClaimItemRepository.Fetch(command).ToList();

            if (appEnvironment.ApplicationParameters.MaxClaimsInClaimBatch > 0)
                return queryResult.Take(appEnvironment.ApplicationParameters.MaxClaimsInClaimBatch).ToList();
            else
                return queryResult;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            return new List<ClaimItem>();
        }
    }


    public List<PatientStatementAccount> GetPatientStatements(string accountNo)
    {
        using AccountUnitOfWork uow = new(appEnvironment);

        var statements = uow.PatientStatementAccountRepository.GetByAccount(accountNo);

        return statements;
    }

    public async Task<bool> SetNoteAlertAsync(string account, bool showAlert) => await Task.Run(() => SetNoteAlert(account, showAlert));

    public bool SetNoteAlert(string account, bool showAlert)
    {
        Log.Instance.Trace($"Entering - account {account} showAlert {showAlert}");
        using AccountUnitOfWork uow = new(appEnvironment, true);
        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        try
        {
            var record = uow.AccountAlertRepository.GetByAccount(account);

            if (record == null)
            {
                record = new AccountAlert
                {
                    AccountNo = account,
                    Alert = showAlert
                };

                uow.AccountAlertRepository.Add(record);
            }
            else
            {
                record.Alert = showAlert;
                uow.AccountAlertRepository.Update(record);
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error updating NoteAlert.");
            return false;
        }

        uow.Commit();
        return true;
    }


    public Account UpdateAccountDemographics(Account acc)
    {
        Log.Instance.Trace("Entering");
        using AccountUnitOfWork uow = new(appEnvironment, true);
        try
        {
            var accResult = uow.AccountRepository.Update(acc);
            var patResult = uow.PatRepository.Save(acc.Pat);
            uow.Commit();

            accResult.Pat = patResult;
            return accResult;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error saving records", ex);
        }
    }

    public async Task<Account> UpdateDiagnosesAsync(Account acc) => await Task.Run(() => UpdateDiagnoses(acc));

    public Account UpdateDiagnoses(Account acc)
    {
        Log.Instance.Trace("Entering");
        using AccountUnitOfWork uow = new(appEnvironment, true);
        if (acc == null)
            throw new ArgumentNullException(nameof(acc));

        var patResult = uow.PatRepository.SaveDiagnoses(acc.Pat);
        acc.Pat = patResult;
        uow.Commit();

        return acc;
    }

    public async Task<int> UpdateStatusAsync(string accountNo, string status) => await Task.Run(() => UpdateStatus(accountNo, status));
    public async Task<Account> UpdateStatusAsync(Account model, string status) => await Task.Run(() => UpdateStatus(model, status));

    public int UpdateStatus(string accountNo, string status)
    {
        Log.Instance.Trace($"Entering - account {accountNo} status {status}");
        using AccountUnitOfWork uow = new(appEnvironment);

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(status))
            throw new ArgumentNullException(nameof(status));
        if (!AccountStatus.IsValid(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

        var returnval = uow.AccountRepository.UpdateStatus(accountNo, status);

        return returnval;
    }

    public Account UpdateStatus(Account model, string status)
    {
        using AccountUnitOfWork uow = new(appEnvironment);
        try
        {
            var result = uow.AccountRepository.UpdateStatus(model, status);
            return result;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error updating status.", ex);
        }
    }

    public Account UpdateStatementFlag(Account acc, string flag)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);

        try
        {
            AddNote(acc.AccountNo, $"Statement flag changed from {acc.Pat.StatementFlag} to {flag}");
            acc.Pat.StatementFlag = flag;
            if (flag != "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.Statements);
                acc.Status = AccountStatus.Statements;
            }
            acc.Pat = uow.PatRepository.Update(acc.Pat, new[] { nameof(Pat.StatementFlag) });
            uow.Commit();
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error updating statement flag.", ex);
        }
        return acc;
    }

    public Ins SaveInsurance(Ins ins)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);
        try
        {
            Ins returnIns;
            if (ins.rowguid == Guid.Empty)
                returnIns = uow.InsRepository.Add(ins);
            else
                returnIns = uow.InsRepository.Update(ins);

            uow.Commit();
            return returnIns;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error updating insurance record.", ex);
        }
    }

    public bool DeleteInsurance(Ins ins)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);
        var retval = uow.InsRepository.Delete(ins);
        uow.Commit();
        return retval;
    }

    public async Task<Account> InsuranceSwapAsync(Account account, InsCoverage swap1, InsCoverage swap2) => await Task.Run(() => InsuranceSwap(account, swap1, swap2));

    public Account InsuranceSwap(Account account, InsCoverage swap1, InsCoverage swap2)
    {
        Log.Instance.Trace($"Entering - Account {account.AccountNo} Ins1 {swap1} Ins2 {swap2}");

        Ins insA = account.Insurances.Where(x => x.Coverage == swap1).FirstOrDefault();
        Ins insB = account.Insurances.Where(x => x.Coverage == swap2).FirstOrDefault();

        using AccountUnitOfWork uow = new(appEnvironment, true);

        try
        {
            insA.Coverage = InsCoverage.Temporary;
            insB.Coverage = swap1;

            insA = uow.InsRepository.Update(insA);
            insB = uow.InsRepository.Update(insB);

            insA.Coverage = swap2;
            insA = uow.InsRepository.Update(insA);

            account.Notes = AddNote(account.AccountNo, $"Insurance swap: {swap1} {insA.PlanName} and {swap2} {insB.PlanName}").ToList();
            account.Insurances = uow.InsRepository.GetByAccount(account.AccountNo);
            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error swapping insurances.");
            throw new ApplicationException("Error during insurance swap");
        }

    }

    public async Task<Account> ChangeDateOfServiceAsync(Account table, DateTime newDate, string reason_comment) => await Task.Run(() => ChangeDateOfService(table, newDate, reason_comment));

    public Account ChangeDateOfService(Account model, DateTime newDate, string reason_comment)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new date {newDate} reason {reason_comment}");
        using AccountUnitOfWork uow = new(appEnvironment, true);

        if (model == null)
            throw new ArgumentNullException(nameof(model));
        if (reason_comment == null)
            throw new ArgumentNullException(nameof(reason_comment));

        DateTime oldServiceDate = DateTime.MinValue;

        // update trans_date on acc table
        if (model.TransactionDate != newDate)
        {
            oldServiceDate = (DateTime)model.TransactionDate;
            model.TransactionDate = newDate;
            model = uow.AccountRepository.Update(model, new[] { nameof(Account.TransactionDate) });

            model.Notes = AddNote(model.AccountNo, $"Service Date changed from {oldServiceDate} to {newDate}").ToList();

            //determine if charges need to be reprocessed.

            //TODO: is there any reason a date of service change should result in changing all charges --
            // except: the date of service on charges will not match new date.

            // option: reprocess all charges, or update service date on charge records

            model.Charges.ForEach(c =>
            {
                c.ServiceDate = newDate;
                c = uow.ChrgRepository.Update(c);
            });

            uow.Commit();
            return model;
        }
        else
        {
            //no change made - transaction date same as new date
            Log.Instance.Warn($"Transaction Date same as new date. No change made. Account {model.AccountNo}, new date {newDate}, reason {reason_comment}");
            return model;
        }
    }

    public async Task<IList<AccountNote>> AddNoteAsync(string account, string noteText) => await Task.Run(() => AddNote(account, noteText));

    public IList<AccountNote> AddNote(string accountNo, string noteText)
    {
        Log.Instance.Trace($"Entering - account {accountNo} note {noteText}");

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(noteText))
            throw new ArgumentNullException(nameof(noteText));

        using AccountUnitOfWork uow = new(appEnvironment, true);

        AccountNote accountNote = new()
        {
            Account = accountNo,
            Comment = noteText
        };
        try
        {
            uow.AccountNoteRepository.Add(accountNote);
            uow.Commit();
            return uow.AccountNoteRepository.GetByAccount(accountNo);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error adding account note.");
            throw new ApplicationException("Error adding account note.", ex);
        }
    }

    public List<AccountNote> GetNotes(string accountNo)
    {
        using AccountUnitOfWork uow = new(appEnvironment);
        var notes = uow.AccountNoteRepository.GetByAccount(accountNo);
        return notes;
    }

    public async Task<Account> ChangeFinancialClassAsync(string accountNo, string newFinCode) => await Task.Run(() => ChangeFinancialClass(accountNo, newFinCode));

    public Account ChangeFinancialClass(string accountNo, string newFinCode)
    {
        Log.Instance.Trace($"Entering - Account {accountNo} New Fin {newFinCode}");
        using AccountUnitOfWork uow = new(appEnvironment, true);

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(newFinCode))
            throw new ArgumentNullException(nameof(newFinCode));

        var record = GetAccount(accountNo);

        var retval = ChangeFinancialClass(record, newFinCode);
        uow.Commit();
        return retval;
    }

    public async Task<Account> ChangeFinancialClassAsync(Account model, string newFinCode) => await Task.Run(() => ChangeFinancialClass(model, newFinCode));

    public Account ChangeFinancialClass(Account model, string newFinCode)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new fin {newFinCode}");
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        else if (newFinCode == null)
            throw new ArgumentNullException(nameof(newFinCode));

        if (model.FinCode == newFinCode)
        {
            throw new ApplicationException($"Chosen fin code is same as current fin code. No change made. Account {model.AccountNo}, Current Fin Code {model.FinCode}, New Fin Code {newFinCode}");
        }

        using AccountUnitOfWork uow = new(appEnvironment, true);

        string oldFinCode = model.FinCode;

        //check that newFincode is a valid fincode
        Fin newFin = uow.FinRepository.GetFin(newFinCode);
        Fin oldFin = model.Fin;

        if (newFin == null)
            throw new ArgumentException($"Financial code {newFinCode} is not valid code.", nameof(newFinCode));

        if (oldFinCode != newFinCode)
        {
            model.FinCode = newFinCode;
            try
            {
                uow.AccountRepository.Update(model, new[] { nameof(Account.FinCode) });
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException($"Exception updating fin code for {model.AccountNo}.", ex);
            }
            model.Notes = AddNote(model.AccountNo, $"Financial code updated from {oldFinCode} to {newFinCode}.").ToList();

            //reprocess charges if needed due to financial code change.
            if (newFin.FinClass != oldFin.FinClass)
            {
                try
                {
                    model.Charges = ReprocessCharges(model, $"Fin Code changed from {oldFinCode} to {newFinCode}").ToList();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error reprocessing charges.", ex);
                }
            }
            else
            {
                model.Charges = UpdateChargesFinCode(model.Charges, newFinCode).ToList();
            }
            uow.Commit();
        }

        Log.Instance.Trace($"Exiting");
        return model;
    }

    public async Task<bool> ChangeClientAsync(Account table, string newClientMnem) => await Task.Run(() => ChangeClient(table, newClientMnem));

    public bool ChangeClient(Account model, string newClientMnem)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new client {newClientMnem}");
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        else if (newClientMnem == null)
            throw new ArgumentNullException(nameof(newClientMnem));

        using AccountUnitOfWork uow = new(appEnvironment, true);

        bool updateSuccess = true;
        string oldClientMnem = model.ClientMnem;

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

                model.ClientMnem = newClientMnem;
                model.Client = newClient;
                try
                {
                    uow.AccountRepository.Update(model, new[] { nameof(Account.ClientMnem) });
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    throw new ApplicationException($"Exception updating client for {model.AccountNo}.", ex);
                }
                AddNote(model.AccountNo, $"Client updated from {oldClientMnem} to {newClientMnem}.");

                //reprocess charges if fin class is client bill (C) to pick up proper discounts.
                if (model.Fin.FinClass == clientFinType)
                {
                    try
                    {
                        ReprocessCharges(model.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }

                if (model.Fin.FinClass == patientFinType)
                {
                    //reprocess charges if fee schedule is different to pick up correct charge amounts
                    if (oldClient.FeeSchedule != newClient.FeeSchedule)
                    {
                        try
                        {
                            ReprocessCharges(model.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
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
                            ReprocessCharges(model.AccountNo, $"Client changed from {oldClientMnem} to {newClientMnem}");
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
                            UpdateChargesClient(model.AccountNo, newClientMnem);
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

        if (charges.Count > 10) //consider parallel processing if number of charges is significant.
        {
            charges.AsParallel().ForAll(chrg => AddRevenueDiagnosisToChrg(chrg));
        }
        else
        {
            charges.ForEach(chrg => AddRevenueDiagnosisToChrg(chrg));
        }

        return charges;
    }

    internal void AddRevenueDiagnosisToChrg(Chrg chrg)
    {
        AccountUnitOfWork uow = new(appEnvironment);

        chrg.Cdm = dictionaryService.GetCdm(chrg.CDMCode, true);
        chrg.ChrgDetails = uow.ChrgDetailRepository.GetByChrgId(chrg.ChrgId).ToList();
        chrg.ChrgDetails.ForEach(detail =>
        {
            detail.RevenueCodeDetail = dictionaryService.GetRevenueCode(detail.RevenueCode);
            detail.DiagnosisPointer = uow.ChrgDiagnosisPointerRepository.GetById(detail.uri);
            var cpt = uow.CptAmaRepository.GetCpt(detail.Cpt4);
            if (cpt != null)
                detail.CptDescription = cpt.ShortDescription;
        });

    }

    public IList<Chrg> UpdateDxPointers(List<Chrg> chrgs)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);

        chrgs.ForEach(c => c.ChrgDetails.ForEach(cd =>
        {
            cd.DiagnosisPointer = uow.ChrgDiagnosisPointerRepository.Save(cd.DiagnosisPointer);
        }));

        uow.Commit();
        return chrgs;
    }

    public async Task<IList<Chrg>> ReprocessChargesAsync(Account account, string comment) => await Task.Run(() => ReprocessCharges(account, comment));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public IList<Chrg> ReprocessCharges(Account account, string comment)
    {
        Log.Instance.Trace($"Entering {account}");

        if (account == null)
            throw new ArgumentNullException(nameof(account));

        using AccountUnitOfWork uow = new(appEnvironment, true);
        try
        {
            var chargesToCredit = account.Charges.Where(x => x.IsCredited == false && x.CDMCode != invoicedCdm).ToList();

            chargesToCredit.ForEach(c =>
            {
                CreditCharge(c.ChrgId, comment);
                AddCharge(account, c.CDMCode, c.Quantity, account.TransactionDate);
            });

            var updatedChrgList = uow.ChrgRepository.GetByAccount(account.AccountNo);

            uow.Commit();
            return updatedChrgList;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            //AbortTransaction();
            throw new ApplicationException("Error reprocessing charges", ex);
        }
    }

    public async Task<IList<Chrg>> ReprocessChargesAsync(string account, string comment) => await Task.Run(() => ReprocessCharges(account, comment));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public IList<Chrg> ReprocessCharges(string account, string comment)
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

    public async Task<IList<Chrg>> UpdateChargesFinCodeAsync(IList<Chrg> charges, string finCode) => await Task.Run(() => UpdateChargesFinCode(charges, finCode));

    public IList<Chrg> UpdateChargesFinCode(IList<Chrg> charges, string finCode)
    {
        Log.Instance.Trace("Entering");

        using AccountUnitOfWork uow = new(appEnvironment, true);

        if (charges == null || charges.Count == 0)
            throw new ApplicationException($"No charges for account {charges.First().AccountNo}");

        var fin = uow.FinRepository.GetFin(finCode) ?? throw new ApplicationException($"Fin {finCode} is not valid");

        var chrgsToUpdate = charges.Where(x => x.IsCredited == false &&
            (x.ClientMnem != appEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
            string.IsNullOrEmpty(appEnvironment.ApplicationParameters.PathologyGroupClientMnem))
            && x.FinancialType == fin.FinClass).ToList();

        chrgsToUpdate.ForEach(c =>
        {
            c.FinCode = finCode;
            c.FinancialType = fin.FinClass;
            c = uow.ChrgRepository.Update(c, new[] { nameof(Chrg.FinancialType), nameof(Chrg.FinCode) });
        });

        uow.Commit();
        return GetCharges(charges.First().AccountNo).ToList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="clientMnem"></param>
    /// <returns></returns>
    public async Task<IList<Chrg>> UpdateChargesClientAsync(string account, string clientMnem) => await Task.Run(() => UpdateChargesClient(account, clientMnem));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="clientMnem"></param>
    /// <returns></returns>
    public IList<Chrg> UpdateChargesClient(string account, string clientMnem)
    {
        Log.Instance.Trace("Entering");

        using AccountUnitOfWork uow = new(appEnvironment, true);

        List<Chrg> charges = GetCharges(account).ToList();

        if (charges == null || charges.Count == 0)
            throw new ApplicationException($"No charges for account {account}");

        var client = uow.ClientRepository.GetClient(clientMnem) ?? throw new ApplicationException($"Client {clientMnem} not valid.");
        charges.ForEach(c =>
        {
            c.ClientMnem = clientMnem;
            c = uow.ChrgRepository.Update(c, new[] { nameof(Chrg.ClientMnem) });
        });

        uow.Commit();
        return charges;
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
    public async Task<Account> AddChargeAsync(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
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
    public Account AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
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


    public Chrg AddCharge(Chrg chrg)
    {
        Log.Instance.Trace("Entering");
        using UnitOfWorkMain uow = new(appEnvironment);

        var newChrg = uow.ChrgRepository.Add(chrg);

        chrg.ChrgDetails.ForEach(cd =>
        {
            cd.ChrgNo = newChrg.ChrgId;
            cd = uow.ChrgDetailRepository.Add(cd);
            newChrg.ChrgDetails.Add(cd);
        });
        newChrg.Cdm = chrg.Cdm;

        return newChrg;
    }

    public async Task<Account> AddChargeAsync(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
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
    public Account AddCharge(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00)
    {
        Log.Instance.Trace($"Entering - account {accData.AccountNo} cdm {cdm}");

        if (accData.Client == null)
            throw new InvalidClientException("Client not valid", accData.ClientMnem);

        if (string.IsNullOrEmpty(accData.Client.FeeSchedule))
            throw new ApplicationException($"Fee Schedule not defined on client. Cannot post charge. Client {accData.ClientMnem}");

        Fin fin = dictionaryService.GetFinCode(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");

        //get the cdm number - if cdm number is not found - abort
        Cdm cdmData = dictionaryService.GetCdm(cdm);
        if (cdmData == null)
        {
            Log.Instance.Error($"CDM {cdm} not found.");
            throw new CdmNotFoundException("CDM not found.", cdm);
        }

        using AccountUnitOfWork uow = new(appEnvironment, true);

        //check account status, change to NEW if it is paid out.
        if (accData.Status == AccountStatus.PaidOut)
        {
            UpdateStatus(accData.AccountNo, AccountStatus.New);
            accData.Status = AccountStatus.New;
        }

        Client chargeClient = accData.Client;

        if (appEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
        {
            //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
            var gb = uow.GlobalBillingCdmRepository.GetCdm(cdm, accData.TransactionDate);
            //hard coding exception for Hardin County for now - 05/09/2023 BSP
            if (gb != null && accData.ClientMnem != pthExceptionClient)
            {
                fin = uow.FinRepository.GetFin(billClientFinCode) ?? throw new ApplicationException($"Fin code {billClientFinCode} not found error {accData.AccountNo}");
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
        List<ChrgDetail> chrgDetails = new();
        foreach (CdmDetail fee in feeSched)
        {
            ChrgDetail chrgDetail = new()
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
                case zFinType:
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
            var cpt = uow.CptAmaRepository.GetCpt(chrgDetail.Cpt4);
            if (cpt != null)
                chrgDetail.CptDescription = cpt.ShortDescription;

            chrgDetails.Add(chrgDetail);
        }

        chrg.NetAmount = amtTotal;
        chrg.HospAmount = ztotal;
        chrg.RetailAmount = retailTotal;

        chrg = uow.ChrgRepository.Add(chrg);
        chrg.Cdm = dictionaryService.GetCdm(chrg.CDMCode);
        chrgDetails.ForEach(d =>
        {
            d.ChrgNo = chrg.ChrgId;
            d = uow.ChrgDetailRepository.Add(d);
            if (d.DiagnosisPointer != null)
            {
                d.DiagnosisPointer.ChrgDetailUri = Convert.ToDouble(d.uri);
                uow.ChrgDiagnosisPointerRepository.Save(d.DiagnosisPointer);
            }
        });
        chrg.ChrgDetails = chrgDetails;

        accData.Charges.Add(chrg);

        uow.Commit();
        Log.Instance.Trace($"Exiting");
        return accData;
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

    public async Task<Account> UnbundlePanelsAsync(Account account) => await Task.Run(() => UnbundlePanels(account));

    public Account UnbundlePanels(Account account)
    {
        var bundledProfiles1 = account.Charges.Where(x => x.CDMCode == "MCL0029" && x.IsCredited == false);
        var bundledProfiles2 = account.Charges.Where(x => x.CDMCode == "MCL0021" && x.IsCredited == false);
        if (!bundledProfiles1.Any() && !bundledProfiles2.Any())
        {
            //nothing to bundle - return
            return account;
        }

        try
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            foreach (var bundledProfile in bundledProfiles1)
            {
                //credit the profile charge
                CreditCharge(bundledProfile.ChrgId, "Unbundling charge");

                //enter charges for each component
                AddCharge(account, "5545154", 1, account.TransactionDate);
                AddCharge(account, "5382522", 1, account.TransactionDate);
                AddCharge(account, "5646008", 1, account.TransactionDate);
            }


            foreach (var bundledProfile in bundledProfiles2)
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

            account.Charges = GetCharges(account.AccountNo).ToList();

            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error during UnbundlePanels", ex);
        }

    }

    public async Task<Account> BundlePanelsAsync(Account account) => await Task.Run(() => BundlePanels(account));

    public Account BundlePanels(Account account)
    {
        List<BundledProfile> bundledProfiles = new()
        {
            new BundledProfile()
            {
                ProfileCdm = "MCL0029",
                ComponentCpt = new List<BundledProfileComponent>()
                {
                    new("85025", "5545154"),
                    new("80053", "5382522"),
                    new("84443", "5646008")
                }
            },
            new BundledProfile()
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
            }
        };

        using AccountUnitOfWork uow = new(appEnvironment, true);

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
                account.Notes = this.AddNote(account.AccountNo, $"Bundled charges into {bundledProfiles[x].ProfileCdm}").ToList();
            }
        }
        account.Charges = GetCharges(account.AccountNo).ToList();

        uow.Commit();
        return account;
    }

    public async Task<Account> ValidateAsync(Account account, bool reprint = false) => await Task.Run(() => Validate(account, reprint));

    /// <summary>
    /// Runs all validation routines on account. Updates validation status and account flags. Errors are stored in the validation status table.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="reprint">Set true if validating account to resubmit the claim with no changes.</param>
    /// <returns>True if account is valid for billing, false if there are validation errors.</returns>
    public Account Validate(Account account, bool reprint = false)
    {
        Log.Instance.Trace($"Entering - account {account}");

        try
        {
            using AccountUnitOfWork uow = new(appEnvironment, true);

            if ((account.Status == AccountStatus.InstSubmitted || account.Status == AccountStatus.ProfSubmitted
                || account.Status == AccountStatus.ClaimSubmitted || account.Status == AccountStatus.Statements
                || account.Status == AccountStatus.Closed || account.Status == AccountStatus.PaidOut) && !reprint)
            {
                //account has been billed, do not validate
                account.AccountValidationStatus.Account = account.AccountNo;
                account.AccountValidationStatus.UpdatedDate = DateTime.Now;
                account.AccountValidationStatus.ValidationText = "Account has already been billed. Did not validate.";
                uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                uow.Commit();
                return account;
            }
            else
            {
                if (account.Fin.FinClass == patientFinType && account.InsurancePrimary != null)
                {
                    if (account.InsurancePrimary.InsCompany != null)
                    {
                        if (account.InsurancePrimary.InsCompany.IsMedicareHmo)
                            account = UnbundlePanels(account);
                        if (!account.InsurancePrimary.InsCompany.IsMedicareHmo)
                            account = BundlePanels(account);
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
                    {
                        account.Status = AccountStatus.New;
                        UpdateStatus(account.AccountNo, AccountStatus.New);
                    }
                }

                //only run LMRP on A fin_code
                if (account.LmrpErrors.Count > 0 && account.FinCode == "A")
                {
                    isAccountValid = false;
                    account.LmrpErrors.ForEach(error =>
                    {
                        account.AccountValidationStatus.ValidationText += error + Environment.NewLine;
                        lmrperrors += error + "\n";
                    });

                    if (!reprint)
                    {
                        account.Status = AccountStatus.New;
                        UpdateStatus(account.AccountNo, AccountStatus.New);
                    }
                }

                if (isAccountValid)
                {
                    account.AccountValidationStatus.ValidationText = "No validation errors.";
                    //update account status if this account has been flagged to bill
                    if (account.Status == AccountStatus.ReadyToBill)
                    {
                        account.Status = account.BillForm;
                        UpdateStatus(account.AccountNo, account.BillForm);
                        //if this is a self-pay, set the statement flag
                        if (account.BillForm == AccountStatus.Statements)
                        {
                            uow.PatRepository.SetStatementFlag(account.AccountNo, "Y");
                            account.Pat.StatementFlag = "Y";
                        }
                    }
                }

                uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
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

                    uow.AccountLmrpErrorRepository.Save(record);
                }
                uow.Commit();
                return account;
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            using AccountUnitOfWork uow = new(appEnvironment);
            account.AccountValidationStatus.Account = account.AccountNo;
            account.AccountValidationStatus.UpdatedDate = DateTime.Now;
            account.AccountValidationStatus.ValidationText = "Exception during Validation. Unable to validate.";
            uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
            return account;
        }
    }

    private async Task<List<string>> ValidateLMRPAsync(Account account) => await Task.Run(() => ValidateLMRP(account));

    private List<string> ValidateLMRP(Account account)
    {
        Log.Instance.Trace($"Entering - account {account}");
        List<string> errorList = new();

        using AccountUnitOfWork uow = new(appEnvironment, true);

        //determine if there are any rules for ama_year
        if (uow.LmrpRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
        {
            // no lmrp rules loaded for this ama year. 
            errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
            return errorList;
        }

        foreach (var cpt4 in account.cpt4List.Distinct())
        {
            if (cpt4 == null)
                continue;
            var ruleDef = uow.LmrpRuleRepository.GetRuleDefinition(cpt4, (DateTime)account.TransactionDate);
            if (ruleDef == null)
                continue;

            bool dxIsValid = ruleDef.DxIsValid == 0 ? false : true;
            bool dxSupported = false;

            foreach (var dx in account.Pat.Diagnoses)
            {
                var rule = uow.LmrpRuleRepository.GetRule(cpt4, dx.Code, (DateTime)account.TransactionDate);
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
        uow.Commit();
        return errorList;
    }

    public event EventHandler<ValidationUpdatedEventArgs> ValidationAccountUpdated;

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

        ValidationAccountUpdated?.Invoke(this, new ValidationUpdatedEventArgs() { UpdateMessage = "Compiling accounts", TimeStamp = DateTime.Now });
        var accounts = uow.AccountSearchRepository.GetBySearch(parameters).ToList();
        int p = 0;
        int t = accounts.Count;
        ValidationAccountUpdated?.Invoke(this, new ValidationUpdatedEventArgs() { UpdateMessage = "Compiled accounts", TimeStamp = DateTime.Now, Processed = p, TotalItems = t });

        accounts.AsParallel().ForAll(account =>
        {
            try
            {
                var accountRecord = this.GetAccount(account.Account);
                this.Validate(accountRecord);
                ClearAccountLock(accountRecord);
                p++;
                ValidationAccountUpdated?.Invoke(this, new ValidationUpdatedEventArgs()
                {
                    AccountNo = accountRecord.AccountNo,
                    ValidationStatus = accountRecord.AccountValidationStatus.ValidationText,
                    TimeStamp = DateTime.Now,
                    Processed = p,
                    TotalItems = t,
                    UpdateMessage = $"Thread {Environment.CurrentManagedThreadId}"
                });
            }
            catch (Exception e)
            {
                Log.Instance.Error(e, "Error during account validation job.");
            }
        });
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

        using AccountUnitOfWork uow = new(appEnvironment, true);

        var source = GetAccount(sourceAccount);
        var destination = GetAccount(destinationAccount);
        if (source == null || destination == null)
        {
            Log.Instance.Error($"Either source or destination account was not valid. {sourceAccount} {destinationAccount}");
            return (false, "Either source or destination account was not valid.");
        }

        var charges = source.Charges.Where(c => c.IsCredited == false).ToList();

        charges.ForEach(charge =>
        {
            CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
            AddCharge(destination, charge.CDMCode, charge.Quantity, (DateTime)destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
        });

        uow.Commit();
        ClearAccountLock(source);
        ClearAccountLock(destination);
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
        using AccountUnitOfWork uow = new(appEnvironment);
        uow.UserProfileRepository.InsertRecentAccount(accountNo, userName);
    }

    public Chk AddPayment(Chk chk)
    {
        Log.Instance.Trace("Entering");
        using AccountUnitOfWork uow = new(appEnvironment, true);

        try
        {
            var newChk = uow.ChkRepository.Add(chk);
            uow.Commit();
            return newChk;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error occurred in Add Payment", ex);
        }
    }

    public IList<Chrg> UpdateDiagnosisPointers(IEnumerable<Chrg> chrgs)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);

        var newChrgs = UpdateDxPointers(chrgs.ToList());
        uow.Commit();
        return newChrgs;
    }

    public async Task ClearClaimStatusAsync(Account account) => await Task.Run(() => ClearClaimStatus(account));

    /// <summary>
    /// Clears all claim flags so account will be picked up in next claim batch
    /// </summary>
    /// <param name="account"></param>
    public Account ClearClaimStatus(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        using AccountUnitOfWork uow = new(appEnvironment, true);
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

            account.Pat = uow.PatRepository.Update(account.Pat, columns);
            UpdateStatus(account.AccountNo, AccountStatus.New);
            account.Status = AccountStatus.New;

            account.Notes = AddNote(account.AccountNo, "Claim status cleared.").ToList();
            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
        }
    }

    public ChrgDetail RemoveChargeModifier(int chrgDetailId)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);
        var retval = uow.ChrgDetailRepository.RemoveModifier(chrgDetailId);
        var chrgDetail = uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
        uow.Commit();
        return chrgDetail;
    }

    public ChrgDetail AddChargeModifier(int chrgDetailId, string modifier)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);
        var retval = uow.ChrgDetailRepository.AddModifier(chrgDetailId, modifier);
        var chrgDetail = uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
        uow.Commit();

        return chrgDetail;
    }

    public Chrg CreditCharge(int chrgId, string comment = "")
    {
        using AccountUnitOfWork uow = new(appEnvironment);

        Log.Instance.Trace($"Entering - chrg number {chrgId} comment {comment}");

        bool setCreditFlag = false;
        bool setOldChrgCreditFlag = false;

        if (chrgId <= 0)
            throw new ArgumentOutOfRangeException(nameof(chrgId));

        var chrg = GetCharge(chrgId) ?? throw new ApplicationException($"Charge number {chrgId} not found.");

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

        var newChrg = chrg.Clone();

        newChrg.IsCredited = setCreditFlag;
        newChrg.ChrgId = 0;
        newChrg.Quantity *= -1;
        newChrg.Comment = comment;
        newChrg.Invoice = null;
        newChrg.PostingDate = DateTime.Today;
        newChrg.ChrgDetails.ForEach(x => x.ChrgNo = 0);

        newChrg = AddCharge(newChrg);

        if (setOldChrgCreditFlag)
            chrg = uow.ChrgRepository.SetCredited(chrgId);

        uow.Commit();
        return newChrg;
    }

    public Chrg SetChargeCreditFlag(int chrgId, bool flag)
    {
        using AccountUnitOfWork uow = new(appEnvironment);

        var chrg = uow.ChrgRepository.SetCredited(chrgId, flag);
        return chrg;
    }

    public Pat SetCollectionsDate(Pat pat)
    {
        using AccountUnitOfWork uow = new(appEnvironment, true);

        pat = uow.PatRepository.Update(pat, new[] { nameof(Pat.BadDebtListDate) });
        uow.Commit();

        return pat;
    }

    public IList<AccountSearch> SearchAccounts(string lastName, string firstName, string mrn, string ssn, string dob,
            string sex, string accountSearch)
    {
        using AccountUnitOfWork uow = new(appEnvironment);
        var results = uow.AccountSearchRepository.GetBySearch(lastName, firstName, mrn, ssn, dob, sex, accountSearch).ToList();

        return results;
    }
}

public class ValidationUpdatedEventArgs : EventArgs
{
    public string AccountNo { get; set; }
    public string ValidationStatus { get; set; }
    public string UpdateMessage { get; set; }
    public DateTime TimeStamp { get; set; }
    public int Processed { get; set; }
    public int TotalItems { get; set; }

}
