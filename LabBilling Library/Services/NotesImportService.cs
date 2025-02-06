using LabBilling.Core.DataAccess;
using System;
using Utilities;
using System.IO;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.Services;

public sealed class NotesImportService
{
    private IAppEnvironment appEnvironment;
    private readonly IUnitOfWork _uow;
    public NotesImportService(IAppEnvironment appEnvironment, IUnitOfWork uow)
    {
        ArgumentNullException.ThrowIfNull(appEnvironment);
        if (!appEnvironment.EnvironmentValid) throw new ArgumentException("App Environment is not valid.");

        this.appEnvironment = appEnvironment;
        _uow = uow;
    }
    public void ImportNotes(string fileName)
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            _uow.StartTransaction();
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

                        _uow.AccountNoteRepository.Add(accountNote);
                    }
                }
                _uow.Commit();
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
