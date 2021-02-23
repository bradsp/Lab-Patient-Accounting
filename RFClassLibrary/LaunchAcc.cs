using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace RFClassLibrary
{
    /// <summary>
    /// This Class is designed to allow users to view the account data in billing using the
    /// current MCLBilling system, from the new datagridview. If the DataGridView does not contain
    /// a column named "Account" if displays a message that the account cannot be viewed.
    /// 
    /// 01/11/2007 wdk
    /// </summary>
    public class LaunchAcc : RFCObject
    {

        static string m_strDatabase;

        /// <summary>
        /// Public constructor for this class
        /// </summary>
        /// <param name="strDatabase"></param>
        public LaunchAcc(string strDatabase)
        {
            m_strDatabase = strDatabase;
        }
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
            OS osb = new OS();

            string strDatabase = m_strDatabase;

            if (string.IsNullOrEmpty(m_strDatabase))
            {
                try
                {
                    string[] args = Environment.GetCommandLineArgs();
                    strDatabase = args[2];
                    if (strDatabase.Contains("MCLBILL") || strDatabase.Contains("MCLOE"))
                    {
                        strDatabase = args[1];
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    if (DateTime.Today.ToString("d") == "5/5/2010")
                    {
                        MessageBox.Show("Exception", "INDEX OUT of RANGE");
                    }
                    strDatabase = "MCLLIVE";
                }
            }


            //#region Debug code
            ////System.Diagnostics.Debugger.Launch(); // 01/24/2008 informational only
            // string strMsg = "";

            //foreach (string str in args)
            //{
            //    strMsg += str + "\n";
            //}
            //MessageBox.Show(strMsg);
            //#endregion Debug code
            if (DateTime.Today.ToString("d") == "5/5/2010")
            {
                MessageBox.Show(strDatabase, strAcc);
            }

            osb.Shell("acc.exe", @"C:\Program Files\Medical Center Laboratory\MCL Billing", string.Format("{0}{1} {2}{3}",
                                        strDatabase[0] == '/' ? "" : @"/",
                                            strDatabase,
                                                strAcc[0] == '/' ? "" : @"/",
                                                    strAcc));
            // does work but need better solution 
            //  ((DataGridView)sender).RowHeaderMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(LaunchAcc.LaunchAcc_EventHandler);

        }

        /// <summary>
        /// Overload for use when attaching to non billing databases.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <param name="strSServer"></param>
        /// <param name="strSDatabase"></param>
        static public void LaunchAcc_EventHandler(object sender, DataGridViewCellMouseEventArgs e, string strSServer, string strSDatabase)
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
            OS osb = new OS();



            //string[] args = Environment.GetCommandLineArgs();
            //string strDatabase = args[2];
            //if (strDatabase.Contains("MCLBILL") || strDatabase.Contains("MCLOE"))
            //{
            //    strDatabase = args[1];
            //}


            //#region Debug code
            ////System.Diagnostics.Debugger.Launch(); // 01/24/2008 informational only
            // string strMsg = "";

            //foreach (string str in args)
            //{
            //    strMsg += str + "\n";
            //}
            //MessageBox.Show(strMsg);
            //#endregion Debug code

            osb.Shell("acc.exe", @"C:\Program Files\Medical Center Laboratory\MCL Billing", string.Format("{0}{1} {2}{3}",
                                        strSDatabase[0] == '/' ? "" : @"/",
                                            strSDatabase,
                                                strAcc[0] == '/' ? "" : @"/",
                                                    strAcc));
            // does work but need better solution 
            //  ((DataGridView)sender).RowHeaderMouseDoubleClick -= new System.Windows.Forms.DataGridViewCellMouseEventHandler(LaunchAcc.LaunchAcc_EventHandler);

        }

        /// <summary>
        /// This is a non static that must be created through instination with a database name.
        /// </summary>
        /// <param name="strAccount">The account number to search for.</param>
        public void LaunchAccount(string strAccount)
        {
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "Blank account passed to LaunchAcc()";
                return;
            }
            if (strAccount[1] == 0 || strAccount[1] == 'A') // if the seconds character is 0 or A remove 
            {
                strAccount = strAccount.Remove(1, 1);
            }
            OS osb = new OS();
            osb.Shell("acc.exe", @"C:\Program Files\Medical Center Laboratory\MCL Billing", string.Format("{0}{1} {2}{3}",
                                        m_strDatabase[0] == '/' ? "" : @"/",
                                            m_strDatabase.Trim(),
                                                strAccount[0] == '/' ? "" : @"/",
                                                    strAccount.Trim()));
        }

        /// <summary>
        /// Overload to handle name searches
        /// Event Hanlder for the data grid views click event, launches the account program for the selected account.
        /// 08/23/2007 rgc/wdk
        /// </summary>
        /// <param name="strName">Patients Name</param>
        public void LaunchAccByName(string strName)
        {
            m_strErrMsg = "Not available at this time.";
            return;
            //if (string.IsNullOrEmpty(strName))
            //{
            //    m_strErrMsg = "Blank name passed to LaunchAcc()";
            //    return;
            //}
            //OS osb = new OS();
            //osb.Shell("acc.exe", @"C:\Program Files\Medical Center Laboratory\MCL Billing", string.Format("{0}{1} ",//{2}{3}",
            //                            m_strDatabase[0] == '/' ? "" : @"/",
            //                              m_strDatabase//,
            //                                           // strName[0] == '/' ? "" : @"/",
            //                                           //     strName
            //                                   )
            //                                   );

        }





    }
}
