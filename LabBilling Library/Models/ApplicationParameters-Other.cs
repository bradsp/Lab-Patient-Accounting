using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Other Category
    [Category(_otherCategory)]
    [Description("Obsolete")]
    public System.String ChargeTotalApp { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("CLIENT")]
    public System.String ClientAccountFinCode { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("CBILL")]
    public System.String ClientInvoiceCdm { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("CBILL")]
    public System.String ChargeInvoiceStatus { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("CAP")]
    public System.String CapitatedChargeStatus { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("N/A")]
    public System.String NotApplicableChargeStatus { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("NEW")]
    public System.String NewChargeStatus { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("C")]
    public System.String ClientFinancialTypeCode { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("M")]
    public System.String PatientFinancialTypeCode { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("Z")]
    public System.String ZFinancialTypecode { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("Y")]
    public System.String BillToClientInvoiceDefaultFinCode { get; set; }
    [Category(_otherCategory)]
    [Description("Specify Financial Code used to identify interfaced accounts where the financial code could not be determined.")]
    [DefaultValue("K")]
    public System.String InvalidFinancialCode { get; set; }
    [Category(_otherCategory)]
    [Description("Specify Client Code used to identify interfaced accounts where the client could not be determined.")]
    [DefaultValue("K")]
    public System.String InvalidClientCode { get; set; }
    [Category(_otherCategory)]
    [Description("Financial Code for Self-Pay accounts")]
    [DefaultValue("E")]
    public System.String SelfPayFinancialCode { get; set; }
    [Category(_otherCategory)]
    [Description("")]
    [DefaultValue("HC")]
    public System.String PathologyBillingClientException { get; set; }

    #endregion Other Category

}
