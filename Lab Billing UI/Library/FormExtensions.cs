using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Forms;


namespace LabBilling.Library
{
    public sealed class FormExtensions
    {
        /// <summary>
        /// Event Handler for the data grid views click event, launches the account program for the selected account.
        /// 08/23/2007 rgc/wdk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static public void LaunchAcc_EventHandler(object sender, DataGridViewCellMouseEventArgs e)
        {
            DataGridView dgvGrid = (DataGridView)sender;
            if (e.RowIndex < 0)
            {
                MessageBox.Show("You did not selected a current row.");
                return;
            }
            string strAcc = "";
            try
            {
                strAcc = dgvGrid["ACCOUNT", e.RowIndex].Value.ToString();
            }
            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
                return;
            }
            if (strAcc.Length < 2)
            {
                MessageBox.Show("Account not long enough"); // don't expect this to happen.
                return;
            }
            if (strAcc[1] == '0' || strAcc[1] == 'A') // if the seconds character is 0 or A remove 
            {
                strAcc = strAcc.Remove(1, 1);
            }

            AccountForm frm = new AccountForm(strAcc, Application.OpenForms[0]);
            frm.Show();

        }

    }


}