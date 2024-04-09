using System.ComponentModel;

namespace LabBilling.Core.Models;

public partial class ApplicationParameters
{
    #region ViewerSlides Category
    [Category(_viewerSlidesCategory)]
    [Description("")]
    public System.String IHCStainsQuery { get; set; }
    [Category(_viewerSlidesCategory)]
    [Description("")]
    public System.String SlidesQuery { get; set; }
    [Category(_viewerSlidesCategory)]
    [Description("New slide application billing changes")]
    public System.DateTime SlidesStartDate { get; set; }
    [Category(_viewerSlidesCategory)]
    [Description("used to set access to the special grid in the application")]
    public System.String SpecialClientsQuery { get; set; }
    [Category(_viewerSlidesCategory)]
    [Description("")]
    public System.String SpecialStainsQuery { get; set; }
    #endregion ViewerSlides Category
}
