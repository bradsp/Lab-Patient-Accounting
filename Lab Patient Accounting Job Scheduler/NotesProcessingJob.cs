using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using Quartz;

namespace LabBillingJobs;

public partial class JobProcessor
{

    [DisallowConcurrentExecution]
    public class NotesProcessingJob : IJob
    {
        private UnitOfWorkMain _uow = new UnitOfWorkMain(Program.AppEnvironment);
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Starting Notes Import. {DateTime.Now}");
            _log.Info($"Starting Notes Import. {DateTime.Now}");

            try
            {
                Console.WriteLine("Try Task.Run() => NotesImport()");
                _log.Debug("Try Task.Run() => NotesImport()");
                await Task.Run(() => NotesImport());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.Error(ex);
                throw new JobExecutionException(ex, true);
            }

        }

        public void NotesImport()
        {
            Console.WriteLine("Beginning notes import.");
            NotesImportService notesImport = new NotesImportService(Program.AppEnvironment, _uow);

            try
            {
                foreach (string filename in Directory.GetFiles(@"\\wthmclbill\shared\Billing\LIVE\claims\Notes", "*.exted"))
                {
                    notesImport.ImportNotes(filename);
                    Console.WriteLine($"Imported {filename}");
                    _log.Info($"Imported {filename}");
                }
                Console.WriteLine("Notes Import complete");
                _log.Info("Notes Import complete");
            }
            catch (Exception ex)
            {
                _log.Error(ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
