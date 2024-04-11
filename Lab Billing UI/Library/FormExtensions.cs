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
        public static (string account, string error) GetDGVAccount(DataGridView dgv, int rowIndex)
        {
            if (rowIndex < 0)
            {                
                return ("", "Row not selected");
            }
            string strAcc;
            try
            {
                strAcc = dgv["ACCOUNT", rowIndex].Value.ToString();
            }
            catch (ArgumentException ae)
            {
                return ("", ae.Message);
            }
            if (strAcc.Length < 2)
            {
                //MessageBox.Show("Account not long enough"); // don't expect this to happen.
                return ("", "Account not long enough");
            }
            if (strAcc[1] == '0' || strAcc[1] == 'A') // if the seconds character is 0 or A remove 
            {
                strAcc = strAcc.Remove(1, 1);
            }

            return (strAcc, "");
        }

    }


}