using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;

namespace LabBilling;

static class Program
{
    public static UserAccount LoggedInUser { get; set; }

    public static AppEnvironment AppEnvironment { get; set; } = new AppEnvironment();

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {

        bool firstInstance;
        Mutex mutex = new Mutex(false, "Local\\" + Application.ProductName, out firstInstance);

        if (!firstInstance)
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

        if (args.Length > 0)
        {
            if (args[0].Contains("TEST"))
            {
                testEnvironment = true;
            }
        }
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Login loginFrm = new(testEnvironment);
        if (loginFrm.ShowDialog() == DialogResult.OK)
        {
            Log.Instance.Info($"Login successful - connection {Helper.ConnVal}");
            MainForm mainForm = new();
            Application.Run(mainForm);
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
            if (exc.InnerException is SqlException)
            {
                SqlException sqlException = (SqlException)exc.InnerException;
                if (sqlException.Message.Contains("Execution Timeout Expired"))
                {
                    Log.Instance.Fatal(sqlException, "SQL Database error.");
                    MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        if (exc is SqlException)
        {
            SqlException sqlException = (SqlException)exc;
            if (sqlException.Message.Contains("Execution Timeout Expired"))
            {
                Log.Instance.Fatal(sqlException, "SQL Database error.");
                MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
        Log.Instance.Fatal(exc, "Unhandled Exception");
        MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception exc = (Exception)e.ExceptionObject;
        //log the exception, display, etc
        if (exc.InnerException != null)
        {
            Log.Instance.Fatal(exc.InnerException, "Unhandled Exception");
            if (exc.InnerException is SqlException)
            {
                SqlException sqlException = (SqlException)exc.InnerException;
                if (sqlException.Message.Contains("Execution Timeout Expired"))
                {
                    Log.Instance.Fatal(sqlException, "SQL Database error.");
                    MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
            }
        }

        if (exc is SqlException)
        {
            SqlException sqlException = (SqlException)exc;
            if (sqlException.Message.Contains("Execution Timeout Expired"))
            {
                Log.Instance.Fatal(sqlException, "SQL Database error.");
                MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }
        Log.Instance.Fatal(exc, "Unhandled Exception");
        MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    }

    static void OnApplicationExit(object sender, EventArgs e)
    {
        NLog.LogManager.Shutdown();
    }
}
