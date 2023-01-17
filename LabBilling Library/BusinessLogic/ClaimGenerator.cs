using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using System.Data.Common;
using System.Collections;
using PetaPoco;
using PetaPoco.Providers;
using RFClassLibrary;
using System.Threading;

namespace LabBilling.Core.BusinessLogic
{
    public class ClaimGenerator
    {
        private SystemParametersRepository parametersdb;

        string dBserverName = null;
        string dBName = null;
        ArrayList m_alNameSuffix = new ArrayList() { "JR", "SR", "I", "II", "III", "IV", "V", "VI", "VII" };

        public string propProductionEnvironment { get; set; }
        private string _connectionString;

        private PetaPoco.Database db; 

        private AccountRepository accountRepository;
        private PatRepository patRepository;
        private ChrgRepository chrgRepository;
        private ChkRepository chkRepository;
        private List<ClaimData> claims;
        private Billing837 billing837;
        private NumberRepository numberRepository;
        private BillingActivityRepository billingActivityRepository;
        private BillingBatchRepository billingBatchRepository;

        public ClaimGenerator(string connectionString)
        {
            _connectionString = connectionString;

            ConnectionString connString = connectionString; 

            dBserverName = connString.ServerName; 
            dBName = connString.DatabaseName; 

            db = new Database(connectionString, new CustomSqlDatabaseProvider());

            parametersdb = new SystemParametersRepository(db);

            propProductionEnvironment = dBName.Contains("LIVE") ? "P" : "T";
            string env = parametersdb.GetProductionEnvironment();

            accountRepository = new AccountRepository(db);
            patRepository = new PatRepository(db);
            chrgRepository = new ChrgRepository(db);
            chkRepository = new ChkRepository(db);
            numberRepository = new NumberRepository(db);
            billingActivityRepository = new BillingActivityRepository(db);
            billingBatchRepository = new BillingBatchRepository(db);

            claims = new List<ClaimData>();

        }

        public int CompileBillingBatch(ClaimType claimType, IProgress<ProgressReportModel> progress, CancellationToken cancellationToken)
        {
            ProgressReportModel report = new ProgressReportModel();
            //compile list of accounts to have claims generated
            billing837 = new Billing837(_connectionString, propProductionEnvironment);
            string batchSubmitterID = parametersdb.GetByKey("fed_tax_id");
            decimal strNum = numberRepository.GetNumber("ssi_batch");
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));
            string batchType;
            string fileLocation;

            List<ClaimItem> claimList;
            Billing837.ClaimType billClaimType;
            string processedStatus = null;

            switch (claimType)
            {
                case ClaimType.Institutional:
                    claimList = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Institutional).ToList();
                    billClaimType = Billing837.ClaimType.Institutional;
                    processedStatus = "SSIUB";
                    batchType = "UB";
                    fileLocation = parametersdb.GetByKey("claim_837i_file_location");
                    break;
                case ClaimType.Professional:
                    claimList = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Professional).ToList();
                    billClaimType = Billing837.ClaimType.Professional;
                    processedStatus = "SSI1500";
                    batchType = "1500";
                    fileLocation = parametersdb.GetByKey("claim_837p_file_location");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ClaimType is not defined.");
            }

            ClaimData claim;

            db.BeginTransaction();
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


                string x12Text = billing837.Generate837ClaimBatch(claims, interchangeControlNumber,
                    batchSubmitterID, fileLocation, billClaimType);

                BillingBatch billingBatch = new BillingBatch();
                billingBatch.Batch = Convert.ToDouble(interchangeControlNumber);
                billingBatch.RunDate = DateTime.Today;
                billingBatch.RunUser = OS.GetUserName();
                billingBatch.X12Text = x12Text;
                billingBatch.ClaimCount = claims.Count;
                billingBatch.TotalBilled = claims.Sum(x => x.TotalChargeAmount);
                billingBatch.BatchType = batchType;

                billingBatchRepository.Add(billingBatch);

                db.CompleteTransaction();

                return claims.Count;
            }
            catch (TaskCanceledException tcex)
            {
                Log.Instance.Fatal(tcex, "Batch processing cancelled by user. Batch has been rolled back.");
                db.AbortTransaction();
                throw new TaskCanceledException();
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, "Exception processing Institutional Claims. Batch has been rolled back. Report error to the Application Administrator.");
                db.AbortTransaction();
            }
            return -1;
        }

        public void RegenerateBatch(double batchNo)
        {

            var batch = billingBatchRepository.GetBatch(batchNo);
            string batchSubmitterID = parametersdb.GetByKey("fed_tax_id");

            LabBilling.Core.Billing837.ClaimType claimType;

            string fileLocation;

            claims.Clear();
            switch (batch.BatchType)
            {
                case "UB":
                    claimType = (Billing837.ClaimType)ClaimType.Institutional;
                    fileLocation = parametersdb.GetByKey("claim_837i_file_location");
                    break;
                case "1500":
                    claimType = (Billing837.ClaimType)ClaimType.Professional;
                    fileLocation = parametersdb.GetByKey("claim_837p_file_location");
                    break;
                default:
                    //not a valid batch
                    return;
            }

            foreach(var account in batch.BillingActivities)
            {
                var claim = GenerateClaim(account.AccountNo);
                if(claim != null)
                    claims.Add(claim);
            }

            if (claims.Count > 0)
            {
                batch.X12Text = billing837.Generate837ClaimBatch(claims, batch.Batch.ToString(),
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

            //billing837 = new Billing837(_connectionString);
            string batchSubmitterID = parametersdb.GetByKey("fed_tax_id");
            decimal strNum = numberRepository.GetNumber("ssi_batch");
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));

            try
            {
                claim = GenerateClaim(accountNo);
                claim.InterchangeControlNumber = interchangeControlNumber;
                claim.BatchSubmitterId = batchSubmitterID;
                claimx12 = Newtonsoft.Json.JsonConvert.SerializeObject(claim);
                //claimx12 = billing837.Build837Claim(claim, interchangeControlNumber, propProductionEnvironment, batchSubmitterID);
            }
            catch (ApplicationException apex)
            {
                Log.Instance.Error(apex);
                return;
            }

            //update status and activity date fields
            //if (claim.ClaimType == ClaimType.Institutional)
            //{
            //    claim.claimAccount.Status = "SSIUB";
            //    claim.claimAccount.Pat.InstitutionalClaimDate = DateTime.Today;
            //    patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.InstitutionalClaimDate) });
            //}
            //if (claim.ClaimType == ClaimType.Professional)
            //{
            //    claim.claimAccount.Status = "SSI1500";
            //    claim.claimAccount.Pat.ProfessionalClaimDate = DateTime.Today;
            //    patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.ProfessionalClaimDate) });
            //}

            //accountRepository.Update(claim.claimAccount, new[] { nameof(Account.Status) });

            //claim.claimAccount.Pat.SSIBatch = interchangeControlNumber;

            BillingActivity billingActivity = new BillingActivity();
            billingActivity.PatientName = claim.claimAccount.PatFullName;
            billingActivity.RunDate = DateTime.Today;
            billingActivity.AccountNo = claim.claimAccount.AccountNo;
            billingActivity.Batch = Convert.ToDouble(interchangeControlNumber);
            //billingActivity.ElectronicBillBatch = Convert.ToDouble(interchangeControlNumber);
            //billingActivity.ElectronicBillStatus = "UB";
            billingActivity.FinancialCode = claim.claimAccount.FinCode;
            billingActivity.InsuranceOrder = claim.claimAccount.Insurances[0].Coverage;
            billingActivity.InsuranceCode = claim.claimAccount.Insurances[0].InsCode;
            billingActivity.InsComplete = DateTime.MinValue;
            billingActivity.TransactionDate = claim.claimAccount.TransactionDate;
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
        public ClaimData GenerateClaim(string account)
        {
            Account accountModel = accountRepository.GetByAccount(account);

            if(!accountRepository.Validate(ref accountModel))
            {
                return null;
            }

            ClaimData claimData = new ClaimData
            {
                claimAccount = accountModel

            };
            claimData.claimAccount.Charges = chrgRepository.GetByAccount(account, false).ToList();            
            claimData.claimAccount.Payments = chkRepository.GetByAccount(account).ToList();

            switch (accountModel.Status)
            {
                case "UB":
                    claimData.ClaimType = ClaimType.Institutional;
                    break;
                case "1500":
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
                claimData.SubmitterId = parametersdb.GetByKey("fed_tax_id");
                claimData.SubmitterName = parametersdb.GetByKey("billing_entity_name");
                claimData.SubmitterContactName = parametersdb.GetByKey("billing_contact");
                claimData.SubmitterContactEmail = parametersdb.GetByKey("billing_phone");
                claimData.SubmitterContactPhone = parametersdb.GetByKey("billing_email");

                claimData.ReceiverOrgName = parametersdb.GetByKey("billing_receiver_name");
                claimData.ReceiverId = parametersdb.GetByKey("billing_receiver_id");
                claimData.ProviderTaxonomyCode = "282N00000X";

                claimData.BillingProviderName = parametersdb.GetByKey("billing_entity_name");
                claimData.BillingProviderAddress = parametersdb.GetByKey("billing_entity_street");
                claimData.BillingProviderCity = parametersdb.GetByKey("billing_entity_city");
                claimData.BillingProviderState = parametersdb.GetByKey("billing_entity_state");
                claimData.BillingProviderZipCode = parametersdb.GetByKey("billing_entity_zip");
                claimData.BillingProviderCountry = parametersdb.GetByKey("billing_entity_country");
                claimData.BillingProviderTaxId = parametersdb.GetByKey("fed_tax_id");
                claimData.BillingProviderUPIN = String.Empty;
                claimData.BillingProviderNPI = parametersdb.GetByKey("npi_number");
                claimData.BillingProviderContactName = parametersdb.GetByKey("billing_contact");
                claimData.BillingProviderContactPhone = parametersdb.GetByKey("billing_phone");
                claimData.BillingProviderContactEmail = parametersdb.GetByKey("billing_email");

                if (claimData.claimAccount.InsurancePrimary.InsCompany.BillAsJmcgh)
                {
                    claimData.BillingProviderName = parametersdb.GetByKey("company2_name");
                    claimData.BillingProviderNPI = parametersdb.GetByKey("wth_npi");
                }

                claimData.PayToAddress = parametersdb.GetByKey("remit_to_address");
                claimData.PayToCity = parametersdb.GetByKey("remit_to_city");
                claimData.PayToState = parametersdb.GetByKey("remit_to_state");
                claimData.PayToZipCode = parametersdb.GetByKey("remit_to_zip");
                claimData.PayToCountry = parametersdb.GetByKey("remit_to_country");

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
                claimData.CliaNumber = parametersdb.GetByKey("primary_clia_no");

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
                    subscriber.PayerName = ins.PlanName; // ins.InsCompany.name;
                    subscriber.PayerAddress = ins.InsCompany.Address1;
                    subscriber.PayerAddress2 = ins.InsCompany.Address2;
                    subscriber.PayerCity = ins.InsCompany.City;
                    subscriber.PayerState = ins.InsCompany.State;
                    subscriber.PayerZipCode = ins.InsCompany.Zip;
                    subscriber.PayerCountry = "US";
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

                foreach (Chrg chrg in claimData.claimAccount.Charges)
                {
                    // public IEnumerable<ClaimLine> ClaimLines { get; set; }
                    foreach (ChrgDetail detail in chrg.ChrgDetails)
                    {
                        ClaimLine claimLine = new ClaimLine();
                        claimLine.ProcedureCode = detail.Cpt4;
                        claimLine.ProcedureModifier1 = detail.Modifier;
                        claimLine.ProcedureModifier2 = detail.Modifer2;
                        claimLine.ProcedureModifier3 = "";
                        claimLine.Description = chrg.Cdm.Description;
                        claimLine.RevenueCodeDescription = detail.RevenueCodeDetail.Description;
                        claimLine.Amount = detail.Amount;
                        claimLine.Quantity = chrg.Quantity;
                        string[] dxptr = detail.DiagCodePointer.Split(':');
                        if(dxptr.Length >= 1)
                            claimLine.DxPtr1 = dxptr[0] ?? "";
                        if (dxptr.Length >= 2) 
                            claimLine.DxPtr2 = dxptr[1] ?? "";
                        if (dxptr.Length >= 3) 
                            claimLine.DxPtr3 = dxptr[2] ?? "";
                        if (dxptr.Length >= 4) 
                            claimLine.DxPtr4 = dxptr[3] ?? "";
                        claimLine.EPSDTIndicator = "";
                        claimLine.FamilyPlanningIndicator = "";
                        claimLine.ServiceDate = chrg.ServiceDate;
                        claimLine.ControlNumber = chrg.CDMCode;
                        claimLine.RevenueCode = detail.RevenueCode;

                        claimData.ClaimLines.Add(claimLine);
                    }
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
