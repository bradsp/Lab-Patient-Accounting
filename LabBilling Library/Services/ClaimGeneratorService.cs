﻿using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Utilities;
using Account = LabBilling.Core.Models.Account;

namespace LabBilling.Core.Services;

public sealed class ClaimGeneratorService
{

    private readonly ArrayList _nameSuffixList = new() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };

    public string PropProductionEnvironment { get; set; }
    private readonly IAppEnvironment _appEnvironment;
    private List<ClaimData> _claims;
    private Billing837Service _billing837;
    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;

    public ClaimGeneratorService(IAppEnvironment appEnvironment)
    {
        this._appEnvironment = appEnvironment;
        ArgumentNullException.ThrowIfNull(appEnvironment);
        if (!appEnvironment.EnvironmentValid)
        {
            throw new ApplicationException("Application Environment not valid.");
        }

        PropProductionEnvironment = appEnvironment.ApplicationParameters.GetProductionEnvironment();

        _claims = new List<ClaimData>();

        _accountService = new(appEnvironment);
        _dictionaryService = new(appEnvironment);

    }


    public int CompileBillingBatch(ClaimType claimType, IProgress<ProgressReportModel> progress, CancellationToken cancellationToken, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        _claims = new List<ClaimData>();

        ProgressReportModel report = new();
        //compile list of accounts to have claims generated
        _billing837 = new Billing837Service(PropProductionEnvironment);
        string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId;

        BillingBatch billingBatch = new()
        {
            RunDate = DateTime.Today,
            RunUser = OS.GetUserName(),
            ClaimCount = 0,
            TotalBilled = 0
        };
        double strNum = 0.00;

        try
        {
            var batch = uow.BillingBatchRepository.Add(billingBatch);
            strNum = batch.Batch;
        }
        catch (Exception ex)
        {
            Log.Instance.Error("Error adding BillingBatch record.", ex);
            return -1;
        }

        string interchangeControlNumber = Convert.ToInt32(strNum).ToString("D9");
        string batchType;
        string fileLocation;

        List<ClaimItem> claimList;
        ClaimType billClaimType;
        string processedStatus = null;

        switch (claimType)
        {
            case ClaimType.Institutional:
                claimList = _accountService.GetClaimItems(ClaimType.Institutional, uow).ToList();
                billClaimType = ClaimType.Institutional;
                processedStatus = AccountStatus.InstSubmitted;
                batchType = AccountStatus.Institutional;
                fileLocation = _appEnvironment.ApplicationParameters.InstitutionalClaimFileLocation;
                break;
            case ClaimType.Professional:
                claimList = _accountService.GetClaimItems(ClaimType.Professional, uow).ToList();
                billClaimType = ClaimType.Professional;
                processedStatus = AccountStatus.ProfSubmitted;
                batchType = AccountStatus.Professional;
                fileLocation = _appEnvironment.ApplicationParameters.ProfessionalClaimFileLocation;
                break;
            default:
                throw new ArgumentOutOfRangeException("ClaimType is not defined.");
        }

        ClaimData claim;

        try
        {
            report.TotalRecords = claimList.Count;

            foreach (ClaimItem item in claimList)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    throw new TaskCanceledException();
                }

                try
                {
                    claim = GenerateClaim(item.AccountNo, uow);
                    if (claim == null)
                    {
                        //validation failed - skip to next record
                        report.RecordsProcessed++;
                        report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                        progress.Report(report);
                        continue;
                    }
                    _claims.Add(claim);
                }
                catch (AccountLockException alex)
                {
                    Log.Instance.Error($"Account locked.", alex);
                    continue;
                }
                catch (ApplicationException apex)
                {
                    Log.Instance.Error(apex);
                    continue;
                }

                //update status and activity date fields
                claim.claimAccount.Status = processedStatus;

                uow.AccountRepository.Update(claim.claimAccount, new[] { nameof(Account.Status) });

                claim.claimAccount.Pat.SSIBatch = interchangeControlNumber;

                switch (claimType)
                {
                    case ClaimType.Institutional:
                        claim.claimAccount.Pat.InstitutionalClaimDate = DateTime.Today;
                        uow.PatRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.InstitutionalClaimDate), nameof(Pat.SSIBatch) });
                        break;
                    case ClaimType.Professional:
                        claim.claimAccount.Pat.ProfessionalClaimDate = DateTime.Today;
                        uow.PatRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.ProfessionalClaimDate), nameof(Pat.SSIBatch) });
                        break;
                    default:
                        break;
                }


                BillingActivity billingActivity = new()
                {
                    PatientName = claim.claimAccount.PatFullName,
                    RunDate = DateTime.Today,
                    AccountNo = claim.claimAccount.AccountNo,
                    Batch = Convert.ToDouble(interchangeControlNumber),
                    ElectronicBillBatch = Convert.ToDouble(interchangeControlNumber),
                    ElectronicBillStatus = batchType,
                    FinancialCode = claim.claimAccount.FinCode,
                    InsuranceOrder = claim.claimAccount.Insurances[0].Coverage,
                    InsuranceCode = claim.claimAccount.Insurances[0].InsCode,
                    InsComplete = DateTime.MinValue,
                    TransactionDate = claim.claimAccount.TransactionDate,
                    Text = Newtonsoft.Json.JsonConvert.SerializeObject(claim),
                    ClaimAmount = claim.TotalChargeAmount - claim.PatientAmountPaid,
                    RunUser = OS.GetUserName()
                };

                uow.BillingActivityRepository.Add(billingActivity);

                report.RecordsProcessed++;
                report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                progress.Report(report);
            }

            string x12Text = string.Empty;
            if (_claims.Count > 0)
            {

                x12Text = _billing837.Generate837ClaimBatch(_claims, interchangeControlNumber,
                    batchSubmitterID, fileLocation, billClaimType);
            }

            billingBatch = uow.BillingBatchRepository.GetBatch(strNum);
            billingBatch.X12Text = x12Text;
            billingBatch.ClaimCount = _claims.Count;
            billingBatch.TotalBilled = _claims.Sum(x => x.TotalChargeAmount);
            billingBatch.BatchType = batchType;
            uow.BillingBatchRepository.Update(billingBatch);

            uow.Commit();

            return _claims.Count;
        }
        catch (TaskCanceledException tcex)
        {
            Log.Instance.Fatal(tcex, "Batch processing cancelled by user. Batch has been rolled back.");
            throw new TaskCanceledException();
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal(ex, $"Exception processing {claimType} Claims. Batch has been rolled back. Report error to the Application Administrator.");
            return -1;
        }
    }

    public void RegenerateBatch(double batchNo, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        _billing837 = new Billing837Service(PropProductionEnvironment);
        var batch = uow.BillingBatchRepository.GetBatch(batchNo);
        string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId;
        string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}", batch.Batch)));

        ClaimType claimType;

        string fileLocation;

        _claims.Clear();
        switch (batch.BatchType)
        {
            case "UB":
                claimType = ClaimType.Institutional;
                fileLocation = _appEnvironment.ApplicationParameters.InstitutionalClaimFileLocation;
                break;
            case "1500":
                claimType = ClaimType.Professional;
                fileLocation = _appEnvironment.ApplicationParameters.ProfessionalClaimFileLocation;
                break;
            default:
                //not a valid batch
                return;
        }

        batch.BillingActivities.ForEach(account =>
        {
            var claim = GenerateClaim(account.AccountNo, true);
            if (claim != null)
                _claims.Add(claim);
        });

        if (_claims.Count > 0)
        {
            batch.X12Text = _billing837.Generate837ClaimBatch(_claims, interchangeControlNumber,
                batchSubmitterID, fileLocation, claimType);

            batch.RunDate = DateTime.Now.Date;
            batch.TotalBilled = _claims.Sum(x => x.TotalChargeAmount);

            uow.BillingBatchRepository.Update(batch);
        }
        uow.Commit();
        return;
    }

    public void CompileClaim(string accountNo, IUnitOfWork uow = null)
    {
        //validate account data before starting - if there are errors do not process claim.
        // primary ins holder name is empty
        // no dx codes
        ClaimData claim;
        string claimx12;
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId;
        decimal strNum = uow.NumberRepository.GetNumber("ssi_batch");
        string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));

        try
        {
            claim = GenerateClaim(accountNo, uow);
            if (claim == null)
            {
                throw new ApplicationException("Error in GenerateClaim");
            }
            claim.InterchangeControlNumber = interchangeControlNumber;
            claim.BatchSubmitterId = batchSubmitterID;
            claimx12 = Newtonsoft.Json.JsonConvert.SerializeObject(claim);
        }
        catch (ApplicationException apex)
        {
            Log.Instance.Error(apex);
            return;
        }

        BillingActivity billingActivity = new()
        {
            PatientName = claim.claimAccount.PatFullName,
            RunDate = DateTime.Today,
            AccountNo = claim.claimAccount.AccountNo,
            Batch = Convert.ToDouble(interchangeControlNumber),
            FinancialCode = claim.claimAccount.FinCode,
            InsuranceOrder = claim.claimAccount.Insurances[0].Coverage,
            InsuranceCode = claim.claimAccount.Insurances[0].InsCode,
            InsComplete = DateTime.MinValue,
            TransactionDate = claim.claimAccount.TransactionDate,
            ClaimAmount = claim.TotalChargeAmount,
            RunUser = OS.GetUserName(),
            Text = claimx12
        };

        uow.BillingActivityRepository.Save(billingActivity);
        uow.Commit();

    }

    public ClaimData GenerateClaim(string account, IUnitOfWork uow = null) => GenerateClaim(account, false, uow);

    /// <summary>
    /// Generates the claim structure for a single account. Adds the entry to the ClaimsData list.
    /// </summary>
    /// <param name="account"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterValueException"></exception>
    public ClaimData GenerateClaim(string account, bool reprint = false, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        Account accountModel = new();
        uow.StartTransaction();

        try
        {
            accountModel = _accountService.GetAccount(account);
        }
        catch (AccountLockException alex)
        {
            Log.Instance.Warn("Account locked. Skipping claim generation.", alex);
            uow.Commit();
            return null;
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error encounter processing {account}", ex);
            _accountService.ClearAccountLock(accountModel);
            uow.Commit();
            return null;
        }

        //there is no balance so nothing to send on claim
        if (accountModel.ClaimBalance <= 0.00)
        {
            uow.Commit();
            _accountService.ClearAccountLock(accountModel);
            return null;
        }

        accountModel = _accountService.Validate(accountModel, reprint);

        if (accountModel.AccountValidationStatus.ValidationText != "No validation errors.")
        {
            _accountService.UpdateStatus(accountModel, AccountStatus.New, uow);
            _accountService.AddNote(accountModel.AccountNo, "Account has validation errors.Reverted to NEW status");
            Log.Instance.Info($"Account {accountModel.AccountNo} has validation errors. Reverted to NEW status");
            uow.Commit();
            _accountService.ClearAccountLock(accountModel);
            return null;
        }

        ClaimData claimData = new()
        {
            claimAccount = accountModel
        };

        claimData.claimAccount.Payments = claimData.claimAccount.Payments.ToList();

        claimData.claimAccount.ClaimCharges = claimData.claimAccount.Charges
            .SelectMany(chrg => chrg.ChrgDetails, (chrg, cd) => new { chrg, cd })
            .Where(x => !x.chrg.IsCredited &&
                        x.chrg.FinancialType == "M" &&
                        x.chrg.CDMCode != "CBILL" &&
                        x.chrg.Status != "CBILL" &&
                        x.cd.Type != "N/A" ||
                        string.IsNullOrEmpty(x.chrg.Invoice))
            .GroupBy(
                x => new
                {
                    claimData.claimAccount.AccountNo,
                    claimData.claimAccount.TransactionDate,
                    x.chrg.CDMCode,
                    x.chrg.RetailAmount,
                    x.cd.Cpt4,
                    x.cd.Type,
                    x.cd.Modifier,
                    x.cd.RevenueCode,
                    x.cd.Modifier2,
                    x.chrg.FinancialType
                })
            .Where(g => g.Sum(x => x.chrg.Quantity) != 0 &&
                        g.Sum(x => x.chrg.Quantity * x.cd.Amount) > 0 &&
                        g.Key.FinancialType == "M")
            .Select(g => new ClaimChargeView
            {
                AccountNo = g.Key.AccountNo,
                TransactionDate = g.Key.TransactionDate,
                Qty = g.Sum(x => x.chrg.Quantity),
                RetailAmount = g.Key.RetailAmount,
                Amount = g.Sum(x => x.cd.Amount * x.chrg.Quantity),
                ChargeId = g.Key.CDMCode,
                CptCode = g.Key.Cpt4,
                Type = g.Key.Type,
                Modifier = string.IsNullOrEmpty(g.Key.Modifier) ? null : g.Key.Modifier,
                RevenueCode = g.Key.RevenueCode,
                Modifier2 = g.Key.Modifier2,
                DiagnosisCodePointer = claimData.claimAccount.ChrgDiagnosisPointers.Where(d => d.CdmCode == g.Key.CDMCode && d.CptCode == g.Key.Cpt4).Select(d => d.DiagnosisPointer).First().ToString(),
                Cdm = _dictionaryService.GetCdm(g.Key.CDMCode, uow),
                RevenueCodeDetail = _dictionaryService.GetRevenueCode(uow,g.Key.RevenueCode)
            }).ToList();


        switch (accountModel.Status)
        {
            case "UB":
                claimData.ClaimType = ClaimType.Institutional;
                break;
            case "SSIUB":
                claimData.ClaimType = ClaimType.Institutional;
                break;
            case "1500":
                claimData.ClaimType = ClaimType.Professional;
                break;
            case "SSI1500":
                claimData.ClaimType = ClaimType.Professional;
                break;
            default:
                break;
        }

        try
        {
            claimData.CreatedDate = DateTime.Now;
            claimData.TransactionTypeCode = "CH";
            claimData.TransactionSetPurpose = "00";  // 00 = original, 18 - reissue
            claimData.SubmitterId = _appEnvironment.ApplicationParameters.FederalTaxId;
            claimData.SubmitterName = _appEnvironment.ApplicationParameters.BillingEntityName;
            claimData.SubmitterContactName = _appEnvironment.ApplicationParameters.BillingContact;
            claimData.SubmitterContactEmail = _appEnvironment.ApplicationParameters.BillingPhone;
            claimData.SubmitterContactPhone = _appEnvironment.ApplicationParameters.BillingEmail;

            claimData.ReceiverOrgName = _appEnvironment.ApplicationParameters.BillingReceiverName;
            claimData.ReceiverId = _appEnvironment.ApplicationParameters.BillingReceiverId;
            claimData.ProviderTaxonomyCode = _appEnvironment.ApplicationParameters.ProviderTaxonomyCode; // "282N00000X";

            claimData.BillingProviderName = _appEnvironment.ApplicationParameters.BillingEntityName;
            claimData.BillingProviderAddress = _appEnvironment.ApplicationParameters.BillingEntityStreet;
            claimData.BillingProviderCity = _appEnvironment.ApplicationParameters.BillingEntityCity;
            claimData.BillingProviderState = _appEnvironment.ApplicationParameters.BillingEntityState;
            claimData.BillingProviderZipCode = _appEnvironment.ApplicationParameters.BillingEntityZip;
            claimData.BillingProviderCountry = _appEnvironment.ApplicationParameters.BillingEntityCounty;
            claimData.BillingProviderTaxId = _appEnvironment.ApplicationParameters.FederalTaxId;
            claimData.BillingProviderUPIN = String.Empty;
            claimData.BillingProviderNPI = _appEnvironment.ApplicationParameters.NPINumber;
            claimData.BillingProviderContactName = _appEnvironment.ApplicationParameters.BillingContact;
            claimData.BillingProviderContactPhone = _appEnvironment.ApplicationParameters.BillingPhone;
            claimData.BillingProviderContactEmail = _appEnvironment.ApplicationParameters.BillingEmail;

            if (claimData.claimAccount.InsurancePrimary.InsCompany.BillAsJmcgh)
            {
                claimData.BillingProviderName = _appEnvironment.ApplicationParameters.Company2Name;
                claimData.BillingProviderNPI = _appEnvironment.ApplicationParameters.WTHNPI;
            }

            claimData.PayToAddress = _appEnvironment.ApplicationParameters.RemitToAddress;
            claimData.PayToCity = _appEnvironment.ApplicationParameters.RemitToCity;
            claimData.PayToState = _appEnvironment.ApplicationParameters.RemitToState;
            claimData.PayToZipCode = _appEnvironment.ApplicationParameters.RemitToZip;
            claimData.PayToCountry = _appEnvironment.ApplicationParameters.RemitToCountry;

            //not needed currently
            //2010AC — PAY-TO PLAN NAME Loop Repeat: 1
            //Required when willing trading partners agree to use this implementation
            //for their subrogation payment requests.
            //1. This loop may only be used when BHT06 = 31.
            claimData.PayToPlanName = "";
            claimData.PayToPlanAddress = "";
            claimData.PayToPlanCity = "";
            claimData.PaytoPlanState = "";
            claimData.PaytoPlanZipCode = "";
            claimData.PayToPlanCountry = "";
            claimData.PayToPlanPrimaryIdentifier = "";
            claimData.PayToPlanSecondaryIdentifier = "";
            claimData.PayToPlanTaxId = "";

            //claim information
            claimData.ClaimIdentifier = account;
            claimData.TotalChargeAmount = claimData.claimAccount.TotalCharges;

            if (claimData.ClaimType == ClaimType.Professional)
            {
                claimData.FacilityCode = "81";
                claimData.FacilityCodeQualifier = "B";
                claimData.ClaimFrequency = "1";
            }
            if (claimData.ClaimType == ClaimType.Institutional)
            {
                claimData.FacilityCode = "14";
                claimData.FacilityCodeQualifier = "A";
                claimData.ClaimFrequency = "1";
                claimData.AdmissionSourceCode = claimData.claimAccount.Client.ClientType.AdmissionSourceCode;
            }
            claimData.ProviderSignatureIndicator = "Y"; //default to Yes
            claimData.ProviderAcceptAssignmentCode = "C";
            claimData.BenefitAssignmentCertificationIndicator = "Y";
            claimData.ReleaseOfInformationCode = "Y";
            claimData.PatientSignatureSourceCode = "";
            claimData.RelatedCausesCode1 = "";
            claimData.RelatedCausesCode2 = "";
            claimData.RelatedCausesCode3 = "";
            claimData.RelatedCausesStateCode = "";
            claimData.RelatedCausesCountryCode = "";
            claimData.SpecialProgramIndicator = "";
            claimData.DelayReasonCode = ""; // do we need to accommodate this potential?

            claimData.OnsetOfCurrentIllness = claimData.claimAccount.TransactionDate;
            claimData.InitialTreatmentDate = claimData.claimAccount.TransactionDate;
            claimData.DateOfAccident = null;
            claimData.StatementFromDate = claimData.claimAccount.TransactionDate;
            claimData.StatementThruDate = claimData.claimAccount.TransactionDate;

            claimData.PatientAmountPaid = claimData.claimAccount.TotalPayments;
            claimData.CliaNumber = _appEnvironment.ApplicationParameters.PrimaryCliaNo;

            claimData.AttendingProviderFirstName = claimData.claimAccount.Pat.Physician.FirstName;
            claimData.AttendingProviderLastName = claimData.claimAccount.Pat.Physician.LastName;
            claimData.AttendingProviderMiddleName = claimData.claimAccount.Pat.Physician.MiddleInitial;
            claimData.AttendingProviderSuffix = "";
            claimData.AttendingProviderNPI = claimData.claimAccount.Pat.Physician.NpiId;

            claimData.ReferringProviderLastName = claimData.claimAccount.Pat.Physician.LastName;
            claimData.ReferringProviderFirstName = claimData.claimAccount.Pat.Physician.FirstName;
            claimData.ReferringProviderMiddleName = claimData.claimAccount.Pat.Physician.MiddleInitial;
            claimData.ReferringProviderSuffix = "";
            claimData.ReferringProviderNPI = claimData.claimAccount.Pat.Physician.NpiId;

            foreach (Ins ins in claimData.claimAccount.Insurances)
            {
                if (ins.IsDeleted)
                    continue;

                ClaimSubscriber subscriber = new()
                {
                    PayerResponsibilitySequenceCode = ins.Coverage switch
                    {
                        "A" => "P",
                        "B" => "S",
                        "C" => "T",
                        _ => throw new InvalidParameterValueException($"Invalid Ins Coverage Code {ins.Coverage}", "Ins.Coverage"),
                    }
                };

                Dictionary<string, string> relationConversion = new()
                {
                    {"01", "18"},
                    {"02", "01"},
                    {"03", "19"},
                    {"04", "G8"},
                    {"09", "21"}
                };

                if (string.IsNullOrEmpty(subscriber.PayerResponsibilitySequenceCode))
                {

                }

                if (subscriber.PayerResponsibilitySequenceCode == "P")
                {
                    subscriber.IndividualRelationshipCode = ins.Relation == "01" ? "18" : String.Empty;
                }
                else
                {
                    subscriber.IndividualRelationshipCode = relationConversion[ins.Relation];
                }

                subscriber.ReferenceIdentification = ins.GroupNumber;
                subscriber.PlanName = string.IsNullOrEmpty(ins.GroupName) ? ins.PlanName : ins.GroupName;

                subscriber.LastName = ins.HolderLastName;
                subscriber.FirstName = ins.HolderFirstName;
                subscriber.MiddleName = ins.HolderMiddleName;
                subscriber.NameSuffix = String.Empty;
                subscriber.NamePrefix = String.Empty;
                subscriber.PrimaryIdentifier = ins.PolicyNumber;
                subscriber.SocSecNumber = ins.CertSSN;
                if (ins.Relation == "01")
                {
                    subscriber.Gender = ins.HolderSex.Coalesce(claimData.claimAccount.Sex);
                    subscriber.DateOfBirth = ins.HolderBirthDate ?? claimData.claimAccount.BirthDate;
                    subscriber.Address = ins.HolderStreetAddress.Coalesce(claimData.claimAccount.Pat.Address1);
                    subscriber.Address2 = string.Empty;
                    subscriber.City = ins.HolderCity.Coalesce(claimData.claimAccount.Pat.City);
                    subscriber.State = ins.HolderState.Coalesce(claimData.claimAccount.Pat.State);
                    subscriber.ZipCode = ins.HolderZip.Coalesce(claimData.claimAccount.Pat.ZipCode);
                    subscriber.Country = "US";
                }
                else
                {
                    subscriber.Gender = ins.HolderSex;
                    subscriber.DateOfBirth = ins.HolderBirthDate;
                    subscriber.Address = ins.HolderStreetAddress;
                    subscriber.Address2 = string.Empty;
                    subscriber.City = ins.HolderCity;
                    subscriber.State = ins.HolderState;
                    subscriber.ZipCode = ins.HolderZip;
                    subscriber.Country = "US";
                }

                if (ins.InsCompany.IsGenericPayor)
                {
                    subscriber.PayerName = ins.PlanName;
                    subscriber.PayerAddress = ins.PlanStreetAddress1;
                    subscriber.PayerAddress2 = ins.PlanStreetAddress2;
                    subscriber.PayerCity = ins.PlanCity;
                    subscriber.PayerState = ins.PlanState;
                    subscriber.PayerZipCode = ins.PlanZip;
                    subscriber.PayerCountry = "US";

                }
                else
                {
                    subscriber.PayerName = ins.PlanName; // ins.InsCompany.name;
                    subscriber.PayerAddress = ins.InsCompany.Address1;
                    subscriber.PayerAddress2 = ins.InsCompany.Address2;
                    subscriber.PayerCity = ins.InsCompany.City;
                    subscriber.PayerState = ins.InsCompany.State;
                    subscriber.PayerZipCode = ins.InsCompany.Zip;
                    subscriber.PayerCountry = "US";
                }
                string strPayer = ins.InsCompany.NThrivePayerNo ?? "UKNOWN";

                if (strPayer == string.Empty)
                {
                    strPayer = "UKNOWN";
                }
                subscriber.PayerIdentifier = strPayer;
                subscriber.InsuranceTypeCode = "";
                subscriber.CoordinationOfBenefitsCode = "";
                subscriber.ConditionResponseCode = "";
                subscriber.EmployementStatusCode = "";

                subscriber.ClaimFilingIndicatorCode = ins.InsCompany.ClaimFilingIndicatorCode;

                subscriber.PayerIdentificationQualifier = ins.InsCompany.ProviderNoQualifer;
                subscriber.BillingProviderSecondaryIdentifier = ins.InsCompany.ProviderNo;

                claimData.Subscribers.Add(subscriber);

            }

            foreach (ClaimChargeView claimCharge in claimData.claimAccount.ClaimCharges)
            {
                ClaimLine claimLine = new()
                {
                    ProcedureCode = claimCharge.CptCode,
                    ProcedureModifier1 = claimCharge.Modifier,
                    ProcedureModifier2 = claimCharge.Modifier2,
                    ProcedureModifier3 = "",
                    Description = claimCharge.Cdm.Description,
                    RevenueCodeDescription = claimCharge.RevenueCodeDetail.Description,
                    Amount = claimCharge.Amount,
                    Quantity = claimCharge.Qty
                };
                string[] dxptr = claimCharge.DiagnosisCodePointer.Split(':');
                if (dxptr.Length >= 1)
                    claimLine.DxPtr1 = dxptr[0] ?? "";
                if (dxptr.Length >= 2)
                    claimLine.DxPtr2 = dxptr[1] ?? "";
                if (dxptr.Length >= 3)
                    claimLine.DxPtr3 = dxptr[2] ?? "";
                if (dxptr.Length >= 4)
                    claimLine.DxPtr4 = dxptr[3] ?? "";
                claimLine.EPSDTIndicator = "";
                claimLine.FamilyPlanningIndicator = "";
                claimLine.ServiceDate = claimCharge.TransactionDate;
                claimLine.ControlNumber = $"{claimCharge.ChargeId}-{claimCharge.CptCode}";
                if (!string.IsNullOrWhiteSpace(claimCharge.Modifier))
                    claimLine.ControlNumber += $"-{claimCharge.Modifier}";
                claimLine.RevenueCode = claimCharge.RevenueCode;

                claimData.ClaimLines.Add(claimLine);
            }

            _accountService.ClearAccountLock(accountModel);
            uow.Commit();
        }
        catch (InvalidParameterValueException ipve)
        {
            Log.Instance.Fatal(ipve, $"{account}");
            _accountService.ClearAccountLock(accountModel);
            throw new InvalidParameterValueException($"Parameter value not found - account {account}", ipve);
        }
        catch (Exception exc)
        {
            Log.Instance.Fatal(exc, $"{account}");
            _accountService.ClearAccountLock(accountModel);
            throw new ApplicationException($"Exception creating claim for {account}", exc);
        }

        return claimData;
    }

    public bool ClearBatch(double batch, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        uow.StartTransaction();

        var data = uow.BillingBatchRepository.GetBatch(batch);

        try
        {
            foreach (var detail in data.BillingActivities)
            {
                var account = uow.AccountRepository.GetByAccount(detail.AccountNo);
                if (account != null)
                {

                    _accountService.ClearClaimStatus(account);
                }
                // dbh - delete history record
                uow.BillingActivityRepository.Delete(detail);
            }
            // batch - delete batch
            uow.BillingBatchRepository.Delete(data);

            uow.Commit();
            return true;
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
            return false;
        }

    }

    public List<BillingBatch> GetBillingBatches(IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.BillingBatchRepository.GetAll();
    }

    public List<BillingActivity> GetBillingBatchActivity(string batch, IUnitOfWork uow = null)
    {
        uow ??= new UnitOfWorkMain(_appEnvironment);
        return uow.BillingActivityRepository.GetBatch(batch);
    }

    readonly Dictionary<string, string> ClaimFilingIndicatorCode = new Dictionary<string, string>()
    {
        { "1B", "BL"},
        { "1H", "CH"},
        { "HM", "HM"},
        { "1D", "MC"}
    };

}
