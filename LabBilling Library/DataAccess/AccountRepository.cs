using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using RFClassLibrary;
using System.Text;
using System.IO;
using LabBilling.Core.BusinessLogic;
using System.Text.Json;

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

        public AccountRepository(string connectionString) : base("acc", connectionString)
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
        }

        public AccountRepository(string connectionString, PetaPoco.Database db) : base("acc", connectionString, db)
        {
            _connection = connectionString;
            patRepository = new PatRepository(_connection, db);
            insRepository = new InsRepository(_connection, db);
            chrgRepository = new ChrgRepository(_connection, db);
            chkRepository = new ChkRepository(_connection, db);
            clientRepository = new ClientRepository(_connection, db);
            accountNoteRepository = new AccountNoteRepository(_connection, db);
            billingActivityRepository = new BillingActivityRepository(_connection, db);
            accountValidationRuleRepository = new AccountValidationRuleRepository(_connection, db);
            accountValidationCriteriaRepository = new AccountValidationCriteriaRepository(_connection, db);
            accountValidationStatusRepository = new AccountValidationStatusRepository(_connection, db);
        }

        public override Account GetById(int id)
        {
            throw new NotImplementedException();
        }

        public AccountSummary GetAccountSummary(string account)
        {
            Log.Instance.Debug($"Entering");

            var records = dbConnection.FetchProc<AccountSummary>("GetAccountSummary", new { accno = account });

            return records[0];
        }

        public Account GetByAccount(string account, bool demographicsOnly = false)
        {
            Log.Instance.Debug($"Entering");

            var record = dbConnection.SingleOrDefault<Account>("where account = @0", account);

            if(!Str.ParseName(record.pat_name, out string strLastName, out string strFirstName, out string strMiddleName, out string strSuffix))
            {
                this.Errors = string.Format("Patient name could not be parsed. {0} {1}\n", record.pat_name, record.account);
            }
            else
            {
                record.pat_name_last = strLastName;
                record.pat_name_first = strFirstName;
                record.pat_name_middle = strMiddleName;
                record.pat_name_suffix = strSuffix;
            }

            if (!demographicsOnly)
            {
                record.Pat = patRepository.GetByAccount(account);
                record.Insurances = insRepository.GetByAccount(account);
                record.Charges = chrgRepository.GetByAccount(account);
                record.Payments = chkRepository.GetByAccount(account);
                record.Notes = accountNoteRepository.GetByAccount(account);
                record.BillingActivities = billingActivityRepository.GetByAccount(account);
                record.AccountValidationStatus = accountValidationStatusRepository.GetByAccount(account);
            }
            Client client;
            if (record.cl_mnem != null)
            {
                client = clientRepository.GetClient(record.cl_mnem);
                record.ClientName = client.cli_nme;
            }

            double? result;

            //populate properties
            result = dbConnection.ExecuteScalar<double>("SELECT dbo.GetAccBalByDate(@0, @1)", account, DateTime.Today);
            record.Balance = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetBadDebtByAccount(@0)", account);
            record.TotalBadDebt = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetAccTotalCharges(@0)", account);
            record.TotalCharges = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetContractualByAccount(@0)", account);
            record.TotalContractual = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetAmtPaidByAccount(@0)", account);
            record.TotalPayments = result == null ? 0.00 : (double)result;
            result = dbConnection.ExecuteScalar<double?>("SELECT dbo.GetWriteOffByAccount(@0)", account);
            record.TotalWriteOff = result == null ? 0.00 : (double)result;
            
            return record;
        }

        public IEnumerable<AccountSearch> GetBySearch(string lastNameSearchText, string firstNameSearchText, string mrnSearchText, string ssnSearchText, string dobSearch, string sexSearch, string accountSearchText)
        {
            Log.Instance.Debug($"Entering");

            if ((lastNameSearchText == string.Empty) && (firstNameSearchText == string.Empty) && (mrnSearchText == string.Empty) 
                && (ssnSearchText == string.Empty)
                && (dobSearch == string.Empty) && (sexSearch == string.Empty) && (accountSearchText == string.Empty))
            {
                return new List<AccountSearch>();
            }

            try
            {

                string nameSearch = "";
                if(!(lastNameSearchText == "" && firstNameSearchText == ""))
                    nameSearch = string.Format("{0}%,{1}%", lastNameSearchText, firstNameSearchText);

                var command = PetaPoco.Sql.Builder
                    .Append("select acc.account, acc.trans_date, acc.pat_name, isnull(acc.ssn, pat.ssn) as 'SSN', pat.dob_yyyy, pat.sex, acc.mri")
                    .Append("from acc left outer join pat on acc.account = pat.account")
                    .Append("where acc.deleted = 0 ")
                    .Append("and (acc.pat_name like @0 or @1 = '')", nameSearch, nameSearch)
                    .Append("and (acc.account = @0 or @1 = '')", accountSearchText, accountSearchText)
                    .Append("and (acc.mri = @0 or @1 = '')", mrnSearchText, mrnSearchText)
                    .Append("and (pat.sex = @0 or @1 = '')", sexSearch, sexSearch)
                    .Append("and (acc.ssn = @0 or @1 = '')", ssnSearchText, ssnSearchText)
                    .Append("and (dob_yyyy = @0 or @1 = '')", dobSearch, dobSearch)
                    .Append("order by acc.trans_date desc");

                return dbConnection.Fetch<AccountSearch>(command);
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception in");
            }

            return new List<AccountSearch>();

        }

        public override object Add(Account table)
        {
            patRepository.Add(table.Pat);
            return base.Add(table);
        }

        public void AddAccount(Account acc)
        {
            this.Add(acc);
            //patRepository.Add(acc.Pat);
        }

        public IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate)
        {
            Log.Instance.Trace("Entering");

            return dbConnection.Fetch<InvoiceSelect>("where cl_mnem = @0 and trans_date <= @1", clientMnem, thruDate);
        }

        public IEnumerable<ClaimItem> GetAccountsForClaims(ClaimType claimType)
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql command;

            try
            {
                switch (claimType)
                {
                    case ClaimType.Institutional:
                        command = PetaPoco.Sql.Builder;
                        break;
                    case ClaimType.Professional:
                        command = PetaPoco.Sql.Builder
                            .Select("status, acc.account, pat_name, ssn, cl_mnem, acc.fin_code, trans_date, ins.plan_nme")
                            .From("acc")
                            .InnerJoin("ins").On("ins.account = acc.account and ins_a_b_c = 'a'")
                            .Where("status = '1500'")
                            .Where("ins_code not in ('CHAMPUS')");
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
            Log.Instance.Trace($"Entering");
            //generate full name field from name parts
            table.pat_name = String.Format("{0},{1} {2} {3}",
                table.pat_name_last,
                table.pat_name_first,
                table.pat_name_middle,
                table.pat_name_suffix);

            table.pat_name = table.pat_name.Trim();

            Log.Instance.Trace("$Exiting");
            return base.Update(table);
        }

        public override bool Update(Account table, IEnumerable<string> columns)
        {
            Log.Instance.Trace($"Entering");
            //generate full name field from name parts
            table.pat_name = String.Format("{0},{1} {2} {3}",
                table.pat_name_last,
                table.pat_name_first,
                table.pat_name_middle,
                table.pat_name_suffix);

            table.pat_name = table.pat_name.Trim();

            Log.Instance.Trace($"Exiting");
            return base.Update(table, columns);
        }

        public bool ChangeDateOfService(ref Account table, DateTime newDate, string reason_comment)
        {
            Log.Instance.Trace($"Entering");

            if(table == null)
                throw new ArgumentNullException("table");
            else if(newDate == null)
                throw new ArgumentNullException("newDate");
            else if (reason_comment == null)
                throw new ArgumentNullException("reason_comment");

            bool updateSuccess = false;
            DateTime oldServiceDate = DateTime.MinValue;

            // update trans_date on acc table
            if (table.trans_date != newDate)
            {
                oldServiceDate = (DateTime)table.trans_date;
                table.trans_date = newDate;
                Update(table, new[] { nameof(Account.trans_date) });

                if (AddNote(table.account, $"Service Date changed from {oldServiceDate} to {newDate}"))
                {
                    table.Notes = accountNoteRepository.GetByAccount(table.account);
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
            Log.Instance.Trace($"Entering");
            bool addSuccess = true;

            AccountNote accountNote = new AccountNote()
            {
                account = account,
                comment = noteText
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

        public bool ChangeFinancialClass(ref Account table, string newFinCode)
        {
            Log.Instance.Trace($"Entering");
            if (table == null)
                throw new ArgumentNullException("table");
            else if (newFinCode == null)
                throw new ArgumentNullException("newDate");

            bool updateSuccess = true;
            string oldFinCode = table.fin_code;

            //check that newFincode is a valid fincode
            FinRepository finRepository = new FinRepository(_connection, dbConnection);

            Fin newFin = finRepository.GetFin(newFinCode);
            Fin oldFin = finRepository.GetFin(oldFinCode);
            
            if(newFin == null)
            {
                throw new ArgumentException($"Financial code {newFinCode} is not valid code.", "newFinCode");
            }

            if(oldFinCode != newFinCode)
            {
                table.fin_code = newFinCode;
                try
                {
                    Update(table, new[] { nameof(Account.fin_code) });
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex);
                    throw new ApplicationException($"Exception updating fin code for {table.account}.", ex);
                }
                AddNote(table.account, $"Financial code updated from {oldFinCode} to {newFinCode}.");

                //reprocess charges if needed due to financial code change.
                if(newFin.type != oldFin.type)
                {
                    try
                    {
                        chrgRepository.ReprocessCharges(table.account);
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
            Log.Instance.Trace($"Entering");
            CdmRepository cdmRepository = new CdmRepository(_connection, dbConnection);
            FinRepository finRepository = new FinRepository(_connection, dbConnection);


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

            Fin fin = finRepository.GetFin(accData.fin_code);

            Chrg chrg = new Chrg();

            //split the patient name
            RFClassLibrary.Str.ParseName(accData.pat_name, out string ln, out string fn, out string mn, out string suffix);

            //now build the charge & detail records
            chrg.account = account;
            chrg.action = "";
            chrg.bill_method = fin.form_type;
            chrg.cdm = cdm;
            chrg.comment = comment;
            chrg.credited = false;
            chrg.facility = "";
            chrg.fin_code = accData.fin_code;
            chrg.fin_type = fin.type;
            chrg.fname = fn;
            chrg.lname = ln;
            chrg.mname = mn;
            chrg.mt_mnem = cdmData.mnem;
            chrg.mt_req_no = refNumber;
            chrg.order_site = "";
            chrg.pat_dob = accData.Pat.dob_yyyy;
            chrg.pat_name = accData.pat_name;
            chrg.pat_ssn = accData.Pat.ssn;
            chrg.performing_site = "";
            chrg.post_date = DateTime.Today;
            chrg.qty = qty;
            chrg.service_date = serviceDate;
            chrg.status = "NEW";
            chrg.unitno = accData.HNE_NUMBER;
            chrg.responsiblephy = "";

            //need to determine the correct fee schedule - for now default to 1
            double ztotal = 0.0;
            double amtTotal = 0.0;
            double retailTotal = 0.0;

            foreach (CdmFeeSchedule1 fee in cdmData.cdmFeeSchedule1)
            {
                ChrgDetail amt = new ChrgDetail();
                amt.cpt4 = fee.cpt4;
                amt.type = fee.type;
                switch (fin.type)
                {
                    case "M":
                        amt.amount = fee.mprice;
                        retailTotal += fee.mprice;
                        ztotal += fee.zprice;
                        break;
                    case "C":
                        amt.amount = fee.cprice;
                        retailTotal += fee.cprice;
                        ztotal += fee.zprice;
                        break;
                    case "Z":
                        amt.amount = fee.zprice;
                        retailTotal += fee.zprice;
                        ztotal += fee.zprice;
                        break;
                    default:
                        amt.amount = fee.mprice;
                        retailTotal += fee.mprice;
                        ztotal += fee.zprice;
                        break;
                }

                amtTotal += amt.amount;

                amt.modi = fee.modi;
                amt.revcode = fee.rev_code;
                amt.mt_req_no = "";
                amt.order_code = fee.billcode;
                amt.bill_type = "";
                amt.bill_method = "";
                amt.diagnosis_code_ptr = "1:";

                chrg.ChrgDetails.Add(amt);
            }

            chrg.net_amt = amtTotal;
            chrg.inp_price = ztotal;
            chrg.retail = retailTotal;

            Log.Instance.Trace($"Exiting");
            return chrgRepository.AddCharge(chrg);
        }

        /// <summary>
        /// Runs all validation checks needed for generating a claim.
        /// </summary>
        /// <param name="account"></param>
        /// <returns>string containing list of errors</returns>
        //public string Validate2(Account account)
        //{
        //    Log.Instance.Trace($"Entering account validate with {account.account}.");

        //    StringBuilder errorList = new StringBuilder();

        //    var rules = accountValidationRuleRepository.GetAll();

        //    foreach(var rule in rules)
        //    {
        //        //TODO: determine if rule should be checked based on criteria


        //        string fromDate = "01/01/2002"; // DateTimeExtension.AddWeeks(DateTime.Today, -52).ToShortDateString();
        //        string thruDate = DateTime.Today.ToShortDateString();
        //        string sqlText = rule.strSql.Replace("{0}", account.account);
        //        sqlText = sqlText.Replace("{1}", fromDate);
        //        sqlText = sqlText.Replace("{2}", thruDate);
        //        sqlText = sqlText.Replace("@", "@@");
        //        var result = dbConnection.Execute(sqlText);
        //        if(rule.valid)
        //        {
        //            //returning no rows is an error
        //            if(result <= 0)
        //            {
        //                errorList.AppendLine(rule.error);
        //            }
        //        }
        //        else
        //        {
        //            //returning rows is an error
        //            if(result > 0)
        //            {
        //                errorList.AppendLine(rule.error);
        //            }
        //        }

        //    }

        //    return errorList.ToString();
        //}


        public bool Validate(Account account)
        {
            ClaimRulesEngine engine = new ClaimRulesEngine(_connection);
            bool retVal = false;
            try
            {
                retVal = engine.Evaluate(account, out StringBuilder errorList);
            }
            catch(RuleProcessException rpex)
            {
                retVal = false;
                //TODO: write error message to account
                string error = rpex.Message;
            }

            return retVal;
        }

    }
}
