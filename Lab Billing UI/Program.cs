using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Threading;
using System.Windows.Forms;

namespace LabBilling
{
    static class Program
    {
        public static Emp LoggedInUser { get; set; }
        //public static string SelectedEnvironment { get; set; }
        //public static string ConnectionString { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LoggedInUser = null;

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Log.Instance.Info($"Launching LabBilling - connection {Helper.ConnVal}");

            Application.ApplicationExit += new EventHandler(OnApplicationExit);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login loginFrm = new Login();
            if(loginFrm.ShowDialog() == DialogResult.OK)
            {
                Application.Run(new MainForm());
            }
            else
            {
                Application.Exit();
            }

            
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            //log the exception, display, etc
            Log.Instance.Fatal(e.Exception, "Unhandled Exception");
            MessageBox.Show(e.Exception.ToString(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //log the exception
            Log.Instance.Fatal((Exception)e.ExceptionObject, "Unhandled exception");
            MessageBox.Show(e.ExceptionObject.ToString(), "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static void OnApplicationExit(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            NLog.LogManager.Shutdown();

        }
    }
}
