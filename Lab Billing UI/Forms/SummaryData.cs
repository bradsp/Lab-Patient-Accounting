using Krypton.Toolkit;
using LabBilling.Logging;

namespace LabBilling.Forms;

/// <summary>
/// Class to group data in sections for placement in a panel
/// </summary>
public class SummaryData
{
    public string Label { get; set; }
    public string Value { get; set; }
    public GroupType Group { get; set; }
    public KryptonLabel DisplayLabel;
    public KryptonLabel ValueLabel;
    public int RowPos { get; set; }
    public int ColPos { get; set; }
    public bool IsHeader { get; }
    public enum GroupType
    {
        Demographics,
        Financial,
        Insurance,
        Diagnoses
    }

    /// <summary>
    /// Constructor for the class.
    /// </summary>
    /// <param name="label">Label for the field</param>
    /// <param name="value">Value of the data to display</param>
    /// <param name="group">Enumerated group where field is placed</param>
    /// <param name="rowpos">Row number where data will be placed</param>
    /// <param name="colpos">Column number where data will be placed</param>
    /// <param name="IsHeader">Is this text the header</param>
    public SummaryData(string label, string value, GroupType group, int rowpos, int colpos, bool IsHeader = false)
    {
        Log.Instance.Trace($"Entering");
        Log.Instance.Debug($"Entering with data {label} : {value}");
        DisplayLabel = new KryptonLabel();
        ValueLabel = new KryptonLabel();

        Label = label;
        Value = value;
        RowPos = rowpos;
        ColPos = colpos;
        Group = group;
        this.IsHeader = IsHeader;

        DisplayLabel.Location = new Point(10, 10);
        if (IsHeader)
        {
            //DisplayLabel.Font = new Font(DisplayLabel.Font.Name, 14, FontStyle.Underline | FontStyle.Bold);
            DisplayLabel.LabelStyle = LabelStyle.TitleControl;
        }
        else
        {
            //DisplayLabel.ForeColor = Color.DarkBlue;
        }
        DisplayLabel.Text = Label;
        DisplayLabel.AutoSize = true;
        DisplayLabel.Height = 80;
        ValueLabel.Location = new Point(10, 10);
        ValueLabel.AutoSize = true;
        ValueLabel.Text = Value;
        ValueLabel.LabelStyle = LabelStyle.BoldControl;
    }
}

