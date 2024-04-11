using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Operations Category
    [Category(_operationsCategory)]
    [Description("use to move the day back in an hourly query")]
    public System.String DemoDays { get; set; }
    [Category(_operationsCategory)]
    [Description("Number of days to keep ADT messages received from an interface.")]
    public System.Int32 FileMaintenanceAdtMessages { get; set; }
    [Category(_operationsCategory)]
    [Description("")]
    public System.Int32 FileMaintenanceMessagesInbound { get; set; }
    [Category(_operationsCategory)]
    [Description("wdk removed 20100709--86003 Added for rolling up multiline forms that exceed 36 for 1500's. Add all cpt4's that need to be rolled up seperated by the")]
    public System.String RollupCpt4s { get; set; }
    #endregion Operations Category
}
