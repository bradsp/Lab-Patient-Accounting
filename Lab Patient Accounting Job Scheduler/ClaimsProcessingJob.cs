using LabBilling.Core.Models;
using LabBilling.Core.Services;
using Quartz;


namespace LabBillingJobs;

public partial class JobProcessor
{
    [DisallowConcurrentExecution]
    public class ClaimsProcessingJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"Starting Claims Processing. {DateTime.Now}");
            _log.Info($"Starting Claims Processing. {DateTime.Now}");

            try
            {
                Console.WriteLine("Try Task.Run() => RunClaimsProcessing()");
                _log.Debug("Try Task.Run() => RunClaimsProcessing()");
                await Task.Run(() => RunClaimsProcessing());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _log.Error(ex);
                throw new JobExecutionException(ex, true);
            }
        }

        public static void RunClaimsProcessing()
        {
            ClaimGeneratorService claimGenerator = new(Program.AppEnvironment);

            CancellationToken cancellationToken = new();
            Progress<ProgressReportModel> progressReportModel = new();

            Console.WriteLine("In RunClaimsProcessing() - Starting ClaimsProcessing job");
            _log.Info("In RunClaimsProcessing() - Starting ClaimsProcessing job");

            try
            {
                int claimsProcessed = 0;
                claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Institutional, progressReportModel, cancellationToken);

                if (claimsProcessed < 0)
                {
                    _log.Info("Error processing institutional claims. No file generated.");
                    Console.WriteLine("Error processing institutional claims. No file generated.");
                }
                else
                {
                    _log.Info($"Institutional claim file generated. {claimsProcessed} claims generated.");
                    Console.WriteLine($"Institutional claim file generated. {claimsProcessed} claims generated.");
                }
            }
            catch (TaskCanceledException tce)
            {
                _log.Error($"Institutional claim Batch cancelled by user", tce);
                Console.WriteLine("Institutional claim batch cancelled by user. No file was generated and batch has been rolled back.");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }

            try
            {
                int claimsProcessed = 0;

                claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Professional, progressReportModel, cancellationToken);

                if (claimsProcessed < 0)
                {
                    _log.Info("Error processing professional claims. No file generated.");
                    Console.WriteLine("Error processing professional claims. No file generated.");
                }
                else
                {
                    _log.Info($"Professional file generated. {claimsProcessed} claims generated.");
                    Console.WriteLine($"Professional file generated. {claimsProcessed} claims generated.");
                }

            }
            catch (TaskCanceledException tce)
            {
                _log.Error($"Claim Batch cancelled by user", tce);
                Console.WriteLine("Professional claim batch cancelled by user. No file was generated and batch has been rolled back.");
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("In RunClaimsProcessing() - Finished ClaimsProcessing job");
            _log.Info("In RunClaimsProcessing() - Finished ClaimsProcessing job");
        }

    }
}
