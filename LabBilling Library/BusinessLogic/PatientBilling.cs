using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using RFClassLibrary;
using PetaPoco;
using System.Data.SqlClient;
using System.Diagnostics;
using LabBilling.Logging;
using PetaPoco.Providers;

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

    public class PatientBilling
    {
        private Database db;
        private string batchNo;
        private DateTime endDate = DateTime.MinValue;
        private PatRepository patRepository;
        private AccountRepository accountRepository;
        private BadDebtRepository badDebtRepository;
        private ChkRepository chkRepository;
        private PatientStatementRepository patientStatementRepository;
        private PatientStatementAccountRepository patientStatementAccountRepository;
        private PatientStatementCernerRepository patientStatementCernerRepository;
        private PatientStatementEncounterActivityRepository patientStatementEncounterActivityRepository;
        private PatientStatementEncounterRepository patientStatementEncounterRepository;
        private SystemParametersRepository parametersRepository;

        public PatientBilling(string connectionString)
        {
            db = new Database(connectionString, new CustomSqlDatabaseProvider());

            accountRepository = new AccountRepository(db);
            patRepository = new PatRepository(db);
            badDebtRepository = new BadDebtRepository(db);
            chkRepository = new ChkRepository(db);
            parametersRepository = new SystemParametersRepository(db);
            patientStatementRepository = new PatientStatementRepository(db);
            patientStatementCernerRepository = new PatientStatementCernerRepository(db);
            patientStatementEncounterRepository = new PatientStatementEncounterRepository(db);
            patientStatementEncounterActivityRepository = new PatientStatementEncounterActivityRepository(db);
            patientStatementAccountRepository = new PatientStatementAccountRepository(db);
        }

        public event EventHandler<ProgressEventArgs> ProgressIncrementedEvent;

        public async Task SendToCollections()
        {
            // set date_sent records in bad_debt table where date_sent is null to today's date
            await Task.Run(() =>
            {
                StringBuilder sb = new StringBuilder();
                string fileName = parametersRepository.GetByKey("CollectionsFileLocation");
                fileName += $"MCL{DateTime.Today.ToString("MMddyyyy")}.txt";

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

                var results = badDebtRepository.GetNotSentRecords();

                if(results.Count() == 0)
                {
                    ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(100, "No records to process."));
                    return;
                }

                db.BeginTransaction();
                int recordsProcessed = 0;
                foreach (var result in results)
                {
                    try
                    {
                        var acc = accountRepository.GetByAccount(result.AccountNo, true);

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
                        fileLine.SetField(17, 355, 360, result.ServiceDate.NullDateToString("mmddyy"));
                        fileLine.SetField(18, 361, 366, result.PaymentDate.NullDateToString("mmddyy"));
                        fileLine.SetField(19, 367, 376, result.Balance.ToString());
                        fileLine.SetField(20, 377, 386, acc.TotalCharges.ToString());

                        sb.AppendLine(fileLine.OutputLine());

                        result.DateSent = DateTime.Now;

                        badDebtRepository.Update(result);

                        // write off accounts where bad_debt.date_sent = today
                        Chk chk = new Chk();
                        chk.AccountNo = result.AccountNo;
                        chk.Batch = 0;
                        chk.WriteOffAmount = acc.Balance;
                        chk.WriteOffDate = DateTime.Today;
                        chk.Source = "BAD_DEBT";
                        chk.Status = "WRITE_OFF";
                        chk.Comment = "BAD DEBT WRITE OFF";
                        chk.IsCollectionPmt = true;

                        chkRepository.Add(chk);

                        ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(++recordsProcessed / results.Count() * 100, $"Processed {result.AccountNo}"));
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Error("Error during collections send. Process aborted.", ex);
                        db.AbortTransaction();
                        throw new ApplicationException("Error during collections send.Process aborted.", ex);
                    }
                }

                File.WriteAllText(fileName, sb.ToString());

                db.CompleteTransaction();
            });
            // email mailer P report
        }

        public void CreateStatementFile(DateTime throughDate)
        {
            if (throughDate == DateTime.MinValue)
            {
                throw new ArgumentException("Through Date is not a valid date.", "throughDate");
            }

            endDate = throughDate;
            batchNo = $"{endDate.Year}{endDate.Month:00}";

            // 5. step 5 - run viewer bad debt and create file
            //AFTER running pat viewer bad debt to create the file do this. 
            FormatStatementFile();

            //upload statement file to DNI from MCLFTP2


            // 6. step 6 run these queries individually

            try
            {
                string sql = $"update dbo.patbill_acc SET date_sent = convert(varchar(10),getdate(),101) " +
                "WHERE nullif(date_sent,'') IS null AND batch_id = @0";

                db.Execute(sql, new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batchNo });

                sql = $"update dbo.patbill_stmt SET statement_submitted_dt_tm = pa.processed_date " +
                "from patbill_stmt ps inner join patbill_acc pa on pa.statement_number = ps.statement_number " +
                "WHERE nullif(ps.statement_submitted_dt_tm,'') is null " +
                "and pa.processed_date is not null";

                db.Execute(sql);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error in Patient Billing Create Statement file", ex);
            }

        }

        private void FormatStatementFile()
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
            //string.Format(@"\\mclftp2\MCLFTP_E\TEMP\PatStatement{0}.txt", DateTime.Now.ToString("yyyyMMddHHmm"));
            string strFileName = $"{parametersRepository.GetByKey("StatementsFileLocation")}PatStatement{DateTime.Now.ToString("yyyyMMddHHmm")}.txt";
            
            StreamWriter sw = new StreamWriter(strFileName);
            sw.AutoFlush = true;

            sw.Write(string.Format("HDR~MCL~~CERNER~MCL~{0}~{1}~T~N~0~0~0\r\n"
                , DateTime.Now.ToString("yyyyMMdd")
                , DateTime.Now.ToString("HHmmss")));

            foreach (DataRow dr in statementDt.Rows)
            {
                var q = $"{nameof(PatientStatement.StatementNumber)} = {dr[nameof(PatientStatement.StatementNumber)].ToString()}";
                // each statement
                DataRow[] drAcc = accountsDt.Select(q);

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

                if (cernerStatementDt.Rows.Count > 0)
                {
                    DataRow[] drStmtMsg = cernerStatementDt.Select($"{nameof(PatientStatementCerner.StatementType)} = 'SMSG' " + 
                        $"and {nameof(PatientStatementCerner.StatementTypeId)} = '{dr[nameof(PatientStatement.StatementNumber)].ToString()}' ");

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
                    sw.Write(string.Format("ACCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}" +
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

                    // each encounter
                    DataRow[] drEnct = encountersDt.Select($"{nameof(PatientStatementEncounter.StatementNumber)} = {dr[nameof(PatientStatement.StatementNumber)].ToString()} " + 
                        $"and {nameof(PatientStatementEncounter.RecordCount)} = '{drAcc[iAcc][nameof(PatientStatementAccount.RecordCountAcct)]}'");

                    //string.Format("statement_number = '{0}' and record_cnt = '{1}'", 
                    //    dr[nameof(PatientStatement.StatementNumber)].ToString().ToUpper(), 
                    //    drAcc[iAcc][nameof(PatientStatementAccount.RecordCountAcct)].ToString().ToUpper()));

                    for (int iEnctr = 0; iEnctr <= drEnct.GetUpperBound(0); iEnctr++)
                    {
                        sw.Write(string.Format("ENCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}" +
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

                        if (cernerStatementDt.Rows.Count > 0)
                        {
                            DataRow[] drEnctMsg = cernerStatementDt.Select($"{nameof(PatientStatementCerner.StatementType)} = 'EMSG' " + 
                                $"and {nameof(PatientStatementCerner.StatementTypeId)} = {dr[nameof(PatientStatement.StatementNumber)].ToString()} " + 
                                $"and {nameof(PatientStatementCerner.Account)} = '{drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrId)]}'");

                            //string.Format("statement_type = 'EMSG' and  statement_type_id = '{0}' and account = '{1}'",
                            //dr[nameof(PatientStatement.StatementNumber)].ToString(), drEnct[iEnctr][nameof(PatientStatementEncounter.PFTEncntrId)].ToString().ToUpper()));

                            for (int iEmsg = 0; iEmsg <= drEnctMsg.GetUpperBound(0); iEmsg++)
                            {
                                sw.Write(string.Format("{0}|{1}|{2}|{3}\r\n",
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.StatementType)].ToString().ToUpper(),
                                    (iEmsg + 1).ToString().ToUpper(),
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.Account)].ToString().ToUpper(),
                                    drEnctMsg[iEmsg][nameof(PatientStatementCerner.StatementText)].ToString().ToUpper()));
                            }
                        }
                        DataRow[] drActv = encountersActivityDt.Select($"{nameof(PatientStatementEncounterActivity.StatementNumber)} = '{dr[nameof(PatientStatement.StatementNumber)].ToString()}' " +
                            $"and {nameof(PatientStatementEncounterActivity.ParentActivityId)} = '{drEnct[iEnctr][nameof(PatientStatementEncounter.RecordCount)]}'");
                                //string.Format("statement_number = '{0}' and parent_activity_id = '{1}'"
                                //, dr[nameof(PatientStatement.StatementNumber)].ToString().ToUpper()
                                //, drEnct[iEnctr][nameof(PatientStatementEncounter.RecordCount)].ToString().ToUpper()));
                        for (int iActv = 0; iActv <= drActv.GetUpperBound(0); iActv++)
                        {

                            sw.Write(string.Format("ACTV|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}" +
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
                        }
                    }
                }
            }

            sw.Write("TRL~MCL\r\n");
            sw.Close();
        }

        public void CompileStatements(DateTime throughDate)
        {
            db.BeginTransaction();

            if(throughDate == DateTime.MinValue)
            {
                throw new ArgumentException("Through Date is not a valid date.", "throughDate");
            }

            endDate = throughDate;
            batchNo = $"{endDate.Year}{endDate.Month.ToString("00")}";
            try
            {
                //run exec usp_prg_pat_bill_update_flags '<last day of prev month>'
                //step 1 - exec_prg_pat_bill_update_flags '{thrudate}'
                //db.ExecuteNonQueryProc("usp_prg_pat_bill_update_flags", new SqlParameter() { ParameterName = "@thrudate", SqlDbType = SqlDbType.DateTime, Value = endDate });

                //run exec usp_prg_pat_bill_compile @batchNo = '<batchNo>', @endDate = '<last day of prev month>'
                db.ExecuteNonQueryProc("usp_prg_pat_bill_compile",
                    new SqlParameter() { ParameterName = "@batchNo", SqlDbType = SqlDbType.VarChar, Value = batchNo },
                    new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Value = endDate });

                db.CompleteTransaction();
            }
            catch (Exception ex)
            {
                db.AbortTransaction();
                throw new ApplicationException("Error in PatientBilling Compile Statements", ex);
            }
        }

    }
}
