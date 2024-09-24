using LabBilling.Core.Models;
using PetaPoco;
using System.Drawing;

namespace LabBilling.Core.DataAccess;

public interface IAppEnvironment
{
    ApplicationParameters ApplicationParameters { get; set; }
    string ConnectionString { get; }
    string ConnectionStringService { get; }
    string DatabaseName { get; set; }
    string Environment { get; set; }
    string LogConnectionString { get; }
    string LogDatabaseName { get; set; }
    string ServerName { get; set; }
    string User { get; set; }
    bool EnvironmentValid { get; }
    Color WindowBackgroundColor { get; set; }
    Color WindowTextColor { get; set; }
    Color MenuBackgroundColor { get; set; }
    Color MenuTextColor { get; set; }
    Color ButtonBackgroundColor { get; set; }
    Color ButtonTextColor { get; set; }
    string TempFilePath { get; }

    string[] GetArgs();
}