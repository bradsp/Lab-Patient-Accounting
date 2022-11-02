using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using LabBilling.Core.BusinessLogic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace LabBilling.Core.DataAccess
{
    public class AccountRepository : RepositoryBase<Account>
    {
        private string _connection;
        private readonly PatRepository patRepository;
        private readonly InsRepository insRepository;
        private readonly ChrgRepository chrgRepository;
        private readonly ChkRepository chkRepository;
        private readonly ClientRepository clientRepository;
        private readonly AccountNoteRepository accountNoteRepository;
        private readonly BillingActivityRepository billingActivityRepository;
        private readonly AccountValidationRuleRepository accountValidationRuleRepository;
        private readonly AccountValidationCriteriaRepository accountValidationCriteriaRepository;
        private readonly AccountValidationStatusRepository accountValidationStatusRepository;
        private readonly LMRPRuleRepository lmrpRuleRepository;
        private readonly FinRepository finRepository;
        private readonly SystemParametersRepository systemParametersRepository;
        private readonly AccountLmrpErrorRepository accountLmrpErrorRepository;

        public AccountRepository(string connectionString) : base(connectionString)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection);
            insRepository = new InsRepository(_connection);
            chrgRepository = new ChrgRepository(_connection);
            chkRepository = new ChkRepository(_connection);
            clientRepository = new ClientRepository(_connection);
            accountNoteRepository = new AccountNoteRepository(_connection);
            billingActivityRepository = new BillingActivityRepository(_connection);
            accountValidationRuleRepository = new AccountValidationRuleRepository(_connection);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(_connection);
            accountValidationStatusRepository = new AccountValidationStatusRepository(_connection);
            accountLmrpErrorRepository = new AccountLmrpErrorRepository(_connection);
            lmrpRuleRepository = new LMRPRuleRepository(_connection);
            finRepository = new FinRepository(_connection);
            systemParametersRepository = new SystemParametersRepository(_connection);
        }

        public AccountRepository(PetaPoco.Database db) : base(db)
        {
            _connection = string.Empty;
            patRepository = new PatRepository(db);
            insRepository = new InsRepository(db);
            chrgRepository = new ChrgRepository(db);
            chkRepository = new ChkRepository(db);
            clientRepository = new ClientRepository(db);
            accountNoteRepository = new AccountNoteRepository(db);
            billingActivityRepository = new BillingActivityRepository(db);
            accountValidationRuleRepository = new AccountValidationRuleRepository(db);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(db);
            accountValidationStatusRepository = new AccountValidationStatusRepository(db);
            accountLmrpErrorRepository = new AccountLmrpErrorRepository(db);
            lmrpRuleRepository = new LMRPRuleRepository(db);
            finRepository = new FinRepository(db);
            systemParametersRepository = new SystemParametersRepository(db);
        }

        public override Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            var record = dbConnection.SingleOrDefault<Account>($"where {this.GetRealColumn(nameof(Account.AccountNo))} = @0", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });


            if (record == null)
                return null;

            //if(!Str.ParseName(record.PatFullName, out string strLastName, out string strFirstName, out string strMiddleName, out string strSuffix))
            //{
            //    this.Errors = string.Format("Patient name could not be parsed. {0} {1}\n", record.PatFullName, record.AccountNo);
            //}
            //else
            //{
            //    record.PatLastName = strLastName;
            //    record.PatFirstName = strFirstName;
            //    record.PatMiddleName = strMiddleName;
            //    record.PatNameSuffix = strSuffix;
            //}

            if (!string.IsNullOrEmpty(record.ClientMnem))
            {
                record.Client = clientRepository.GetClient(record.ClientMnem);
                record.ClientName = record.Client.Name;
            }

            if (!demographicsOnly)
            {
                record.Pat = patRepository.GetByAccount(account);
                record.Insurances = insRepository.GetByAccount(account);
                record.InsurancePrimary = insRepository.GetByAccount(account, InsCoverage.Primary);
                record.InsuranceSecondary = insRepository.GetByAccount(account, InsCoverage.Secondary);
                record.InsuranceTertiary = insRepository.GetByAccount(account, InsCoverage.Tertiary);
                record.Charges = chrgRepository.GetByAccount(account);
                record.Payments = chkRepository.GetByAccount(account);
                record.Notes = accountNoteRepository.GetByAccount(account);
                record.BillingActivities = billingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = accountValidationStatusRepository.GetByAccount(account);
                record.Fin = finRepository.GetFin(record.FinCode);

                DateTime outpBillStartDate;
                DateTime questStartDate = new DateTime(2012,10,1);
                DateTime questEndDate = new DateTime(2020,5,31);
                DateTime arbitraryEndDate = new DateTime(2016, 12, 31);

                if(!DateTime.TryParse(systemParametersRepository.GetByKey("outpatient_bill_start"), out outpBillStartDate))
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
                            if (record.FinCode == "B" && record.InsurancePrimary.PolicyNumber.StartsWith("ZXK"))
                            {
                                record.BillForm = "QUEST";
                            }
                            else if (record.FinCode == "D" && record.TransactionDate.IsBetween(questStartDate, questEndDate))
                            {
                                record.BillForm = "QUEST";
                            }
                            else
                            {
                                record.BillForm = record.InsurancePrimary.InsCompany.BillForm;
                            }
                        }
                        else
                        {
                            record.BillForm = "UNDEFINED";
                        }
                    }
                }
            }

            object result;

            //populate properties
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetAccBalance(@0)", account);
            if (result != DBNull.Value && result != null)
                record.Balance = Convert.ToDouble(result); 
            
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetBadDebtByAccount(@0)", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            if (result != DBNull.Value && result != null)
                record.TotalBadDebt = Convert.ToDouble(result); 
            
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetAccTotalCharges(@0)", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            if (result != DBNull.Value && result != null)
                record.TotalCharges = Convert.ToDouble(result); 
            
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetContractualByAccount(@0)", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            if (result != DBNull.Value && result != null)
                record.TotalContractual = Convert.ToDouble(result); 
            
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetAmtPaidByAccount(@0)", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            if (result != DBNull.Value && result != null)
                record.TotalPayments = Convert.ToDouble(result); 
            
            result = dbConnection.ExecuteScalar<object>("SELECT dbo.GetWriteOffByAccount(@0)", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });
            if (result != DBNull.Value && result != null)
                record.TotalWriteOff = Convert.ToDouble(result);

            return record;
        }


        public override object Add(Account table)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");
            patRepository.Add(table.Pat);
            return base.Add(table);
        }

        public void AddAccount(Account acc)
        {
            Log.Instance.Trace($"Entering - account {acc.AccountNo}");
            if (string.IsNullOrEmpty(acc.Status))
                acc.Status = "NEW";
            acc.PatFullName = $"{acc.PatLastName},{acc.PatFirstName} {acc.PatMiddleName}".Trim();
            this.Add(acc);
        }

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace($"Entering - client {clientMnem} thruDate {thruDate}");

            return dbConnection.Fetch<InvoiceSelect>($"where {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.cl_mnem))} = @0 " + 
                $"and {this.GetRealColumn(typeof(InvoiceSelect), nameof(InvoiceSelect.trans_date))} <= @1", 
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = clientMnem }, 
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });
        }

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType)
        {
            Log.Instance.Trace($"Entering - claimType {claimType}");

            PetaPoco.Sql command;

            try
            {
                switch (claimType)
                {
                    case ClaimType.Institutional:
                        command = PetaPoco.Sql.Builder
                            .Select("status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'A'")
                            .Where("status = 'UB'");
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select("status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'A'")
                            .Where("status = '1500'");
                            //.Where("ins_code not in ('CHAMPUS')");
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
            //generate full name field from name parts
            table.PatFullName = String.Format("{0},{1} {2} {3}",
                table.PatLastName,
                table.PatFirstName,
                table.PatMiddleName,
                table.PatNameSuffix);

            table.PatFullName = table.PatFullName.Trim();

            Log.Instance.Trace("Exiting");
            return base.Update(table);
        }

        public override bool Update(Account table, IEnumerable<string> columns)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");
            //generate full name field from name parts
            table.PatFullName = String.Format("{0},{1} {2} {3}",
                table.PatLastName,
                table.PatFirstName,
                table.PatMiddleName,
                table.PatNameSuffix);

            table.PatFullName = table.PatFullName.Trim();

            Log.Instance.Trace($"Exiting");
            return base.Update(table, columns);
        }

        public bool UpdateDiagnoses(Account acc)
        {
            if(patRepository.SaveDiagnoses(acc.Pat))
            {
                acc.Pat = patRepository.GetByAccount(acc.AccountNo);
                return true;
            }
            return false;
        }

        public int UpdateStatus(string accountNo, string status)
        {
            Log.Instance.Trace($"Entering - account {accountNo} status {status}");
            return dbConnection.Update<Account>("set status = @0, mod_date = @1, mod_user = @2, mod_prg = @3, mod_host = @4 where account = @5",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = status },
                new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = DateTime.Now },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.UserName.ToString() },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = System.AppDomain.CurrentDomain.FriendlyName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = Environment.MachineName },
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = accountNo });
        }

        public bool InsuranceSwap(string accountNo, InsCoverage swap1, InsCoverage swap2)
        {
            dbConnection.ExecuteNonQueryProc("SwapInsurances",
                new SqlParameter() { ParameterName="@account", SqlDbType = SqlDbType.VarChar, Value = accountNo },
                new SqlParameter() { ParameterName="@ins1", SqlDbType = SqlDbType.VarChar, Value = swap1.ToString() },
                new SqlParameter() { ParameterName="@ins2", SqlDbType = SqlDbType.VarChar, Value = swap2.ToString() });

            return false;
        }

        public bool ChangeDateOfService(ref Account table, DateTime newDate, string reason_comment)
        {
            Log.Instance.Trace($"Entering - account  {table.AccountNo} new date {newDate} reason {reason_comment}");

            if(table == null)
                throw new ArgumentNullException("table");
            else if(newDate == null)
                throw new ArgumentNullException("newDate");
            else if (reason_comment == null)
                throw new ArgumentNullException("reason_comment");

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

            AccountNote accountNote = new AccountNote()
            {
                Account = account,
                Comment = noteText
            };
            try
            {
                accountNoteRepository.Add(accountNote);
            }
            catch(Exception ex)
            {
                addSuccess = false;
                Log.Instance.Error(ex, "Error adding account note.");
                throw new ApplicationException("Error adding account note.", ex);
            }

            Log.Instance.Trace($"Exiting");
            return addSuccess;
        }

        public bool ChangeFinancialClass(string account, string newFinCode)
        {
            Log.Instance.Trace($"Entering - Account {account} New Fin {newFinCode}");
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
                throw new ArgumentNullException("table");
            else if (newFinCode == null)
                throw new ArgumentNullException("newDate");

            bool updateSuccess = true;
            string oldFinCode = table.FinCode;

            //check that newFincode is a valid fincode
            FinRepository finRepository = new FinRepository(dbConnection);

            Fin newFin = finRepository.GetFin(newFinCode);
            Fin oldFin = finRepository.GetFin(oldFinCode);
            
            if(newFin == null)
            {
                throw new ArgumentException($"Financial code {newFinCode} is not valid code.", "newFinCode");
            }

            if(oldFinCode != newFinCode)
            {
                table.FinCode = newFinCode;
                try
                {
                    Update(table, new[] { nameof(Account.FinCode) });
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex);
                    throw new ApplicationException($"Exception updating fin code for {table.AccountNo}.", ex);
                }
                AddNote(table.AccountNo, $"Financial code updated from {oldFinCode} to {newFinCode}.");

                //reprocess charges if needed due to financial code change.
                if(newFin.FinClass != oldFin.FinClass)
                {
                    try
                    {
                        chrgRepository.ReprocessCharges(table.AccountNo);
                    }
                    catch(Exception ex)
                    {
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }
            }
            else
            {
                updateSuccess = false;
            }

            Log.Instance.Trace($"Exiting");
            return updateSuccess;
        }

        public bool ChangeClient(ref Account table, string newClientMnem)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo} new client {newClientMnem}");
            if (table == null)
                throw new ArgumentNullException("table");
            else if (newClientMnem == null)
                throw new ArgumentNullException("newClientMnem");

            bool updateSuccess = true;
            string oldClientMnem = table.ClientMnem;

            ClientRepository clientRepository = new ClientRepository(dbConnection);
            Client oldClient = clientRepository.GetClient(oldClientMnem);
            Client newClient = clientRepository.GetClient(newClientMnem);

            if(newClient == null)
                throw new ArgumentException($"Client mnem {newClientMnem} is not valid.", "newClientMnem");

            if(oldClientMnem != newClientMnem)
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
                        chrgRepository.ReprocessCharges(table.AccountNo);
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Error reprocessing charges.", ex);
                    }
                }
            }
            else
            {
                updateSuccess = false;
            }

            Log.Instance.Trace($"Exiting");
            return updateSuccess;
        }


        /// <summary>
        /// Add a charge to an account. The account must exist.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="cdm"></param>
        /// <param name="qty"></param>
        /// <param name="comment"></param>
        /// <returns>Charge number of newly entered charge or < 0 if an error occurs.</returns>
        public int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
        {
            Log.Instance.Trace($"Entering - account {account} cdm {cdm}");
            CdmRepository cdmRepository = new CdmRepository(dbConnection);
            FinRepository finRepository = new FinRepository(dbConnection);

            //verify the account exists - if not return -1
            Account accData = GetByAccount(account);
            if (accData == null)
            {
                Log.Instance.Error($"Account {account} not found");
                throw new AccountNotFoundException("Account not found.", account);
            } 
            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = cdmRepository.GetCdm(cdm);
            if (cdmData == null)
            {
                Log.Instance.Error($"CDM {cdm} not found.");
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = finRepository.GetFin(accData.FinCode);

            Chrg chrg = new Chrg();

            //split the patient name
            RFClassLibrary.Str.ParseName(accData.PatFullName, out string ln, out string fn, out string mn, out string suffix);

            //now build the charge & detail records
            chrg.AccountNo = account;
            chrg.Action = "";
            chrg.BillMethod = fin.ClaimType;
            chrg.CDMCode = cdm;
            chrg.Comment = comment;
            chrg.IsCredited = false;
            chrg.Facility = "";
            chrg.FinCode = accData.FinCode;
            chrg.FinancialType = fin.FinClass;
            chrg.PatFirstName = fn;
            chrg.PatLastName = ln;
            chrg.PatMiddleName = mn;
            chrg.OrderMnem = cdmData.Mnem;
            chrg.LISReqNo = refNumber;
            chrg.OrderingSite = "";
            chrg.PatBirthDate = accData.Pat.BirthDate;
            chrg.PatFullName = accData.PatFullName;
            chrg.PatSocSecNo = accData.SocSecNo;
            chrg.PerformingSite = "";
            chrg.PostingDate = DateTime.Today;
            chrg.Quantity = qty;
            chrg.ServiceDate = serviceDate;
            chrg.Status = "NEW";
            chrg.UnitNo = accData.EMPINumber;
            chrg.ResponsibleProvider = "";

            //need to determine the correct fee schedule - for now default to 1
            double ztotal = 0.0;
            double amtTotal = 0.0;
            double retailTotal = 0.0;

            foreach (CdmFeeSchedule1 fee in cdmData.CdmFeeSchedule1)
            {
                ChrgDetail chrgDetail = new ChrgDetail();
                chrgDetail.Cpt4 = fee.Cpt4;
                chrgDetail.Type = fee.Type;
                switch (fin.FinClass)
                {
                    case "M":
                        chrgDetail.Amount = fee.MClassPrice;
                        retailTotal += fee.MClassPrice;
                        ztotal += fee.ZClassPrice;
                        break;
                    case "C":
                        chrgDetail.Amount = fee.CClassPrice;
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
                chrgDetail.DiagCodePointer = "1:";

                chrg.ChrgDetails.Add(chrgDetail);
            }

            chrg.NetAmount = amtTotal;
            chrg.HospAmount = ztotal;
            chrg.RetailAmount = retailTotal;

            Log.Instance.Trace($"Exiting");
            return chrgRepository.AddCharge(chrg);
        }

        public bool Validate(ref Account account)
        {
            Log.Instance.Trace($"Entering - account {account}");

            BusinessLogic.Validators.ClaimValidator claimValidator = new BusinessLogic.Validators.ClaimValidator();

            account.LmrpErrors = ValidateLMRP(account);

            var validationResult = claimValidator.Validate(account);
            bool isAccountValid = false;

            string lmrperrors = null;
            foreach (var error in account.LmrpErrors)
            {
                account.AccountValidationStatus.validation_text += error + "\n";
                lmrperrors += error + "\n";
            }

            account.AccountValidationStatus.account = account.AccountNo;
            account.AccountValidationStatus.mod_date = DateTime.Now;

            if (!validationResult.IsValid)
            {
                isAccountValid = false;
                account.AccountValidationStatus.validation_text = validationResult.ToString();
            }
            else if (account.LmrpErrors.Count > 0)
            {
                isAccountValid = false;
            }
            else
            {
                isAccountValid=true;
                account.AccountValidationStatus.validation_text = "No validation errors.";
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

            return isAccountValid;
        }

        private List<string> ValidateLMRP(Account account)
        {
            Log.Instance.Trace($"Entering - account {account}");
            List<string> errorList = new List<string>();

            //determine if there are any rules for ama_year
            if(lmrpRuleRepository.RulesLoaded((DateTime)account.TransactionDate) <= 0)
            {
                // no lmrp rules loaded for this ama year. 
                errorList.Add("Rules not loaded for AMA_Year. DO NOT BILL.");
                return errorList;
            }

            foreach(var cpt4 in account.cpt4List.Distinct())
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

                if(!dxSupported)
                {
                    errorList.Add($"LMRP Violation - No dx codes support medical necessity for cpt {cpt4}.");
                }
            }
            return errorList;
        }
    
    
        public async Task ValidateUnbilledAccountsAsync()
        {
            Log.Instance.Trace($"Entering");

            DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);

            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")
            };

            AccountSearchRepository accountSearchRepository = new AccountSearchRepository(dbConnection);

            var accounts = await Task.Run(() => accountSearchRepository.GetBySearch(parameters).ToList());

            foreach(var account in accounts)
            {
                try
                {
                    var accountRecord = this.GetByAccount(account.Account);
                    this.Validate(ref accountRecord);
                }
                catch(Exception e)
                {
                    Log.Instance.Error(e, "Error during account validation job.");
                }
            }

        }
    
    
    }
}
