using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LabBilling
{
    static class Program
    {
        public static Emp LoggedInUser { get; set; }
        //public static string SelectedEnvironment { get; set; }
        public static string ConnectionString { get; set; }
        public static string Server { get; set; }
        public static string Database { get; set; }
        public static string LogDatabase { get; set; }
        public static string Environment { get; set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

            bool firstInstance;
            Mutex mutex = new Mutex(false, "Local\\" + Application.ProductName, out firstInstance);

            if(!firstInstance)
            {
                MessageBox.Show("Application is already running.");
                return;
            }

            LoggedInUser = null;
            Log.Instance.Info($"Launching Lab Patient Accounting");

            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            Application.ApplicationExit += new EventHandler(OnApplicationExit);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            bool testEnvironment = false;

            if(args.Length > 0)
            {
                if (args[0].Contains("TEST"))
                {
                    testEnvironment = true;
                }
            }

            Login loginFrm = new Login(testEnvironment);
            if(loginFrm.ShowDialog() == DialogResult.OK)
            {
                Log.Instance.Info($"Login successful - connection {Helper.ConnVal}");
                Application.Run(new MainForm());
            }
            else
            {
                Application.Exit();
            }        
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Exception exc = (Exception)e.Exception;

            //log the exception, display, etc
            if (exc.InnerException != null)
            {
                Log.Instance.Fatal(exc.InnerException, "Unhandled Exception");
            }
            Log.Instance.Fatal(exc, "Unhandled Exception");
            MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            //log the exception
            if(ex.InnerException != null)
            {
                Log.Instance.Fatal(ex.InnerException, "Unhandled exception");
            }
            Log.Instance.Fatal(ex, "Unhandled exception");
            MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        }

        static void OnApplicationExit(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;

            NLog.LogManager.Shutdown();

        }
    }
}
