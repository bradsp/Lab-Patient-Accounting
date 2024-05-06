using LabBilling.Core.DataAccess;

namespace LabBilling.Core.Services;
public interface IDictionaryImportService
{
    event DictionaryImportService.RecordProcessedHandler RecordProcessed;

    void ImportICD(string filename, string year, IAppEnvironment appEnvironment);
}