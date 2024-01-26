using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using Utilities;
using Microsoft.Data.SqlClient;
using LabBilling.Logging;
using Microsoft.AspNetCore.Http;

namespace LabBilling.Core
{

    public class ProgressEventArgs : EventArgs
    {
        public int PercentComplete { get; private set; } = 0;
        public string Status { get; private set; }

        public ProgressEventArgs(int percentComplete, string status)
        {
            PercentComplete = percentComplete;
            Status = status;
        }
    }

    public sealed class PatientBilling : DataAccess.Database
    {
        private string batchNo;
        private DateTime endDate = DateTime.MinValue;
        private readonly PatRepository patRepository;
        private readonly AccountRepository accountRepository;
        private readonly BadDebtRepository badDebtRepository;
        private readonly ChkRepository chkRepository;
        private readonly PatientStatementRepository patientStatementRepository;
        private readonly PatientStatementAccountRepository patientStatementAccountRepository;
        private readonly PatientStatementCernerRepository patientStatementCernerRepository;
        private readonly PatientStatementEncounterActivityRepository patientStatementEncounterActivityRepository;
        private readonly PatientStatementEncounterRepository patientStatementEncounterRepository;
        private readonly SystemParametersRepository parametersRepository;
        private IAppEnvironment _appEnvironment;

        public PatientBilling(IAppEnvironment appEnvironment) : base(appEnvironment.ConnectionString)
        {
            _appEnvironment = appEnvironment;

            accountRepository = new AccountRepository(appEnvironment);
            patRepository = new PatRepository(appEnvironment);
            badDebtRepository = new BadDebtRepository(appEnvironment);
            chkRepository = new ChkRepository(appEnvironment);
            parametersRepository = new SystemParametersRepository(appEnvironment);
            patientStatementRepository = new PatientStatementRepository(appEnvironment);
            patientStatementCernerRepository = new PatientStatementCernerRepository(appEnvironment);
            patientStatementEncounterRepository = new PatientStatementEncounterRepository(appEnvironment);
            patientStatementEncounterActivityRepository = new PatientStatementEncounterActivityRepository(appEnvironment);
            patientStatementAccountRepository = new PatientStatementAccountRepository(appEnvironment);
        }

        public event EventHandler<ProgressEventArgs> ProgressIncrementedEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Filename generated</returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<string> SendToCollections()
        {
            // set date_sent records in bad_debt table where date_sent is null to today's date
            return await Task.Run(() =>
            {
                var results = badDebtRepository.GetNotSentRecords();

                if (!results.Any())
                {
                    ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(100, "No records to process."));
                    return "";
                }

                dbConnection.BeginTransaction();
                int recordsProcessed = 0;
                foreach (var result in results)
                {
                    try
                    {
                        var acc = accountRepository.GetByAccount(result.AccountNo, false);

                        result.DateSent = DateTime.Now;

                        badDebtRepository.Update(result, new[] { nameof(BadDebt.DateSent) });

                        accountRepository.AddNote(acc.AccountNo, $"Account sent to collections. Write off amount {acc.Balance}");

                        // write off accounts where bad_debt.date_sent = today
                        Chk chk = new()
                        {
                            AccountNo = result.AccountNo,
                            Batch = 0,
                            WriteOffAmount = acc.Balance,
                            WriteOffDate = DateTime.Today,
                            DateReceived = DateTime.Today,
                            Source = "BAD_DEBT",
                            Status = "WRITE_OFF",
                            Comment = "BAD DEBT WRITE OFF",
                            IsCollectionPmt = true
                        };

                        chkRepository.Add(chk);

                        //update bad debt date on pat record
                        acc.Pat.SentToCollectionsDate = DateTime.Today;
                        patRepository.Update(acc.Pat, new[] { nameof(Pat.SentToCollectionsDate) });

                        int cnt = results.Count();
                        decimal processed = ++recordsProcessed / (decimal)cnt;
                        int percentComplete = Convert.ToInt32(processed * 100);
                        ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(percentComplete, $"Processed {result.AccountNo}"));
                        dbConnection.CompleteTransaction();
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Error("Error during collections send. Process aborted.", ex);
                        dbConnection.AbortTransaction();
                        throw new ApplicationException("Error during collections send.Process aborted.", ex);
                    }
                }

                return GenerateCollectionsFile(results);

            });
            // email mailer P report


        }

        public int RegenerateCollectionsFile(DateTime tDate)
        {
            var results = badDebtRepository.GetSentByDate(tDate);

            if (results.Any())
            {
                GenerateCollectionsFile(results);
            }

            return results.Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="records"></param>
        /// <returns>Filename generated</returns>
        /// <exception cref="ApplicationException"></exception>
        private string GenerateCollectionsFile(IEnumerable<BadDebt> records)
        {
            // set date_sent records in bad_debt table where date_sent is null to today's date

            //get first record for date
            DateTime? date = records.First<BadDebt>().DateSent;

            if (!date.HasValue)
                return "";

            StringBuilder sb = new();
            string fileName = _appEnvironment.ApplicationParameters.CollectionsFileLocation;
            fileName += $"MCL{date:MMddyyyy}.txt";

            // create collections file to send to MSCB
            FixedFileLine fileLine = new FixedFileLine(20);
            //create header
            fileLine.SetField(1, 1, 20, "DEBTOR_LAST_NAME");
            fileLine.SetField(2, 21, 35, "DEBTOR_FIRST_NAME");
            fileLine.SetField(3, 36, 60, "STREET_ADDR_1");
            fileLine.SetField(4, 61, 85, "STREET_ADDR_2");
            fileLine.SetField(5, 86, 103, "CITY");
            fileLine.SetField(6, 104, 118, "STATE_ZP");
            fileLine.SetField(7, 119, 133, "SPOUSE");
            fileLine.SetField(8, 134, 145, "PHONE");
            fileLine.SetField(9, 146, 155, "SSN");
            fileLine.SetField(10, 156, 175, "LICENSE");
            fileLine.SetField(11, 176, 210, "EMPLOYMENT");
            fileLine.SetField(12, 211, 245, "REMARKS");
            fileLine.SetField(13, 246, 270, "ACCOUNT_NUMBER");
            fileLine.SetField(14, 271, 290, "PATIENT_NAME");
            fileLine.SetField(15, 291, 325, "REMARKS_2");
            fileLine.SetField(16, 326, 354, "MISC");
            fileLine.SetField(17, 355, 360, "SVC_DT");
            fileLine.SetField(18, 361, 366, "PMT_DT");
            fileLine.SetField(19, 367, 376, "BALANCE");
            fileLine.SetField(20, 377, 386, "ORIG_CHRG");

            sb.AppendLine(fileLine.OutputLine());

            if (!records.Any())
            {
                ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(100, "No records to process."));
                return "";
            }

            int recordsProcessed = 0;
            foreach (var result in records)
            {
                try
                {
                    var acc = accountRepository.GetByAccount(result.AccountNo, false);

                    fileLine = new FixedFileLine(20);
                    //create header
                    fileLine.SetField(1, 1, 20, result.DebtorLastName);
                    fileLine.SetField(2, 21, 35, result.DebtorFirstName);
                    fileLine.SetField(3, 36, 60, result.StreetAddress1);
                    fileLine.SetField(4, 61, 85, result.StreetAddress2);
                    fileLine.SetField(5, 86, 103, result.City);
                    fileLine.SetField(6, 104, 118, result.StateZip);
                    fileLine.SetField(7, 119, 133, result.Spouse);
                    fileLine.SetField(8, 134, 145, result.Phone);
                    fileLine.SetField(9, 146, 155, result.SocialSecurityNo);
                    fileLine.SetField(10, 156, 175, result.LicenseNumber);
                    fileLine.SetField(11, 176, 210, result.Employment);
                    fileLine.SetField(12, 211, 245, result.Remarks);
                    fileLine.SetField(13, 246, 270, result.AccountNo);
                    fileLine.SetField(14, 271, 290, result.PatientName);
                    fileLine.SetField(15, 291, 325, result.Remarks2);
                    fileLine.SetField(16, 326, 354, result.Misc);
                    fileLine.SetField(17, 355, 360, result.ServiceDate.NullDateToString("MMddyy"));
                    fileLine.SetField(18, 361, 366, result.PaymentDate.NullDateToString("MMddyy"));
                    fileLine.SetField(19, 367, 376, result.Balance.ToString("f2"));
                    fileLine.SetField(20, 377, 386, acc.TotalCharges.ToString("f2"));

                    sb.AppendLine(fileLine.OutputLine());

                    int cnt = records.Count();
                    decimal processed = ++recordsProcessed / (decimal)cnt;
                    int percentComplete = Convert.ToInt32(processed * 100);
                    ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(percentComplete, $"Processed {result.AccountNo}"));
                }
                catch (Exception ex)
                {
                    Log.Instance.Error("Error during collections send. Process aborted.", ex);
                    dbConnection.AbortTransaction();
                    throw new ApplicationException("Error during collections send.Process aborted.", ex);
                }
            }

            File.WriteAllText(fileName, sb.ToString());

            // email mailer P report
            return fileName;
        }

        public bool BatchPreviouslyRun(string batchNo)
        {
            int cnt = patientStatementRepository.GetStatementCount(batchNo);

            if (cnt > 0)
                return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="throughDate"></param>
        /// <returns>Path of file created.</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public string CreateStatementFile(DateTime throughDate)
        {
            if (throughDate == DateTime.MinValue)
            {
                throw new ArgumentException("Through Date is not a valid date.", "throughDate");
            }

            endDate = throughDate;
            batchNo = $"{endDate.Year}{endDate.Month:00}";

            // 5. step 5 - run viewer bad debt and create file
            //AFTER running pat viewer bad debt to create the file do this. 
            var filename = FormatStatementFile();

            //upload statement file to DNI from MCLFTP2
            // 6. step 6 run these queries individually

            try
            {
                string sql = $"update dbo.patbill_acc SET date_sent = convert(varchar(10),getdate(),101) " +
                "WHERE nullif(date_sent,'') IS null AND batch_id = @0";

                dbConnection.Execute(sql, new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batchNo });

                sql = $"update dbo.patbill_stmt SET statement_submitted_dt_tm = pa.processed_date " +
                "from patbill_stmt ps inner join patbill_acc pa on pa.statement_number = ps.statement_number " +
                "WHERE nullif(ps.statement_submitted_dt_tm,'') is null " +
                "and pa.processed_date is not null";

                dbConnection.Execute(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Patient Billing Create Statement file", ex);
            }

            return filename;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Path of file created.</returns>
        private string FormatStatementFile()
        {
            var statements = patientStatementRepository.GetByBatch(batchNo);
            var accounts = patientStatementAccountRepository.GetByBatch(batchNo);
            var encounters = patientStatementEncounterRepository.GetByBatch(batchNo);
            var encountersActivity = patientStatementEncounterActivityRepository.GetByBatch(batchNo);
            var cernerStatement = patientStatementCernerRepository.GetByBatch(batchNo);

            DataTable statementDt = HelperExtensions.ConvertToDataTable(statements);
            DataTable accountsDt = HelperExtensions.ConvertToDataTable(accounts);
            DataTable encountersDt = HelperExtensions.ConvertToDataTable(encounters);
            DataTable encountersActivityDt = HelperExtensions.ConvertToDataTable(encountersActivity);
            DataTable cernerStatementDt = HelperExtensions.ConvertToDataTable(cernerStatement);

            //todo: get file path from parameters
            string strFileName = $"{_appEnvironment.ApplicationParameters.StatementsFileLocation}PatStatement{DateTime.Now:yyyyMMddHHmm}.txt";

            StreamWriter sw = new(strFileName)
            {
                AutoFlush = true
            };

            //sw.Write(string.Format("HDR~MCL~~CERNER~MCL~{0}~{1}~T~N~0~0~0\r\n"
            //    , DateTime.Now.ToString("yyyyMMdd")
            //    , DateTime.Now.ToString("HHmmss")));

            sw.WriteLine(new DelimitedFileLine('~')
            {
                [0] = "HDR",
                [1] = "MCL",
                [3] = "CERNER",
                [4] = "MCL",
                [5] = DateTime.Now.ToString("yyyyMMdd"),
                [6] = DateTime.Now.ToString("HHmmss"),
                [7] = "T",
                [8] = "N",
                [9] = "0",
                [10] = "0",
                [11] = "0"
            }.ToString());

            foreach (DataRow dr in statementDt.Rows)
            {
                var q = $"{nameof(PatientStatement.StatementNumber)} = {dr[nameof(PatientStatement.StatementNumber)]}";
                // each statement
                DataRow[] drAcc = accountsDt.Select(q);
                DelimitedFileLine line = new('|');
                line.AddField("STMT");
                line.AddField(dr[nameof(PatientStatement.RecordCount)].ToString());
                line.AddField(dr[nameof(PatientStatement.StatementNumber)].ToString());
                line.AddField(dr[nameof(PatientStatement.BillingEntityStreet)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityCity)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityState)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityZip)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityFedTaxId)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityPhone)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BillingEntityFax)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToStreet)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToStreet2)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToCity)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToState)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToZip)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.RemitToOrgName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorStreet)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorStreet2)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorCity)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorState)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorZip)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AmountDue)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.DateDue)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BalanceForward)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AgingBucketCurrent)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AgingBucket30Day)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AgingBucket60Day)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AgingBucket90Day)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.StatementTotalAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.InsuranceBilledAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BalanceForwardAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.BalanceForwardDate)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.PrimaryHealthPlanName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.PrimaryHealthPlanPolicyNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.PrimaryHealthPlanGroupNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.SecondaryHealthPlanName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.SecondaryHealthPlanPolicyNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.SecondaryHealthPlanGroupNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TertiaryHealthPlanName)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TertiaryHealthPlanPolicyNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TertiaryHealthPlanGroupNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.StatementTime)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.PageNumber)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.InsurancePending)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.UnpaidBalance)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.PatientBalance)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TotalPaidSinceLastStatement)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.InsuranceDiscount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.ContactText)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TransmissionDateTime)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorCountry)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorSSN)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorPhone)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.StatementSubmittedDateTime)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.IncludesEstPatLib)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.TotalChargeAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.NonCoveredChargeAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.ABNChargeAmount)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EstContractAllowanceAmtInd)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EstContractAllowanceAmt)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrDeductibleRemAmtInd)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.DeductibleAppliedAmt)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrCopayAmtInd)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrCopayAmt)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrCoinsurancePctInd)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrCoinsurancePct)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EncntrCoinsuranceAmt)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.MaxOutOfPocketAmtInd)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.AmtOverMaxOutOfPocket)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.EstPatientLiabAmt)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.OnlineBillpayUrl)].ToString().ToUpper());
                line.AddField(dr[nameof(PatientStatement.GuarantorAccessCode)].ToString().ToUpper());

                sw.WriteLine(line.ToString());                

                /*
                sw.Write(string.Format("STMT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}" +
                    "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}" +
                    "|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}|{54}|{55}" +
                    "|{56}|{57}|{58}|{59}|{60}|{61}|{62}|{63}|{64}|{65}|{66}|{67}|{68}|{69}|{70}|{71}|{72}\r\n",
                    dr[nameof(PatientStatement.RecordCount)].ToString(),                        //0
                    dr[nameof(PatientStatement.StatementNumber)].ToString(),
                    dr[nameof(PatientStatement.BillingEntityStreet)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityCity)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityState)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityZip)].ToString().ToUpper(),            //5
                    dr[nameof(PatientStatement.BillingEntityFedTaxId)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityName)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityPhone)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BillingEntityFax)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.RemitToStreet)].ToString().ToUpper(),               //10
                    dr[nameof(PatientStatement.RemitToStreet2)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.RemitToCity)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.RemitToState)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.RemitToZip)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.RemitToOrgName)].ToString().ToUpper(),             //15
                    dr[nameof(PatientStatement.GuarantorStreet)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorStreet2)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorCity)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorState)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorZip)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorName)].ToString().ToUpper(),                //21
                    dr[nameof(PatientStatement.AmountDue)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.DateDue)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BalanceForward)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.AgingBucketCurrent)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.AgingBucket30Day)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.AgingBucket60Day)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.AgingBucket90Day)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.StatementTotalAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.InsuranceBilledAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BalanceForwardAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.BalanceForwardDate)].ToString().ToUpper(),          //30
                    dr[nameof(PatientStatement.PrimaryHealthPlanName)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.PrimaryHealthPlanPolicyNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.PrimaryHealthPlanGroupNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.SecondaryHealthPlanName)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.SecondaryHealthPlanPolicyNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.SecondaryHealthPlanGroupNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TertiaryHealthPlanName)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TertiaryHealthPlanPolicyNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TertiaryHealthPlanGroupNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.StatementTime)].ToString().ToUpper(),                //40
                    dr[nameof(PatientStatement.PageNumber)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.InsurancePending)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.UnpaidBalance)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.PatientBalance)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TotalPaidSinceLastStatement)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.InsuranceDiscount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.ContactText)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TransmissionDateTime)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorCountry)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorSSN)].ToString().ToUpper(),                 //50
                    dr[nameof(PatientStatement.GuarantorPhone)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.StatementSubmittedDateTime)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.IncludesEstPatLib)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.TotalChargeAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.NonCoveredChargeAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.ABNChargeAmount)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EstContractAllowanceAmtInd)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EstContractAllowanceAmt)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EncntrDeductibleRemAmtInd)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.DeductibleAppliedAmt)].ToString().ToUpper(),        //60
                    dr[nameof(PatientStatement.EncntrCopayAmtInd)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EncntrCopayAmt)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EncntrCoinsurancePctInd)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EncntrCoinsurancePct)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EncntrCoinsuranceAmt)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.MaxOutOfPocketAmtInd)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.AmtOverMaxOutOfPocket)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.EstPatientLiabAmt)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.OnlineBillpayUrl)].ToString().ToUpper(),
                    dr[nameof(PatientStatement.GuarantorAccessCode)].ToString().ToUpper()          //70
                    ));
                */

                if (cernerStatementDt.Rows.Count > 0)
                {
                    DataRow[] drStmtMsg = cernerStatementDt.Select($"{nameof(PatientStatementCerner.StatementType)} = 'SMSG' " +
                        $"and {nameof(PatientStatementCerner.StatementTypeId)} = '{dr[nameof(PatientStatement.StatementNumber)]}' ");

                    for (int iSmsg = 0; iSmsg <= drStmtMsg.GetUpperBound(0); iSmsg++)
                    {
                        sw.Write(string.Format("{0}|{1}|{2}\r\n",
                            drStmtMsg[iSmsg][nameof(PatientStatementCerner.StatementType)].ToString().ToUpper(),
                            (iSmsg + 1).ToString().ToUpper(),
                            drStmtMsg[iSmsg][nameof(PatientStatementCerner.StatementText)].ToString().ToUpper()));
                    }

                }
                // each account
                for (int iAcc = 0; iAcc <= drAcc.GetUpperBound(0); iAcc++)
                {
                    line = new('|');

                    line.AddField("ACCT");
                    line.AddField((iAcc + 1).ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientAccountNumber)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountId)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientName)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountSubtotal)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.TotalAccountSubtotal)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountAmtDue)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountInsPending)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountDatesOfService)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountUnpaidBalance)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountPatientBalance)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountPaidSinceLastStatement)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountInsDiscount)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountDateDue)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AccountHealthPlanName)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientDateOfBirth)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientDateOfDeath)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientSex)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.PatientVip)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.IncludesEstPatLib)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.TotalChargeAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.NonCoveredChargeAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.ABNChargeAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EstContractAllowanceAmtInd)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EstContractAllowanceAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrDeductibleRemAmtInd)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.DeductibleAppliedAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrCopayAmtInd)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrCopayAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsurancePctInd)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsurancePct)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsuranceAmt)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.MaximumOutOfPocketAmtInd)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.AmtOverMaxOutOfPocket)].ToString().ToUpper());
                    line.AddField(drAcc[iAcc][nameof(PatientStatementAccount.EstPatientLiabAmt)].ToString().ToUpper());

                    sw.WriteLine(line.ToString());

                    /*                    sw.Write(string.Format("ACCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}" +
                                            "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}" +
                                            "\r\n",
                                            (iAcc + 1).ToString().ToUpper(),//drAcc[iAcc]["record_cnt_acct"].ToString(), //0
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientAccountNumber)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountId)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientName)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountSubtotal)].ToString().ToUpper(),                 //4
                                            drAcc[iAcc][nameof(PatientStatementAccount.TotalAccountSubtotal)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountAmtDue)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountInsPending)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountDatesOfService)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountUnpaidBalance)].ToString().ToUpper(),                  //9
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountPatientBalance)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountPaidSinceLastStatement)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountInsDiscount)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountDateDue)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AccountHealthPlanName)].ToString().ToUpper(),            //14
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientDateOfBirth)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientDateOfDeath)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientSex)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.PatientVip)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.IncludesEstPatLib)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.TotalChargeAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.NonCoveredChargeAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.ABNChargeAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EstContractAllowanceAmtInd)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EstContractAllowanceAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrDeductibleRemAmtInd)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.DeductibleAppliedAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrCopayAmtInd)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrCopayAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsurancePctInd)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsurancePct)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EncntrCoinsuranceAmt)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.MaximumOutOfPocketAmtInd)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.AmtOverMaxOutOfPocket)].ToString().ToUpper(),
                                            drAcc[iAcc][nameof(PatientStatementAccount.EstPatientLiabAmt)].ToString().ToUpper()

                                            ));
                    */

                    // each encounter
                    DataRow[] drEnct = encountersDt.Select($"{nameof(PatientStatementEncounter.StatementNumber)} = {dr[nameof(PatientStatement.StatementNumber)]} " +
                        $"and {nameof(PatientStatementEncounter.RecordCount)} = '{drAcc[iAcc][nameof(PatientStatementAccount.RecordCountAcct)]}'");

                    for (int iEnctr = 0; iEnctr <= drEnct.GetUpperBound(0); iEnctr++)
                    {
                        line = new('|');

                        line.AddField("ENCT");
                        line.AddField((iEnctr + 1).ToString());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrNumber)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrId)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PlaceOfService)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrDatesOfService)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrAmtDue)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvName)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgName)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr2)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr3)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr4)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgCity)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgState)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgZip)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgPhone)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvHrs)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrUnpaidBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPatientBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPaidSinceLastStmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrInsDiscount)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrOrdMgmtActType)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrOrgMgmtCatType)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrHealthPlanName)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrInPending)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTotal)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrAdmitDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDischargeDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrMedicalService)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrType)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrFinancialClass)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrVIP)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrQualifier)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTotalCharges)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.TotalPatientPayments)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.TotalPatientAdjustments)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.TotalInsurancePayments)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.TotalInsuranceAdjustments)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrAssignedAgency)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanFlag)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanStatus)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanOrigAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanPayAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanBeginDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanDelinqAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPriClmOrigTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPriClmCurTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSecClmOrigTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTSecClmCurTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerClmOrigTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerClmCurTransDateTime)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPrimInsuranceBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSecInsuranceBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerInsuranceBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSelfPayBalance)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.AttendingPhysicianName)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.IncludesEstPatLiab)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.TotalChargeAmount)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.NonCoveredChargeAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.ABNChargeAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmtInd)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDeductibleRemAmtInd)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDeductibleRemAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.DeductibleAppliedAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCopayAmtInd)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCopayAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsurancePctInd)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsurancePct)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsuranceAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.MaximumOutOfPocketAmtInd)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.MaximumOutOfPocketAmt)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.AmtOverMaxOutOfPocket)].ToString().ToUpper());
                        line.AddField(drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmt)].ToString().ToUpper());

                        sw.WriteLine(line.ToString());

/*                        sw.Write(string.Format("ENCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}" +
                        "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}" +
                        "|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}|{54}|{55}" +
                        "|{56}|{57}|{58}|{59}|{60}|{61}|{62}|{63}|{64}|{65}|{66}|{67}|{68}|{69}|{70}|{71}|{72}|{73}\r\n",
                        (iEnctr + 1).ToString(),//drEnct[iEnctr]["record_cnt"].ToString(), //0
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrNumber)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrId)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PlaceOfService)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrDatesOfService)].ToString().ToUpper(),   //4
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrAmtDue)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvName)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgName)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr2)].ToString().ToUpper(),    //9
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr3)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgStreetAddr4)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgCity)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgState)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgZip)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvOrgPhone)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrProvHrs)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrUnpaidBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPatientBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPaidSinceLastStmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrInsDiscount)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrOrdMgmtActType)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrOrgMgmtCatType)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrHealthPlanName)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrInPending)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTotal)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrAdmitDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDischargeDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrMedicalService)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrType)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrFinancialClass)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrVIP)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrQualifier)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTotalCharges)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.TotalPatientPayments)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.TotalPatientAdjustments)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.TotalInsurancePayments)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.TotalInsuranceAdjustments)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrAssignedAgency)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanFlag)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanStatus)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanOrigAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanPayAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanBeginDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPayPlanDelinqAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPriClmOrigTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPriClmCurTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSecClmOrigTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTSecClmCurTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerClmOrigTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerClmCurTransDateTime)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrPrimInsuranceBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSecInsuranceBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrTerInsuranceBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrSelfPayBalance)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.AttendingPhysicianName)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.IncludesEstPatLiab)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.TotalChargeAmount)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.NonCoveredChargeAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.ABNChargeAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmtInd)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDeductibleRemAmtInd)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrDeductibleRemAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.DeductibleAppliedAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCopayAmtInd)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCopayAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsurancePctInd)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsurancePct)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EncntrCoinsuranceAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.MaximumOutOfPocketAmtInd)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.MaximumOutOfPocketAmt)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.AmtOverMaxOutOfPocket)].ToString().ToUpper(),
                        drEnct[iEnctr][nameof(PatientStatementEncounter.EstContractAllowanceAmt)].ToString().ToUpper()

                        ));
*/
                        if (cernerStatementDt.Rows.Count > 0)
                        {
                            DataRow[] drEnctMsg = cernerStatementDt.Select($"{nameof(PatientStatementCerner.StatementType)} = 'EMSG' " +
                                $"and {nameof(PatientStatementCerner.StatementTypeId)} = {dr[nameof(PatientStatement.StatementNumber)]} " +
                                $"and {nameof(PatientStatementCerner.Account)} = '{drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrId)]}'");

                            for (int iEmsg = 0; iEmsg <= drEnctMsg.GetUpperBound(0); iEmsg++)
                            {
                                sw.Write(string.Format("{0}|{1}|{2}|{3}\r\n",
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.StatementType)].ToString().ToUpper(),
                                    (iEmsg + 1).ToString().ToUpper(),
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.Account)].ToString().ToUpper(),
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.StatementText)].ToString().ToUpper()));
                            }
                        }
                        DataRow[] drActv = encountersActivityDt.Select($"{nameof(PatientStatementEncounterActivity.StatementNumber)} = '{dr[nameof(PatientStatement.StatementNumber)]}' " +
                            $"and {nameof(PatientStatementEncounterActivity.ParentActivityId)} = '{drEnct[iEnctr][nameof(PatientStatementEncounter.RecordCount)]}'");

                        for (int iActv = 0; iActv <= drActv.GetUpperBound(0); iActv++)
                        {
                            line = new('|');

                            line.AddField("ACTV");
                            line.AddField((iActv + 1).ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.EncntrNumber)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityId)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityType)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDate)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDescription)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityCode)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityAmount)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.Units)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.CptCode)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.CptDescription)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.DrgCode)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.RevenueCode)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.RevenueCodeDescription)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.HCPCSCode)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.HPCPSDescription)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.OrderMgmtActivityType)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityAmountDue)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDateOfService)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityPatientBalance)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityInsDiscount)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransType)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransSubType)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransAmount)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityHealthPlanName)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityInsPending)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDrCrFlag)].ToString().ToUpper());
                            line.AddField(drActv[iActv][nameof(PatientStatementEncounterActivity.ParentActivityId)].ToString().ToUpper());

                            /*                            sw.Write(string.Format("ACTV|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}" +
                                                            "|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}" +
                                                            "|{26}|{27}" +
                                                            "\r\n",
                                                        (iActv + 1).ToString().ToUpper(), //drActv[iActv]["record_cnt"].ToString(), //0
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.EncntrNumber)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityId)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityType)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDate)].ToString().ToUpper(),                  //4
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDescription)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityCode)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityAmount)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.Units)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.CptCode)].ToString().ToUpper(),                       //9
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.CptDescription)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.DrgCode)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.RevenueCode)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.RevenueCodeDescription)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.HCPCSCode)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.HPCPSDescription)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.OrderMgmtActivityType)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityAmountDue)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDateOfService)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityPatientBalance)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityInsDiscount)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransType)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransSubType)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityTransAmount)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityHealthPlanName)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityInsPending)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ActivityDrCrFlag)].ToString().ToUpper(),
                                                        drActv[iActv][nameof(PatientStatementEncounterActivity.ParentActivityId)].ToString().ToUpper()

                                                        ));
                            */
                            
                            sw.WriteLine(line.ToString());
                        }
                    }
                }
            }

            sw.Write("TRL~MCL\r\n");
            sw.Close();
            return strFileName;
        }

        public async Task CompileStatementsAsync(DateTime throughDate) => await Task.Run(() => CompileStatements(throughDate));

        public void CompileStatements(DateTime throughDate)
        {
            dbConnection.BeginTransaction();

            if (throughDate == DateTime.MinValue)
            {
                throw new ArgumentException("Through Date is not a valid date.", "throughDate");
            }

            endDate = throughDate;
            batchNo = $"{endDate.Year}{endDate.Month:00}";
            try
            {
                //run exec usp_prg_pat_bill_update_flags '<last day of prev month>'
                //step 1 - exec_prg_pat_bill_update_flags '{thrudate}'
                dbConnection.ExecuteNonQueryProc("usp_prg_pat_bill_update_flags",
                    new SqlParameter() { ParameterName = "@thrudate", SqlDbType = SqlDbType.DateTime, Value = endDate });

                //run exec usp_prg_pat_bill_compile @batchNo = '<batchNo>', @endDate = '<last day of prev month>'
                dbConnection.ExecuteNonQueryProc("usp_prg_pat_bill_compile",
                    new SqlParameter() { ParameterName = "@batchNo", SqlDbType = SqlDbType.VarChar, Value = batchNo },
                    new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Value = endDate });

                dbConnection.CompleteTransaction();
            }
            catch (Exception ex)
            {
                dbConnection.AbortTransaction();
                throw new ApplicationException("Error in PatientBilling Compile Statements", ex);
            }
        }

    }
}
