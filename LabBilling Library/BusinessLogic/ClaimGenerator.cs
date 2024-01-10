using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using System.Collections;
using RFClassLibrary;
using System.Threading;

namespace LabBilling.Core.BusinessLogic
{
    public sealed class ClaimGenerator : DataAccess.Database
    {

        readonly ArrayList NameSuffixList = new() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };

        public string PropProductionEnvironment { get; set; }
        private string _connectionString;
        private IAppEnvironment _appEnvironment;

        private readonly AccountRepository accountRepository;
        private readonly PatRepository patRepository;
        private readonly ChrgRepository chrgRepository;
        private readonly ChkRepository chkRepository;
        private List<ClaimData> claims;
        private Billing837 billing837;
        private readonly NumberRepository numberRepository;
        private readonly BillingActivityRepository billingActivityRepository;
        private readonly BillingBatchRepository billingBatchRepository;

        public ClaimGenerator(IAppEnvironment appEnvironment) : base(appEnvironment.ConnectionString)
        {
            if(appEnvironment == null)
            {
                throw new ArgumentNullException(nameof(appEnvironment));
            }
            if(!appEnvironment.EnvironmentValid)
            {
                throw new ApplicationException("Application Environment not valid.");
            }

            _connectionString = appEnvironment.ConnectionString;
            _appEnvironment = appEnvironment;

            ConnectionString connString = _connectionString;
            _appEnvironment.ApplicationParameters = appEnvironment.ApplicationParameters;


            PropProductionEnvironment = appEnvironment.ApplicationParameters.GetProductionEnvironment();

            accountRepository = new AccountRepository(appEnvironment);
            patRepository = new PatRepository(appEnvironment);
            chrgRepository = new ChrgRepository(appEnvironment);
            chkRepository = new ChkRepository(appEnvironment);
            numberRepository = new NumberRepository(appEnvironment);
            billingActivityRepository = new BillingActivityRepository(appEnvironment);
            billingBatchRepository = new BillingBatchRepository(appEnvironment);

            claims = new List<ClaimData>();

        }

        public int CompileBillingBatch(ClaimType claimType, IProgress<ProgressReportModel> progress, CancellationToken cancellationToken)
        {
            claims = new List<ClaimData>();

            ProgressReportModel report = new ProgressReportModel();
            //compile list of accounts to have claims generated
            billing837 = new Billing837(PropProductionEnvironment);
            string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId;

            BillingBatch billingBatch = new()
            {
                RunDate = DateTime.Today,
                RunUser = OS.GetUserName(),
                ClaimCount = 0,
                TotalBilled = 0
            };

            var batch = billingBatchRepository.Add(billingBatch);

            double strNum = Convert.ToDouble(batch.ToString());

            string interchangeControlNumber = Convert.ToInt32(strNum).ToString("D9");
            string batchType;
            string fileLocation;
            int maxClaims = _appEnvironment.ApplicationParameters.MaxClaimsInClaimBatch;

            List<ClaimItem> claimList;
            Billing837.ClaimType billClaimType;
            string processedStatus = null;

            switch (claimType)
            {
                case ClaimType.Institutional:
                    claimList = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Institutional, maxClaims).ToList();
                    billClaimType = Billing837.ClaimType.Institutional;
                    processedStatus = AccountStatus.InstSubmitted; 
                    batchType = AccountStatus.Institutional; 
                    fileLocation = _appEnvironment.ApplicationParameters.InstitutionalClaimFileLocation;
                    break;
                case ClaimType.Professional:
                    claimList = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Professional, maxClaims).ToList();
                    billClaimType = Billing837.ClaimType.Professional;
                    processedStatus = AccountStatus.ProfSubmitted;
                    batchType = AccountStatus.Professional;
                    fileLocation = _appEnvironment.ApplicationParameters.ProfessionalClaimFileLocation;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ClaimType is not defined.");
            }

            ClaimData claim;

            dbConnection.BeginTransaction();
            try
            {
                report.TotalRecords = claimList.Count();

                foreach (ClaimItem item in claimList)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        throw new TaskCanceledException();
                    }

                    try
                    {                        
                        claim = GenerateClaim(item.account);
                        if(claim == null)
                        {
                            //validation failed - skip to next record
                            report.RecordsProcessed++;
                            report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                            progress.Report(report);
                            continue;
                        }
                        claims.Add(claim);
                    }
                    catch (ApplicationException apex)
                    {
                        Log.Instance.Error(apex);
                        continue;
                    }

                    //update status and activity date fields
                    claim.claimAccount.Status = processedStatus;

                    accountRepository.Update(claim.claimAccount, new[] { nameof(Account.Status) });

                    claim.claimAccount.Pat.SSIBatch = interchangeControlNumber;

                    switch (claimType)
                    {
                        case ClaimType.Institutional:
                            claim.claimAccount.Pat.InstitutionalClaimDate = DateTime.Today;
                            patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.InstitutionalClaimDate), nameof(Pat.SSIBatch) });
                            break;
                        case ClaimType.Professional:
                            claim.claimAccount.Pat.ProfessionalClaimDate = DateTime.Today;
                            patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.ProfessionalClaimDate), nameof(Pat.SSIBatch) });
                            break;
                        default:
                            break;
                    }


                    BillingActivity billingActivity = new BillingActivity();
                    billingActivity.PatientName = claim.claimAccount.PatFullName;
                    billingActivity.RunDate = DateTime.Today;
                    billingActivity.AccountNo = claim.claimAccount.AccountNo;
                    billingActivity.Batch = Convert.ToDouble(interchangeControlNumber);
                    billingActivity.ElectronicBillBatch = Convert.ToDouble(interchangeControlNumber);
                    billingActivity.ElectronicBillStatus = batchType;
                    billingActivity.FinancialCode = claim.claimAccount.FinCode;
                    billingActivity.InsuranceOrder = claim.claimAccount.Insurances[0].Coverage;
                    billingActivity.InsuranceCode = claim.claimAccount.Insurances[0].InsCode;
                    billingActivity.InsComplete = DateTime.MinValue;
                    billingActivity.TransactionDate = claim.claimAccount.TransactionDate;
                    billingActivity.Text = Newtonsoft.Json.JsonConvert.SerializeObject(claim);
                    billingActivity.RunUser = OS.GetUserName();

                    billingActivityRepository.Add(billingActivity);

                    report.RecordsProcessed++;
                    report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                    progress.Report(report);
                }

                string x12Text = string.Empty;
                if (claims.Count > 0)
                {

                    x12Text = billing837.Generate837ClaimBatch(claims, interchangeControlNumber,
                        batchSubmitterID, fileLocation, billClaimType);
                }

                billingBatch = billingBatchRepository.GetBatch(strNum);
                billingBatch.X12Text = x12Text;
                billingBatch.ClaimCount = claims.Count;
                billingBatch.TotalBilled = claims.Sum(x => x.TotalChargeAmount);
                billingBatch.BatchType = batchType;
                billingBatchRepository.Update(billingBatch);

                dbConnection.CompleteTransaction();

                return claims.Count;
            }
            catch (TaskCanceledException tcex)
            {
                Log.Instance.Fatal(tcex, "Batch processing cancelled by user. Batch has been rolled back.");
                dbConnection.AbortTransaction();
                throw new TaskCanceledException();
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, $"Exception processing {claimType} Claims. Batch has been rolled back. Report error to the Application Administrator.");
                dbConnection.AbortTransaction();
            }
            return -1;
        }

        public void RegenerateBatch(double batchNo)
        {
            billing837 = new Billing837(PropProductionEnvironment);
            var batch = billingBatchRepository.GetBatch(batchNo);
            string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId; 
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}", batch.Batch)));

            LabBilling.Core.Billing837.ClaimType claimType;

            string fileLocation;

            claims.Clear();
            switch (batch.BatchType)
            {
                case "UB":
                    claimType = (Billing837.ClaimType)ClaimType.Institutional;
                    fileLocation = _appEnvironment.ApplicationParameters.InstitutionalClaimFileLocation;
                    break;
                case "1500":
                    claimType = (Billing837.ClaimType)ClaimType.Professional;
                    fileLocation = _appEnvironment.ApplicationParameters.ProfessionalClaimFileLocation;
                    break;
                default:
                    //not a valid batch
                    return;
            }

            foreach(var account in batch.BillingActivities)
            {
                var claim = GenerateClaim(account.AccountNo, true);
                if(claim != null)
                    claims.Add(claim);
            }

            if (claims.Count > 0)
            {
                batch.X12Text = billing837.Generate837ClaimBatch(claims, interchangeControlNumber,
                    batchSubmitterID, fileLocation, claimType);

                batch.RunDate = DateTime.Now.Date;
                batch.TotalBilled = claims.Sum(x => x.TotalChargeAmount);

                billingBatchRepository.Update(batch);
            }
            return;
        }

        public void CompileClaim(string accountNo)
        {
            //validate account data before starting - if there are errors do not process claim.
            // primary ins holder name is empty
            // no dx codes
            ClaimData claim;
            string claimx12;

            string batchSubmitterID = _appEnvironment.ApplicationParameters.FederalTaxId;
            decimal strNum = numberRepository.GetNumber("ssi_batch");
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));

            try
            {
                claim = GenerateClaim(accountNo);
                if(claim == null)
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

            BillingActivity billingActivity = new BillingActivity();
            billingActivity.PatientName = claim.claimAccount.PatFullName;
            billingActivity.RunDate = DateTime.Today;
            billingActivity.AccountNo = claim.claimAccount.AccountNo;
            billingActivity.Batch = Convert.ToDouble(interchangeControlNumber);
            billingActivity.FinancialCode = claim.claimAccount.FinCode;
            billingActivity.InsuranceOrder = claim.claimAccount.Insurances[0].Coverage;
            billingActivity.InsuranceCode = claim.claimAccount.Insurances[0].InsCode;
            billingActivity.InsComplete = DateTime.MinValue;
            billingActivity.TransactionDate = claim.claimAccount.TransactionDate;
            billingActivity.ClaimAmount = claim.TotalChargeAmount;
            billingActivity.RunUser = OS.GetUserName();
            billingActivity.Text = claimx12;

            billingActivityRepository.Save(billingActivity);

        }

        /// <summary>
        /// Generates the claim structure for a single account. Adds the entry to the ClaimsData list.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterValueException"></exception>
        public ClaimData GenerateClaim(string account, bool reprint = false)
        {
            Account accountModel = accountRepository.GetByAccount(account);

            //there is no balance so nothing to send on claim
            if(accountModel.ClaimBalance <= 0.00)
            {
                return null;
            }

            if(!accountRepository.Validate(accountModel, reprint))
            {
                return null;
            }

            ClaimData claimData = new ClaimData
            {
                claimAccount = accountModel

            };
            claimData.claimAccount.ClaimCharges = chrgRepository.GetClaimCharges(account).ToList();            
            claimData.claimAccount.Payments = chkRepository.GetByAccount(account).ToList();

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
                claimData.ProviderTaxonomyCode = "282N00000X";

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
                if(claimData.ClaimType == ClaimType.Institutional)
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

                    ClaimSubscriber subscriber = new ClaimSubscriber();
                    switch(ins.Coverage)
                    {
                        case "A":
                            subscriber.PayerResponsibilitySequenceCode = "P";
                            break;
                        case "B":
                            subscriber.PayerResponsibilitySequenceCode = "S";
                            break;
                        case "C":
                            subscriber.PayerResponsibilitySequenceCode = "T";
                            break;
                        default:
                            throw new InvalidParameterValueException($"Invalid Ins Coverage Code {ins.Coverage}", "Ins.Coverage");
                    }

                    Dictionary<string, string> relationConversion = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        {"01", "18"},
                        {"02", "01"},
                        {"03", "19"},
                        {"04", "G8"},
                        {"09", "21"}
                    };

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

                    if(ins.InsCompany.IsGenericPayor)
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

                foreach(ClaimChargeView claimCharge in claimData.claimAccount.ClaimCharges)
                {
                    ClaimLine claimLine = new ClaimLine();
                    claimLine.ProcedureCode = claimCharge.CptCode;
                    claimLine.ProcedureModifier1 = claimCharge.Modifier;
                    claimLine.ProcedureModifier2 = claimCharge.Modifier2;
                    claimLine.ProcedureModifier3 = "";
                    claimLine.Description = claimCharge.Cdm.Description;
                    claimLine.RevenueCodeDescription = claimCharge.RevenueCodeDetail.Description;
                    claimLine.Amount = claimCharge.Amount;
                    claimLine.Quantity = claimCharge.Qty;
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
                    if(!string.IsNullOrWhiteSpace(claimCharge.Modifier)) 
                        claimLine.ControlNumber += $"-{claimCharge.Modifier}";
                    claimLine.RevenueCode = claimCharge.RevenueCode;

                    claimData.ClaimLines.Add(claimLine);
                }

            }
            catch (InvalidParameterValueException ipve)
            {
                Log.Instance.Fatal(ipve, $"{account}");
                throw new InvalidParameterValueException($"Parameter value not found - account {account}", ipve);
            }
            catch(Exception exc)
            {
                Log.Instance.Fatal(exc, $"{account}");
                throw new ApplicationException($"Exception creating claim for {account}", exc);
            }

            return claimData;
        }

        readonly Dictionary<string, string> ClaimFilingIndicatorCode = new Dictionary<string, string>()
        {
            { "1B", "BL"},
            { "1H", "CH"},
            { "HM", "HM"},
            { "1D", "MC"}
        };

    }
}
