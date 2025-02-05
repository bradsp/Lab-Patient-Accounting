using System;
using System.Collections.Generic;
using System.IO;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.Services;

public class RecordProcessedArgs : EventArgs
{
    public int RecordsProcessed { get; set; }
    public string IcdCode { get; set; }
}

public class DictionaryImportService
{
    public delegate void RecordProcessedHandler(object source, RecordProcessedArgs args);
    public event RecordProcessedHandler RecordProcessed;
    private readonly IUnitOfWork _uow;

    public DictionaryImportService(IAppEnvironment appEnvironment, IUnitOfWork uow)
    {
        _uow = uow;
    }

    public void ImportICD(string filename, string year, IAppEnvironment appEnvironment)
    {
        _uow.StartTransaction();

        List<IcdLines> icdLines = new List<IcdLines>();

        if(_uow.DictDxRepository.AMAYearExists(year))
        {
            return;
        }

        int processed = 0;
        try
        {
            //open file
            using (var fileStream = File.OpenRead(filename))
            using (var reader = new StreamReader(fileStream))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var icd = new IcdLines()
                    {
                        OrderNo = line.Substring(0, 5),
                        IcdCode = line.Substring(6, 7).Trim(),
                        Header = line.Substring(14, 1),
                        ShortDescription = line.Substring(16, 60).Trim(),
                        LongDescription = line.Substring(75).Trim()
                    };

                    var icd2 = new DictDx()
                    {
                        DxCode = line.Substring(6, 7).Trim(),
                        Description = line.Substring(16, 50).Trim(),
                        Version = year,
                        AmaYear = year,

                    };

                    //add icd to database
                    _uow.DictDxRepository.Add(icd2);
                    
                    RecordProcessed?.Invoke(this, new RecordProcessedArgs() { RecordsProcessed = processed++, IcdCode = icd2.DxCode });
                }
            }

            _uow.Commit();
        }
        catch (Exception ex)
        {
            Logging.Log.Instance.Error(ex);
            return;
        }
    }

    private class IcdLines
    {
        public string OrderNo;
        public string IcdCode;
        public string Header;
        public string ShortDescription;
        public string LongDescription;
    }

}
