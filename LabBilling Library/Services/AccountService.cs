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

    public AccountService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
        _dictionaryService = new(appEnvironment);
    }


    /// <summary>
    /// Retrieves a list of insurance records associated with a given account number.
    /// </summary>
    /// <param name="accountNo">The account number to retrieve insurance records for.</param>
    /// <param name="uow">Optional unit of work for database transactions. If not provided, a new unit of work is created.</param>
    /// <returns>A list of insurance records associated with the specified account number.</returns>
    public List<Ins> GetInsByAccount(string accountNo, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var records = uow.InsRepository.GetInsByAccount(accountNo);

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
                ins.InsCompany = _dictionaryService.GetInsCompany(ins.InsCode, uow);
            }
            ins.InsCompany ??= new InsCompany();

            StringExtensions.ParseCityStZip(ins.PlanCityState, out string strCity, out string strState, out string strZip);
            ins.PlanCity = strCity;
            ins.PlanState = strState;
            ins.PlanZip = strZip;

        }

        return records;
    }

    public Ins GetInsByAccount(string accountNo, InsCoverage coverage, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var records = GetInsByAccount(accountNo, uow);
        return records.Where(x => x.Coverage == coverage).FirstOrDefault();
    }

    public Pat GetPatByAccount(Account account, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var record = uow.PatRepository.GetPatByAccount(account);

        record.Physician = _dictionaryService.GetProvider(record.ProviderId, uow);
        if (record.Physician != null)
            record.Physician.SanctionedProvider = _dictionaryService.GetSanctionedProvider(uow, record.Physician?.NpiId);

        string amaYear = FunctionRepository.GetAMAYear(account.TransactionDate);

        record.Diagnoses = new List<PatDiag>();
        if (record.Dx1 != null && record.Dx1 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx1, amaYear);
            record.Dx1Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 1, Code = record.Dx1, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx2 != null && record.Dx2 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx2, amaYear);
            record.Dx2Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 2, Code = record.Dx2, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx3 != null && record.Dx3 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx3, amaYear);
            record.Dx3Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 3, Code = record.Dx3, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx4 != null && record.Dx4 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx4, amaYear);
            record.Dx4Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 4, Code = record.Dx4, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx5 != null && record.Dx5 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx5, amaYear);
            record.Dx5Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 5, Code = record.Dx5, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx6 != null && record.Dx6 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx6, amaYear);
            record.Dx6Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 6, Code = record.Dx6, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx7 != null && record.Dx7 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx7, amaYear);
            record.Dx7Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 7, Code = record.Dx7, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx8 != null && record.Dx8 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx8, amaYear);
            record.Dx8Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 8, Code = record.Dx8, Description = dictRecord?.Description ?? "" });
        }
        if (record.Dx9 != null && record.Dx9 != "")
        {
            var dictRecord = _dictionaryService.GetDiagnosis(uow, record.Dx9, amaYear);
            record.Dx9Desc = dictRecord?.Description ?? "";
            record.Diagnoses.Add(new PatDiag { No = 9, Code = record.Dx9, Description = dictRecord?.Description ?? "" });
        }

        return record;
    }

    public decimal GetNextAccountNumber(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.NumberRepository.GetNumber("account");
    }

    /// <summary>
    /// Get only minimal account info. Only pulls data from acc table. Does not load charges or other detail info.
    /// </summary>
    /// <param name="accountNo"></param>
    /// <returns></returns>
    public Account GetAccountMinimal(string accountNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWork.UnitOfWorkMain(_appEnvironment);
        var acc = uow.AccountRepository.GetByAccount(accountNo);
        return acc;
    }



    public (bool locksuccessful, AccountLock lockInfo) GetAccountLock(string account, IUnitOfWork uow = null)
    {
        ArgumentNullException.ThrowIfNull(account);
        uow.StartTransaction();

        var alock = uow.AccountLockRepository.GetLock(account);

        if (alock == null || string.IsNullOrEmpty(alock?.AccountNo))
        {
            //there is no lock on this account - establish one
            alock = uow.AccountLockRepository.Add(account);
            uow.Commit();
            AccountLocked?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock established for account {account}." });
            return (true, alock);
        }
        else
        {
            // check to see if the lock is this user and machine
            if (alock.UpdatedUser == _appEnvironment.User && alock.UpdatedHost == Environment.MachineName)
            {
                uow.Commit();
                AccountLocked?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock established for account {account}." });
                return (true, alock);
            }
        }
        uow.Commit();
        return (false, alock);
    }

    public List<AccountLock> GetAccountLocks(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.AccountLockRepository.GetAll();
    }

    public bool ClearAccountLock(Account account, IUnitOfWork uow = null)
    {
        ArgumentNullException.ThrowIfNull(account);
        uow ??= new UnitOfWorkMain(_appEnvironment);

        if (account.AccountLockInfo == null)
        {
            account.AccountLockInfo = uow.AccountLockRepository.GetLock(account.AccountNo);
            if (account.AccountLockInfo == null)
            {
                Log.Instance.Warn($"Account Lock not found for {account.AccountNo}");
                return false;
            }
        }
        return ClearAccountLock(account.AccountLockInfo.id, uow);
    }

    public bool ClearAccountLock(int id, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var retval = uow.AccountLockRepository.Delete(id);
        AccountLockCleared?.Invoke(this, new AccountEventArgs() { UpdateMessage = $"Lock id {id} cleared." });
        return retval;
    }

    public bool ClearAccountLocks(string username, string hostname, IUnitOfWork uow = null)
    {
        try
        {
            uow ??= new UnitOfWorkMain(_appEnvironment);
            return uow.AccountLockRepository.DeleteByUserHost(username, hostname);
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex);
            return false;
        }
    }

    public async Task<Account> GetAccountAsync(string account) => await Task.Run(() => GetAccount(account));
    public async Task<Account> GetAccountAsync(string account, IUnitOfWork uow) => await Task.Run(() => GetAccount(account, uow));
    public async Task<Account> GetAccountAsync(string account, bool demographicsOnly = false, bool secureLock = true, IUnitOfWork uow = null) => await Task.Run(() => GetAccount(account, demographicsOnly, secureLock, uow));


    public Account GetAccount(string account) => GetAccount(account, false, true, null);
    public Account GetAccount(string account, IUnitOfWork uow = null) => GetAccount(account, false, true, uow);
    public Account GetAccount(string account, bool demographicsOnly = false, bool secureLock = true, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        bool locksuccessful = false;
        AccountLock alock = null;

        if (secureLock)
        {
            //get lock before loading account
            (locksuccessful, alock) = GetAccountLock(account, uow);
            if (!locksuccessful)
            {
                throw new AccountLockException(alock);
            }
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
            ClearAccountLock(alock.id, uow);
            return null;
        }

        record.AccountLockInfo = alock;
        if (record == null)
            return null;

        if (!string.IsNullOrEmpty(record.ClientMnem))
        {
            if (record.ClientMnem != _appEnvironment.ApplicationParameters.InvalidFinancialCode)
            {
                record.Client = _dictionaryService.GetClient(record.ClientMnem, uow);
            }
        }
        record.Pat = GetPatByAccount(record, uow);
        record.Charges = GetCharges(uow, account, true, true, null, false).ToList();

        if (record.FinCode != _appEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            record.ChrgDiagnosisPointers = uow.ChrgDiagnosisPointerRepository.GetByAccount(account).ToList();

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
                        record.ChrgDiagnosisPointers.Add(uow.ChrgDiagnosisPointerRepository.Add(ptr));
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
                    uow.ChrgDiagnosisPointerRepository.Delete(d);
                    temp.Add(d);
                }
            });

            temp.ForEach(d => record.ChrgDiagnosisPointers.Remove(d));
        }
        record.Payments = uow.ChkRepository.GetByAccount(account);

        if (!demographicsOnly)
        {
            record.Insurances = GetInsByAccount(account, uow);
            record.Notes = uow.AccountNoteRepository.GetByAccount(account);
            record.BillingActivities = uow.BillingActivityRepository.GetByAccount(account);
            record.AccountValidationStatus = uow.AccountValidationStatusRepository.GetByAccount(account);
            record.Fin = uow.FinRepository.GetFin(record.FinCode);
            record.AccountAlert = uow.AccountAlertRepository.GetByAccount(account);
            record.PatientStatements = uow.PatientStatementAccountRepository.GetByAccount(account);
        }

        return record;
    }

    public Pat ClearCollectionsListDate(string accountNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var pat = uow.PatRepository.GetByKey(accountNo);
        pat.BadDebtListDate = null;
        uow.PatRepository.Update(pat, new List<string>() { nameof(Pat.BadDebtListDate) });
        return pat;
    }

    public double GetBalance(string accountNo, IUnitOfWork uow)
    {
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
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.ChargeInvoiceStatus })
                .Append($"or ({accTableName}.{uow.AccountRepository.GetRealColumn(nameof(Account.FinCode))} <> @0 and {chrgTableName}.{uow.ChrgRepository.GetRealColumn(nameof(Chrg.Status))} not in (@1, @2, @3)))",
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.ChargeInvoiceStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.CapitatedChargeStatus },
                    new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = _appEnvironment.ApplicationParameters.NotApplicableChargeStatus })
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
    public async Task<object> AddAsync(Account table, IUnitOfWork uow) => await Task.Run(() => Add(table, uow));

    public Account Add(Account table, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {table.AccountNo}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        if (table.FinCode != "CLIENT")
            table.PatFullName = table.PatNameDisplay;

        table.Status = AccountStatus.New;

        table.TransactionDate = table.TransactionDate.Date;

        uow.AccountRepository.Add(table);

        //make sure Pat record has an account number
        if (table.Pat.AccountNo != table.AccountNo)
            table.Pat.AccountNo = table.AccountNo;

        var pat = GetPatByAccount(table, uow);
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
        var result = GetAccountLock(table.AccountNo, uow);
        if (result.locksuccessful)
            table.AccountLockInfo = result.lockInfo;
        uow.Commit();

        return table;
    }

    public async Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate, IUnitOfWork uow) => await Task.Run(() => GetInvoiceAccounts(clientMnem, thruDate, uow));

    public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate, IUnitOfWork uow)
    {
        Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");
        return uow.InvoiceSelectRepository.GetByClientAndDate(clientMnem, thruDate);
    }

    public async Task<IEnumerable<ClaimItem>> GetAccountsForClaimsAsync(IUnitOfWork uow, ClaimType claimType, int maxClaims = 0) => await Task.Run(() => GetAccountsForClaims(uow, claimType, maxClaims));

    public IEnumerable<ClaimItem> GetAccountsForClaims(IUnitOfWork uow, ClaimType claimType, int maxClaims = 0)
    {
        Log.Instance.Trace($"Entering - claimType {claimType}");
        try
        {
            var queryResult = GetClaimItems(claimType, uow);
            return queryResult;
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex, $"Exception in");
        }
        Log.Instance.Trace("Exiting");

        return new List<ClaimItem>();
    }

    public List<ClaimItem> GetClaimItems(ClaimType claimType, IUnitOfWork uow)
    {
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

    public List<PatientStatementAccount> GetPatientStatements(string accountNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var statements = uow.PatientStatementAccountRepository.GetByAccount(accountNo);

        return statements;
    }

    public async Task<bool> SetNoteAlertAsync(string account, bool showAlert, IUnitOfWork uow) => await Task.Run(() => SetNoteAlert(account, showAlert, uow));

    public bool SetNoteAlert(string account, bool showAlert, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {account} showAlert {showAlert}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
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

    public Account UpdateAccountDemographics(Account acc, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
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

    public async Task<Account> UpdateDiagnosesAsync(Account acc, IUnitOfWork uow = null) => await Task.Run(() => UpdateDiagnoses(acc, uow));

    public Account UpdateDiagnoses(Account acc, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        if (acc == null)
            throw new ArgumentNullException(nameof(acc));

        var patResult = uow.PatRepository.SaveDiagnoses(acc.Pat);
        acc.Pat = patResult;
        uow.Commit();

        return acc;
    }

    public async Task<int> UpdateStatusAsync(string accountNo, string status, IUnitOfWork uow) => await Task.Run(() => UpdateStatus(accountNo, status, uow));
    public async Task<Account> UpdateStatusAsync(Account model, string status, IUnitOfWork uow) => await Task.Run(() => UpdateStatus(model, status, uow));

    public int UpdateStatus(string accountNo, string status, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {accountNo} status {status}");

        uow ??= new UnitOfWork.UnitOfWorkMain(_appEnvironment);


        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(status))
            throw new ArgumentNullException(nameof(status));
        if (!AccountStatus.IsValid(status))
            throw new ArgumentOutOfRangeException(nameof(status), "Invalid status");

        var returnval = uow.AccountRepository.UpdateStatus(accountNo, status);

        return returnval;
    }

    public Account UpdateStatus(Account model, string status, IUnitOfWork uow = null)
    {
        try
        {
            uow ??= new UnitOfWorkMain(_appEnvironment);
            var result = uow.AccountRepository.UpdateStatus(model, status);
            return result;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error updating status.", ex);
        }
    }

    public Account UpdateStatementFlag(Account acc, string flag, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        uow.StartTransaction();

        try
        {
            AddNote(acc.AccountNo, $"Statement flag changed from {acc.Pat.StatementFlag} to {flag}", uow);
            acc.Pat.StatementFlag = flag;
            if (flag != "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.Statements, uow);
                acc.Status = AccountStatus.Statements;
            }
            if (flag == "N")
            {
                UpdateStatus(acc.AccountNo, AccountStatus.New, uow);
                acc.Status = AccountStatus.New;
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

    public Ins SaveInsurance(Ins ins, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");

        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
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

    public bool DeleteInsurance(Ins ins, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
        var retval = uow.InsRepository.Delete(ins);
        uow.Commit();
        return retval;
    }

    public async Task<Account> InsuranceSwapAsync(Account account, InsCoverage swap1, InsCoverage swap2, IUnitOfWork uow = null) => await Task.Run(() => InsuranceSwap(account, swap1, swap2, uow));

    public Account InsuranceSwap(Account account, InsCoverage swap1, InsCoverage swap2, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Account {account.AccountNo} Ins1 {swap1} Ins2 {swap2}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        Ins insA = account.Insurances.Where(x => x.Coverage == swap1).FirstOrDefault();
        Ins insB = account.Insurances.Where(x => x.Coverage == swap2).FirstOrDefault();

        uow.StartTransaction();

        try
        {
            insA.Coverage = InsCoverage.Temporary;
            insB.Coverage = swap1;

            insA = uow.InsRepository.Update(insA);
            insB = uow.InsRepository.Update(insB);

            insA.Coverage = swap2;
            insA = uow.InsRepository.Update(insA);

            account.Notes = AddNote(account.AccountNo, $"Insurance swap: {swap1} {insA.PlanName} and {swap2} {insB.PlanName}", uow).ToList();
            account.Insurances = GetInsByAccount(account.AccountNo, uow);
            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error swapping insurances.");
            throw new ApplicationException("Error during insurance swap");
        }

    }

    public async Task<Account> ChangeDateOfServiceAsync(Account table, DateTime newDate, string reason_comment, IUnitOfWork uow = null) => await Task.Run(() => ChangeDateOfService(table, newDate, reason_comment, uow));

    public Account ChangeDateOfService(Account model, DateTime newDate, string reason_comment, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new date {newDate} reason {reason_comment}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(reason_comment);

        DateTime oldServiceDate = DateTime.MinValue;

        // update trans_date on acc table
        if (model.TransactionDate != newDate)
        {
            oldServiceDate = (DateTime)model.TransactionDate;
            model.TransactionDate = newDate;
            model = uow.AccountRepository.Update(model, new[] { nameof(Account.TransactionDate) });

            model.Notes = AddNote(model.AccountNo, $"Service Date changed from {oldServiceDate} to {newDate}", uow).ToList();

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

    public async Task<IList<AccountNote>> AddNoteAsync(string account, string noteText, IUnitOfWork uow) => await Task.Run(() => AddNote(account, noteText, uow));

    public IList<AccountNote> AddNote(string accountNo, string noteText, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {accountNo} note {noteText}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(noteText))
            throw new ArgumentNullException(nameof(noteText));

        uow.StartTransaction();

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

    public List<AccountNote> GetNotes(string accountNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var notes = uow.AccountNoteRepository.GetByAccount(accountNo);
        return notes;
    }

    public async Task<Account> ChangeFinancialClassAsync(string accountNo, string newFinCode, IUnitOfWork uow = null) => await Task.Run(() => ChangeFinancialClass(accountNo, newFinCode, uow));

    public Account ChangeFinancialClass(string accountNo, string newFinCode, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - Account {accountNo} New Fin {newFinCode}");
        uow ??= new UnitOfWork.UnitOfWorkMain(_appEnvironment);

        uow.StartTransaction();

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));
        if (string.IsNullOrEmpty(newFinCode))
            throw new ArgumentNullException(nameof(newFinCode));

        var record = GetAccount(accountNo, uow);

        var retval = ChangeFinancialClass(record, newFinCode, uow);
        uow.Commit();
        return retval;
    }

    public async Task<Account> ChangeFinancialClassAsync(Account model, string newFinCode, IUnitOfWork uow = null) => await Task.Run(() => ChangeFinancialClass(model, newFinCode, uow));

    public Account ChangeFinancialClass(Account model, string newFinCode, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new fin {newFinCode}");
        uow ??= new UnitOfWork.UnitOfWorkMain(_appEnvironment);

        if (model == null)
            throw new ArgumentNullException(nameof(model));
        else if (newFinCode == null)
            throw new ArgumentNullException(nameof(newFinCode));

        if (model.FinCode == newFinCode)
        {
            throw new ApplicationException($"Chosen fin code is same as current fin code. No change made. Account {model.AccountNo}, Current Fin Code {model.FinCode}, New Fin Code {newFinCode}");
        }

        uow.StartTransaction();

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
                if (model.ReadyToBill)
                {
                    //clear ready to bill
                    model.Status = AccountStatus.New;
                    model.Notes = AddNote(model.AccountNo, "Ready to Bill status cleared due to financial class change.", uow).ToList();
                }
                uow.AccountRepository.Update(model, new[] { nameof(Account.FinCode), nameof(Account.Status) });
                model.FinCode = newFin.FinCode;
                model.Fin = newFin;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                throw new ApplicationException($"Exception updating fin code for {model.AccountNo}.", ex);
            }
            model.Notes = AddNote(model.AccountNo, $"Financial code updated from {oldFinCode} to {newFinCode}.", uow).ToList();

            //reprocess charges if needed due to financial code change.
            if (newFin.FinClass != oldFin.FinClass)
            {
                try
                {
                    model.Charges = ReprocessCharges(model, $"Fin Code changed from {oldFinCode} to {newFinCode}", uow).ToList();
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Error reprocessing charges.", ex);
                }
            }
            else
            {
                model.Charges = UpdateChargesFinCode(model.Charges, newFinCode, uow).ToList();
            }
            uow.Commit();
            model = Validate(model, false, uow);
        }

        Log.Instance.Trace($"Exiting");
        return model;
    }

    public async Task<bool> ChangeClientAsync(Account table, string newClientMnem, IUnitOfWork uow = null) => await Task.Run(() => ChangeClient(table, newClientMnem, uow));

    public bool ChangeClient(Account model, string newClientMnem, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {model.AccountNo} new client {newClientMnem}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (model == null)
            throw new ArgumentNullException(nameof(model));
        else ArgumentNullException.ThrowIfNull(newClientMnem);

        uow.StartTransaction();

        bool updateSuccess = true;
        string oldClientMnem = model.ClientMnem;

        if (oldClientMnem == null)
        {
            //account does not have a valid current client defined
            uow.Commit();
            throw new ApplicationException($"Account {model.AccountNo} does not have a valid client defined.");
        }

        Client oldClient = uow.ClientRepository.GetClient(oldClientMnem);
        Client newClient = uow.ClientRepository.GetClient(newClientMnem);

        if (oldClient == null)
        {
            uow.Commit();
            Log.Instance.Error("Client mnem {oldClientMnem} is not valid.");
            throw new ApplicationException($"Client mnem {oldClientMnem} is not valid.");
        }

        if (newClient == null)
        {
            uow.Commit();
            throw new ArgumentException($"Client mnem {newClientMnem} is not valid.", nameof(newClientMnem));
        }

        if (string.IsNullOrEmpty(newClient.FeeSchedule) || newClient.FeeSchedule == "0")
        {
            uow.Commit();
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
                    uow.AccountRepository.Update(model, new[] { nameof(Account.ClientMnem) });
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    uow.Rollback();
                    throw new ApplicationException($"Exception updating client for {model.AccountNo}.", ex);
                }
                AddNote(model.AccountNo, $"Client updated from {oldClientMnem} to {newClientMnem}.", uow);

                //reprocess charges if fin class is client bill (C) to pick up proper discounts.
                if (model.Fin.FinClass == _appEnvironment.ApplicationParameters.ClientFinancialTypeCode)
                {
                    try
                    {
                        model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}", uow).ToList();
                    }
                    catch (Exception ex)
                    {
                        uow.Rollback();
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
                            model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}", uow).ToList();
                        }
                        catch (Exception ex)
                        {
                            uow.Rollback();
                            throw new ApplicationException("Error reprocessing charges.", ex);
                        }
                    }
                    else if (oldClient.ClientMnem == _appEnvironment.ApplicationParameters.PathologyBillingClientException
                        || newClient.ClientMnem == _appEnvironment.ApplicationParameters.PathologyBillingClientException)
                    {
                        try
                        {
                            model.Charges = ReprocessCharges(model, $"Client changed from {oldClientMnem} to {newClientMnem}", uow).ToList();
                        }
                        catch (Exception ex)
                        {
                            uow.Rollback();
                            throw new ApplicationException("Error reprocessing charges.", ex);
                        }
                    }
                    else
                    {
                        try
                        {
                            model.Charges = UpdateChargesClient(model.AccountNo, newClientMnem, uow).ToList();
                        }
                        catch (Exception ex)
                        {
                            uow.Rollback();
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

    public Chrg GetCharge(int chrgNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var charge = uow.ChrgRepository.GetById(chrgNo);

        charge.Cdm = uow.CdmRepository.GetCdm(charge.CDMCode);
        charge.ChrgDetails = uow.ChrgDetailRepository.GetByChrgId(chrgNo).ToList();

        return charge;
    }

    public IList<ClaimChargeView> GetClaimCharges(string accountNo, IUnitOfWork uow)
    {
        var charges = uow.ChrgRepository.GetClaimCharges(accountNo);

        foreach (var chrg in charges)
        {
            chrg.RevenueCodeDetail = uow.RevenueCodeRepository.GetByCode(chrg.RevenueCode);
            chrg.Cdm = uow.CdmRepository.GetCdm(chrg.ChargeId, true);
        }

        return charges;
    }

    public IList<Chrg> GetCharges(IUnitOfWork uow, string accountNo, bool showCredited = true, bool includeInvoiced = true, DateTime? asOfDate = null, bool excludeCBill = true)
    {
        Log.Instance.Debug($"Entering - account {accountNo}");

        if (string.IsNullOrEmpty(accountNo))
            throw new ArgumentNullException(nameof(accountNo));

        List<Chrg> charges = uow.ChrgRepository.GetByAccount(accountNo, showCredited, includeInvoiced, asOfDate, excludeCBill);

        charges.ForEach(chrg =>
        {
            chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, true, uow);
            AddRevenueDiagnosisToChrg(chrg, uow);
        });


        return charges;
    }

    internal void AddRevenueDiagnosisToChrg(Chrg chrg, IUnitOfWork uow)
    {
        chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, true, uow);
        chrg.ChrgDetails = uow.GetRepository<ChrgDetailRepository>(true).GetByChrgId(chrg.ChrgId).ToList();
        chrg.ChrgDetails.ForEach(detail =>
        {
            detail.RevenueCodeDetail = _dictionaryService.GetRevenueCode(uow, detail.RevenueCode);
            var cpt = uow.GetRepository<CptAmaRepository>(true).GetCpt(detail.Cpt4);
            if (cpt != null)
                detail.CptDescription = cpt.ShortDescription;
        });

    }

    public async Task<IList<Chrg>> ReprocessChargesAsync(Account account, string comment, IUnitOfWork uow) => await Task.Run(() => ReprocessCharges(account, comment, uow));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public IList<Chrg> ReprocessCharges(Account account, string comment, IUnitOfWork uow)
    {
        Log.Instance.Trace($"Entering {account}");

        if (account == null)
            throw new ArgumentNullException(nameof(account));

        uow.StartTransaction();
        try
        {
            var chargesToCredit = account.Charges.Where(x => x.IsCredited == false && x.CDMCode != _appEnvironment.ApplicationParameters.ClientInvoiceCdm).ToList();

            chargesToCredit.ForEach(c =>
            {
                CreditCharge(c.ChrgId, comment, uow);
                AddCharge(new AddChargeParameters() 
                { 
                    Account = account, 
                    Cdm = c.CDMCode, 
                    Quantity = c.Quantity, 
                    ServiceDate = account.TransactionDate, 
                    Uow = uow 
                });
            });

            var updatedChrgList = GetCharges(uow, account.AccountNo);

            uow.Commit();
            return updatedChrgList;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error reprocessing charges", ex);
        }
    }

    public async Task<IList<Chrg>> ReprocessChargesAsync(string account, string comment, IUnitOfWork uow) => await Task.Run(() => ReprocessCharges(account, comment, uow));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public IList<Chrg> ReprocessCharges(string account, string comment, IUnitOfWork uow)
    {
        Log.Instance.Trace($"Entering {account}");

        if (string.IsNullOrEmpty(account))
            throw new ArgumentNullException(nameof(account));

        var acc = GetAccount(account, uow);

        if (acc == null)
        {
            Log.Instance.Error($"Account {account} is not a valid account.");
            throw new AccountNotFoundException($"Account is not a valid account.", account);
        }

        return ReprocessCharges(acc, comment, uow);
    }

    public async Task<IList<Chrg>> UpdateChargesFinCodeAsync(IList<Chrg> charges, string finCode, IUnitOfWork uow) => await Task.Run(() => UpdateChargesFinCode(charges, finCode, uow));

    public IList<Chrg> UpdateChargesFinCode(IList<Chrg> charges, string finCode, IUnitOfWork uow)
    {
        Log.Instance.Trace("Entering");
        try
        {
            uow.StartTransaction();

            if (charges == null || charges.Count == 0)
            {
                Log.Instance.Info($"No charges to update.");
                uow.Commit();
                return charges;
            }
            //throw new ApplicationException($"No charges for account {charges.First().AccountNo}");

            var fin = uow.FinRepository.GetFin(finCode) ?? throw new ApplicationException($"Fin {finCode} is not valid");

            var chrgsToUpdate = charges.Where(x => x.IsCredited == false &&
                (x.ClientMnem != _appEnvironment.ApplicationParameters.PathologyGroupClientMnem ||
                string.IsNullOrEmpty(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem))
                && x.FinancialType == fin.FinClass).ToList();

            chrgsToUpdate.ForEach(c =>
            {
                c.FinCode = finCode;
                c.FinancialType = fin.FinClass;
                c = uow.ChrgRepository.Update(c, new[] { nameof(Chrg.FinancialType), nameof(Chrg.FinCode) });
            });

            uow.Commit();
            return GetCharges(uow, charges.First().AccountNo).ToList();
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
    public async Task<IList<Chrg>> UpdateChargesClientAsync(string account, string clientMnem, IUnitOfWork uow) => await Task.Run(() => UpdateChargesClient(account, clientMnem, uow));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="account"></param>
    /// <param name="clientMnem"></param>
    /// <returns></returns>
    public IList<Chrg> UpdateChargesClient(string account, string clientMnem, IUnitOfWork uow)
    {
        Log.Instance.Trace("Entering");

        uow.StartTransaction();

        List<Chrg> charges = GetCharges(uow, account).ToList();

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

    public Chrg AddCharge(Chrg chrg, IUnitOfWork uow)
    {
        Log.Instance.Trace("Entering");

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

    public async Task<Account> AddChargeAsync(AddChargeParameters parameters)
        => await Task.Run(() => AddCharge(parameters));

    /// <summary>
    /// Adds a charge to an account using the specified parameters.
    /// </summary>
    /// <param name="parameters">
    /// An instance of <see cref="AddChargeParameters"/> containing the details of the charge to be added:
    /// <list type="bullet">
    /// <item><term>Account</term> (optional): The <see cref="Account"/> object to add the charge to.</item>
    /// <item><term>AccountNumber</term> (optional): The account number if the <c>Account</c> object is not provided.</item>
    /// <item><term>Cdm</term> (required): The CDM code representing the charge.</item>
    /// <item><term>Quantity</term> (required): The quantity of the charge to add.</item>
    /// <item><term>ServiceDate</term> (required): The date of service for the charge.</item>
    /// <item><term>Comment</term> (optional): Any additional comments related to the charge.</item>
    /// <item><term>RefNumber</term> (optional): A reference number associated with the charge.</item>
    /// <item><term>MiscAmount</term> (optional): An optional miscellaneous amount for the charge.</item>
    /// <item><term>Uow</term> (optional): An instance of <see cref="IUnitOfWork"/> for database transactions.</item>
    /// </list>
    /// </param>
    /// <returns>
    /// The updated <see cref="Account"/> object after the charge has been added.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the <c>Cdm</c> parameter is null or empty.
    /// </exception>
    /// <exception cref="AccountNotFoundException">
    /// Thrown when the specified account does not exist.
    /// </exception>
    /// <exception cref="InvalidClientException">
    /// Thrown when the account's client information is invalid.
    /// </exception>
    /// <exception cref="CdmNotFoundException">
    /// Thrown when the specified CDM code is not found in the system.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when the <c>Quantity</c> is less than or equal to zero.
    /// </exception>
    /// <remarks>
    /// This method allows adding a charge to an account by providing all necessary details encapsulated in an
    /// <see cref="AddChargeParameters"/> object. It handles transaction management and commits the changes to the database.
    /// </remarks>
    public Account AddCharge(AddChargeParameters parameters)
    {
        //check for required parameters
        ArgumentNullException.ThrowIfNull(parameters);

        ArgumentNullException.ThrowIfNullOrEmpty(parameters.Cdm);

        //set default values for optional parameters
        string comment = parameters.Comment ?? string.Empty;
        string refNumber = parameters.RefNumber ?? string.Empty;
        double miscAmount = parameters.MiscAmount;

        Account accData;
        if(parameters.Account == null)
        {
            //see if account number is provided and get account
            if (string.IsNullOrEmpty(parameters.AccountNumber))
                throw new ArgumentNullException(nameof(parameters.AccountNumber));

            accData = GetAccount(parameters.AccountNumber, parameters.Uow);
            if(accData == null)
                throw new AccountNotFoundException("Account is not a valid account.", parameters.AccountNumber);
        }
        else
        {
            accData = parameters.Account;
        }

        Log.Instance.Trace($"Entering - account {accData.AccountNo} cdm {parameters.Cdm}");
        var uow = parameters.Uow ?? new UnitOfWorkMain(_appEnvironment);

        if (accData.Client == null)
            throw new InvalidClientException("Client not valid", accData.ClientMnem);

        if (string.IsNullOrEmpty(accData.Client.FeeSchedule))
            throw new ApplicationException($"Fee Schedule not defined on client. Cannot post charge. Client {accData.ClientMnem}");

        Fin fin = _dictionaryService.GetFinCode(uow, accData.FinCode) ?? throw new ApplicationException($"No fincode on account {accData.AccountNo}");

        //get the cdm number - if cdm number is not found - abort
        Cdm cdmData = _dictionaryService.GetCdm(parameters.Cdm, uow);
        if (cdmData == null)
        {
            Log.Instance.Error($"CDM {parameters.Cdm} not found.");
            throw new CdmNotFoundException("CDM not found.", parameters.Cdm);
        }

        uow.StartTransaction();

        //check account status, change to NEW if it is paid out.
        if (accData.Status == AccountStatus.PaidOut)
        {
            UpdateStatus(accData.AccountNo, AccountStatus.New, uow);
            accData.Status = AccountStatus.New;
        }

        Client chargeClient = accData.Client;

        if (_appEnvironment.ApplicationParameters.PathologyGroupBillsProfessional)
        {
            //check for global billing cdm - if it is, change client to Pathology Group, fin to Y, and get appropriate prices
            var gb = uow.GlobalBillingCdmRepository.GetCdm(parameters.Cdm, accData.TransactionDate);
            //hard coding exception for Hardin County for now - 05/09/2023 BSP
            if (gb != null && accData.ClientMnem != _appEnvironment.ApplicationParameters.PathologyBillingClientException)
            {
                fin = uow.FinRepository.GetFin(_appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
                    ?? throw new ApplicationException($"Fin code {_appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode} not found error {accData.AccountNo}");
                chargeClient = uow.ClientRepository.GetClient(_appEnvironment.ApplicationParameters.PathologyGroupClientMnem);
            }
        }

        Chrg chrg = new()
        {
            //now build the charge & detail records
            AccountNo = accData.AccountNo,
            BillMethod = fin.ClaimType,
            CDMCode = parameters.Cdm,
            Comment = comment,
            IsCredited = false,
            FinCode = fin.FinCode,
            ClientMnem = chargeClient.ClientMnem,
            FinancialType = fin.FinClass,
            OrderMnem = cdmData.Mnem,
            LISReqNo = refNumber,
            PostingDate = DateTime.Today,
            Quantity = parameters.Quantity,
            ServiceDate = parameters.ServiceDate
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
                var cliDiscount = chargeClient.Discounts?.Find(c => c.Cdm == parameters.Cdm);
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
            var cpt = uow.CptAmaRepository.GetCpt(chrgDetail.Cpt4);
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
                accData.ChrgDiagnosisPointers.Add(uow.ChrgDiagnosisPointerRepository.Add(ptr));
            }
        }

        chrg.NetAmount = amtTotal;
        chrg.HospAmount = ztotal;
        chrg.RetailAmount = retailTotal;

        chrg = uow.ChrgRepository.Add(chrg);
        chrg.Cdm = _dictionaryService.GetCdm(chrg.CDMCode, uow);
        chrgDetails.ForEach(d =>
        {
            d.ChrgNo = chrg.ChrgId;
            d = uow.ChrgDetailRepository.Add(d);
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

    public async Task<Account> UnbundlePanelsAsync(Account account, IUnitOfWork uow) => await Task.Run(() => UnbundlePanels(account, uow));

    public Account UnbundlePanels(Account account, IUnitOfWork uow)
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
            uow.StartTransaction();

            foreach (var bundledProfile in bundledProfiles1)
            {
                //credit the profile charge
                CreditCharge(bundledProfile.ChrgId, "Unbundling charge", uow);

                //enter charges for each component
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5545154", Quantity = 1, ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5382522", Quantity = 1, ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5646008", Quantity = 1, ServiceDate = account.TransactionDate, Uow = uow });
            }


            foreach (var bundledProfile in bundledProfiles2)
            {
                //credit the profile charge
                CreditCharge(bundledProfile.ChrgId, "Unbundling charge", uow);

                //enter charges for each component
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5545154", ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5646012", ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5646086", ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5646054", ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5728026", ServiceDate = account.TransactionDate, Uow = uow });
                AddCharge(new AddChargeParameters() { Account = account, Cdm = "5728190", ServiceDate = account.TransactionDate, Uow = uow });
            }

            account.Charges = GetCharges(uow, account.AccountNo).ToList();

            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error during UnbundlePanels", ex);
        }

    }

    public async Task<Account> BundlePanelsAsync(Account account, IUnitOfWork uow) => await Task.Run(() => BundlePanels(account, uow));

    public Account BundlePanels(Account account, IUnitOfWork uow)
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

        uow.StartTransaction();

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
                    CreditCharge(bundledProfiles[x].ComponentCpt[i].ChrgId, $"Bundling to {bundledProfiles[x].ProfileCdm}", uow);
                }

                this.AddCharge(new AddChargeParameters() { Account = account, Cdm = bundledProfiles[x].ProfileCdm, Quantity = 1, ServiceDate = account.TransactionDate, Uow = uow });
                account.Notes = this.AddNote(account.AccountNo, $"Bundled charges into {bundledProfiles[x].ProfileCdm}", uow).ToList();
            }
        }
        account.Charges = GetCharges(uow, account.AccountNo).ToList();

        uow.Commit();
        return account;
    }

    public async Task<Account> ValidateAsync(Account account, bool reprint = false, IUnitOfWork uow = null) => await Task.Run(() => Validate(account, reprint, uow));

    /// <summary>
    /// Runs all validation routines on account. Updates validation status and account flags. Errors are stored in the validation status table.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="reprint">Set true if validating account to resubmit the claim with no changes.</param>
    /// <returns>True if account is valid for billing, false if there are validation errors.</returns>
    public Account Validate(Account account, bool reprint = false, IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - account {account}");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        try
        {
            uow.StartTransaction();

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
                if (account.Fin.FinClass == _appEnvironment.ApplicationParameters.PatientFinancialTypeCode
                    && account.InsurancePrimary != null)
                {
                    if (account.InsurancePrimary.InsCompany != null)
                    {
                        if (account.InsurancePrimary.InsCompany.IsMedicareHmo)
                            account = UnbundlePanels(account, uow);
                        if (!account.InsurancePrimary.InsCompany.IsMedicareHmo)
                            account = BundlePanels(account, uow);
                    }
                }

                Services.Validators.ClaimValidator claimValidator = new();
                account.LmrpErrors = ValidateLMRP(account, uow);
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
                        UpdateStatus(account.AccountNo, AccountStatus.New, uow);
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
                        UpdateStatus(account.AccountNo, AccountStatus.New, uow);
                    }
                }

                if (isAccountValid)
                {
                    account.AccountValidationStatus.ValidationText = "No validation errors.";
                    //update account status if this account has been flagged to bill
                    if (account.Status == AccountStatus.ReadyToBill)
                    {
                        account.Status = account.BillForm;
                        UpdateStatus(account.AccountNo, account.BillForm, uow);
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
            uow.AccountValidationStatusRepository.Save(account.AccountValidationStatus);
            return account;
        }
    }

    private async Task<List<string>> ValidateLMRPAsync(Account account, IUnitOfWork uow) => await Task.Run(() => ValidateLMRP(account, uow));

    private List<string> ValidateLMRP(Account account, IUnitOfWork uow)
    {
        Log.Instance.Trace($"Entering - account {account}");
        List<string> errorList = new();

        uow.StartTransaction();

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

            bool dxIsValid = ruleDef.DxIsValid != 0;
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
                //check if charge has a GZ modifier
                if (!account.BillableCharges.Any(x => x.ChrgDetails.Where(y => y.Cpt4 == cpt4).Any(z => z.Modifier == "GZ" || z.Modifier2 == "GZ")))
                {
                    errorList.Add($"LMRP Violation - No dx codes support medical necessity for cpt {cpt4}.");
                }                
            }
        }
        uow.Commit();
        return errorList;
    }

    public void ValidateUnbilledAccounts(IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);

        (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
            (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, DateTime.Today.AddDays((_appEnvironment.ApplicationParameters.BillingInitialHoldDays)*-1).ToShortDateString()),
            (nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, AccountStatus.New),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.ClientAccountFinCode),
            (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, _appEnvironment.ApplicationParameters.SelfPayFinancialCode)
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
                using var lUow = new UnitOfWorkMain(_appEnvironment);
                var accountRecord = this.GetAccount(account.Account, lUow);
                this.Validate(accountRecord, false, uow);
                ClearAccountLock(accountRecord, lUow);
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

    public async Task ValidateUnbilledAccountsAsync(IUnitOfWork uow) => await Task.Run(() => ValidateUnbilledAccounts(uow));

    /// <summary>
    /// Move all charges between accounts. Credits the charges from sourceAccount and charges them on destinationAccount.
    /// SourceAccount will have a zero total charge amount.
    /// </summary>
    /// <param name="sourceAccount"></param>
    /// <param name="destinationAccount"></param>
    public (bool isSuccess, string error) MoveCharges(string sourceAccount, string destinationAccount, IUnitOfWork uow = null  )
    {
        if (string.IsNullOrEmpty(sourceAccount) || string.IsNullOrEmpty(destinationAccount))
        {
            throw new ArgumentException("One or both arguments are null or empty.");
        }
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        var source = GetAccount(sourceAccount, uow);
        var destination = GetAccount(destinationAccount, uow);
        if (source == null || destination == null)
        {
            Log.Instance.Error($"Either source or destination account was not valid. {sourceAccount} {destinationAccount}");
            return (false, "Either source or destination account was not valid.");
        }

        var charges = source.Charges.Where(c => c.IsCredited == false).ToList();

        charges.ForEach(charge =>
        {
            CreditCharge(charge.ChrgId, $"Move to {destinationAccount}", uow);
            AddCharge(new AddChargeParameters() { Quantity = charge.Quantity, Account = destination, Cdm = charge.CDMCode, ServiceDate = (DateTime)destination.TransactionDate, Comment = $"Moved from {sourceAccount}", Uow = uow });
        });

        uow.Commit();
        ClearAccountLock(source, uow);
        ClearAccountLock(destination, uow);
        return (true, string.Empty);
    }

    public async Task MoveChargeAsync(string sourceAccount, string destinationAccount, int chrgId, IUnitOfWork uow) => await Task.Run(() => MoveCharge(sourceAccount, destinationAccount, chrgId, uow));

    /// <summary>
    /// Moves a single charge from sourceAccount to destinationAccount
    /// </summary>
    /// <param name="sourceAccount"></param>
    /// <param name="destinationAccount"></param>
    /// <param name="chrgId"></param>
    /// <returns>Credited charge record</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public Chrg MoveCharge(string sourceAccount, string destinationAccount, int chrgId, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (string.IsNullOrEmpty(sourceAccount) || string.IsNullOrEmpty(destinationAccount))
        {
            throw new ArgumentException("One or both arguments are null or empty.");
        }
        uow.StartTransaction();
        var source = GetAccount(sourceAccount, uow);
        var destination = GetAccount(destinationAccount, uow);

        var charge = source.Charges.SingleOrDefault(c => c.ChrgId == chrgId);

        if (charge.IsCredited)
        {
            throw new ApplicationException("Charge is already credited.");
        }

        var creditedCharge = CreditCharge(charge.ChrgId, $"Move to {destinationAccount}", uow);
        AddCharge(new AddChargeParameters()
        {
            AccountNumber = destinationAccount,
            Cdm = charge.CDMCode,
            Quantity = charge.Quantity,
            ServiceDate = (DateTime)destination.TransactionDate,
            Comment = $"Moved from {sourceAccount}",
            RefNumber = charge.ReferenceReq,
            MiscAmount = 0,
            Uow = uow
        });
        uow.Commit();
        return creditedCharge;
    }

    public void AddRecentlyAccessedAccount(string accountNo, string userName, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.UserProfileRepository.InsertRecentAccount(accountNo, userName);
    }

    public Chk AddPayment(Chk chk, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        try
        {
            if (chk.IsRefund)
                chk.Status = "REFUND";

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

    public Chk ReversePayment(Chk chk, string comment, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();
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

            newChk = uow.ChkRepository.Add(newChk);
            uow.Commit();
            return newChk;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            throw new ApplicationException("Error occurred in Reverse Payment", ex);
        }
    }

    public ChrgDiagnosisPointer UpdateDiagnosisPointer(ChrgDiagnosisPointer ptr, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        ChrgDiagnosisPointer retPtr;
        if (ptr.Id < 1)
            retPtr = uow.ChrgDiagnosisPointerRepository.Add(ptr);
        else
            retPtr = uow.ChrgDiagnosisPointerRepository.Update(ptr);

        uow.Commit();
        return retPtr;
    }

    public async Task ClearClaimStatusAsync(Account account, IUnitOfWork uow) => await Task.Run(() => ClearClaimStatus(account, uow));

    /// <summary>
    /// Clears all claim flags so account will be picked up in next claim batch
    /// </summary>
    /// <param name="account"></param>
    public Account ClearClaimStatus(Account account, IUnitOfWork uow = null)
    {
        Log.Instance.Trace("Entering");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        uow.StartTransaction();
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
            UpdateStatus(account.AccountNo, AccountStatus.New, uow);
            account.Status = AccountStatus.New;

            account.Notes = AddNote(account.AccountNo, "Claim status cleared.", uow).ToList();
            uow.Commit();
            return account;
        }
        catch (Exception ex)
        {
            throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
        }
    }

    public ChrgDetail RemoveChargeModifier(int chrgDetailId, IUnitOfWork uow)
    {
        uow.StartTransaction();
        var retval = uow.ChrgDetailRepository.RemoveModifier(chrgDetailId);
        var chrgDetail = uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
        uow.Commit();
        return chrgDetail;
    }

    public ChrgDetail AddChargeModifier(int chrgDetailId, string modifier, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        try
        {
            uow.StartTransaction();
            var retval = uow.ChrgDetailRepository.UpdateModifier(chrgDetailId, modifier);
            var chrgDetail = uow.ChrgDetailRepository.GetByKey((object)chrgDetailId);
            uow.Commit();

            return chrgDetail;
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex.Message);
            throw new ApplicationException("Error adding modifier.", apex);
        }
    }

    public Chrg CreditCharge(int chrgId, string comment = "", IUnitOfWork uow = null)
    {
        Log.Instance.Trace($"Entering - chrg number {chrgId} comment {comment}");
        uow ??= new UnitOfWorkMain(_appEnvironment);
        bool setCreditFlag = false;
        bool setOldChrgCreditFlag = false;

        if (chrgId <= 0)
            throw new ArgumentOutOfRangeException(nameof(chrgId));

        var chrg = GetCharge(chrgId, uow) ?? throw new ApplicationException($"Charge number {chrgId} not found.");

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

        newChrg = AddCharge(newChrg, uow);

        if (setOldChrgCreditFlag)
            chrg = uow.ChrgRepository.SetCredited(chrgId);

        uow.Commit();
        return newChrg;
    }

    public Chrg SetChargeCreditFlag(int chrgId, bool flag, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var chrg = uow.ChrgRepository.SetCredited(chrgId, flag);
        return chrg;
    }

    public Pat SetCollectionsDate(Pat pat, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        pat = uow.PatRepository.Update(pat, new[] { nameof(Pat.BadDebtListDate) });
        uow.Commit();

        return pat;
    }

    public IList<AccountSearch> SearchAccounts(string lastName, string firstName, string mrn, string ssn, string dob,
            string sex, string accountSearch, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        var results = uow.AccountSearchRepository.GetBySearch(lastName, firstName, mrn, ssn, dob, sex, accountSearch).ToList();
        return results;
    }
}
