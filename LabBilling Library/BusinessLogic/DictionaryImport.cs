using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using NPOI.SS.Formula.Atp;

namespace LabBilling.Core.BusinessLogic
{
    public class RecordProcessedArgs : EventArgs
    {
        public int RecordsProcessed { get; set; }
        public string IcdCode { get; set; }
    }

    public class DictionaryImport
    {
        public delegate void RecordProcessedHandler(object source, RecordProcessedArgs args);
        public event RecordProcessedHandler RecordProcessed;

        public void ImportICD(string filename, string year, IAppEnvironment appEnvironment)
        {

            List<IcdLines> icdLines = new List<IcdLines>();
            DictDxRepository dictDxRepository = new DictDxRepository(appEnvironment);

            if(dictDxRepository.AMAYearExists(year))
            {
                return;
            }

            dictDxRepository.BeginTransaction();
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
                        dictDxRepository.Add(icd2);
                        
                        RecordProcessed?.Invoke(this, new RecordProcessedArgs() { RecordsProcessed = processed++, IcdCode = icd2.DxCode });
                    }
                }

                dictDxRepository.CompleteTransaction();
            }
            catch (Exception ex)
            {
                Logging.Log.Instance.Error(ex);
                dictDxRepository.AbortTransaction();
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
}
