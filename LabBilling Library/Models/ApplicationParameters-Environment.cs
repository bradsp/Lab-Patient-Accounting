using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
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

}
