using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace LabBilling.Core.Models;

public sealed class ApplicationParameters
{
    private const string _collectionsCategory = "Collections";
    private const string _accountingCategory = "Accounting";
    private const string _billingCategory = "Billing";
    private const string _chargingCategory = "Charging";
    private const string _companyCategory = "Company";
    private const string _documentationSiteCategory = "DocumentationSite";
    private const string _environmentCategory = "Environment";
    private const string _invoicingCategory = "Invoicing";
    private const string _pathologyGroupCategory = "PathologyGroup";
    private const string _operationsCategory = "Operations";
    private const string _systemCategory = "System";
    private const string _otherCategory = "Other";
    private const string _viewerSlidesCategory = "ViewerSlides";

    #region Collections Category
    [Category(_collectionsCategory), Description("")]
    public System.String CollectionsFileLocation { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String CollectionsSftpPassword { get; set; }
    [Category(_collectionsCategory), Description(""), DefaultValue("")]
    public System.String CollectionsSftpServer { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String CollectionsSftpUsername { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String CollectionsSftpUploadPath { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String StatementsFileLocation { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String StatementsSftpPassword { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String StatementsSftpServer { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String StatementsSftpUsername { get; set; }
    [Category(_collectionsCategory), Description("")]
    public System.String StatementsSftpUploadPath { get; set; }
    [Category(_collectionsCategory), Description("Number of statements sent before sending account to collections.")]
    public Int32 NumberOfStatementsBeforeCollection { get; set; }
    #endregion Collections Category

    #region Accounting Category
    [Category(_accountingCategory), Description("Bank account number")]
    public System.String BankAccountNumber { get; set; }
    [Category(_accountingCategory), Description("Name of bank for deposits")]
    public System.String BankName { get; set; }
    [Category(_accountingCategory), Description("Bank Routing number")]
    public System.String BankRoutingNumber { get; set; }
    #endregion Accounting Category


    #region Billing Category
    [Category(_billingCategory), Description("Pipe-delimited list of usernames who are authorized to run client invoices.")]
    public System.String AuthorizedToRunClientBills { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String BHGroupNo { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String BHProviderNo { get; set; }
    [Category(_billingCategory), Description("No of days to hold claims for billing"), DefaultValue(10)]
    public System.Int32 BillingInitialHoldDays { get; set; }
    [Category(_billingCategory), Description("Tax Id of Entity receiving 837 claims")]
    public System.String BillingReceiverId { get; set; }
    [Category(_billingCategory), Description("Entity receiving 837 claims")]
    public System.String BillingReceiverName { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String BlueCrossProvNo { get; set; }
    [Category(_billingCategory), Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")]
    public System.String BlueCrossReceiverNo { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String BundleInsurance { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String ChampusGroupNo { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String CNetReceiverId { get; set; }
    [Category(_billingCategory), Description("CODE STATS")]
    public System.String CodeStats { get; set; }
    [Category(_billingCategory), Description("Sets the date to run bad debt")]
    public System.DateTime CollectionsRun { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String DBMailProfile { get; set; }
    [Category(_billingCategory), Description("use this to turn off or on the remaining validations that are hard coded in the application")]
    public System.Boolean Diagnosis { get; set; }
    [Category(_billingCategory), Description("ENABLE INSURANCE DIAGNOSIS POINTERS"), DefaultValue(true)]
    public System.String DiagnosisCodePointerSelect { get; set; }
    [Category(_billingCategory), Description("Turn off the Viewer for Atlanta once electronic is fixed")]
    public System.DateTime ElectronicBillDate { get; set; }
    [Category(_billingCategory), Description("changed eob address")]
    public System.String EobAddress { get; set; }
    [Category(_billingCategory), Description("")]
    public System.DateTime GlobalBillingStartDate { get; set; }
    [Category(_billingCategory), Description("Location to place generated 837i files")]
    public System.String InstitutionalClaimFileLocation { get; set; }
    [Category(_billingCategory), Description("Max number of claims in a claim batch. Set to 0 for unlimited."), DefaultValue(0)]
    public System.Int32 MaxClaimsInClaimBatch { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String MedicaidProviderId { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String MedicareBProviderNo { get; set; }
    [Category(_billingCategory), Description("Added to facilitate CAHABA medicare billing")]
    public System.String MedicareElectronicReceiverId { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String MedicareProviderNo { get; set; }
    [Category(_billingCategory), Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")]
    public System.String MedicareReceiverId { get; set; }
    [Category(_billingCategory), Description("Added to facilitate CAHABA medicare billing")]
    public System.String MedicareSubmitterId { get; set; }
    [Category(_billingCategory), Description("SET NURSING HOME BILL THRU DATE")]
    public System.String NursingHomeBillThruDate { get; set; }
    [Category(_billingCategory), Description("Added for use if outpatient becomes valid.")]
    public System.DateTime OutpatientBillStart { get; set; }
    [Category(_billingCategory), Description("Location to place 837p claim files.")]
    public System.String ProfessionalClaimFileLocation { get; set; }
    [Category(_billingCategory), Description("new form to be used")]
    public System.DateTime ProfessionalClaimStartDate { get; set; }
    [Category(_billingCategory), Description(""), DefaultValue("282N00000X")]
    public System.String ProviderTaxonomyCode { get; set; }
    [Category(_billingCategory), Description("location of hospitals remittance files")]
    public System.String RemitImportDirectory { get; set; }
    [Category(_billingCategory), Description("date files last imported")]
    public System.DateTime RemitPostingDate { get; set; }
    [Category(_billingCategory), Description("location of local remittance files")]
    public System.String RemitProcessingDirectory { get; set; }
    [Category(_billingCategory), Description("Discontinue the swapping of insurance's and allow secondary billing via applications")]
    public System.DateTime SecondaryBilling { get; set; }
    [Category(_billingCategory), Description("")]
    public System.Double SmallBalanceAmount { get; set; }
    [Category(_billingCategory), Description("Used by ViewerAcc to seperate the new claim methodolgy from the old method.")]
    public System.DateTime SSIStartDate { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String TLCProviderId { get; set; }
    [Category(_billingCategory), Description("")]
    public System.String UHCProviderId { get; set; }
    [Category(_billingCategory), Description("Set to 1 when we start using for stored procedures to skip making new accounts")]
    public System.Int32 UseBillMethod { get; set; }
    #endregion Billing Category

    #region Charing Category
    [Category(_chargingCategory), Description("Pipe delimited list of active fee schedules")]
    public System.String FeeSchedules { get; set; }
    [Category(_chargingCategory), Description("")]
    public System.String GeneralHealthPanelInsurances { get; set; }
    [Category(_chargingCategory), Description("")]
    public System.String GeneralHealthPanelTests { get; set; }
    [Category(_chargingCategory), Description("")]
    public System.String OBPanelInsurances { get; set; }
    [Category(_chargingCategory), Description("")]
    public System.String OBPanelTests { get; set; }
    [Category(_chargingCategory), Description("SET QUEST EMAIL RECEIPENTS")]
    public System.String QuestBillingEmailRecipients { get; set; }
    #endregion Charging Category

    #region Company Category
    [Category(_companyCategory), Description("")]
    public System.String BillingContact { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityCounty { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEmail { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityFax { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityFedTaxId { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityStreet { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingEntityZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String BillingPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Address { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2City { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2CityStateZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Contact { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Fax { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Name { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Phone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2State { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String Company2Zip { get; set; }
    [Category(_companyCategory), Description("20120112 changed from pob per carol for claimsnet")]
    public System.String CompanyAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyCity { get; set; }
    [Category(_companyCategory), Description("20120112 zip changed for claimsnet per carol")]
    public System.String CompanyCityStateZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyContact { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyFax { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyName { get; set; }
    [Category(_companyCategory), Description("20090102 wdk should this ever need to go into an electornic 837 file check the format to see if the non numberic data is allowed")]
    public System.String CompanyPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String CompanyState { get; set; }
    [Category(_companyCategory), Description("20110112 changed zip and added plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 2010AA N4-03")]
    public System.String CompanyZip { get; set; }
    [Category(_companyCategory), Description("09/26/2008 wdk facility JMCGH address change request and approved by Carol")]
    public System.String FacilityAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FacilityCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FacilityName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FacilityState { get; set; }
    [Category(_companyCategory), Description("20120112 changed to 5010 format added the plus 4|20090102 wdk For Electronic billing this cannot contain a dash or blank per the 837 manual in Loop 20")]
    public System.String FacilityZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String FederalTaxId { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyPhone { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceCompanyZipCode { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String InvoiceLogoImagePath { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String NPINumber { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String PrimaryCliaNo { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToAddress { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToCity { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToCountry { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToOrganizationName { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToState { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToStreet { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToStreet2 { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String RemitToZip { get; set; }
    [Category(_companyCategory), Description("")]
    public System.String WTHNPI { get; set; }
    #endregion Company Category


    #region Documentation Site Category
    [Category(_documentationSiteCategory), Description("Account Charge Entry Documentation")]
    public System.String AccountChargeEntryUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Account Management Documentation")]
    public System.String AccountManagementUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Batch Remittance Documentation")]
    public System.String BatchRemittanceUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Charge Master Maintenance Documentation")]
    public System.String ChargeMasterMaintenanceUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Claims Management Documentation ")]
    public System.String ClaimsManagementUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Client Invoicing Documentation")]
    public System.String ClientInvoicingUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Client Maintenance Documentation")]
    public System.String ClientMaintenanceUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Root URL for documentation site")]
    public System.String DocumentationSiteUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Insurance Plan Maintenance Documentation")]
    public System.String InsurancePlanMaintenanceUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Latest Updates Documentation")]
    public System.String LatestUpdatesUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Patient Collections Documentation")]
    public System.String PatientCollectionsUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Patient Statements Documentation")]
    public System.String PatientStatementsUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Physician Maintenance Documentation")]
    public System.String PhysicianMaintenanceUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Worklist Documentation")]
    public System.String WorklistUrl { get; set; }
    #endregion Documentation Site Category

    #region Environment Category
    [Category(_environmentCategory), Description("")]
    public System.String Default1500Printer { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultClientRequisitionPrinter { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultCytologyRequisitionPrinter { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultDetailBillPrinter { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultFilePath { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultMedicareBillPath { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultNursingHomePrinter { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultPathologyReqPrinter { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultSpoolFileDrive { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultSpoolFilePath { get; set; }
    [Category(_environmentCategory), Description("")]
    public System.String DefaultUBPrinter { get; set; }
    #endregion Environment Category

    #region Invoicing Category
    [Category(_invoicingCategory), Description("SET CBILL FILTER")]
    public System.String ClientBillFilter { get; set; }
    [Category(_invoicingCategory), Description("Network location where client invoice pdfs are stored")]
    public System.String InvoiceFileLocation { get; set; }
    #endregion Invoicing Category

    #region Pathology Group Category
    [Category(_pathologyGroupCategory), Description("Client mnemonic for the pathology group. This is used for invoicing PC component when the lab does not do Pathology group billing.")]
    public System.String PathologyGroupClientMnem { get; set; }
    [Category(_pathologyGroupCategory), Description("Set to true when professional charges to billed by the pathology group.")]
    public System.Boolean PathologyGroupBillsProfessional { get; set; }
    #endregion Pathology Group Category

    #region Operations Category
    [Category(_operationsCategory), Description("use to move the day back in an hourly query")]
    public System.String DemoDays { get; set; }
    [Category(_operationsCategory), Description("")]
    public System.Int32 FileMaintenanceAdtMessages { get; set; }
    [Category(_operationsCategory), Description("")]
    public System.Int32 FileMaintenanceMessagesInbound { get; set; }
    [Category(_operationsCategory), Description("wdk removed 20100709--86003 Added for rolling up multiline forms that exceed 36 for 1500's. Add all cpt4's that need to be rolled up seperated by the")]
    public System.String RollupCpt4s { get; set; }
    #endregion Operations Category

    #region Other Category
    [Category(_otherCategory), Description("")]
    public System.String ChargeTotalApp { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("CLIENT")]
    public System.String ClientAccountFinCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("CBILL")]
    public System.String ClientInvoiceCdm { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("CBILL")]
    public System.String ChargeInvoiceStatus { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("CAP")]
    public System.String CapitatedChargeStatus { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("N/A")]
    public System.String NotApplicableChargeStatus { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("NEW")]
    public System.String NewChargeStatus { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("C")]
    public System.String ClientFinancialTypeCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("M")]
    public System.String PatientFinancialTypeCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("Z")]
    public System.String ZFinancialTypecode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("Y")]
    public System.String BillToClientInvoiceDefaultFinCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("K")]
    public System.String InvalidFinancialCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("K")]
    public System.String InvalidClientCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("E")]
    public System.String SelfPayFinancialCode { get; set; }
    [Category(_otherCategory), Description(""), DefaultValue("HC")]
    public System.String PathologyBillingClientException { get; set; }

    #endregion Other Category

    #region System Category
    [Category(_systemCategory), Description("")]
    public System.String AccessMedPlusProvNo { get; set; }
    [Category(_systemCategory), Description(""), DefaultValue(true)]
    public System.Boolean AllowChargeEntry { get; set; }
    [Category(_systemCategory), Description(""), DefaultValue(true)]
    public System.Boolean AllowEditing { get; set; }
    [Category(_systemCategory), Description(""), DefaultValue(true)]
    public System.Boolean AllowPaymentAdjustmentEntry { get; set; }
    [Category(_systemCategory), Description("SET SELECT/MOVE OF ACCOUNTS ")]
    public System.String AllowSelMove { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String ArchiveDB { get; set; }
    [Category(_systemCategory), Description("Specifies production or non-production usage")]
    public System.String DatabaseEnvironment { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String ICDVersion { get; set; }
    [Category(_systemCategory), Description("used for reports")]
    public System.String LabDirector { get; set; }
    [Category(_systemCategory), Description(""), DefaultValue(false)]
    public System.Boolean ProcessPCCharges { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String ReportingPortalUrl { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String SystemVersion { get; set; }
    [Category(_systemCategory), Description(""), DefaultValue(4)]
    public System.Int32 TabsOpenLimit { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String ServiceUser { get; set; }
    [Category(_systemCategory), Description("")]
    public System.String ServiceUserPassword { get; set; }
    #endregion System Category

    #region ViewerSlides Category
    [Category(_viewerSlidesCategory), Description("")]
    public System.String IHCStainsQuery { get; set; }
    [Category(_viewerSlidesCategory), Description("")]
    public System.String SlidesQuery { get; set; }
    [Category(_viewerSlidesCategory), Description("New slide application billing changes")]
    public System.DateTime SlidesStartDate { get; set; }
    [Category(_viewerSlidesCategory), Description("used to set access to the special grid in the application")]
    public System.String SpecialClientsQuery { get; set; }
    [Category(_viewerSlidesCategory), Description("")]
    public System.String SpecialStainsQuery { get; set; }
    #endregion ViewerSlides Category

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
        var attributeValue = attributeInfo?.Value ?? "";

        return attributeValue;
    }

    public string GetProductionEnvironment()
    {
        string env = this.DatabaseEnvironment;
        if (env == "Production")
            return "P";
        else
            return "T";
    }
}
