using LabBilling.Core.Models;
using Org.BouncyCastle.Crypto.Tls;
using PetaPoco.Providers;
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

        public SystemParametersRepository systemParametersRepository;

        private PetaPoco.Database _database;

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

                    SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                    {
                        InitialCatalog = DatabaseName,
                        DataSource = ServerName,
                        IntegratedSecurity = true,
                        ConnectTimeout = 30
                    };

                    return myBuilder.ConnectionString;
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
                myBuilder.ConnectTimeout = 30;

                return myBuilder.ConnectionString;
            }
        }

        public PetaPoco.Database Database
        {
            get
            {
                if (_database == null)
                    _database = new PetaPoco.Database(ConnectionString, new CustomSqlDatabaseProvider());

                return _database;
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
                SqlConnectionStringBuilder myBuilder = new SqlConnectionStringBuilder
                {
                    InitialCatalog = LogDatabaseName,
                    DataSource = ServerName,
                    IntegratedSecurity = true,
                    ConnectTimeout = 30
                };

                return myBuilder.ConnectionString;
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
