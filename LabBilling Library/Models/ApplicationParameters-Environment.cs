using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Environment Category
    [Category(_environmentCategory), Description("Default printer for 1500 claim forms")]
    public System.String Default1500Printer { get; set; }
    
    [Category(_environmentCategory), Description("Default dot-matrix printer for client requisition forms (pin-fed)")]
    public System.String DefaultClientRequisitionPrinter { get; set; }
    
    [Category(_environmentCategory), Description("Default dot-matrix printer for cytology requisition forms (pin-fed)")]
    public System.String DefaultCytologyRequisitionPrinter { get; set; }
    
    [Category(_environmentCategory), Description("Default printer for detail bill forms")]
    public System.String DefaultDetailBillPrinter { get; set; }
    
    [Category(_environmentCategory), Description("Default file path for storing generated files")]
    public System.String DefaultFilePath { get; set; }
    
    [Category(_environmentCategory), Description("Default path for Medicare bill files")]
    public System.String DefaultMedicareBillPath { get; set; }
    
    [Category(_environmentCategory), Description("Default printer for nursing home forms")]
    public System.String DefaultNursingHomePrinter { get; set; }
    
    [Category(_environmentCategory), Description("Default dot-matrix printer for pathology requisition forms (pin-fed)")]
    public System.String DefaultPathologyReqPrinter { get; set; }
    
    [Category(_environmentCategory), Description("Default drive letter for spool files")]
    public System.String DefaultSpoolFileDrive { get; set; }

    [Category(_environmentCategory), Description("Default path for spool files")]
    public System.String DefaultSpoolFilePath { get; set; }
    
    [Category(_environmentCategory), Description("Default printer for UB claim forms")]
    public System.String DefaultUBPrinter { get; set; }
    
    [Category(_environmentCategory), Description("Enable raw PCL5 printing for dot-matrix printers (bypasses GDI for pin-fed forms)")]
    public System.Boolean UseDotMatrixRawPrinting { get; set; }
    
    #endregion Environment Category

}
