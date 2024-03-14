using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LabBilling.Core.Services;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;
using Quartz;


namespace LabBillingJobs
{
    public partial class JobProcessor
    {
        public class ClaimsProcessingJob : IJob
        {
            public async Task Execute(IJobExecutionContext context)
            {
                Console.WriteLine($"Starting Claims Processing. {DateTime.Now}");
                log.Info($"Starting Claims Processing. {DateTime.Now}");

                try
                {
                    Console.WriteLine("Try Task.Run() => RunClaimsProcessing()");
                    log.Debug("Try Task.Run() => RunClaimsProcessing()");
                    await Task.Run(() => RunClaimsProcessing());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    log.Error(ex);
                    throw new JobExecutionException(ex, true);
                }
            }

            public static void RunClaimsProcessing()
            {
                ClaimGeneratorService claimGenerator = new(Program.AppEnvironment);

                CancellationToken cancellationToken = new();
                Progress<ProgressReportModel> progressReportModel = new Progress<ProgressReportModel>();

                Console.WriteLine("In RunClaimsProcessing() - Starting ClaimsProcessing job");
                log.Info("In RunClaimsProcessing() - Starting ClaimsProcessing job");                

                try
                {
                    int claimsProcessed = 0;
                    claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Institutional, progressReportModel, cancellationToken);

                    if (claimsProcessed < 0)
                    {
                        log.Info("Error processing institutional claims. No file generated.");
                        Console.WriteLine("Error processing institutional claims. No file generated.");
                    }
                    else
                    {
                        log.Info($"Institutional claim file generated. {claimsProcessed} claims generated.");
                        Console.WriteLine($"Institutional claim file generated. {claimsProcessed} claims generated.");
                    }
                }
                catch (TaskCanceledException tce)
                {
                    log.Error($"Institutional claim Batch cancelled by user", tce);
                    Console.WriteLine("Institutional claim batch cancelled by user. No file was generated and batch has been rolled back.");
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    Console.WriteLine(ex.Message);
                }

                try
                {
                    int claimsProcessed = 0;

                    claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Professional, progressReportModel, cancellationToken);

                    if (claimsProcessed < 0)
                    {
                        log.Info("Error processing professional claims. No file generated.");
                        Console.WriteLine("Error processing professional claims. No file generated.");
                    }
                    else
                    {
                        log.Info($"Professional file generated. {claimsProcessed} claims generated.");
                        Console.WriteLine($"Professional file generated. {claimsProcessed} claims generated.");
                    }

                }
                catch (TaskCanceledException tce)
                {
                    log.Error($"Claim Batch cancelled by user", tce);
                    Console.WriteLine("Professional claim batch cancelled by user. No file was generated and batch has been rolled back.");
                }
                catch(Exception ex)
                {
                    log.Error(ex.Message);
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("In RunClaimsProcessing() - Finished ClaimsProcessing job");
                log.Info("In RunClaimsProcessing() - Finished ClaimsProcessing job");
            }

        }
    }
}
