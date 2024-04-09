using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Accounting Category
    [Category(_accountingCategory), Description("Bank account number")]
    public System.String BankAccountNumber { get; set; }

    [Category(_accountingCategory), Description("Name of bank for deposits")]
    public System.String BankName { get; set; }

    [Category(_accountingCategory), Description("Bank Routing number")]
    public System.String BankRoutingNumber { get; set; }

    #endregion Accounting Category
}
