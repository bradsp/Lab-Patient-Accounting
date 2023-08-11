using PetaPoco.Providers;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public class Database
    {
        protected PetaPoco.IDatabase dbConnection = null;
        private string connectionString;

        public Database(string connectionString)
        {
            this.connectionString = connectionString;
            Initialize();
        }

        private void Initialize()
        {
            dbConnection = DatabaseConfiguration
                .Build()
                .UsingConnectionString(connectionString)
                .UsingProvider<CustomSqlDatabaseProvider>(new CustomSqlDatabaseProvider())
                .UsingCommandTimeout(180)
                .WithAutoSelect()
                .UsingDefaultMapper<MyMapper>(new MyMapper())
                .Create();
        }
    }
}
