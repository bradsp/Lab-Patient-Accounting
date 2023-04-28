using LabBilling.Core.Models;
using Org.BouncyCastle.Crypto.Tls;
using PetaPoco.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class AppEnvironment : IAppEnvironment
    {
        private PetaPoco.Database _database;

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
        }

        public string DatabaseName { get; set; }
        public string ServerName { get; set; }
        public string LogDatabaseName { get; set; }
        public string Environment { get; set; }

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
                        SystemParametersRepository repository = new SystemParametersRepository(this);
                        _appParms = repository.LoadParameters();
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
