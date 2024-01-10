using System;
using System.Windows.Forms;
using WinFormsLibrary;

namespace LabBilling.Forms
{
    public class BaseForm : Form
    {
        public BaseForm() : base()
        {

        }

        protected override void OnLoad(EventArgs e)
        {

            this.BackColor = Program.AppEnvironment.WindowBackgroundColor;
            this.ForeColor = Program.AppEnvironment.WindowTextColor;

            this.GetAllControls<Button>().ForEach(b =>
            {
                b.BackColor = Program.AppEnvironment.ButtonBackgroundColor;
                b.ForeColor = Program.AppEnvironment.ButtonTextColor;
            });

            this.GetAllControls<TextBox>().ForEach(t =>
            {
                t.BackColor = Program.AppEnvironment.WindowBackgroundColor;
                t.ForeColor = Program.AppEnvironment.WindowTextColor;
            });

            this.GetAllControls<Label>().ForEach(label =>
            {
                label.ForeColor = Program.AppEnvironment.WindowTextColor;
            });

            this.GetAllControls<ComboBox>().ForEach(comboBox =>
            {
                comboBox.BackColor = Program.AppEnvironment.WindowBackgroundColor;
                comboBox.ForeColor = Program.AppEnvironment.WindowTextColor;
            });

            base.OnLoad(e);
        }

    }
}
