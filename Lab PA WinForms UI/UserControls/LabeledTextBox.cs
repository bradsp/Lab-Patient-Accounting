using System.ComponentModel;

namespace LabBilling.Core;

public partial class LabeledTextBox : UserControl
{
    public LabeledTextBox()
    {
        InitializeComponent();
    }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string Label
    {
        get { return label1.Text; }
        set { label1.Text = value; }
    }

}
