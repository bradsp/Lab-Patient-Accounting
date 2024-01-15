using System.Windows.Forms;

namespace LabBilling.UserControls
{
    public class LabDataGridView : System.Windows.Forms.DataGridView
    {
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if(keyData == Keys.Enter)
            {
                base.ProcessTabKey(Keys.Tab);
                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                base.ProcessTabKey(Keys.Tab);
                return true;
            }
            return base.ProcessDataGridViewKey(e);
        }

    }
}
