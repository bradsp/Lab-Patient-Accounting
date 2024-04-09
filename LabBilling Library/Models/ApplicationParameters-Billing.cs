using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Billing Category
    [Category(_billingCategory)]
    [Description("Pipe-delimited list of usernames who are authorized to run client invoices.")]
    public System.String AuthorizedToRunClientBills { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String BHGroupNo { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String BHProviderNo { get; set; }

    [Category(_billingCategory)]
    [Description("No of days to hold claims for billing")]
    [DefaultValue(10)]
    public System.Int32 BillingInitialHoldDays { get; set; }

    [Category(_billingCategory)]
    [Description("Tax Id of Entity receiving 837 claims")]
    public System.String BillingReceiverId { get; set; }

    [Category(_billingCategory)]
    [Description("Entity receiving 837 claims")]
    public System.String BillingReceiverName { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String BlueCrossProvNo { get; set; }

    [Category(_billingCategory)]
    [Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")]
    public System.String BlueCrossReceiverNo { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String BundleInsurance { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String ChampusGroupNo { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String CNetReceiverId { get; set; }

    [Category(_billingCategory)]
    [Description("CODE STATS")]
    public System.String CodeStats { get; set; }

    [Category(_billingCategory)]
    [Description("Sets the date to run bad debt")]
    public System.DateTime CollectionsRun { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String DBMailProfile { get; set; }

    [Category(_billingCategory)]
    [Description("use this to turn off or on the remaining validations that are hard coded in the application")]
    public System.Boolean Diagnosis { get; set; }

    [Category(_billingCategory)]
    [Description("ENABLE INSURANCE DIAGNOSIS POINTERS")]
    [DefaultValue(true)]
    public System.String DiagnosisCodePointerSelect { get; set; }

    [Category(_billingCategory)]
    [Description("Turn off the Viewer for Atlanta once electronic is fixed")]
    public System.DateTime ElectronicBillDate { get; set; }

    [Category(_billingCategory)]
    [Description("changed eob address")]
    public System.String EobAddress { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.DateTime GlobalBillingStartDate { get; set; }

    [Category(_billingCategory)]
    [Description("Location to place generated 837i files")]
    public System.String InstitutionalClaimFileLocation { get; set; }

    [Category(_billingCategory)]
    [Description("Max number of claims in a claim batch. Set to 0 for unlimited.")]
    [DefaultValue(0)]
    public System.Int32 MaxClaimsInClaimBatch { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String MedicaidProviderId { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String MedicareBProviderNo { get; set; }

    [Category(_billingCategory)]
    [Description("Added to facilitate CAHABA medicare billing")]
    public System.String MedicareElectronicReceiverId { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String MedicareProviderNo { get; set; }

    [Category(_billingCategory)]
    [Description("09/19/2008 changed from 00890 to 00390 to fix Bluecross files")]
    public System.String MedicareReceiverId { get; set; }

    [Category(_billingCategory)]
    [Description("Added to facilitate CAHABA medicare billing")]
    public System.String MedicareSubmitterId { get; set; }

    [Category(_billingCategory)]
    [Description("SET NURSING HOME BILL THRU DATE")]
    public System.String NursingHomeBillThruDate { get; set; }

    [Category(_billingCategory)]
    [Description("Added for use if outpatient becomes valid.")]
    public System.DateTime OutpatientBillStart { get; set; }

    [Category(_billingCategory)]
    [Description("Location to place 837p claim files.")]
    public System.String ProfessionalClaimFileLocation { get; set; }

    [Category(_billingCategory)]
    [Description("new form to be used")]
    public System.DateTime ProfessionalClaimStartDate { get; set; }

    [Category(_billingCategory)]
    [Description(""), DefaultValue("282N00000X")]
    public System.String ProviderTaxonomyCode { get; set; }

    [Category(_billingCategory)]
    [Description("location of hospitals remittance files")]
    public System.String RemitImportDirectory { get; set; }

    [Category(_billingCategory)]
    [Description("date files last imported")]
    public System.DateTime RemitPostingDate { get; set; }

    [Category(_billingCategory)]
    [Description("location of local remittance files")]
    public System.String RemitProcessingDirectory { get; set; }

    [Category(_billingCategory)]
    [Description("Discontinue the swapping of insurance's and allow secondary billing via applications")]
    public System.DateTime SecondaryBilling { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.Double SmallBalanceAmount { get; set; }

    [Category(_billingCategory)]
    [Description("Used by ViewerAcc to seperate the new claim methodolgy from the old method.")]
    public System.DateTime SSIStartDate { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String TLCProviderId { get; set; }

    [Category(_billingCategory)]
    [Description("")]
    public System.String UHCProviderId { get; set; }

    [Category(_billingCategory)]
    [Description("Set to 1 when we start using for stored procedures to skip making new accounts")]
    public System.Int32 UseBillMethod { get; set; }

    #endregion Billing Category
}
