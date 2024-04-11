using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Invoicing Category
    [Category(_invoicingCategory), Description("SET CBILL FILTER")]
    public System.String ClientBillFilter { get; set; }
    [Category(_invoicingCategory), Description("Network location where client invoice pdfs are stored")]
    public System.String InvoiceFileLocation { get; set; }
    #endregion Invoicing Category
}
