using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using LabBilling.Core.Services;
using LabBilling.Forms;

namespace LabBilling;

static class Program
{
    public static UserAccount LoggedInUser { get; set; }
    public static AppEnvironment AppEnvironment { get; private set; }
    public static UnitOfWorkMain UnitOfWork { get; private set; }
    public static UnitOfWorkSystem UnitOfWorkSystem { get; private set; }


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
            UnitOfWorkSystem = new(AppEnvironment);
        }
        catch (Exception ex)
        {
            Log.Instance.Fatal("Failed to initialize application environment.", ex);
            MessageBox.Show("Failed to initialize application environment. Please check the configuration and try again.", "Initialization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Create AuthenticationService
        var authenticationService = new AuthenticationService(UnitOfWorkSystem);
        //This app will default to integrated authentication.
        //Pass the windows username to the AuthenticateIntegrated method.
        var windowsUsername = Environment.UserName;
        Log.Instance.Info($"Windows Username: {windowsUsername}");

        // Pass the windows username to the AuthenticateIntegrated method.
        var user = authenticationService.AuthenticateIntegrated(windowsUsername);

        if (user.Access == UserStatus.None)
        {
            Log.Instance.Error($"User not authorized: {windowsUsername}");
            MessageBox.Show("User not authorized. Please contact your system administrator.", "User Not Authorized", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        if (user == null)
        {
            //integrated login has failed. Ask user for a username and password and attempt to authenticate.
            Log.Instance.Info("Integrated login failed. Prompting for username and password.");

            int maxAttempts = 3;
            int attemptCount = 0;
            bool isAuthenticated = false;

            do
            {
                using LoginForm loginForm = new();
                var result = loginForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string username = loginForm.Username;
                    string password = loginForm.Password;
                    attemptCount++;

                    // Authenticate the user
                    var authResult = authenticationService.Authenticate(username, password);

                    if (authResult.isAuthenticated)
                    {
                        // Retrieve the authenticated user account
                        LoggedInUser = authResult.user;

                        // Complete AppEnvironment initialization
                        AppEnvironment.UserAccount = LoggedInUser;
                        AppEnvironment.User = LoggedInUser.UserName;
                        AppEnvironment.ApplicationParameters = UnitOfWorkSystem.SystemParametersRepository.LoadParameters();

                        // Set up UnitOfWork for the main application
                        UnitOfWork = new UnitOfWorkMain(AppEnvironment);

                        isAuthenticated = true;
                        break;
                    }
                    else
                    {
                        Log.Instance.Warn($"Authentication failed for user '{username}'. Attempt {attemptCount} of {maxAttempts}.");
                        MessageBox.Show("Invalid username or password.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // User canceled the login form
                    Log.Instance.Info("User canceled the login. Exiting application.");
                    Application.Exit();
                    return;
                }
            }
            while (attemptCount < maxAttempts);

            if (!isAuthenticated)
            {
                Log.Instance.Error("Maximum login attempts exceeded. Exiting application.");
                MessageBox.Show("Maximum login attempts exceeded.", "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

        }

        LoggedInUser = user;
        //complete AppEnvironment Initialization
        AppEnvironment.UserAccount = LoggedInUser;
        AppEnvironment.User = LoggedInUser.UserName;
        AppEnvironment.ApplicationParameters = UnitOfWorkSystem.SystemParametersRepository.LoadParameters();

        //set up UnitOfWork for the main application
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

        AppEnvironment.LogDatabaseName = testEnvironment
            ? Properties.Settings.Default.TestLogDbName
            : Properties.Settings.Default.ProdLogDbName;

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
