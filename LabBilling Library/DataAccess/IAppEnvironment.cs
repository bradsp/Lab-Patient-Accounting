using LabBilling.Core.Models;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    public interface IAppEnvironment
    {
        ApplicationParameters ApplicationParameters { get; set; }
        string ConnectionString { get; }
        string ConnectionStringService { get; }
        IDatabase Database { get; }
        string DatabaseName { get; set; }
        string Environment { get; set; }
        string LogConnectionString { get; }
        string LogDatabaseName { get; set; }
        string ServerName { get; set; }
        string User { get; set; }
        bool EnvironmentValid { get; }
        DbContext Context { get; }

        string[] GetArgs();
    }
}