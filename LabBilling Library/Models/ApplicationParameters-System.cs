using System.ComponentModel;

namespace LabBilling.Core.Models;
public partial class ApplicationParameters
{
    #region System Category
    [Category(_systemCategory)]
    [Description("")]
    public System.String AccessMedPlusProvNo { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    [DefaultValue(true)]
    public System.Boolean AllowChargeEntry { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    [DefaultValue(true)]
    public System.Boolean AllowEditing { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    [DefaultValue(true)]
    public System.Boolean AllowPaymentAdjustmentEntry { get; set; }

    [Category(_systemCategory)]
    [Description("SET SELECT/MOVE OF ACCOUNTS ")]
    public System.String AllowSelMove { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    public System.String ArchiveDB { get; set; }

    [Category(_systemCategory)]
    [Description("Specifies production or non-production usage")]
    public System.String DatabaseEnvironment { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    public System.String ICDVersion { get; set; }

    [Category(_systemCategory)]
    [Description("Name of Laboratory Director to display on reports.")]
    public System.String LabDirector { get; set; }

    [Category(_systemCategory)]
    [Description("Specify if professional charges are processed for billing.")]
    [DefaultValue(false)]
    public System.Boolean ProcessPCCharges { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    public System.String ReportingPortalUrl { get; set; }

    [Category(_systemCategory)]
    [Description("For future use")]
    public System.String SystemVersion { get; set; }

    [Category(_systemCategory)]
    [Description("Specify the number of tabs that can be open at one time.")]
    [DefaultValue(4)]
    public System.Int32 TabsOpenLimit { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    public System.String ServiceUser { get; set; }

    [Category(_systemCategory)]
    [Description("")]
    public System.String ServiceUserPassword { get; set; }

    [Category(_systemCategory)]
    [Description("Specifies the level of logging. Debug and Trace levels will impact application performance. Changing this value required re-launching the application to take effect.")]
    [DefaultValue("Error")]
    [TypeConverter(typeof(ApplicationParameters.LogLevelTypeConverter))]
    public System.String LogLevel { get; set; } = "Error";

    private string _logLocation = null;
    [Category(_systemCategory)]
    [Description("Specify where log records will be written. Changing this value requires re-launching the application to take effect.")]
    [DefaultValue("Database")]
    [TypeConverter(typeof(LogLocationTypeConverter))]
    public System.String LogLocation { get { return _logLocation; } set { _logLocation = value; } }

    [Category(_systemCategory)]
    [Description("Specify the file path to write log entries if LogLocation is set to FilePath. Changing this value requires re-launching the application to take effect.")]
    [DefaultValue("")]
    public System.String LogFilePath { get; set; }

    #endregion SystemCategory

}
