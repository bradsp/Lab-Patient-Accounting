using LabBilling.Core.Models;
using System;
using Microsoft.Data.SqlClient;
using System.Drawing;
using LabBilling.Core.UnitOfWork;
using LabBilling.Core.Services;

namespace LabBilling.Core.DataAccess
{
    public class AppEnvironment : IAppEnvironment
    {
        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string LogDatabaseName { get; set; }
        public string Environment { get; set; }

        public string ServiceUsername { get; set; }
        public string ServicePassword { get; set; }

        public bool IntegratedAuthentication { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }

        public bool RunAsService { get; set; } = false;

        public Color WindowBackgroundColor { get; set; } = Color.White;
        public Color WindowTextColor { get; set; } = Color.Black;
        public Color MenuBackgroundColor { get; set; } = Color.White;
        public Color MenuTextColor { get; set; } = Color.Black;

        public Color ButtonBackgroundColor { get; set; } = Color.LightCyan;
        public Color ButtonTextColor { get; set; } = Color.Black;

        private const bool dbEncrypt = false;
        private const bool dbTrustServerCert = true;

        public AppEnvironment()
        {

        }

        public bool EnvironmentValid { 
            get 
            {
                if (string.IsNullOrEmpty(ServerName))
                {
                    return false;
                }

                if (string.IsNullOrEmpty(DatabaseName))
                {
                    return false;
                }

                if(ApplicationParameters == null)
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
                if (!RunAsService)
                {
                    if (string.IsNullOrEmpty(ServerName))
                    {
                        throw new ApplicationException("ServerName value not set.");
                    }

                    if (string.IsNullOrEmpty(DatabaseName))
                    {
                        throw new ApplicationException("DatabaseName value not set.");
                    }

                    if(IntegratedAuthentication)
                    {
                        SqlConnectionStringBuilder myBuilder = new()
                        {
                            InitialCatalog = DatabaseName,
                            DataSource = ServerName,
                            IntegratedSecurity = true,
                            ApplicationName = Utilities.OS.GetAppName(),
                            Encrypt = dbEncrypt,
                            TrustServerCertificate = dbTrustServerCert,
                            ConnectTimeout = 30
                        };

                        return myBuilder.ConnectionString;
                    }
                    else
                    {
                        SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                        {
                            InitialCatalog = DatabaseName,
                            DataSource = ServerName,
                            IntegratedSecurity = false,
                            UserID = UserName,
                            Password = Password,
                            ConnectTimeout = 30,
                            Encrypt = dbEncrypt,
                            TrustServerCertificate = dbTrustServerCert,
                            ApplicationName = Utilities.OS.GetAppName()
                        };
                        return myBuilder.ConnectionString;
                    }
                }
                else
                {
                    return ConnectionStringService;
                }
            }
        }

        public string ConnectionStringService
        {
            get
            {
                if (string.IsNullOrEmpty(ServerName))
                {
                    throw new ApplicationException("ServerName value not set.");
                }

                if (string.IsNullOrEmpty(DatabaseName))
                {
                    throw new ApplicationException("DatabaseName value not set.");
                }

                if(string.IsNullOrEmpty(ServiceUsername))
                {
                    throw new ApplicationException("ServiceUserName value not set.");
                }

                if(string.IsNullOrEmpty(ServicePassword))
                {
                    throw new ApplicationException("ServicePassword value not set.");
                }

                SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                {
                    InitialCatalog = DatabaseName,
                    DataSource = ServerName,
                    IntegratedSecurity = false,
                    UserID = ServiceUsername,
                    Password = ServicePassword,
                    ApplicationName = Utilities.OS.GetAppName(),
                    ConnectTimeout = 30,
                    Encrypt = dbEncrypt,
                    TrustServerCertificate = dbTrustServerCert
                };

                return myBuilder.ConnectionString;
            }
        }

        public string User { get; set; }

        private ApplicationParameters _appParms;
        public ApplicationParameters ApplicationParameters 
        { 
            get
            {
                if (_appParms == null)
                {
                    ApplicationParameters = new ApplicationParameters();
                    if(EnvironmentValid)
                    {
                        SystemService systemService = new(this);
                        _appParms = systemService.LoadSystemParameters();
                    }    
                }
                return _appParms;
            }
            set
            {
                _appParms = value;
            }
        }

        public string LogConnectionString
        {
            get
            {
                if (IntegratedAuthentication)
                {
                    SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                    {
                        InitialCatalog = LogDatabaseName,
                        DataSource = ServerName,
                        IntegratedSecurity = true,
                        ApplicationName = Utilities.OS.GetAppName(),
                        ConnectTimeout = 30,
                        Encrypt = dbEncrypt,
                        TrustServerCertificate = dbTrustServerCert,
                    };

                    return myBuilder.ConnectionString;
                }
                else
                {
                    SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                    {
                        InitialCatalog = LogDatabaseName,
                        DataSource = ServerName,
                        IntegratedSecurity = false,
                        UserID = UserName,
                        Password = Password,
                        ApplicationName = Utilities.OS.GetAppName(),
                        ConnectTimeout = 30,
                        Encrypt = dbEncrypt,
                        TrustServerCertificate = dbTrustServerCert
                    };

                    return myBuilder.ConnectionString;
                }
            }
        }

        public string[] GetArgs()
        {
            //ConnectionString connString = Helper.ConnVal;

            string[] args = new string[2];

            args[0] = ServerName;
            args[1] = DatabaseName;

            return args;
        }
    }
}
