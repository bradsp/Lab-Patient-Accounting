using LabBilling.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities;
using System.IO;
using LabBilling.Core.Models;
using Newtonsoft.Json.Linq;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.Services
{
    public sealed class NotesImportService
    {
        private IAppEnvironment appEnvironment;
        public NotesImportService(IAppEnvironment appEnvironment)
        {
            if (appEnvironment == null) throw new ArgumentNullException(nameof(appEnvironment));
            if (!appEnvironment.EnvironmentValid) throw new ArgumentException("App Environment is not valid.");

            this.appEnvironment = appEnvironment;
        }
        public void ImportNotes(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                using UnitOfWorkMain unitOfWork = new(appEnvironment, true);
                string line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split('|');
                        if (fields.Length > 0)
                        {
                            AccountNote accountNote = new()
                            {
                                Account = fields[1]
                            };
                            DateTime noteDate = DateTime.MinValue;
                            noteDate = noteDate.ValidateDate(fields[6]);

                            string comment = $"{noteDate} - {fields[5]}";
                            accountNote.Comment = comment;

                            unitOfWork.AccountNoteRepository.Add(accountNote);
                        }
                    }
                    unitOfWork.Commit();
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex, "Exception encountered importing notes.");
                    throw new ApplicationException("Error during notes import.", ex);
                }
            }
            string newfilename = $"{fileName}.processed";
            File.Move(fileName, newfilename);
        }

    }
}
