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
using Log = LabBilling.Logging.Log;
using LabBilling.Core.UnitOfWork;
using PetaPoco;

namespace LabBilling.Core.Services;


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

public sealed class PatientBillingService
{
    private string batchNo;
    private DateTime endDate = DateTime.MinValue;

    private readonly IAppEnvironment appEnvironment;
    private readonly AccountService accountService;

    public PatientBillingService(IAppEnvironment appEnvironment)
    {
        this.appEnvironment = appEnvironment;
        accountService = new(appEnvironment);
    }

    public event EventHandler<ProgressEventArgs> ProgressIncrementedEvent;

    public async Task<string> SendToCollectionsAsync() => await Task.Run(() => SendToCollections());

    public BadDebt GetCollectionRecord(string accountNo)
    {
        using PatientStatementUnitOfWork uow = new(appEnvironment);

        return uow.BadDebtRepository.GetRecord(accountNo);
    }

    public BadDebt SaveCollectionRecord(BadDebt badDebt)
    {
        using PatientStatementUnitOfWork uow = new(appEnvironment, true);

        var retval = uow.BadDebtRepository.Update(badDebt);
        uow.Commit();

        return retval;

    }

    /// <summary>
    /// Accounts that have been marked for collections will be flagged and placed in file for collection agency.
    /// </summary>
    /// <returns>Filename generated</returns>
    /// <exception cref="ApplicationException"></exception>
    public string SendToCollections()
    {
        // set date_sent records in bad_debt table where date_sent is null to today's date
        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
        var results = unitOfWork.BadDebtRepository.GetNotSentRecords();

        if (!results.Any())
        {
            ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(100, "No records to process."));
            return "";
        }
       
        int recordsProcessed = 0;
        foreach (var result in results)
        {
            try
            {
                var acc = accountService.GetAccount(result.AccountNo, true);

                result.DateSent = DateTime.Now;

                unitOfWork.BadDebtRepository.Update(result, new[] { nameof(BadDebt.DateSent) });

                accountService.AddNote(acc.AccountNo, $"Account sent to collections. Write off amount {acc.Balance}");

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

                unitOfWork.ChkRepository.Add(chk);

                //update bad debt date on pat record
                acc.Pat.SentToCollectionsDate = DateTime.Today;
                unitOfWork.PatRepository.Update(acc.Pat, new[] { nameof(Pat.SentToCollectionsDate) });

                int cnt = results.Count();
                decimal processed = ++recordsProcessed / (decimal)cnt;
                int percentComplete = Convert.ToInt32(processed * 100);
                ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(percentComplete, $"Processed {result.AccountNo}"));
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error during collections send. Process aborted.", ex);
                
                throw new ApplicationException("Error during collections send.Process aborted.", ex);
            }
        }
        unitOfWork.Commit();
        return GenerateCollectionsFile(results);
      
    }

    public int RegenerateCollectionsFile(DateTime tDate)
    {
        UnitOfWorkMain unitOfWork = new(appEnvironment);

        var results = unitOfWork.BadDebtRepository.GetSentByDate(tDate);

        if (results.Any())
        {
            GenerateCollectionsFile(results);
        }

        return results.Count();
    }


    public async Task<string> GenerateCollectionsFileAsync(IEnumerable<BadDebt> records) => await Task.Run(() => GenerateCollectionsFile(records));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="records"></param>
    /// <returns>Filename generated</returns>
    /// <exception cref="ApplicationException"></exception>
    private string GenerateCollectionsFile(IEnumerable<BadDebt> records)
    {
        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
        // set date_sent records in bad_debt table where date_sent is null to today's date

        //get first record for date
        DateTime? date = records.First<BadDebt>().DateSent;

        if (!date.HasValue)
            return "";

        StringBuilder sb = new();
        string fileName = appEnvironment.ApplicationParameters.CollectionsFileLocation;
        fileName += $"MCL{date:MMddyyyy}.txt";

        // create collections file to send to MSCB
        FixedFileLine fileLine = new(20);
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
                var acc = accountService.GetAccount(result.AccountNo);

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
                unitOfWork.Context.AbortTransaction();
                throw new ApplicationException("Error during collections send.Process aborted.", ex);
            }
        }
        unitOfWork.Commit();
        File.WriteAllText(fileName, sb.ToString());

        // TODO: email mailer P report
        return fileName;
    }

    public bool BatchPreviouslyRun(string batchNo)
    {
        UnitOfWorkMain uow = new(appEnvironment);
        int cnt = uow.PatientStatementRepository.GetStatementCount(batchNo);

        if (cnt > 0)
            return true;

        return false;
    }

    public List<PatientStatement> GetStatements(string accountNo)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var accountstatements = uow.PatientStatementAccountRepository.GetByAccount(accountNo);
        var statements = accountstatements.Select(x => x.StatementNumber.ToString()).ToList();

        var records = uow.PatientStatementRepository.GetByStatement(statements);

        records.ForEach(record =>
        {
            record.Accounts = uow.PatientStatementAccountRepository.GetByStatement(record.StatementNumber);
            record.Encounters = uow.PatientStatementEncounterRepository.GetByStatement(record.StatementNumber);
            record.EncounterActivity = uow.PatientStatementEncounterActivityRepository.GetByStatement(record.StatementNumber);
        });

        return records;

    }

    public PatientStatement GetStatement(long statementNumber)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var record = uow.PatientStatementRepository.GetByStatement(statementNumber);
        record.Accounts = uow.PatientStatementAccountRepository.GetByStatement(record.StatementNumber);
        record.Encounters = uow.PatientStatementEncounterRepository.GetByStatement(record.StatementNumber);
        record.EncounterActivity = uow.PatientStatementEncounterActivityRepository.GetByStatement(record.StatementNumber);

        return record;
    }

    public List<PatientStatement> GetStatementsByBatch(string batch)
    {
        using UnitOfWorkMain uow = new(appEnvironment);

        var records = uow.PatientStatementRepository.GetByBatch(batch);

        records.ForEach(record =>
        {
            record.Accounts = uow.PatientStatementAccountRepository.GetByStatement(record.StatementNumber);
            record.Encounters = uow.PatientStatementEncounterRepository.GetByStatement(record.StatementNumber);
            record.EncounterActivity = uow.PatientStatementEncounterActivityRepository.GetByStatement(record.StatementNumber);
        });

        return records;

    }

    public PatientStatement AddPatientStatement(PatientStatement record)
    {
        using UnitOfWorkMain uow = new(appEnvironment, true);

        uow.PatientStatementRepository.Add(record);
        record.Accounts.ForEach(a => uow.PatientStatementAccountRepository.Add(a));
        record.Encounters.ForEach(e => uow.PatientStatementEncounterRepository.Add(e));
        record.EncounterActivity.ForEach(ea => uow.PatientStatementEncounterActivityRepository.Add(ea));

        var result = GetStatement(record.StatementNumber);

        uow.Commit();
        return result;
    }

    public async Task<string> CreateStatementFileAsync(DateTime throughDate) => await Task.Run(() => CreateStatementFile(throughDate));

    /// <summary>
    /// 
    /// </summary>
    /// <param name="throughDate"></param>
    /// <returns>Path of file created.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ApplicationException"></exception>
    public string CreateStatementFile(DateTime throughDate)
    {
        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
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

            unitOfWork.Context.Execute(sql, new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = batchNo });

            sql = $"update dbo.patbill_stmt SET statement_submitted_dt_tm = pa.processed_date " +
            "from patbill_stmt ps inner join patbill_acc pa on pa.statement_number = ps.statement_number " +
            "WHERE nullif(ps.statement_submitted_dt_tm,'') is null " +
            "and pa.processed_date is not null";

            unitOfWork.Context.Execute(sql);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error in Patient Billing Create Statement file", ex);
        }
        unitOfWork.Commit();
        return filename;

    }

    private async Task<string> FormatStatementFileAsync() => await Task.Run(() => FormatStatementFile());

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Path of file created.</returns>
    private string FormatStatementFile()
    {
        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

        var statements = unitOfWork.PatientStatementRepository.GetByBatch(batchNo);
        var accounts = unitOfWork.PatientStatementAccountRepository.GetByBatch(batchNo);
        var encounters = unitOfWork.PatientStatementEncounterRepository.GetByBatch(batchNo);
        var encountersActivity = unitOfWork.PatientStatementEncounterActivityRepository.GetByBatch(batchNo);
        var cernerStatement = unitOfWork.PatientStatementCernerRepository.GetByBatch(batchNo);

        DataTable statementDt = HelperExtensions.ConvertToDataTable(statements);
        DataTable accountsDt = HelperExtensions.ConvertToDataTable(accounts);
        DataTable encountersDt = HelperExtensions.ConvertToDataTable(encounters);
        DataTable encountersActivityDt = HelperExtensions.ConvertToDataTable(encountersActivity);
        DataTable cernerStatementDt = HelperExtensions.ConvertToDataTable(cernerStatement);

        //todo: get file path from parameters
        string strFileName = $"{appEnvironment.ApplicationParameters.StatementsFileLocation}PatStatement{DateTime.Now:yyyyMMddHHmm}.txt";

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
        unitOfWork.Commit();
        return strFileName;
    }

    public async Task CompileStatementsAsync(DateTime throughDate) => await Task.Run(() => CompileStatements(throughDate));

    public void CompileStatements(DateTime throughDate)
    {
        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

        if (throughDate == DateTime.MinValue)
        {
            throw new ArgumentException("Through Date is not a valid date.", nameof(throughDate));
        }

        endDate = throughDate;
        batchNo = $"{endDate.Year}{endDate.Month:00}";
        try
        {
            //run exec usp_prg_pat_bill_update_flags '<last day of prev month>'
            //step 1 - exec_prg_pat_bill_update_flags '{thrudate}'
            unitOfWork.Context.ExecuteNonQueryProc("usp_prg_pat_bill_update_flags",
                new SqlParameter() { ParameterName = "@thrudate", SqlDbType = SqlDbType.DateTime, Value = endDate });

            //run exec usp_prg_pat_bill_compile @batchNo = '<batchNo>', @endDate = '<last day of prev month>'
            unitOfWork.Context.ExecuteNonQueryProc("usp_prg_pat_bill_compile",
                new SqlParameter() { ParameterName = "@batchNo", SqlDbType = SqlDbType.VarChar, Value = batchNo },
                new SqlParameter() { ParameterName = "@endDate", SqlDbType = SqlDbType.DateTime, Value = endDate });

            unitOfWork.Commit();
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Error in PatientBilling Compile Statements", ex);
        }
    }

    public async Task CompileStatementsNewAsync(DateTime throughDate) => await Task.Run(() => CompileStatementsNew(throughDate));

    public void CompileStatementsNew(DateTime throughDate)
    {
        /*
        --This procedure replaces the functionality in the PatBill program.
        -- Uses vw_acc_pat -where mailer<> 'N' and trans_date<@thrudate
        --order by pat_name, account
        --errors need to be written to a log table
        -- if fin code is X, Y, W, Z, or CLIENT - do not do anything
        -- if account balance < 2.50 - do not process -write to error log
        -- update flags based on current flag
        -- if mailer == Y - update to 1
        --    mailer == 1 - update to 2
        --    mailer == 2 - update to 3
        --    mailer == P - no change
        -- if mailer is 3, 4, 5, 6 - Place on Collection List, update bd_list_date to today
        -- if mailer is 1, 2 - update last_dm to today, increment mailer
        -- if mailer is Y - update first_dm to today, update last_dm to today, set mailer to 1
        -- if mailer is P - update last_dm to today
        -- create table to record actions taken to accounts
        */

        using UnitOfWorkMain unitOfWork = new(appEnvironment, true);

        //get accounts with status of STMT
        var accounts = unitOfWork.AccountRepository.GetByStatus(AccountStatus.Statements);
        //starting statement number
        double temp = (throughDate.Year * 10000000.0) + (throughDate.Month * 100000.0) + 100001.0;
        long statementNo = Convert.ToInt64(temp);
        string batchId = throughDate.ToString("yyyyMM");
        //foreach account - determine action based on number of statements received
        int totalAccounts = accounts.Count();
        int processedAccounts = 0;

        try
        {
            //unitOfWork.Context.BeginTransaction();

            foreach (var account in accounts)
            {
                var acc = unitOfWork.AccountRepository.GetByAccount(account);

                if (acc != null)
                {
                    if (acc.PatientStatements.Count > appEnvironment.ApplicationParameters.NumberOfStatementsBeforeCollection
                        && acc.Pat.StatementFlag != "P")
                    {
                        //send account to collections

                        //write account to baddebt table
                        BadDebt bd = new()
                        {
                            AccountNo = acc.AccountNo,
                            DebtorLastName = acc.Pat.GuarantorLastName,
                            DebtorFirstName = acc.Pat.GuarantorFirstName,
                            StreetAddress1 = acc.Pat.GuarantorAddress,
                            StreetAddress2 = "",
                            City = acc.Pat.GuarantorCity,
                            StateZip = acc.Pat.GuarantorState + " " + acc.Pat.GuarantorZipCode,
                            Spouse = acc.InsurancePrimary.Relation == "02" ? acc.InsurancePrimary.HolderFullName : "",
                            Phone = acc.Pat.PrimaryPhone.Filter(new List<char> { '(', ')', '-' }),
                            SocialSecurityNo = acc.SocSecNo.Replace("-", ""),
                            Employment = acc.InsurancePrimary.Employer.Left(35),
                            PatientName = acc.PatFullName.Left(20),
                            Misc = acc.BirthDate?.ToString("mm/dd/yyyy"),
                            ServiceDate = acc.TransactionDate,
                            Balance = acc.Balance,
                            DateEntered = DateTime.Today
                        };

                        unitOfWork.BadDebtRepository.Add(bd);

                        //update account status to ?? COLL
                        unitOfWork.AccountRepository.UpdateStatus(acc.AccountNo, AccountStatus.Collections);

                    }
                    else
                    {
                        //generate a statement
                        //20230210083 -- increment

                        //if account balance < 2.50, do not generate statement
                        var patientStatement = GenerateStatement(ref statementNo, batchId, acc);

                        unitOfWork.PatientStatementRepository.Add(patientStatement);

                    }

                }

                decimal processed = ++processedAccounts / (decimal)totalAccounts;
                int percentComplete = Convert.ToInt32(processed * 100);
                ProgressIncrementedEvent?.Invoke(this, new ProgressEventArgs(percentComplete, $"Processed {acc.AccountNo}"));
            }
            unitOfWork.Commit();
        } 
        catch(Exception e)
        {
            Log.Instance.Error(e, "Error in statement generation.");
            Log.Instance.Debug(unitOfWork.Context.LastSQL);
            //unitOfWork.Context.AbortTransaction();
            throw new ApplicationException("Error in statement generation.", e);
        }
    }

    private PatientStatement GenerateStatement(ref long statementNo, string batchId, Account acc)
    {
        PatientStatementAccount patientStatementAccount = new()
        {
            StatementNumber = statementNo++,
            RecordCountAcct = "1", //increment on acc.pat_name, dob_yyyy, acc.ssn, acc.sex
            PatientName = acc.PatFullName.Left(40),
            AccountSubtotal = "SUBTOTAL",
            TotalAccountSubtotal = acc.Balance,
            AccountAmtDue = acc.Balance,
            AccountInsPending = 0.00,
            AccountDatesOfService = acc.TransactionDate.ToString("mm/dd/yyyy"),
            AccountUnpaidBalance = acc.Balance,
            AccountPatientBalance = acc.Balance,
            AccountPaidSinceLastStatement = 0.00, // get payments since last mailer
            AccountInsDiscount = acc.TotalContractual,
            AccountDateDue = DateTime.Today.AddMonths(1).ToShortDateString(),
            AccountHealthPlanName = acc.InsurancePrimary.PlanName,
            PatientDateOfBirth = acc.BirthDate?.ToString("mm/dd/yyyy"),
            PatientSex = acc.Sex,
            IncludesEstPatLib = 0,
            TotalChargeAmt = acc.TotalCharges,
            NonCoveredChargeAmt = 0,
            ABNChargeAmt = 0,
            EstContractAllowanceAmtInd = 0,
            EstContractAllowanceAmt = 0,
            EncntrDeductibleRemAmtInd = 0,
            DeductibleAppliedAmt = 0,
            EncntrCopayAmtInd = 0,
            EncntrCopayAmt = 0,
            EncntrCoinsurancePctInd = 0,
            EncntrCoinsurancePct = 0,
            EncntrCoinsuranceAmt = 0,
            MaximumOutOfPocketAmtInd = 0,
            AmtOverMaxOutOfPocket = 0,
            EstPatientLiabAmt = 0,
            AccountMsg = "", //get days since last payment
            FirstDataMailer = acc.Pat.FirstStatementDate,
            LastDataMailer = acc.Pat.LastStatementDate,
            MailerCount = acc.PatientStatements.Count, //get number of statements generated
            ProcessedDate = DateTime.Today,
            BatchId = batchId,
            AgingBucket30 = 0,
            AgingBucket60 = 0,
            AgingBucket90 = 0,
            AgingBucketCurrent = 0
        };

        PatientStatement patientStatement = new()
        {
            RecordType = "STMT",
            RecordCount = 0,
            BillingEntityStreet = appEnvironment.ApplicationParameters.BillingEntityStreet,
            BillingEntityCity = appEnvironment.ApplicationParameters.BillingEntityCity,
            BillingEntityState = appEnvironment.ApplicationParameters.BillingEntityState,
            BillingEntityZip = appEnvironment.ApplicationParameters.BillingEntityZip,
            BillingEntityFedTaxId = appEnvironment.ApplicationParameters.BillingEntityFedTaxId,
            BillingEntityFax = appEnvironment.ApplicationParameters.BillingEntityFax,
            BillingEntityName = appEnvironment.ApplicationParameters.BillingEntityName,
            BillingEntityPhone = appEnvironment.ApplicationParameters.BillingEntityPhone,
            RemitToStreet = appEnvironment.ApplicationParameters.RemitToStreet,
            RemitToStreet2 = appEnvironment.ApplicationParameters.RemitToStreet2,
            RemitToCity = appEnvironment.ApplicationParameters.RemitToCity,
            RemitToState = appEnvironment.ApplicationParameters.RemitToState,
            RemitToZip = appEnvironment.ApplicationParameters.RemitToZip,
            RemitToOrgName = appEnvironment.ApplicationParameters.RemitToOrganizationName,
            GuarantorStreet = acc.Pat.GuarantorAddress,
            GuarantorCity = acc.Pat.GuarantorCity,
            GuarantorState = acc.Pat.GuarantorState,
            GuarantorZip = acc.Pat.GuarantorZipCode,
            GuarantorName = acc.Pat.GuarantorFullName,
            AmountDue = patientStatementAccount.AccountAmtDue,
            TotalPaidSinceLastStatement = patientStatementAccount.AccountPaidSinceLastStatement,
            EstPatientLiabAmt = patientStatementAccount.IncludesEstPatLib,
            StatementTotalAmount = patientStatementAccount.TotalChargeAmt,
            AgingBucketCurrent = patientStatementAccount.AgingBucketCurrent,
            AgingBucket30Day = patientStatementAccount.AgingBucket30,
            AgingBucket60Day = patientStatementAccount.AgingBucket60,
            AgingBucket90Day = patientStatementAccount.AgingBucket90,
            StatementTime = "12:00",
            StatementNumber = patientStatementAccount.StatementNumber
        };


        patientStatement.Accounts.Add(patientStatementAccount);

        PatientStatementEncounter patientStatementEncounter = new()
        {
            RecordType = "ENCT",
            RecordCount = Convert.ToInt32(patientStatementAccount.RecordCountAcct),
            EncntrNumber = patientStatementAccount.AccountId,
            PFTEncntrDatesOfService = patientStatementAccount.AccountDatesOfService,
            StatementNumber = patientStatementAccount.StatementNumber,
            PFTEncntrAmtDue = acc.ClaimBalance.ToString(),
            PFTEncntrPatientBalance = 0.00,
            PFTEncntrPaidSinceLastStmt = 0.00,
            PFTEncntrInPending = 0.00,
            PFTEncntrTotal = acc.ClaimBalance.ToString(),
            EncntrAdmitDateTime = acc.TransactionDate.ToShortDateString(),
            EncntrDischargeDateTime = acc.TransactionDate.ToShortDateString(),
            EncntrType = "REF LAB OUTREACH",
            PFTEncntrTotalCharges = acc.BillableCharges.Sum(x => x.Quantity * x.NetAmount).ToString(),
            TotalPatientPayments = 0.00,
            TotalPatientAdjustments = 0.00,
            BatchId = batchId
        };

        patientStatement.Encounters.Add(patientStatementEncounter);


        int encntractcnt = 1;
        foreach (var chrg in acc.Charges.Where(x => x.IsCredited == false && x.FinancialType == "M"))
        {

            PatientStatementEncounterActivity patientStatementEncounterActivity = new()
            {
                StatementNumber = patientStatementAccount.StatementNumber,
                RecordType = "ACTV",
                RecordCount = encntractcnt++,
                EncntrNumber = patientStatementAccount.PatientAccountNumber,
                ActivityId = patientStatementAccount.AccountId,
                ActivityDate = patientStatementAccount.AccountDatesOfService,
                ActivityDescription = "LABORTORY",
                Units = chrg.Quantity,
                CptCode = chrg.CDMCode,
                CptDescription = chrg.Cdm.Description,
                ActivityAmountDue = chrg.NetAmount,
                ActivityDateOfService = patientStatementAccount.AccountDatesOfService,
                ActivityPatientBalance = 0.00,
                ActivityInsDiscount = 0.00,
                ActivityTransType = "CHARGE",
                ActivityTransAmount = chrg.NetAmount,
                ActivityInsPending = 0.00,
                ActivityDrCrFlag = chrg.Quantity >= 0 ? 1 : -1,
                ParentActivityId = Convert.ToInt32(patientStatementAccount.RecordCountAcct),
                BatchId = batchId
            };

            patientStatement.EncounterActivity.Add(patientStatementEncounterActivity);
        }

        return patientStatement;
    }
}
