using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LabBilling.Core.Models
{
    public sealed class ApplicationParameters
    {

        [Category(""), Description("")] 
        public System.String CollectionsFileLocation { get; set; }
        [Category(""), Description("")] 
        public System.String CollectionsSftpPassword { get; set; }
        [Category(""), Description(""), DefaultValue("")] 
        public System.String CollectionsSftpServer { get; set; }
        [Category(""), Description("")] 
        public System.String CollectionsSftpUsername { get; set; }
        [Category(""), Description("")] 
        public System.String StatementsFileLocation { get; set; }
        [Category(""), Description("")] 
        public System.String StatementsSftpPassword { get; set; }
        [Category(""), Description("")] 
        public System.String StatementsSftpServer { get; set; }
        [Category(""), Description("")] 
        public System.String StatementsSftpUsername { get; set; }


        [Category("Accounting"), Description("Bank account number")] 
        public System.String BankAccountNumber { get; set; }
        [Category("Accounting"), Description("Name of bank for deposits")] 
        public System.String BankName { get; set; }
        [Category("Accounting"), Description("Bank Routing number")] 
        public System.String BankRoutingNumber { get; set; }


        [Category("Billing"), Description("SET AUTHORIZED CBILL USERS")] 
        public System.String AuthorizedToRunClientBills { get; set; }
        [Category("Billing"), Description("")] 
        public System.String BHGroupNo { get; set; }
        [Category("Billing"), Description("")] 
        public System.String BHProviderNo { get; set; }
        [Category("Billing"), Description("Tax Id of Entity receiving 837 claims")] 
        public System.String BillingReceiverId { get; set; }
        [Category("Billing"), Description("Entity receiving 837 claims")] 
        public System.String BillingReceiverName { get; set; }
        [Category("Billing"), Description("")] 
        public System.String BlueCrossProvNo { get; set; }
        [Category("Billing"), Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")] 
        public System.String BlueCrossReceiverNo { get; set; }
        [Category("Billing"), Description("")] 
        public System.String BundleInsurance { get; set; }
        [Category("Billing"), Description("")] 
        public System.String ChampusGroupNo { get; set; }
        [Category("Billing"), Description("")] 
        public System.String CNetReceiverId { get; set; }
        [Category("Billing"), Description("CODE STATS ")] 
        public System.String CodeStats { get; set; }
        [Category("Billing"), Description("Sets the date to run bad debt")] 
        public System.DateTime CollectionsRun { get; set; }
        [Category("Billing"), Description("")] 
        public System.String DBMailProfile { get; set; }
        [Category("Billing"), Description("use this to turn off or on the remaining validations that are hard coded in the application")] 
        public System.Boolean Diagnosis { get; set; }
        [Category("Billing"), Description("ENABLE INSURANCE DIAGNOSIS POINTERS"), DefaultValue(true)] 
        public System.String DiagnosisCodePointerSelect { get; set; }
        [Category("Billing"), Description("Turn off the Viewer for Atlanta once electronic is fixed")] 
        public System.DateTime ElectronicBillDate { get; set; }
        [Category("Billing"), Description("changed eob address")] 
        public System.String EobAddress { get; set; }
        [Category("Billing"), Description("")] 
        public System.DateTime GlobalBillingStartDate { get; set; }
        [Category("Billing"), Description("Location to place generated 837i files")] 
        public System.String InstitutionalClaimFileLocation { get; set; }
        [Category("Billing"), Description("Max number of claims in a claim batch. Set to 0 for unlimited."), DefaultValue(0)] 
        public System.Int32 MaxClaimsInClaimBatch { get; set; }
        [Category("Billing"), Description("")] 
        public System.String MedicaidProviderId { get; set; }
        [Category("Billing"), Description("")] 
        public System.String MedicareBProviderNo { get; set; }
        [Category("Billing"), Description("Added to facilitate CAHABA medicare billing")] 
        public System.String MedicareElectronicReceiverId { get; set; }
        [Category("Billing"), Description("")] 
        public System.String MedicareProviderNo { get; set; }
        [Category("Billing"), Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")] 
        public System.String MedicareReceiverId { get; set; }
        [Category("Billing"), Description("Added to facilitate CAHABA medicare billing")] 
        public System.String MedicareSubmitterId { get; set; }
        [Category("Billing"), Description("SET NURSING HOME BILL THRU DATE")] 
        public System.String NursingHomeBillThruDate { get; set; }
        [Category("Billing"), Description("Added for use if outpatient becomes valid.")] 
        public System.DateTime OutpatientBillStart { get; set; }
        [Category("Billing"), Description("Location to place 837p claim files.")] 
        public System.String ProfessionalClaimFileLocation { get; set; }
        [Category("Billing"), Description("new form to be used")] 
        public System.DateTime ProfessionalClaimStartDate { get; set; }
        [Category("Billing"), Description("location of hospitals remittance files")] 
        public System.String RemitImportDirectory { get; set; }
        [Category("Billing"), Description("date files last imported")] 
        public System.DateTime RemitPostingDate { get; set; }
        [Category("Billing"), Description("location of local remittance files")] 
        public System.String RemitProcessingDirectory { get; set; }
        [Category("Billing"), Description("Discontinue the swapping of insurance's and allow secondary billing via applications")] 
        public System.DateTime SecondaryBilling { get; set; }
        [Category("Billing"), Description("")] 
        public System.Double SmallBalanceAmount { get; set; }
        [Category("Billing"), Description("Added for use in ViewerAcc")] 
        public System.DateTime SSIBillThruDate { get; set; }
        [Category("Billing"), Description("Used by ViewerAcc to seperate the new claim methodolgy from the old method.")] 
        public System.DateTime SSIStartDate { get; set; }
        [Category("Billing"), Description("")] 
        public System.String TLCProviderId { get; set; }
        [Category("Billing"), Description("")] 
        public System.String UHCProviderId { get; set; }
        [Category("Billing"), Description("Set to 1 when we start using for stored procedures to skip making new accounts")] 
        public System.Int32 UseBillMethod { get; set; }


        [Category("Charging"), Description("ENABLE FEE SCHEULES")] 
        public System.String FeeSchedules { get; set; }
        [Category("Charging"), Description("")] 
        public System.String GeneralHealthPanelInsurances { get; set; }
        [Category("Charging"), Description("")] 
        public System.String GeneralHealthPanelTests { get; set; }
        [Category("Charging"), Description("")] 
        public System.String OBPanelInsurances { get; set; }
        [Category("Charging"), Description("")] 
        public System.String OBPanelTests { get; set; }
        [Category("Charging"), Description("SET QUEST EMAIL RECEIPENTS")] 
        public System.String QuestBillingEmailRecipients { get; set; }


        [Category("Company"), Description("")] 
        public System.String BillingContact { get; set; }
        [Category("Company"), Description("")]
        public System.String BillingEntityCounty { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEmail { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityCity { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityFax { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityFedTaxId { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityName { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityPhone { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityState { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityStreet { get; set; }
        [Category("Company"), Description("")] 
        public System.String BillingEntityZip { get; set; }
        [Category("Company"), Description("20090102 wdk Submitter PER requires this number without punctuation of any kind")] 
        public System.String BillingPhone { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Address { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2City { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2CityStateZip { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Contact { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Fax { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Name { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Phone { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2State { get; set; }
        [Category("Company"), Description("")] 
        public System.String Company2Zip { get; set; }
        [Category("Company"), Description("20120112 changed from pob per carol for claimsnet")] 
        public System.String CompanyAddress { get; set; }
        [Category("Company"), Description("")] 
        public System.String CompanyCity { get; set; }
        [Category("Company"), Description("20120112 zip changed for claimsnet per carol")] 
        public System.String CompanyCityStateZip { get; set; }
        [Category("Company"), Description("")] 
        public System.String CompanyContact { get; set; }
        [Category("Company"), Description("")] 
        public System.String CompanyFax { get; set; }
        [Category("Company"), Description("")] 
        public System.String CompanyName { get; set; }
        [Category("Company"), Description("20090102 wdk should this ever need to go into an electornic 837 file check the format to see if the non numberic data is allowed")] 
        public System.String CompanyPhone { get; set; }
        [Category("Company"), Description("")] 
        public System.String CompanyState { get; set; }
        [Category("Company"), Description("20110112 changed zip and added plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 2010AA N4-03")] 
        public System.String CompanyZip { get; set; }
        [Category("Company"), Description("09/26/2008 wdk facility JMCGH address change request and approved by Carol")] 
        public System.String FacilityAddress { get; set; }
        [Category("Company"), Description("")] 
        public System.String FacilityCity { get; set; }
        [Category("Company"), Description("")] 
        public System.String FacilityName { get; set; }
        [Category("Company"), Description("")] 
        public System.String FacilityState { get; set; }
        [Category("Company"), Description("20120112 changed to 5010 format added the plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 20")] 
        public System.String FacilityZip { get; set; }
        [Category("Company"), Description("")] 
        public System.String FederalTaxId { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyAddress { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyCity { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyName { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyPhone { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyState { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceCompanyZipCode { get; set; }
        [Category("Company"), Description("")] 
        public System.String InvoiceLogoImagePath { get; set; }
        [Category("Company"), Description("")] 
        public System.String NPINumber { get; set; }
        [Category("Company"), Description("")] 
        public System.String PrimaryCliaNo { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToAddress { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToCity { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToCountry { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToOrganizationName { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToState { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToStreet { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToStreet2 { get; set; }
        [Category("Company"), Description("")] 
        public System.String RemitToZip { get; set; }
        [Category("Company"), Description("")] 
        public System.String WTHNPI { get; set; }


        [Category("DocumentationSite"), Description("Account Charge Entry Documentation")] 
        public System.String AccountChargeEntryUrl { get; set; }
        [Category("DocumentationSite"), Description("Account Management Documentation")] 
        public System.String AccountManagementUrl { get; set; }
        [Category("DocumentationSite"), Description("Batch Remittance Documentation")] 
        public System.String BatchRemittanceUrl { get; set; }
        [Category("DocumentationSite"), Description("Charge Master Maintenance Documentation")] 
        public System.String ChargeMasterMaintenanceUrl { get; set; }
        [Category("DocumentationSite"), Description("Claims Management Documentation ")] 
        public System.String ClaimsManagementUrl { get; set; }
        [Category("DocumentationSite"), Description("Client Invoicing Documentation")] 
        public System.String ClientInvoicingUrl { get; set; }
        [Category("DocumentationSite"), Description("Client Maintenance Documentation")] 
        public System.String ClientMaintenanceUrl { get; set; }
        [Category("DocumentationSite"), Description("Root URL for documentation site")] 
        public System.String DocumentationSiteUrl { get; set; }
        [Category("DocumentationSite"), Description("Insurance Plan Maintenance Documentation")] 
        public System.String InsurancePlanMaintenanceUrl { get; set; }
        [Category("DocumentationSite"), Description("Latest Updates Documentation")] 
        public System.String LatestUpdatesUrl { get; set; }
        [Category("DocumentationSite"), Description("Patient Collections Documentation")] 
        public System.String PatientCollectionsUrl { get; set; }
        [Category("DocumentationSite"), Description("Patient Statements Documentation")] 
        public System.String PatientStatementsUrl { get; set; }
        [Category("DocumentationSite"), Description("Physician Maintenance Documentation")] 
        public System.String PhysicianMaintenanceUrl { get; set; }
        [Category("DocumentationSite"), Description("Worklist Documentation")] 
        public System.String WorklistUrl { get; set; }


        [Category("Environment"), Description("")] 
        public System.String Default1500Printer { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultClientRequisitionPrinter { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultCytologyRequisitionPrinter { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultDetailBillPrinter { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultFilePath { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultMedicareBillPath { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultNursingHomePrinter { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultPathologyReqPrinter { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultSpoolFileDrive { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultSpoolFilePath { get; set; }
        [Category("Environment"), Description("")] 
        public System.String DefaultUBPrinter { get; set; }


        [Category("Invoicing"), Description("SET CBILL FILTER")] 
        public System.String ClientBillFilter { get; set; }
        [Category("Invoicing"), Description("Network location where client invoice pdfs are stored")] 
        public System.String InvoiceFileLocation { get; set; }


        [Category("Operations"), Description("use to move the day back in an hourly query")] 
        public System.String DemoDays { get; set; }
        [Category("Operations"), Description("")] 
        public System.Int32 FileMaintenanceAdtMessages { get; set; }
        [Category("Operations"), Description("")] 
        public System.Int32 FileMaintenanceMessagesInbound { get; set; }
        [Category("Operations"), Description("wdk removed 20100709--86003 Added for rolling up multiline forms that exceed 36 for 1500's. Add all cpt4's that need to be rolled up seperated by the")] 
        public System.String RollupCpt4s { get; set; }


        [Category("Other"), Description("")] 
        public System.String ChargeTotalApp { get; set; }


        [Category("System"), Description("")] 
        public System.String AccessMedPlusProvNo { get; set; }
        [Category("System"), Description(""), DefaultValue(true)] 
        public System.Boolean AllowChargeEntry { get; set; }
        [Category("System"), Description(""), DefaultValue(true)] 
        public System.Boolean AllowEditing { get; set; }
        [Category("System"), Description(""), DefaultValue(true)] 
        public System.Boolean AllowPaymentAdjustmentEntry { get; set; }
        [Category("System"), Description("SET SELECT/MOVE OF ACCOUNTS ")] 
        public System.String AllowSelMove { get; set; }
        [Category("System"), Description("")] 
        public System.String ArchiveDB { get; set; }
        [Category("System"), Description("Specifies production or non-production usage")] 
        public System.String DatabaseEnvironment { get; set; }
        [Category("System"), Description("")] 
        public System.String ICDVersion { get; set; }
        [Category("System"), Description("used for reports")] 
        public System.String LabDirector { get; set; }
        [Category("System"), Description(""), DefaultValue(false)] 
        public System.Boolean ProcessPCCharges { get; set; }
        [Category("System"), Description("")] 
        public System.String ReportingPortalUrl { get; set; }
        [Category("System"), Description("")] 
        public System.String SystemVersion { get; set; }
        [Category("System"), Description(""), DefaultValue(4)] 
        public System.Int32 TabsOpenLimit { get; set; }
        [Category("System"), Description("")]
        public System.String ServiceUser { get; set; }
        [Category("System"), Description("")]
        public System.String ServiceUserPassword { get; set; }

        [Category("ViewerSlides"), Description("")] 
        public System.String IHCStainsQuery { get; set; }
        [Category("ViewerSlides"), Description("")] 
        public System.String SlidesQuery { get; set; }
        [Category("ViewerSlides"), Description("New slide application billing changes")] 
        public System.DateTime SlidesStartDate { get; set; }
        [Category("ViewerSlides"), Description("used to set access to the special grid in the application")] 
        public System.String SpecialClientsQuery { get; set; }
        [Category("ViewerSlides"), Description("")] 
        public System.String SpecialStainsQuery { get; set; }

        public static ApplicationParameters Load(string xml)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ApplicationParameters));
            using (StringReader xmlReader = new StringReader(xml))
            {
                var appParm = (ApplicationParameters)xmlSerializer.Deserialize(xmlReader);
                return appParm;
            }
        }

        public string GetDescription(string propertyName)
        {
            var prop = typeof(ApplicationParameters).GetProperty(propertyName);
            var descriptionInfo = prop.GetCustomAttribute<DescriptionAttribute>();
            var description = descriptionInfo.Description;

            return description;
        }

        public string GetCategory(string propertyName)
        {
            var prop = typeof(ApplicationParameters).GetProperty(propertyName);
            var attributeInfo = prop.GetCustomAttribute<CategoryAttribute>();
            var attributeValue = attributeInfo.Category;

            return attributeValue;
        }

        public object GetDefaultValue(string propertyName)
        {
            var prop = typeof(ApplicationParameters).GetProperty(propertyName);
            var attributeInfo = prop.GetCustomAttribute<DefaultValueAttribute>();
            var attributeValue = attributeInfo.Value;

            return attributeValue;
        }
        public string GetProductionEnvironment()
        {
            //string env = GetByKey("dbenvironment");
            string env = this.DatabaseEnvironment;
            if (env == "Production")
                return "P";
            else
                return "T";
        }
    }

}
