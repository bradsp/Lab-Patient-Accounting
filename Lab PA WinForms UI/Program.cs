using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Services;

namespace LabBilling;

static class Program
{
    public static UserAccount LoggedInUser { get; set; }
    public static AppEnvironment AppEnvironment { get; private set; }
    public static UnitOfWorkMain UnitOfWork { get; private set; }


    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        bool firstInstance;
        Mutex mutex = new(false, "Local\\" + Application.ProductName, out firstInstance);

        if (!firstInstance)
        {
            MessageBox.Show("Application is already running.");
            return;
        }

        Log.Instance.Info($"Launching Lab Patient Accounting");

        Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        Application.ApplicationExit += new EventHandler(OnApplicationExit);

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        bool testEnvironment = false;

        if (args.Length > 0 && args[0].Contains("TEST"))
        {
            testEnvironment = true;
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);

        try
        {
            AppEnvironment = new AppEnvironment();
            AppEnvironment.IntegratedAuthentication = true;
            InitializeAppEnvironment(testEnvironment);
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal("Failed to initialize application environment.", ex);
            MessageBox.Show("Failed to initialize application environment. Please check the configuration and try again.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Create AuthenticationService
        var authenticationService = new AuthenticationService(AppEnvironment.ConnectionString);
        //This app will default to integrated authentication.
        //Pass the windows username to the AuthenticateIntegrated method.
        var windowsUsername = Environment.UserName;
        Log.Instance.Info($"Windows Username: {windowsUsername}");

        // Pass the windows username to the AuthenticateIntegrated method.
        var user = authenticationService.AuthenticateIntegrated(windowsUsername);
        
        if(user == null)
        {
            Log.Instance.Error($"User not found in database: {windowsUsername}");
            MessageBox.Show("User not found in database. Please contact your system administrator.", "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        LoggedInUser = user;
        //complete AppEnvironment Initialization
        AppEnvironment.UserAccount = LoggedInUser;
        AppEnvironment.User = LoggedInUser.UserName;
        AppEnvironment.ApplicationParameters = UnitOfWork.SystemParametersRepository.LoadParameters();
        UnitOfWork = new UnitOfWorkMain(AppEnvironment);
        //run MainForm
        Application.Run(new MainForm());

    }

    private static void InitializeAppEnvironment(bool testEnvironment)
    {
        // Set basic configuration relevant to the login process
        AppEnvironment.DatabaseName = testEnvironment
            ? Properties.Settings.Default.TestDbName
            : Properties.Settings.Default.ProdDbName;

        AppEnvironment.ServerName = testEnvironment
            ? Properties.Settings.Default.TestDbServer
            : Properties.Settings.Default.ProdDbServer;

        AppEnvironment.IntegratedAuthentication = testEnvironment
            ? Properties.Settings.Default.TestIntegratedSecurity
            : Properties.Settings.Default.ProdIntegratedSecurity;

        if (string.IsNullOrEmpty(AppEnvironment.ConnectionString))
        {
            throw new ArgumentException("Connection string is null or empty.", nameof(AppEnvironment.ConnectionString));
        }
    }

    static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
    {
        Exception exc = (Exception)e.Exception;

        if (exc.InnerException != null)
        {
            Log.Instance.Fatal(exc.InnerException, "Unhandled Exception");
            if (exc.InnerException is SqlException sqlException1 && sqlException1.Message.Contains("Execution Timeout Expired"))
            {
                Log.Instance.Fatal(sqlException1, "SQL Database error.");
                MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        if (exc is SqlException sqlException && sqlException.Message.Contains("Execution Timeout Expired"))
        {
            Log.Instance.Fatal(sqlException, "SQL Database error.");
            MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        Log.Instance.Fatal(exc, "Unhandled Exception");
        MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    }

    static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        Exception exc = (Exception)e.ExceptionObject;

        if (exc.InnerException != null)
        {
            Log.Instance.Fatal(exc.InnerException, "Unhandled Exception");
            if (exc.InnerException is SqlException sqlException1 && sqlException1.Message.Contains("Execution Timeout Expired"))
            {
                Log.Instance.Fatal(sqlException1, "SQL Database error.");
                MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
        }

        if (exc is SqlException sqlException && sqlException.Message.Contains("Execution Timeout Expired"))
        {
            Log.Instance.Fatal(sqlException, "SQL Database error.");
            MessageBox.Show("Execution timeout expired. Report this to your system administrator.", "SQL Database Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            return;
        }

        Log.Instance.Fatal(exc, "Unhandled Exception");
        MessageBox.Show("An unhandled exception has been encountered. Report this to your system administrator.", "Unhandled Exception", MessageBoxButtons.OK, MessageBoxIcon.Stop);
    }

    static void OnApplicationExit(object sender, EventArgs e)
    {
        NLog.LogManager.Shutdown();
    }
}
