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
        private BillingHistoryRepository billingHistoryRepository;
        private BillingBatchRepository billingBatchRepository;

        public ClaimGenerator(string connectionString)
        {
            _connectionString = connectionString;

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = connectionString;

            dBserverName = (string)dbConnectionStringBuilder["Server"];
            dBName = (string)dbConnectionStringBuilder["Database"];

            db = new Database(connectionString, new CustomSqlDatabaseProvider());

            parametersdb = new SystemParametersRepository(db);

            propProductionEnvironment = dBName.Contains("LIVE") ? "P" : "T";
            string[] strArgs = new string[3];
            strArgs[0] = dBName.Contains("LIVE") ? "/LIVE" : "/TEST";
            strArgs[1] = dBserverName;
            strArgs[2] = dBName;

            accountRepository = new AccountRepository(db);
            patRepository = new PatRepository(db);
            chrgRepository = new ChrgRepository(db);
            chkRepository = new ChkRepository(db);
            numberRepository = new NumberRepository(db);
            billingHistoryRepository = new BillingHistoryRepository(db);
            billingBatchRepository = new BillingBatchRepository(db);

            claims = new List<ClaimData>();

        }

        /// <summary>
        /// Compiles claims for professional (837p) billing.
        /// </summary>
        /// <returns>Number of claims generated. Returns -1 if there was an error.</returns>
        public int CompileProfessionalBilling(IProgress<ProgressReportModel> progress)
        {
            ProgressReportModel report = new ProgressReportModel();

            //compile list of accounts to have claims generated
            billing837 = new Billing837(_connectionString);
            string batchSubmitterID = parametersdb.GetByKey("fed_tax_id");
            decimal strNum = numberRepository.GetNumber("ssi_batch");
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));

            //acc records where status = "1500" and primary insurance is not "CHAMPUS"
            var list = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Professional);
            ClaimData claim;

            db.BeginTransaction();
            try
            {
                report.TotalRecords = list.Count();
                foreach (ClaimItem item in list)
                {
                    //validate account data before starting - if there are errors do not process claim.
                    // primary ins holder name is empty
                    // no dx codes

                    claim = GenerateClaim(item.account);
                    claims.Add(claim);

                    //update status and activity date fields
                    claim.claimAccount.Status = "SSI1500";

                    accountRepository.Update(claim.claimAccount, new[] { nameof(Account.Status) });

                    claim.claimAccount.Pat.ProfessionalClaimDate = DateTime.Today;
                    claim.claimAccount.Pat.SSIBatch = interchangeControlNumber;

                    patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.ProfessionalClaimDate), nameof(Pat.SSIBatch) });

                    BillingHistory billingHistory = new BillingHistory();
                    billingHistory.PatientName = claim.claimAccount.PatFullName;
                    billingHistory.RunDate = DateTime.Today;
                    billingHistory.Account = claim.claimAccount.AccountNo;
                    billingHistory.Batch = Convert.ToDouble(interchangeControlNumber);
                    billingHistory.ElectronicBillBatch = Convert.ToDouble(interchangeControlNumber);
                    billingHistory.ElectronicBillStatus = "1500";
                    billingHistory.FinancialCode = claim.claimAccount.FinCode;
                    billingHistory.InsuranceOrder = claim.claimAccount.Insurances[0].Coverage;
                    billingHistory.InsuranceCode = claim.claimAccount.Insurances[0].InsCode;
                    billingHistory.InsComplete = DateTime.MinValue;
                    billingHistory.TransactionDate = claim.claimAccount.TransactionDate;
                    billingHistory.RunUser = OS.GetUserName();

                    billingHistoryRepository.Add(billingHistory);

                    report.RecordsProcessed++;
                    report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                    progress.Report(report);
                }

                string x12Text = billing837.Generate837ClaimBatch(claims, interchangeControlNumber, propProductionEnvironment, batchSubmitterID, parametersdb.GetByKey("claim_837p_file_loation"), Billing837.ClaimType.Professional);

                BillingBatch billingBatch = new BillingBatch();
                billingBatch.batch = Convert.ToDouble(interchangeControlNumber);
                billingBatch.run_date = DateTime.Today;
                billingBatch.run_user = OS.GetUserName();
                billingBatch.x12_text = x12Text;
                billingBatch.claim_count = claims.Count;

                billingBatchRepository.Add(billingBatch);

                db.CompleteTransaction();

                return claims.Count;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, "Exception processing Professional Claims. Batch has been rolled back. Report error to the Application Administrator.");
                db.AbortTransaction();
            }

            return -1;
        }

        public int CompileInstitutionalBilling(IProgress<ProgressReportModel> progress)
        {
            ProgressReportModel report = new ProgressReportModel();
            //compile list of accounts to have claims generated
            billing837 = new Billing837(_connectionString);
            string batchSubmitterID = parametersdb.GetByKey("fed_tax_id");
            decimal strNum = numberRepository.GetNumber("ssi_batch");
            string interchangeControlNumber = string.Format("{0:D9}", int.Parse(string.Format("{0}{1}", DateTime.Now.Year, strNum)));

            //acc records where status = "UB" and primary insurance is not "CHAMPUS"
            var list = accountRepository.GetAccountsForClaims(AccountRepository.ClaimType.Institutional);
            ClaimData claim;

            db.BeginTransaction();
            try
            {
                report.TotalRecords = list.Count();
                foreach (ClaimItem item in list)
                {
                    //validate account data before starting - if there are errors do not process claim.
                    // primary ins holder name is empty
                    // no dx codes
                    try
                    {
                        claim = GenerateClaim(item.account);
                        claims.Add(claim);
                    }
                    catch(ApplicationException apex)
                    {
                        Log.Instance.Error(apex);
                        continue;
                    }

                    //update status and activity date fields
                    claim.claimAccount.Status = "SSIUB";

                    accountRepository.Update(claim.claimAccount, new[] { nameof(Account.Status) });

                    claim.claimAccount.Pat.ProfessionalClaimDate = DateTime.Today;
                    claim.claimAccount.Pat.SSIBatch = interchangeControlNumber;

                    patRepository.Update(claim.claimAccount.Pat, new[] { nameof(Pat.InstitutionalClaimDate), nameof(Pat.SSIBatch) });

                    BillingHistory billingHistory = new BillingHistory();
                    billingHistory.PatientName = claim.claimAccount.PatFullName;
                    billingHistory.RunDate = DateTime.Today;
                    billingHistory.Account = claim.claimAccount.AccountNo;
                    billingHistory.Batch = Convert.ToDouble(interchangeControlNumber);
                    billingHistory.ElectronicBillBatch = Convert.ToDouble(interchangeControlNumber);
                    billingHistory.ElectronicBillStatus = "UB";
                    billingHistory.FinancialCode = claim.claimAccount.FinCode;
                    billingHistory.InsuranceOrder = claim.claimAccount.Insurances[0].Coverage;
                    billingHistory.InsuranceCode = claim.claimAccount.Insurances[0].InsCode;
                    billingHistory.InsComplete = DateTime.MinValue;
                    billingHistory.TransactionDate = claim.claimAccount.TransactionDate;
                    billingHistory.RunUser = OS.GetUserName();

                    billingHistoryRepository.Add(billingHistory);

                    report.RecordsProcessed++;
                    report.PercentageComplete = Convert.ToInt16((report.RecordsProcessed / report.TotalRecords) * 100);
                    progress.Report(report);
                }

                string x12Text = billing837.Generate837ClaimBatch(claims, interchangeControlNumber, propProductionEnvironment, batchSubmitterID, parametersdb.GetByKey("claim_837i_file_loation"), Billing837.ClaimType.Institutional);

                BillingBatch billingBatch = new BillingBatch();
                billingBatch.batch = Convert.ToDouble(interchangeControlNumber);
                billingBatch.run_date = DateTime.Today;
                billingBatch.run_user = OS.GetUserName();
                billingBatch.x12_text = x12Text;
                billingBatch.claim_count = claims.Count;

                billingBatchRepository.Add(billingBatch);

                db.CompleteTransaction();

                return claims.Count;
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex, "Exception processing Institutional Claims. Batch has been rolled back. Report error to the Application Administrator.");
                db.AbortTransaction();
            }

            return -1;
        }


        /// <summary>
        /// Generates the claim structure for a single account. Adds the entry to the ClaimsData list.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        /// <exception cref="InvalidParameterValueException"></exception>
        public ClaimData GenerateClaim(string account)
        {
            
            //do all the fun work of building the ClaimData object & return it.

            ClaimData claimData = new ClaimData
            {
                claimAccount = accountRepository.GetByAccount(account)

            };
            claimData.claimAccount.Charges = chrgRepository.GetByAccount(account, false).ToList();
            claimData.claimAccount.Payments = chkRepository.GetByAccount(account).ToList();

            try
            {
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
                claimData.BillingProviderNPI = parametersdb.GetByKey("wth_npi");
                claimData.BillingProviderContactName = parametersdb.GetByKey("billing_contact");
                claimData.BillingProviderContactPhone = parametersdb.GetByKey("billing_phone");
                claimData.BillingProviderContactEmail = parametersdb.GetByKey("billing_email");

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
                claimData.TotalChargeAmount = claimData.claimAccount.TotalCharges.ToString();
                claimData.FacilityCode = "14";
                claimData.ClaimFrequency = "1";
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

                claimData.PatientAmountPaid = claimData.claimAccount.TotalPayments;
                claimData.CliaNumber = parametersdb.GetByKey("primary_clia_no");

                claimData.ReferringProviderLastName = claimData.claimAccount.Pat.Physician.LastName;
                claimData.ReferringProviderFirstName = claimData.claimAccount.Pat.Physician.FirstName;
                claimData.ReferringProviderMiddleName = claimData.claimAccount.Pat.Physician.MiddleInitial;
                claimData.ReferringProviderSuffix = "";
                claimData.ReferringProviderNPI = claimData.claimAccount.Pat.Physician.NpiId;
                Dictionary<string, string> dicOP = new Dictionary<string, string>();
                dicOP.Add("MC", "440002");
                dicOP.Add("BC", "1000427");
                dicOP.Add("TNBC", "1000427");
                dicOP.Add("UHC", "626010402");
                foreach (Ins ins in claimData.claimAccount.Insurances)
                {
                    //        public IEnumerable<ClaimSubscriber> Subscribers { get; set; }
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
                    subscriber.IndividualRelationshipCode = ins.Relation == "01" ? "18" : String.Empty;
                    subscriber.ReferenceIdentification = ins.GroupNumber;
                    subscriber.PlanName = string.IsNullOrEmpty(ins.GroupName) ? ins.PlanName : ins.GroupName;
                   
                    subscriber.LastName = ins.HolderLastName;
                    subscriber.FirstName = ins.HolderFirstName;
                    subscriber.MiddleName = ins.HolderMiddleName;
                    subscriber.NameSuffix = String.Empty;
                    subscriber.NamePrefix = String.Empty;
                    subscriber.PrimaryIdentifier = ins.PolicyNumber;
                    subscriber.DateOfBirth = ins.HolderBirthDate;
                    subscriber.Gender = ins.HolderSex;
                    subscriber.SocSecNumber = ins.CertSSN;

                    subscriber.Address = ins.HolderAddress;
                    subscriber.Address2 = "";
                    subscriber.City = ins.HolderCity;
                    subscriber.State = ins.HolderState;
                    subscriber.ZipCode = ins.HolderZip;
                    subscriber.Country = "US";

                    subscriber.PayerName = ins.InsCompany.name;
                    subscriber.PayerAddress = ins.InsCompany.addr1;
                    subscriber.PayerAddress2 = ins.InsCompany.addr2;
                    subscriber.PayerCity = ins.InsCompany.City;
                    subscriber.PayerState = ins.InsCompany.State;
                    subscriber.PayerZipCode = ins.InsCompany.Zip;
                    subscriber.PayerCountry = "US";
                    string strPayer = ins.InsCompany.payer_no;
                    if(dicOP.ContainsKey(ins.InsCode))
                    {
                        if(!dicOP.TryGetValue(ins.InsCode, out strPayer))
                        {
                            strPayer = ins.InsCompany.payer_no;
                        }
                    }
                    if (strPayer == string.Empty)
                    {
                        strPayer = "UKNOWN";
                    }
                    subscriber.PayerIdentifier = strPayer;
                    subscriber.InsuranceTypeCode = "";
                    subscriber.CoordinationOfBenefitsCode = "";
                    subscriber.ConditionResponseCode = "";
                    subscriber.EmployementStatusCode = "";

                    string strClaimFilingIndicatorCode = null;

                    if (!string.IsNullOrEmpty(ins.InsCompany.provider_no_qualifier))
                    {
                        if (!ClaimFilingIndicatorCode.TryGetValue(ins.InsCompany.provider_no_qualifier, out strClaimFilingIndicatorCode))
                        {
                            strClaimFilingIndicatorCode = "CI";
                        }
                    }
                    else
                    {
                        strClaimFilingIndicatorCode = "CI";
                    }
                    subscriber.ClaimFilingIndicatorCode = strClaimFilingIndicatorCode;

                    subscriber.PayerIdentificationQualifier = ins.InsCompany.provider_no_qualifier;
                    subscriber.BillingProviderSecondaryIdentifier = ins.InsCompany.provider_no;

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
