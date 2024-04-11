using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region Charging Category
    [Category(_chargingCategory)]
    [Description("Pipe delimited list of active fee schedules")]
    public System.String FeeSchedules { get; set; }
    [Category(_chargingCategory)]
    [Description("")]
    public System.String GeneralHealthPanelInsurances { get; set; }
    [Category(_chargingCategory)]
    [Description("")]
    public System.String GeneralHealthPanelTests { get; set; }
    [Category(_chargingCategory), Description("")]
    public System.String OBPanelInsurances { get; set; }
    [Category(_chargingCategory)]
    [Description("")]
    public System.String OBPanelTests { get; set; }
    [Category(_chargingCategory)]
    [Description("SET QUEST EMAIL RECEIPENTS")]
    public System.String QuestBillingEmailRecipients { get; set; }
    #endregion Charging Category

}
