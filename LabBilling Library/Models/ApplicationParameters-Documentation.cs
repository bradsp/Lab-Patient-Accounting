using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Documentation Site Category
    [Category(_documentationSiteCategory), Description("Account Charge Entry Documentation")]
    public System.String AccountChargeEntryUrl { get; set; }
    [Category(_documentationSiteCategory), Description("Account Management Documentation")]
    public System.String AccountManagementUrl { get; set; }
    [Category(_documentationSiteCategory)]
    [Description("Batch Remittance Documentation")]
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

}
