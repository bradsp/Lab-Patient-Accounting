using LabBilling.Core.Models;
using Org.BouncyCastle.Crypto.Tls;
using PetaPoco;
using PetaPoco.Providers;
using RFClassLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

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

        public SystemParametersRepository systemParametersRepository;

        public bool RunAsService { get; set; } = false;

        public AppEnvironment()
        {
            try
            {
                systemParametersRepository = new SystemParametersRepository(this);
            }
            catch (ApplicationException apex)
            {
                //ok to ignore
            }
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
                        SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                        {
                            InitialCatalog = DatabaseName,
                            DataSource = ServerName,
                            IntegratedSecurity = true,
                            ApplicationName = RFClassLibrary.OS.GetAppName(),
                            ConnectTimeout = 30
                        };

                        return myBuilder.ConnectionString;
                    }
                    else
                    {
                        SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder();
                        myBuilder.InitialCatalog = DatabaseName;
                        myBuilder.DataSource = ServerName;
                        myBuilder.IntegratedSecurity = false;
                        myBuilder.UserID = UserName;
                        myBuilder.Password = Password;
                        myBuilder.ConnectTimeout = 30;
                        myBuilder.ApplicationName = RFClassLibrary.OS.GetAppName();
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

                SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder();

                myBuilder.InitialCatalog = DatabaseName;
                myBuilder.DataSource = ServerName;
                myBuilder.IntegratedSecurity = false;
                myBuilder.UserID = ServiceUsername;
                myBuilder.Password = ServicePassword;
                myBuilder.ApplicationName = RFClassLibrary.OS.GetAppName();
                myBuilder.ConnectTimeout = 30;

                return myBuilder.ConnectionString;
            }
        }

        //public PetaPoco.IDatabase Database
        //{
        //    get
        //    {
        //        if (_database == null)
        //        {
        //            //_database = new PetaPoco.Database(ConnectionString, new CustomSqlDatabaseProvider());

        //            _database = DatabaseConfiguration
        //                .Build()
        //                .UsingConnectionString(ConnectionString)
        //                .UsingProvider<CustomSqlDatabaseProvider>(new CustomSqlDatabaseProvider())
        //                .UsingCommandTimeout(180)
        //                .WithAutoSelect()
        //                .UsingDefaultMapper<MyMapper>(new MyMapper())
        //                .Create();
                    
        //        }
        //        return _database;
        //    }
        //}

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
                        if (systemParametersRepository == null)
                            systemParametersRepository = new SystemParametersRepository(this);
                        _appParms = systemParametersRepository.LoadParameters();
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
                        ApplicationName = RFClassLibrary.OS.GetAppName(),
                        ConnectTimeout = 30
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
                        ApplicationName = RFClassLibrary.OS.GetAppName(),
                        ConnectTimeout = 30
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
