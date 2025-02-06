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
using Utilities;
using Log = LabBilling.Logging.Log;

namespace LabBilling.Core.Services;

public sealed class AccountService
{
    private readonly IAppEnvironment _appEnvironment;

    private readonly DictionaryService _dictionaryService;

    public event EventHandler<ValidationUpdatedEventArgs> ValidationAccountUpdated;
    public event EventHandler<AccountEventArgs> AccountValidated;
    public event EventHandler<AccountEventArgs> AccountLocked;
    public event EventHandler<AccountEventArgs> AccountLockCleared;

    private readonly IUnitOfWork _uow;

    public AccountService(IAppEnvironment appEnvironment, IUnitOfWork uow)
    {
        this._appEnvironment = appEnvironment;
        _dictionaryService = new(appEnvironment, uow);
        _uow = uow;
    }

    public List<Ins> GetInsByAccount(string accountNo)
    {
        var records = _uow.InsRepository.GetInsByAccount(accountNo);

        foreach (Ins ins in records)
        {
            if (!string.IsNullOrEmpty(ins.HolderFullName))
            {
                if (string.IsNullOrEmpty(ins.HolderLastName) || string.IsNullOrEmpty(ins.HolderFirstName))
                {
                    if (!StringExtensions.ParseName(ins.HolderFullName.ToString(),
                        out string lname, out string fname, out string mname, out string suffix))
                    {
                        //error parsing name
                        Log.Instance.Info($"Insurance holder name could not be parsed. {ins.HolderFullName}");
                    }

                    ins.HolderLastName = lname;
                    ins.HolderFirstName = fname;
                    ins.HolderMiddleName = mname;
                }
                else
                {
                    ins.HolderFullName = $"{ins.HolderLastName},{ins.HolderFirstName} {ins.HolderMiddleName}";
                }
            }
            else
            {
                ins.HolderFullName = string.Empty;
            }

            if (ins.InsCode != null)
            {
                ins.InsCompany = _dictionaryService.GetInsCompany(ins.InsCode);
            }
            ins.InsCompany ??= new InsCompany();

            StringExtensions.ParseCityStZip(ins.PlanCityState, out string strCity, out string strState, out string strZip);
            ins.PlanCity = strCity;
            ins.PlanState = strState;
            ins.PlanZip = strZip;

        }

        return records;
    }

    public Ins GetInsByAccount(string accountNo, InsCoverage coverage)
    {
        var records = GetInsByAccount(accountNo);
        return records.Where(x => x.Coverage == coverage).FirstOrDefault();
    }

    public Pat GetPatByAccount(Account account)
    {
        var record = _uow.PatRepository.GetPatByAccount(account);

        record.Physician = _dictionaryService.GetProvider(record.ProviderId);
        if (record.Physician != null)
            record.Physician.SanctionedProvider = _dictionaryService.GetSanctionedProvider(record.Physician?.NpiId);

        string amaYear = FunctionRepository.GetAMAYear(account.TransactionDate);

        record.Diagnoses = new List<PatDiag>();
        if (record.Dx1 != null && record.Dx1 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx1, amaYear);
            record.Dx1Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 1, Code = record.Dx1, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx2 != null && record.Dx2 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx2, amaYear);
            record.Dx2Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 2, Code = record.Dx2, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx3 != null && record.Dx3 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx3, amaYear);
            record.Dx3Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 3, Code = record.Dx3, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx4 != null && record.Dx4 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx4, amaYear);
            record.Dx4Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 4, Code = record.Dx4, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx5 != null && record.Dx5 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx5, amaYear);
            record.Dx5Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 5, Code = record.Dx5, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx6 != null && record.Dx6 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx6, amaYear);
            record.Dx6Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 6, Code = record.Dx6, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx7 != null && record.Dx7 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx7, amaYear);
            record.Dx7Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 7, Code = record.Dx7, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx8 != null && record.Dx8 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx8, amaYear);
            record.Dx8Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 8, Code = record.Dx8, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx9 != null && record.Dx9 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(record.Dx9, amaYear);
            record.Dx9Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 9, Code = record.Dx9, Description = dictRecord?.Description ?? "" });
        }

        return record;
    }

    public decimal GetNextAccountNumber()
    {
        return _uow.NumberRepository.GetNumber("account");
    }

    /// <summary>
    /// Get only minimal account info. Only pulls data from acc table. Does not load charges or other detail info.
    /// </summary>
    /// <param name="accountNo"></param>
    /// <returns></returns>
    public Account GetAccountMinimal(string accountNo)
    {
        var acc = _uow.AccountRepository.GetByAccount(accountNo);
        return acc;
    }

    public async Task<Account> GetAccountAsync(string account) => await Task.Run(() => GetAccount(account));
    public async Task<Account> GetAccountAsync(string account, bool demographicsOnly = false) => await Task.Run(() => GetAccount(account, demographicsOnly));
    public async Task<Account> GetAccountAsync(string account, bool demographicsOnly = false, bool secureLock = true) => await Task.Run(() => GetAccount(account, demographicsOnly, secureLock));


    public (bool locksuccessful, AccountLock lockInfo) GetAccountLock(string account)
    {
        _uow.StartTransaction();

        var alock = _uow.AccountLockRepository.GetLock(account);

        if (alock == null || string.IsNullOrEmpty(alock?.AccountNo))
        {
            //there is no lock on this account - establish one
            alock = _uow.AccountLockRepository.Add(account);
            _uow.Commit();
            AccountLocked?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock established for account {account}." });
            return (true, alock);
        }
        else
        {
            // check to see if the lock is this user and machine
            if (alock.UpdatedUser == _appEnvironment.User && alock.UpdatedHost == Environment.MachineName)
            {
                _uow.Commit();
                AccountLocked?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock established for account {account}." });
                return (true, alock);
            }
        }
        _uow.Commit();
        return (false, alock);
    }

    public List<AccountLock> GetAccountLocks()
    {
        return _uow.AccountLockRepository.GetAll();
    }

    public bool ClearAccountLock(Account account)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (account.AccountLockInfo == null)
        {
            account.AccountLockInfo = _uow.AccountLockRepository.GetLock(account.AccountNo);
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
        var retval = _uow.AccountLockRepository.Delete(id);
        AccountLockCleared?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock id {id} cleared." });
        return retval;
    }

    public bool ClearAccountLocks(string username, string hostname)
    {
        try
        {
            return _uow.AccountLockRepository.DeleteByUserHost(username, hostname);
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex);
            return false;
        }
    }

    public Account GetAccount(string account, bool demographicsOnly = false, bool secureLock = true)
    {
        Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

        bool locksuccessful = false;
        AccountLock alock = null;

        if (secureLock)
        {
            //get lock before loading account
            (locksuccessful, alock) = GetAccountLock(account);
            if (!locksuccessful)
            {
                throw new AccountLockException(alock);
            }
        }

        Account record = null;
        try
        {
            record = _uow.AccountRepository.GetByAccount(account);
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
            if (record.ClientMnem != _appEnvironment.ApplicationParameters.InvalidFinancialCode)
            {
                record.Client = _dictionaryService.GetClient(record.ClientMnem);
            }
        }
        record.Pat = GetPatByAccount(record);
        record.Charges = GetCharges(account, true, true, null, false).ToList();

        if (record.FinCode != _appEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            record.ChrgDiagnosisPointers = _uow.ChrgDiagnosisPointerRepository.GetByAccount(account).ToList();

            record.Charges.Where(c => c.CDMCode != _appEnvironment.ApplicationParameters.ClientInvoiceCdm).ToList().ForEach(chrg =>
            {
                chrg.ChrgDetails.ForEach(cd =>
                {
                    var diagPtr = record.ChrgDiagnosisPointers.Find(c => c.CdmCode == chrg.CDMCode && c.CptCode == cd.Cpt4);
                    if (diagPtr == null)
                    {
                        //add a new diag ptr
                        ChrgDiagnosisPointer ptr = new()
                        {
                            AccountNo = record.AccountNo,
                            CdmCode = chrg.CDMCode,
                            CptCode = cd.Cpt4,
                            DiagnosisPointer = "1:"
                        };
                        record.ChrgDiagnosisPointers.Add(_uow.ChrgDiagnosisPointerRepository.Add(ptr));
                    }
                });
            });
            List<ChrgDiagnosisPointer> temp = new List<ChrgDiagnosisPointer>();
            record.ChrgDiagnosisPointers.ForEach(d =>
            {
                d.CdmDescription = record.Cdms.Where(c => c.ChargeId == d.CdmCode).FirstOrDefault()?.Description;
                if (!string.IsNullOrEmpty(d.CptCode))
                {
                    d.CptDescription = record.Cdms.Where(c => c.ChargeId == d.CdmCode).FirstOrDefault()?.CdmDetails?.Where(cd => cd.Cpt4 == d.CptCode).FirstOrDefault()?.Description;
                }
                if (string.IsNullOrEmpty(d.CdmDescription) || string.IsNullOrEmpty(d.CptDescription))
                {
                    _uow.ChrgDiagnosisPointerRepository.Delete(d);
                    temp.Add(d);
                }
            });

            temp.ForEach(d => record.ChrgDiagnosisPointers.Remove(d));
        }
        record.Payments = _uow.ChkRepository.GetByAccount(account);

        if (!demographicsOnly)
        {
            record.Insurances = GetInsByAccount(account);
            record.Notes = _uow.AccountNoteRepository.GetByAccount(account);
            record.BillingActivities = _uow.BillingActivityRepository.GetByAccount(account);
            record.AccountValidationStatus = _uow.AccountValidationStatusRepository.GetByAccount(account);
            record.Fin = _uow.FinRepository.GetFin(record.FinCode);
            record.AccountAlert = _uow.AccountAlertRepository.GetByAccount(account);
            record.PatientStatements = _uow.PatientStatementAccountRepository.GetByAccount(account);
        }

        return record;
    }

    public Pat ClearCollectionsListDate(string accountNo)
    {
        var pat = _uow.PatRepository.GetByKey(accountNo);
        pat.BadDebtListDate = null;
        _uow.PatRepository.Update(pat, new List<string>() { nameof(Pat.BadDebtListDate) });
        return pat;
    }

    public double GetBalance(string accountNo)
    {
        string chrgTableName = _uow.ChrgRepository.TableInfo.TableName;
        string accTableName = _uow.AccountRepository.TableInfo.TableName;
        string chkTableName = _uow.ChkRepository.TableInfo.TableName;

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        try
        {
            var sql = Sql.Builder
                .Select(new[]
                {
                    $"coalesce(sum({_uow.ChrgRepository.GetRealColumn(nameof(Chrg.Quantity))} * {_uow.ChrgRepository.GetRealColumn(nameof(Chrg.NetAmount))}), 0.00) as 'Total'"
                })
                .From(chrgTableName)
                .InnerJoin(accTableName)
                .On($"{accTableName}.{_uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} = {chrgTableName}.{_uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))}")
                .Where($"{chrgTableName}.{_uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo))} = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo })
                .Where($"(({accTableName}.{_uow.AccountRepository.GetRealColumn(nameof(Account.FinCode))} = @0 and {chrgTableName}.{_uow.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} <> @1)",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.ChargeInvoiceStatus })
                .Append($"or ({accTableName}.{_uow.AccountRepository.GetRealColumn(nameof(Account.FinCode))} <> @0 and {chrgTableName}.{_uow.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} not in (@1, @2, @3)))",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.ChargeInvoiceStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.CapitatedChargeStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.NotApplicableChargeStatus })
                .GroupBy(chrgTableName + "." + _uow.ChrgRepository.GetRealColumn(nameof(Chrg.AccountNo)));

            double charges = _uow.Context.SingleOrDefault<double>(sql);

            sql = Sql.Builder
                .Select(new[]
                {
                    $"coalesce(sum({_uow.ChkRepository.GetRealColumn(nameof(Chk.PaidAmount))} + {_uow.ChkRepository.GetRealColumn(nameof(Chk.ContractualAmount))} + {_uow.ChkRepository.GetRealColumn(nameof(Chk.WriteOffAmount))}), 0.00) as 'Total'"
                })
                .From(_uow.ChkRepository.TableInfo.TableName)
                .Where($"{chkTableName}.{_uow.ChkRepository.GetRealColumn(nameof(Chk.AccountNo))} = @0",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo })
                .GroupBy(_uow.ChkRepository.GetRealColumn(nameof(Chk.AccountNo)));

            double adj = (double)_uow.Context.SingleOrDefault<double>(sql);

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

        _uow.StartTransaction();

        if (table.FinCode != "CLIENT")
            table.PatFullName = table.PatNameDisplay;

        table.Status = AccountStatus.New;

        table.TransactionDate = table.TransactionDate.Date;

        _uow.AccountRepository.Add(table);

        //make sure Pat record has an account number
        if (table.Pat.AccountNo != table.AccountNo)
            table.Pat.AccountNo = table.AccountNo;

        var pat = GetPatByAccount(table);
        if (pat == null)
            _uow.PatRepository.Add(table.Pat);
        else
            _uow.PatRepository.Update(table.Pat);

        table.Insurances.ForEach(ins =>
        {
            if (ins.Account != table.AccountNo)
                ins.Account = table.AccountNo;

            _uow.InsRepository.Save(ins);
        });
        var result = GetAccountLock(table.AccountNo);
        if (result.locksuccessful)
            table.AccountLockInfo = result.lockInfo;
        _uow.Commit();

        return table;
    }

    public async Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate) => await Task.Run(() => GetInvoiceAccounts(clientMnem, thruDate));

    public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
    {
        Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");
        return _uow.InvoiceSelectRepository.GetByClientAndDate(clientMnem, thruDate);
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
        PetaPoco.Sql command;

        string selMaxRecords = string.Empty;
        string accTableName = _uow.AccountRepository.TableName;
        string insTableName = _uow.InsRepository.TableName;

        var selectCols = new[]
        {
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.Status)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.AccountNo)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.PatName)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.SocSecNum)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.ClientMnem)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.FinCode)),
            accTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.TransactionDate)),
            insTableName + "." + _uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.InsPlanName))
        };

        command = Sql.Builder
            .Select(selectCols)
            .From(accTableName)
            .InnerJoin(insTableName)
            .On($"{insTableName}.{_uow.InsRepository.GetRealColumn(nameof(Ins.Account))} = {accTableName}.{_uow.AccountRepository.GetRealColumn(nameof(Account.AccountNo))} and {_uow.InsRepository.GetRealColumn(nameof(Ins.Coverage))} = '{InsCoverage.Primary}'");

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

            command.OrderBy($"{_uow.ClaimItemRepository.GetRealColumn(nameof(ClaimItem.TransactionDate))}");

            var queryResult = _uow.ClaimItemRepository.Fetch(command).ToList();

            if (_appEnvironment.ApplicationParameters.MaxClaimsInClaimBatch > 0)
                return queryResult.Take(_appEnvironment.ApplicationParameters.MaxClaimsInClaimBatch).ToList();
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
        var statements = _uow.PatientStatementAccountRepository.GetByAccount(accountNo);

        return statements;
    }

    public async Task<bool> SetNoteAlertAsync(string account, bool showAlert) => await Task.Run(() => SetNoteAlert(account, showAlert));

    public bool SetNoteAlert(string account, bool showAlert)
    {
        Log.Instance.Trace($"Entering - account {account} showAlert {showAlert}");
        _uow.StartTransaction();
        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        try
        {
            var record = _uow.AccountAlertRepository.GetByAccount(account);

            if (record == null)
            {
                record = new AccountAlert
                {
                    AccountNo = account,
                    Alert = showAlert
                };

                _uow.AccountAlertRepository.Add(record);
            }
            else
            {
                record.Alert = showAlert;
                _uow.AccountAlertRepository.Update(record);
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error updating NoteAlert.");
            return false;
        }

        _uow.Commit();
        return true;
    }

    public Account UpdateAccountDemographics(Account acc)
    {
        Log.Instance.Trace("Entering");
        _uow.StartTransaction();
        try
        {
            var accResult = _uow.AccountRepository.Update(acc);
            var patResult = _uow.PatRepository.Save(acc.Pat);
            _uow.Commit();

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
        _uow.StartTransaction();
        if (acc == null)
            throw new ArgumentNullException(nameof(acc));

        var patResult = _uow.PatRepository.SaveDiagnoses(acc.Pat);
        acc.Pat = patResult;
        _uow.Commit();

        return acc;
    }

    public async Task<int> UpdateStatusAsync(string accountNo, string status) => await Task.Run(() => UpdateStatus(accountNo, status));
    public async Task<Account> UpdateStatusAsync(Account model, string status) => await Task.Run(() => UpdateStatus(model, status));

    public int UpdateStatus(string accountNo, string status)
    {
        Log.Instance.Trace($"Entering - account {accountNo} status {status}");

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(status))
            throw new ArgumentNullException(nameof(status));
        if (!AccountStatus.IsValid(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

        var returnval = _uow.AccountRepository.UpdateStatus(accountNo, status);

        return returnval;
    }

    public Account UpdateStatus(Account model, string status)
    {
        try
        {
            var result = _uow.AccountRepository.UpdateStatus(model, status);
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
        _uow.StartTransaction();

        try
        {
            AddNote(acc.AccountNo, $"Statement flag changed from {acc.Pat.StatementFlag} to {flag}");
            acc.Pat.StatementFlag = flag;
            if (flag != "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.Statements);
                acc.Status = AccountStatus.Statements;
            }
            if (flag == "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.New);
                acc.Status = AccountStatus.New;
            }
            acc.Pat = _uow.PatRepository.Update(acc.Pat, new[] { nameof(Pat.StatementFlag) });
            _uow.Commit();
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
        _uow.StartTransaction();
        try
        {
            Ins returnIns;
            if (ins.rowguid == Guid.Empty)
                returnIns = _uow.InsRepository.Add(ins);
            else
                returnIns = _uow.InsRepository.Update(ins);

            _uow.Commit();
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
        _uow.StartTransaction();
        var retval = _uow.InsRepository.Delete(ins);
        _uow.Commit();
        return retval;
    }

    public async Task<Account> InsuranceSwapAsync(Account account, InsCoverage swap1, InsCoverage swap2) => await Task.Run(() => InsuranceSwap(account, swap1, swap2));

    public Account InsuranceSwap(Account account, InsCoverage swap1, InsCoverage swap2)
    {
        Log.Instance.Trace($"Entering - Account {account.AccountNo} Ins1 {swap1} Ins2 {swap2}");

        Ins insA = account.Insurances.Where(x => x.Coverage == swap1).FirstOrDefault();
        Ins insB = account.Insurances.Where(x => x.Coverage == swap2).FirstOrDefault();

        _uow.StartTransaction();

        try
        {
            insA.Coverage = InsCoverage.Temporary;
            insB.Coverage = swap1;

            insA = _uow.InsRepository.Update(insA);
            insB = _uow.InsRepository.Update(insB);

            insA.Coverage = swap2;
            insA = _uow.InsRepository.Update(insA);

            account.Notes = AddNote(account.AccountNo, $"Insurance swap: {swap1} {insA.PlanName} and {swap2} {insB.PlanName}").ToList();
            account.Insurances = GetInsByAccount(account.AccountNo);
            _uow.Commit();
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
        _uow.StartTransaction();

        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(reason_comment);

        DateTime oldServiceDate = DateTime.MinValue;

        // update trans_date on acc table
        if (model.TransactionDate != newDate)
        {
            oldServiceDate = (DateTime)model.TransactionDate;
            model.TransactionDate = newDate;
            model = _uow.AccountRepository.Update(model, new[] { nameof(Account.TransactionDate) });

            model.Notes = AddNote(model.AccountNo, $"Service Date changed from {oldServiceDate} to {newDate}").ToList();

            //determine if charges need to be reprocessed.

            //TODO: is there any reason a date of service change should result in changing all charges --
            // except: the date of service on charges will not match new date.

            // option: reprocess all charges, or update service date on charge records

            model.Charges.ForEach(c =>
            {
                c.ServiceDate = newDate;
                c = _uow.ChrgRepository.Update(c);
            });

            _uow.Commit();
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

        _uow.StartTransaction();

        AccountNote accountNote = new()
        {
            Account = accountNo,
            Comment = noteText
        };
        try
        {
            _uow.AccountNoteRepository.Add(accountNote);
            _uow.Commit();
            return _uow.AccountNoteRepository.GetByAccount(accountNo);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error adding account note.");
            throw new ApplicationException("Error adding account note.", ex);
        }
    }

    public List<AccountNote> GetNotes(string accountNo)
    {
        var notes = _uow.AccountNoteRepository.GetByAccount(accountNo);
        return notes;
    }

    public async Task<Account> ChangeFinancialClassAsync(string accountNo, string newFinCode) => await Task.Run(() => ChangeFinancialClass(accountNo, newFinCode));

    public Account ChangeFinancialClass(string accountNo, string newFinCode)
    {
        Log.Instance.Trace($"Entering - Account {accountNo} New Fin {newFinCode}");
        _uow.StartTransaction();

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(newFinCode))
            throw new ArgumentNullException(nameof(newFinCode));

        var record = GetAccount(accountNo);

        var retval = ChangeFinancialClass(record, newFinCode);
        _uow.Commit();
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

        _uow.StartTransaction();

        string oldFinCode = model.FinCode;

        //check that newFincode is a valid fincode
        Fin newFin = _uow.FinRepository.GetFin(newFinCode);
        Fin oldFin = model.Fin;

        if (newFin == null)
            throw new ArgumentException($"Financial code {newFinCode} is not valid code.", nameof(newFinCode));

        if (oldFinCode != newFinCode)
        {
            model.FinCode = newFinCode;
            try
            {
                if (model.ReadyToBill)
                {
                    //clear ready to bill
                    model.Status = AccountStatus.New;
                    model.Notes = AddNote(model.AccountNo, "Ready to Bill status cleared due to financial class change.").ToList();
                }
                _uow.AccountRepository.Update(model, new[] { nameof(Account.FinCode), nameof(Account.Status) });
                model.FinCode = newFin.FinCode;
                model.Fin = newFin;
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
            _uow.Commit();
            model = Validate(model);
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
        else ArgumentNullException.ThrowIfNull(newClientMnem);

        _uow.StartTransaction();

        bool updateSuccess = true;
        string oldClientMnem = model.ClientMnem;

        if (oldClientMnem == null)
        {
            //account does not have a valid current client defined
            _uow.Commit();
            throw new ApplicationException($"Account {model.AccountNo} does not have a valid client defined.");
        }

        Client oldClient = _uow.ClientRepository.GetClient(oldClientMnem);
        Client newClient = _uow.ClientRepository.GetClient(newClientMnem);

        if (oldClient == null)
        {
            _uow.Commit();
            Log.Instance.Error("Client mnem {oldClientMnem} is not valid.");
            throw new ApplicationException($"Client mnem {oldClientMnem} is not valid.");
        }

        if (newClient == null)
        {
            _uow.Commit();
            throw new ArgumentException($"Client mnem {newClientMnem} is not valid.", nameof(newClientMnem));
        }

        if (string.IsNullOrEmpty(newClient.FeeSchedule) || newClient.FeeSchedule == "0")
        {
            _uow.Commit();
            throw new ApplicationException($"Fee schedule not defined on client {newClientMnem}. Client change aborted.");
        }

        try
        {
            if (oldClientMnem != newClientMnem)
            {

                model.ClientMnem = newClientMnem;
                model.Client = newClient;
                try
                {
                    _uow.AccountRepository.Update(model, new[] { nameof(Account.ClientMnem) });
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    _uow.Rollback();
                    throw new ApplicationException($"Exception updating client for {model.AccountNo}.", ex);
                }
                AddNote(model.AccountNo, $"Client updated from {oldClientMnem} to {newClientMnem}.");

                //reprocess charges if fin class is client bill (C) to pick up proper discounts.
                if (model.Fin.FinClass == _appEnvironment.ApplicationParameters.ClientFinancialTypeCode)
                {
                    try
                    {
                        model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}").ToList();
                    }
                    catch (Exception ex)
                    {
                        _uow.Rollback();
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }

                if (model.Fin.FinClass == _appEnvironment.ApplicationParameters.PatientFinancialTypeCode)
                {
                    //reprocess charges if fee schedule is different to pick up correct charge amounts
                    if (oldClient.FeeSchedule != newClient.FeeSchedule)
                    {
                        try
                        {
                            model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}").ToList();
                        }
                        catch (Exception ex)
                        {
                            _uow.Rollback();
                            throw new ApplicationException("Error reprocessing charges.", ex);
                        }
                    }
                    else if (oldClient.ClientMnem == _appEnvironment.ApplicationParameters.PathologyBillingClientException
                        || newClient.ClientMnem == _appEnvironment.ApplicationParameters.PathologyBillingClientException)
                    {
                        try
                        {
                            model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}").ToList();
                        }
                        catch (Exception ex)
                        {
                            _uow.Rollback();
                            throw new ApplicationException("Error reprocessing charges.", ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            model.Charges = UpdateChargesClient(model.AccountNo, newClientMnem).ToList();
                        }
                        catch (Exception ex)
                        {
                            _uow.Rollback();
                            throw new ApplicationException("Error updating charge client.", ex);
                        }
                    }
                }

                _uow.Commit();
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
        var charge = _uow.ChrgRepository.GetById(chrgNo);

        charge.Cdm = _uow.CdmRepository.GetCdm(charge.CDMCode);
        charge.ChrgDetails = _uow.ChrgDetailRepository.GetByChrgId(chrgNo).ToList();

        return charge;
    }

    public IList<ClaimChargeView> GetClaimCharges(string accountNo)
    {
        var charges = _uow.ChrgRepository.GetClaimCharges(accountNo);

        foreach (var chrg in charges)
        {
            chrg.RevenueCodeDetail = _uow.RevenueCodeRepository.GetByCode(chrg.RevenueCode);
            chrg.Cdm = _uow.CdmRepository.GetCdm(chrg.ChargeId, true);
        }

        return charges;
    }

    public IList<Chrg> GetCharges(string accountNo, bool showCredited = true, bool includeInvoiced = true, DateTime? asOfDate = null, bool excludeCBill = true)
    {
        Log.Instance.Debug($"Entering - account {accountNo}");

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));

        List<Chrg> charges = _uow.ChrgRepository.GetByAccount(accountNo, showCredited, includeInvoiced, asOfDate, excludeCBill);

        if (charges.Count > 10) //consider parallel processing if number of charges is significant.
        {
            charges.AsParallel().ForAll(chrg =>
            {
                chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, true);
                AddRevenueDiagnosisToChrg(chrg);
            });
        }
        else
        {
            charges.ForEach(chrg =>
            {
                chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, true);
                AddRevenueDiagnosisToChrg(chrg);
            });
        }
        return charges;
    }

    internal void AddRevenueDiagnosisToChrg(Chrg chrg)
    {
        chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, true);
        chrg.ChrgDetails = _uow.ChrgDetailRepository.GetByChrgId(chrg.ChrgId).ToList();
        chrg.ChrgDetails.ForEach(detail =>
        {
            detail.RevenueCodeDetail = _dictionaryService.GetRevenueCode(detail.RevenueCode);
            var cpt = _uow.CptAmaRepository.GetCpt(detail.Cpt4);
            if (cpt != null)
                detail.CptDescription = cpt.ShortDescription;
        });

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

        _uow.StartTransaction();
        try
        {
            var chargesToCredit = account.Charges.Where(x => x.IsCredited == false && x.CDMCode != _appEnvironment.ApplicationParameters.ClientInvoiceCdm).ToList();

            chargesToCredit.ForEach(c =>
            {
                CreditCharge(c.ChrgId, comment);
                AddCharge(account, c.CDMCode, c.Quantity, account.TransactionDate);
            });

            var updatedChrgList = GetCharges(account.AccountNo);

            _uow.Commit();
            return updatedChrgList;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
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
        try
        {
            _uow.StartTransaction();

            if (charges == null || charges.Count == 0)
            {
                Log.Instance.Info($"No charges to update.");
                _uow.Commit();
                return charges;
            }
            //throw new ApplicationException($"No charges for account {charges.First().AccountNo}");

            var fin = _uow.FinRepository.GetFin(finCode) ?? throw new ApplicationException($"Fin {finCode} is not valid");

            var chrgsToUpdate = charges.Where(x => x.IsCredited == false &&
                (x.ClientMnem != _appEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
                string.IsNullOrEmpty(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem))
                && x.FinancialType == fin.FinClass).ToList();

            chrgsToUpdate.ForEach(c =>
            {
                c.FinCode = finCode;
                c.FinancialType = fin.FinClass;
                c = _uow.ChrgRepository.Update(c, new[] { nameof(Chrg.FinancialType), nameof(Chrg.FinCode) });
            });

            _uow.Commit();
            return GetCharges(charges.First().AccountNo).ToList();
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error updating charges.", ex);
        }
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

        _uow.StartTransaction();

        List<Chrg> charges = GetCharges(account).ToList();

        if (charges == null || charges.Count == 0)
            throw new ApplicationException($"No charges for account {account}");

        var client = _uow.ClientRepository.GetClient(clientMnem) ?? throw new ApplicationException($"Client {clientMnem} not valid.");
        charges.ForEach(c =>
        {
            c.ClientMnem = clientMnem;
            c = _uow.ChrgRepository.Update(c, new[] { nameof(Chrg.ClientMnem) });
        });

        _uow.Commit();
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

        var newChrg = _uow.ChrgRepository.Add(chrg);

        chrg.ChrgDetails.ForEach(cd =>
        {
            cd.ChrgNo = newChrg.ChrgId;
            cd = _uow.ChrgDetailRepository.Add(cd);
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

        Fin fin = _dictionaryService.GetFinCode(accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");

        //get the cdm number - if cdm number is not found - abort
        Cdm cdmData = _dictionaryService.GetCdm(cdm);
        if (cdmData == null)
        {
            Log.Instance.Error($"CDM {cdm} not found.");
            throw new CdmNotFoundException("CDM not found.", cdm);
        }

        _uow.StartTransaction();

        //check account status, change to NEW if it is paid out.
        if (accData.Status == AccountStatus.PaidOut)
        {
            UpdateStatus(accData.AccountNo, AccountStatus.New);
            accData.Status = AccountStatus.New;
        }

        Client chargeClient = accData.Client;

        if (_appEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
        {
            //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
            var gb = _uow.GlobalBillingCdmRepository.GetCdm(cdm, accData.TransactionDate);
            //hard coding exception for Hardin County for now - 05/09/2023 BSP
            if (gb != null && accData.ClientMnem != _appEnvironment.ApplicationParameters.PathologyBillingClientException)
            {
                fin = _uow.FinRepository.GetFin(_appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
                    ?? throw new ApplicationException($"Fin code {_appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode} not found error {accData.AccountNo}");
                chargeClient = _uow.ClientRepository.GetClient(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem);
            }
        }

        Chrg chrg = new()
        {
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


        if (fin.FinClass == _appEnvironment.ApplicationParameters.PatientFinancialTypeCode)
            chrg.Status = cdmData.MClassType == _appEnvironment.ApplicationParameters.NotApplicableChargeStatus ? _appEnvironment.ApplicationParameters.NotApplicableChargeStatus : _appEnvironment.ApplicationParameters.NewChargeStatus;
        else if (fin.FinClass == _appEnvironment.ApplicationParameters.ClientFinancialTypeCode)
            chrg.Status = cdmData.CClassType == _appEnvironment.ApplicationParameters.NotApplicableChargeStatus ? _appEnvironment.ApplicationParameters.NotApplicableChargeStatus : _appEnvironment.ApplicationParameters.NewChargeStatus;
        else if (fin.FinClass == _appEnvironment.ApplicationParameters.ZFinancialTypecode)
            chrg.Status = cdmData.ZClassType == _appEnvironment.ApplicationParameters.NotApplicableChargeStatus ? _appEnvironment.ApplicationParameters.NotApplicableChargeStatus : _appEnvironment.ApplicationParameters.NewChargeStatus;
        else
            chrg.Status = _appEnvironment.ApplicationParameters.NewChargeStatus;

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
                Cpt4 = fee.Cpt4,
                Type = fee.Type
            };

            if (fin.FinClass == _appEnvironment.ApplicationParameters.PatientFinancialTypeCode)
            {
                chrgDetail.Amount = fee.MClassPrice;
                retailTotal += fee.MClassPrice;
                ztotal += fee.ZClassPrice;
            }
            else if (fin.FinClass == _appEnvironment.ApplicationParameters.ClientFinancialTypeCode)
            {
                var cliDiscount = chargeClient.Discounts?.Find(c => c.Cdm == cdm);
                double discountPercentage = chargeClient.DefaultDiscount;
                if (cliDiscount != null)
                {
                    discountPercentage = cliDiscount.PercentDiscount;
                }
                chrgDetail.Amount = fee.CClassPrice - (fee.CClassPrice * (discountPercentage / 100));
                retailTotal += fee.CClassPrice;
                ztotal += fee.ZClassPrice;
            }
            else if (fin.FinClass == _appEnvironment.ApplicationParameters.ZFinancialTypecode)
            {
                chrgDetail.Amount = fee.ZClassPrice;
                retailTotal += fee.ZClassPrice;
                ztotal += fee.ZClassPrice;
            }
            else
            {
                chrgDetail.Amount = fee.MClassPrice;
                retailTotal += fee.MClassPrice;
                ztotal += fee.ZClassPrice;
            }

            if (cdmData.Variable)
            {
                chrgDetail.Amount = miscAmount;
            }

            amtTotal += chrgDetail.Amount;

            chrgDetail.Modifier = fee.Modifier;
            chrgDetail.RevenueCode = fee.RevenueCode;
            chrgDetail.OrderCode = fee.BillCode;
            var cpt = _uow.CptAmaRepository.GetCpt(chrgDetail.Cpt4);
            if (cpt != null)
                chrgDetail.CptDescription = cpt.ShortDescription;

            chrgDetails.Add(chrgDetail);
            if (accData.ChrgDiagnosisPointers == null)
                accData.ChrgDiagnosisPointers = new();
            var diagPtr = accData.ChrgDiagnosisPointers?.Find(c => c.CdmCode == chrg.CDMCode && c.CptCode == chrgDetail.Cpt4);
            if (diagPtr == null)
            {
                //add a new diag ptr
                ChrgDiagnosisPointer ptr = new()
                {
                    AccountNo = accData.AccountNo,
                    CdmCode = chrg.CDMCode,
                    CptCode = chrgDetail.Cpt4,
                    DiagnosisPointer = "1:"
                };
                accData.ChrgDiagnosisPointers.Add(_uow.ChrgDiagnosisPointerRepository.Add(ptr));
            }
        }

        chrg.NetAmount = amtTotal;
        chrg.HospAmount = ztotal;
        chrg.RetailAmount = retailTotal;

        chrg = _uow.ChrgRepository.Add(chrg);
        chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode);
        chrgDetails.ForEach(d =>
        {
            d.ChrgNo = chrg.ChrgId;
            d = _uow.ChrgDetailRepository.Add(d);
        });
        chrg.ChrgDetails = chrgDetails;

        accData.Charges.Add(chrg);


        _uow.Commit();
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
            _uow.StartTransaction();

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

            _uow.Commit();
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

        _uow.StartTransaction();

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

        _uow.Commit();
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
            _uow.StartTransaction();

            if ((account.Status == AccountStatus.InstSubmitted || account.Status == AccountStatus.ProfSubmitted
                || account.Status == AccountStatus.ClaimSubmitted || account.Status == AccountStatus.Statements
                || account.Status == AccountStatus.Closed || account.Status == AccountStatus.PaidOut) && !reprint)
            {
                //account has been billed, do not validate
                account.AccountValidationStatus.Account = account.AccountNo;
                account.AccountValidationStatus.UpdatedDate = DateTime.Now;
                account.AccountValidationStatus.ValidationText = "Account has already been billed. Did not validate.";
                _uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
                _uow.Commit();
                return account;
            }
            else
            {
                if (account.Fin.FinClass == _appEnvironment.ApplicationParameters.PatientFinancialTypeCode
                    && account.InsurancePrimary != null)
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
                            _uow.PatRepository.SetStatementFlag(account.AccountNo, "Y");
                            account.Pat.StatementFlag = "Y";
                        }
                    }
                }

                _uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
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

                    _uow.AccountLmrpErrorRepository.Save(record);
                }
                _uow.Commit();
                AccountValidated?.Invoke(this, new AccountEventArgs() {  Account = account, AccountNo = account.AccountNo, UpdateMessage = account.AccountValidationStatus.ValidationText });
                return account;
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            account.AccountValidationStatus.Account = account.AccountNo;
            account.AccountValidationStatus.UpdatedDate = DateTime.Now;
            account.AccountValidationStatus.ValidationText = "Exception during Validation. Unable to validate.";
            _uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
            return account;
        }
    }

    private async Task<List<string>> ValidateLMRPAsync(Account account) => await Task.Run(() => ValidateLMRP(account));

    private List<string> ValidateLMRP(Account account)
    {
        Log.Instance.Trace($"Entering - account {account}");
        List<string> errorList = new();

        _uow.StartTransaction();

        //determine if there are any rules for ama_year
        if (_uow.LmrpRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
        {
            // no lmrp rules loaded for this ama year. 
            errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
            return errorList;
        }

        foreach (var cpt4 in account.cpt4List.Distinct())
        {
            if (cpt4 == null)
                continue;
            var ruleDef = _uow.LmrpRuleRepository.GetRuleDefinition(cpt4, (DateTime)account.TransactionDate);
            if (ruleDef == null)
                continue;

            bool dxIsValid = ruleDef.DxIsValid != 0;
            bool dxSupported = false;

            foreach (var dx in account.Pat.Diagnoses)
            {
                var rule = _uow.LmrpRuleRepository.GetRule(cpt4, dx.Code, (DateTime)account.TransactionDate);
                if (dxIsValid && rule != null)
                    dxSupported = true;

                if (!dxIsValid && rule == null)
                    dxSupported = true;
            }

            if (!dxSupported)
            {
                //check if charge has a GZ modifier
                if (!account.BillableCharges.Any(x => x.ChrgDetails.Where(y => y.Cpt4 == cpt4).Any(z => z.Modifier == "GZ" || z.Modifier2 == "GZ")))
                {
                    errorList.Add($"LMRP Violation - No dx codes support medical necessity for cpt {cpt4}.");
                }                
            }
        }
        _uow.Commit();
        return errorList;
    }

    public void ValidateUnbilledAccounts()
    {
        Log.Instance.Trace($"Entering");


        (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
            (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, DateTime.Today.AddDays((_appEnvironment.ApplicationParameters.BillingInitialHoldDays)*-1).ToShortDateString()),
            (nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.New),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.ClientAccountFinCode),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.SelfPayFinancialCode)
        };

        ValidationAccountUpdated?.Invoke(this, new ValidationUpdatedEventArgs() { UpdateMessage = "Compiling accounts", TimeStamp = DateTime.Now });
        var accounts = _uow.AccountSearchRepository.GetBySearch(parameters).ToList();
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

        _uow.StartTransaction();

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

        _uow.Commit();
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
    /// <returns>Credited charge record</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public Chrg MoveCharge(string sourceAccount, string destinationAccount, int chrgId)
    {
        if (string.IsNullOrEmpty(sourceAccount) || string.IsNullOrEmpty(destinationAccount))
        {
            throw new ArgumentException("One or both arguments are null or empty.");
        }
        _uow.StartTransaction();
        var source = GetAccount(sourceAccount);
        var destination = GetAccount(destinationAccount);

        var charge = source.Charges.SingleOrDefault(c => c.ChrgId == chrgId);

        if (charge.IsCredited)
        {
            throw new ApplicationException("Charge is already credited.");
        }

        var creditedCharge = CreditCharge(charge.ChrgId, $"Move to {destinationAccount}");
        AddCharge(destinationAccount, charge.CDMCode, charge.Quantity, destination.TransactionDate, $"Moved from {sourceAccount}", charge.ReferenceReq);
        _uow.Commit();
        return creditedCharge;
    }

    public void AddRecentlyAccessedAccount(string accountNo, string userName)
    {
        _uow.UserProfileRepository.InsertRecentAccount(accountNo, userName);
    }

    public Chk AddPayment(Chk chk)
    {
        Log.Instance.Trace("Entering");
        _uow.StartTransaction();

        try
        {
            if (chk.IsRefund)
                chk.Status = "REFUND";

            var newChk = _uow.ChkRepository.Add(chk);
            _uow.Commit();
            return newChk;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error occurred in Add Payment", ex);
        }
    }

    public Chk ReversePayment(Chk chk, string comment)
    {
        Log.Instance.Trace("Entering");
        _uow.StartTransaction();
        try
        {
            Chk newChk = new()
            {
                IsDeleted = chk.IsDeleted,
                AccountNo = chk.AccountNo,
                ChkDate = chk.ChkDate,
                CheckNo = chk.CheckNo,
                DateReceived = chk.DateReceived,
                PaidAmount = chk.PaidAmount * -1,
                ContractualAmount = chk.ContractualAmount * -1,
                WriteOffAmount = chk.WriteOffAmount * -1,
                IsRefund = !chk.IsRefund,
                PostingDate = DateTime.Today,
                Status = chk.Status,
                Source = chk.Source,
                FinCode = chk.FinCode,
                WriteOffDate = chk.WriteOffDate == null ? null : DateTime.Today,
                Invoice = chk.Invoice,
                Comment = comment,
                IsCollectionPmt = chk.IsCollectionPmt,
                Cpt4Code = chk.Cpt4Code,
                WriteOffCode = chk.WriteOffCode,
                EftDate = chk.EftDate,
                EftNumber = chk.EftNumber,
                InsCode = chk.InsCode,
                ClaimAdjCode = chk.ClaimAdjCode,
                ClaimAdjGroupCode = chk.ClaimAdjGroupCode,
                FacilityCode = chk.FacilityCode,
                ClaimNo = chk.ClaimNo
            };

            newChk = _uow.ChkRepository.Add(newChk);
            _uow.Commit();
            return newChk;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error occurred in Reverse Payment", ex);
        }
    }

    public ChrgDiagnosisPointer UpdateDiagnosisPointer(ChrgDiagnosisPointer ptr)
    {
        _uow.StartTransaction();

        ChrgDiagnosisPointer retPtr;
        if (ptr.Id < 1)
            retPtr = _uow.ChrgDiagnosisPointerRepository.Add(ptr);
        else
            retPtr = _uow.ChrgDiagnosisPointerRepository.Update(ptr);

        _uow.Commit();
        return retPtr;
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

        _uow.StartTransaction();
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

            account.Pat = _uow.PatRepository.Update(account.Pat, columns);
            UpdateStatus(account.AccountNo, AccountStatus.New);
            account.Status = AccountStatus.New;

            account.Notes = AddNote(account.AccountNo, "Claim status cleared.").ToList();
            _uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
        }
    }

    public ChrgDetail RemoveChargeModifier(int chrgDetailId)
    {
        _uow.StartTransaction();
        var retval = _uow.ChrgDetailRepository.RemoveModifier(chrgDetailId);
        var chrgDetail = _uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
        _uow.Commit();
        return chrgDetail;
    }

    public ChrgDetail AddChargeModifier(int chrgDetailId, string modifier)
    {

        try
        {
            _uow.StartTransaction();
            var retval = _uow.ChrgDetailRepository.UpdateModifier(chrgDetailId, modifier);
            var chrgDetail = _uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
            _uow.Commit();

            return chrgDetail;
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex.Message);
            throw new ApplicationException("Error adding modifier.", apex);
        }
    }

    public Chrg CreditCharge(int chrgId, string comment = "")
    {
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
            chrg = _uow.ChrgRepository.SetCredited(chrgId);

        _uow.Commit();
        return newChrg;
    }

    public Chrg SetChargeCreditFlag(int chrgId, bool flag)
    {
        var chrg = _uow.ChrgRepository.SetCredited(chrgId, flag);
        return chrg;
    }

    public Pat SetCollectionsDate(Pat pat)
    {
        _uow.StartTransaction();

        pat = _uow.PatRepository.Update(pat, new[] { nameof(Pat.BadDebtListDate) });
        _uow.Commit();

        return pat;
    }

    public IList<AccountSearch> SearchAccounts(string lastName, string firstName, string mrn, string ssn, string dob,
            string sex, string accountSearch)
    {
        var results = _uow.AccountSearchRepository.GetBySearch(lastName, firstName, mrn, ssn, dob, sex, accountSearch).ToList();

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

public class AccountEventArgs : EventArgs
{
    public string AccountNo { get; set; }
    public string UpdateMessage { get; set; }
    public Account Account { get; set; }
}
