using LabBilling.Core.Models;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.IO;

namespace LabBilling.Core.DataAccess;

public class AppEnvironment : IAppEnvironment
{
    public string DatabaseName { get; set; }
    public string ServerName { get; set; }
    public string LogDatabaseName { get; set; }
    public string Environment { get; set; }

    public string ServiceUsername { get; set; }
    public string ServicePassword { get; set; }

    public bool IntegratedAuthentication { get; set; }

    public UserAccount UserAccount { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public bool RunAsService { get; set; } = false;

    public Color WindowBackgroundColor { get; set; } = Color.White;
    public Color WindowTextColor { get; set; } = Color.Black;
    public Color MenuBackgroundColor { get; set; } = Color.White;
    public Color MenuTextColor { get; set; } = Color.Black;

    public Color ButtonBackgroundColor { get; set; } = Color.LightCyan;
    public Color ButtonTextColor { get; set; } = Color.Black;

    public string TempFilePath => Path.GetTempPath() + @"LABPA\";

    private const bool _dbEncrypt = false;
    private const bool _dbTrustServerCert = true;

    public AppEnvironment()
    {
        // Initialize any necessary properties or fields here
    }

    public bool EnvironmentValid
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                return false;
            }

            if (ApplicationParameters == null)
            {
                return false;
            }

            return true;
        }
    }

    public string ConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                return string.Empty;
            }

            try
            {
                SqlConnectionStringBuilder myBuilder = new()
                {
                    InitialCatalog = DatabaseName,
                    DataSource = ServerName,
                    IntegratedSecurity = IntegratedAuthentication,
                    ApplicationName = Utilities.OS.GetAppName(),
                    Encrypt = _dbEncrypt,
                    TrustServerCertificate = _dbTrustServerCert,
                    MultipleActiveResultSets = true,
                    MinPoolSize = 10,
                    MaxPoolSize = 200,
                    Pooling = true,
                    ConnectTimeout = 30
                };

                if (!IntegratedAuthentication)
                {
                    myBuilder.UserID = UserName;
                    myBuilder.Password = Password;
                }

                return myBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error building connection string.", ex);
                return string.Empty;
            }
        }
    }

    public string ConnectionStringService => ConnectionString;

    public string User { get; set; }

    private ApplicationParameters _appParms;
    public ApplicationParameters ApplicationParameters { get; set; }

    public string LogConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(LogDatabaseName))
            {
                return string.Empty;
            }

            try
            {
                SqlConnectionStringBuilder myBuilder = new()
                {
                    InitialCatalog = LogDatabaseName,
                    DataSource = ServerName,
                    IntegratedSecurity = IntegratedAuthentication,
                    ApplicationName = Utilities.OS.GetAppName(),
                    ConnectTimeout = 30,
                    Encrypt = _dbEncrypt,
                    MultipleActiveResultSets = true,
                    MinPoolSize = 10,
                    MaxPoolSize = 200,
                    Pooling = true,
                    TrustServerCertificate = _dbTrustServerCert,
                };


                if (!IntegratedAuthentication)
                {
                    myBuilder.UserID = UserName;
                    myBuilder.Password = Password;
                }

                return myBuilder.ConnectionString;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error building log connection string.", ex);
                return string.Empty;
            }
        }
    }

    public string[] GetArgs()
    {
        string[] args = new string[2];

        args[0] = ServerName;
        args[1] = DatabaseName;

        return args;
    }
}
