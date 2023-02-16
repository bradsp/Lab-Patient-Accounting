using LabBilling.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RFClassLibrary;
using System.IO;
using LabBilling.Core.Models;
using Newtonsoft.Json.Linq;
using LabBilling.Logging;

namespace LabBilling.Core.BusinessLogic
{
    public class NotesImport
    {
        private string _connectionString;

        private AccountRepository _accountRepository;
        private AccountNoteRepository _accountNoteRepository;

        public NotesImport(string connectionString)
        {
            _connectionString = connectionString;
            _accountRepository = new AccountRepository(_connectionString);
            _accountNoteRepository = new AccountNoteRepository(_connectionString);

        }
        public void ImportNotes(string fileName)
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] fields = line.Split('|');
                        if (fields.Length > 0)
                        {
                            AccountNote accountNote = new AccountNote();
                            accountNote.Account = fields[1];
                            DateTime noteDate = DateTime.MinValue;
                            noteDate = noteDate.ValidateDate(fields[3]);

                            string comment = $"{noteDate} - {fields[5]}";
                            accountNote.Comment = comment;

                            _accountNoteRepository.Add(accountNote);
                        }
                    }

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
