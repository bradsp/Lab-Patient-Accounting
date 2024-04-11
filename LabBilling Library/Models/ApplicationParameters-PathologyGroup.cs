using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Pathology Group Category
    [Category(_pathologyGroupCategory), Description("Client mnemonic for the pathology group. This is used for invoicing PC component when the lab does not do Pathology group billing.")]
    public System.String PathologyGroupClientMnem { get; set; }
    [Category(_pathologyGroupCategory), Description("Set to true when professional charges to billed by the pathology group.")]
    public System.Boolean PathologyGroupBillsProfessional { get; set; }
    #endregion Pathology Group Category
}
