using LabBilling.Core.DataAccess;
using System;
using System.Windows.Forms;
using WinFormsLibrary;

namespace Utilities;

public class BaseForm : Form
{
    private readonly IAppEnvironment appEnvironment;
    public BaseForm(IAppEnvironment appEnvironment) : base()
    {
        this.appEnvironment = appEnvironment;
    }

    protected override void OnLoad(EventArgs e)
    {

        this.BackColor = appEnvironment.WindowBackgroundColor;
        this.ForeColor = appEnvironment.WindowTextColor;

        this.GetAllControls<Button>().ForEach(b =>
        {
            b.BackColor = appEnvironment.ButtonBackgroundColor;
            b.ForeColor = appEnvironment.ButtonTextColor;
        });

        this.GetAllControls<TextBox>().ForEach(t =>
        {
            t.BackColor = appEnvironment.WindowBackgroundColor;
            t.ForeColor = appEnvironment.WindowTextColor;
        });

        this.GetAllControls<Label>().ForEach(label =>
        {
            label.ForeColor = appEnvironment.WindowTextColor;
        });

        this.GetAllControls<ComboBox>().ForEach(comboBox =>
        {
            comboBox.BackColor = appEnvironment.WindowBackgroundColor;
            comboBox.ForeColor = appEnvironment.WindowTextColor;
        });

        base.OnLoad(e);
    }

}
