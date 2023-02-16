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
using NPOI.HSSF.Record;
using PetaPoco;

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

        public AccountRepository(string connectionString) : base(connectionString)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection);
            insRepository = new InsRepository(_connection);
            chrgRepository = new ChrgRepository(_connection);
            chkRepository = new ChkRepository(_connection);
            clientRepository = new ClientRepository(_connection);
            clientDiscountRepository = new ClientDiscountRepository(_connection);
            accountNoteRepository = new AccountNoteRepository(_connection);
            billingActivityRepository = new BillingActivityRepository(_connection);
            accountValidationRuleRepository = new AccountValidationRuleRepository(_connection);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(_connection);
            accountValidationStatusRepository = new AccountValidationStatusRepository(_connection);
            accountLmrpErrorRepository = new AccountLmrpErrorRepository(_connection);
            lmrpRuleRepository = new LMRPRuleRepository(_connection);
            finRepository = new FinRepository(_connection);
            systemParametersRepository = new SystemParametersRepository(_connection);
            cdmRepository = new CdmRepository(_connection);
        }

        public AccountRepository(PetaPoco.Database db) : base(db)
        {
            _connection = string.Empty;
            patRepository = new PatRepository(db);
            insRepository = new InsRepository(db);
            chrgRepository = new ChrgRepository(db);
            chkRepository = new ChkRepository(db);
            clientRepository = new ClientRepository(db);
            clientDiscountRepository = new ClientDiscountRepository(db);
            accountNoteRepository = new AccountNoteRepository(db);
            billingActivityRepository = new BillingActivityRepository(db);
            accountValidationRuleRepository = new AccountValidationRuleRepository(db);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(db);
            accountValidationStatusRepository = new AccountValidationStatusRepository(db);
            accountLmrpErrorRepository = new AccountLmrpErrorRepository(db);
            lmrpRuleRepository = new LMRPRuleRepository(db);
            finRepository = new FinRepository(db);
            systemParametersRepository = new SystemParametersRepository(db);
            cdmRepository = new CdmRepository(db);
        }

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Log.Instance.Trace($"Entering - account {account} demographicsOnly {demographicsOnly}");

            var record = dbConnection.SingleOrDefault<Account>($"where {this.GetRealColumn(nameof(Account.AccountNo))} = @0",
                new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = account });


            if (record == null)
                return null;

            if (!string.IsNullOrEmpty(record.ClientMnem))
            {
                if (record.ClientMnem != "K")
                {
                    record.Client = clientRepository.GetClient(record.ClientMnem);
                    record.ClientName = record.Client.Name;
                }
            }

            if (!demographicsOnly)
            {
                record.Pat = patRepository.GetByAccount(record);
                record.Insurances = insRepository.GetByAccount(account);
                record.Charges = chrgRepository.GetByAccount(account, true, true, null, false);
                record.Payments = chkRepository.GetByAccount(account);
                record.Notes = accountNoteRepository.GetByAccount(account);
                record.BillingActivities = billingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = accountValidationStatusRepository.GetByAccount(account);
                record.Fin = finRepository.GetFin(record.FinCode);

                DateTime outpBillStartDate;
                DateTime questStartDate = new DateTime(2012, 10, 1);
                DateTime questEndDate = new DateTime(2020, 5, 31);
                DateTime arbitraryEndDate = new DateTime(2016, 12, 31);

                if (!DateTime.TryParse(systemParametersRepository.GetByKey("outpatient_bill_start"), out outpBillStartDate))
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

                            //if (record.FinCode == "B" && record.InsurancePrimary.PolicyNumber.StartsWith("ZXK"))
                            //{
                            //    record.BillForm = "QUEST";
                            //}
                            //else if (record.FinCode == "D" && record.TransactionDate.IsBetween(questStartDate, questEndDate))
                            //{
                            //    record.BillForm = "QUEST";
                            //}
                            //else
                            //{
                            //    record.BillForm = record.InsurancePrimary.InsCompany.BillForm;
                            //}
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

            if (record.FinCode == "CLIENT")
            {
                record.TotalCharges = record.Charges.Where(x => x.Status != "CBILL").Sum(x => x.Quantity * x.NetAmount);
                record.TotalPayments = record.Payments.Where(x => x.Status != "CBILL").Sum(x => x.PaidAmount);
                record.TotalContractual = record.Payments.Where(x => x.Status != "CBILL").Sum(x => x.ContractualAmount);
                record.TotalWriteOff = record.Payments.Where(x => x.Status != "CBILL").Sum(x => x.WriteOffAmount);
            }
            else
            {
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
            }

            return record;
        }


        public override object Add(Account table)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");
            table.PatFullName = table.PatNameDisplay;
            table.Status = "NEW";

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
                acc.Status = "NEW";

            acc.PatFullName = acc.PatNameDisplay;

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

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType, int maxClaims = 0)
        {
            Log.Instance.Trace($"Entering - claimType {claimType}");

            PetaPoco.Sql command;

            string selMaxRecords = string.Empty;

            if(maxClaims > 0)
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
                            .Where("status = 'UB'");
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select($"{selMaxRecords}status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From(_tableName)
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'A'")
                            .Where("status = '1500'");
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
            if (string.IsNullOrEmpty(table.Status))
                table.Status = "NEW";
            table.PatFullName = table.PatNameDisplay;

            Log.Instance.Trace("Exiting");
            return base.Update(table);
        }

        public override bool Update(Account table, IEnumerable<string> columns)
        {
            Log.Instance.Trace($"Entering - account {table.AccountNo}");
            //generate full name field from name parts
            if (string.IsNullOrEmpty(table.Status))
                table.Status = "NEW";
            table.PatFullName = table.PatNameDisplay;

            Log.Instance.Trace($"Exiting");
            return base.Update(table, columns);
        }

        public bool UpdateDiagnoses(Account acc)
        {
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
                new SqlParameter() { ParameterName = "@account", SqlDbType = SqlDbType.VarChar, Value = accountNo },
                new SqlParameter() { ParameterName = "@ins1", SqlDbType = SqlDbType.VarChar, Value = swap1.ToString() },
                new SqlParameter() { ParameterName = "@ins2", SqlDbType = SqlDbType.VarChar, Value = swap2.ToString() });

            return false;
        }

        public bool ChangeDateOfService(ref Account table, DateTime newDate, string reason_comment)
        {
            Log.Instance.Trace($"Entering - account  {table.AccountNo} new date {newDate} reason {reason_comment}");

            if (table == null)
                throw new ArgumentNullException("table");
            else if (newDate == null)
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

            if (newFin == null)
            {
                throw new ArgumentException($"Financial code {newFinCode} is not valid code.", "newFinCode");
            }

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

        public int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null)
        {
            Log.Instance.Trace($"Entering - account {account} cdm {cdm}");

            //verify the account exists - if not return -1
            Account accData = GetByAccount(account);
            if (accData == null)
            {
                Log.Instance.Error($"Account {account} not found");
                throw new AccountNotFoundException("Account not found.", account);
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

            if(accData.Client == null)
            {
                throw new InvalidClientException("Client not valid", accData.ClientMnem);
            }

            //check account status, change to NEW if it is paid out.
            if (accData.Status == "PAID_OUT")
            {
                UpdateStatus(accData.AccountNo, "NEW");
                accData.Status = "NEW";
            }

            //get the cdm number - if cdm number is not found - abort
            Cdm cdmData = cdmRepository.GetCdm(cdm);
            if (cdmData == null)
            {
                Log.Instance.Error($"CDM {cdm} not found.");
                throw new CdmNotFoundException("CDM not found.", cdm);
            }

            Fin fin = finRepository.GetFin(accData.FinCode);
            if (fin == null)
            {
                throw new ApplicationException($"No fincode on account {accData.AccountNo}");
            }
            Chrg chrg = new Chrg();

            //now build the charge & detail records
            chrg.AccountNo = accData.AccountNo;
            chrg.Action = "";
            chrg.BillMethod = fin.ClaimType;
            chrg.CDMCode = cdm;
            chrg.Comment = comment;
            chrg.IsCredited = false;
            chrg.Facility = "";
            chrg.FinCode = accData.FinCode;
            chrg.FinancialType = fin.FinClass;
            chrg.PatFirstName = accData.PatFirstName;
            chrg.PatLastName = accData.PatLastName;
            chrg.PatMiddleName = accData.PatMiddleName;
            chrg.OrderMnem = cdmData.Mnem;
            chrg.LISReqNo = refNumber;
            chrg.OrderingSite = "";
            chrg.PatBirthDate = accData.BirthDate;
            chrg.PatFullName = accData.PatFullName;
            chrg.PatSocSecNo = accData.SocSecNo;
            chrg.PerformingSite = "";
            chrg.PostingDate = DateTime.Today;
            chrg.Quantity = qty;
            chrg.ServiceDate = serviceDate;
            chrg.Status = "NEW";
            chrg.UnitNo = accData.EMPINumber;
            chrg.ResponsibleProvider = "";

            List<ICdmDetail> feeSched = null;

            switch (accData.Client.FeeSchedule)
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
                ChrgDetail chrgDetail = new ChrgDetail();
                chrgDetail.DiagnosisPointer = new ChrgDiagnosisPointer();
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
                        //todo: calculate client discount
                        var cliDiscount = accData.Client.Discounts.Find(c => c.Cdm == cdm);
                        double discountPercentage = accData.Client.DefaultDiscount;
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

            //foreach(var profile in bundledProfiles)
            for (int x = 0; x < bundledProfiles.Count; x++)
            {
                //foreach(var cpt in profile.ComponentCpt)
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


        public bool Validate(ref Account account, bool reprint = false)
        {
            Log.Instance.Trace($"Entering - account {account}");

            try
            {
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
                            if (!account.InsurancePrimary.InsCompany.IsMedicareHmo)
                                BundlePanels(account);
                        }
                    }

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
                        //update account status back to new
                        UpdateStatus(account.AccountNo, "NEW");
                    }
                    else if (account.LmrpErrors.Count > 0)
                    {
                        isAccountValid = false;
                        UpdateStatus(account.AccountNo, "NEW");
                    }
                    else
                    {
                        isAccountValid = true;
                        account.AccountValidationStatus.validation_text = "No validation errors.";
                        //update account status if this account has been flagged to bill
                        if(account.Status == "RTB")
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

                    return isAccountValid;
                }
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);

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

            DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);

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

            AccountSearchRepository accountSearchRepository = new AccountSearchRepository(dbConnection);

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
            AddCharge(destinationAccount, charge.CDMCode, charge.Quantity, (DateTime)charge.ServiceDate, $"Moved from {sourceAccount}", charge.ReferenceReq);

        }

        /// <summary>
        /// Clears all claim flags so account will be picked up in next claim batch
        /// </summary>
        /// <param name="account"></param>
        public void ClearClaimStatus(Account account)
        {
            if (account == null)
                throw new ArgumentNullException("account");

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
                account.Status = "NEW";

                patRepository.Update(account.Pat, columns);
                UpdateStatus(account.AccountNo, "NEW");
            }
            catch(Exception ex)
            {
                throw new ApplicationException($"Exception clearing billing status on {account.AccountNo}", ex);
            }
        }

    }
}
