using LabBilling.Core.Services;
using Quartz;
using Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBillingJobs
{
    public partial class JobProcessor
    {
        public class NotesProcessingJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Starting Notes Import. {DateTime.Now}");
                log.Info($"Starting Notes Import. {DateTime.Now}");

                try
                {
                    Console.WriteLine("Try Task.Run() => NotesImport()");
                    log.Debug("Try Task.Run() => NotesImport()");
                    await Task.Run(() => NotesImport());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    log.Error(ex);
                    throw new JobExecutionException(ex, true);
                }

            }

            public void NotesImport()
            {
                Console.WriteLine("Beginning notes import.");
                NotesImportService notesImport = new NotesImportService(Program.AppEnvironment);

                try
                {
                    foreach (string filename in Directory.GetFiles(@"\\wthmclbill\shared\Billing\LIVE\claims\Notes", "*.exted"))
                    {
                        notesImport.ImportNotes(filename);
                        Console.WriteLine($"Imported {filename}");
                        log.Info($"Imported {filename}");
                    }
                    Console.WriteLine("Notes Import complete");
                    log.Info("Notes Import complete");
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
