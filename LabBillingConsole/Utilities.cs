using LabBilling.Core;
using LabBilling.Core.Services;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using PetaPoco;
using Spectre.Console;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using LabBilling.Logging;
using LabBilling.Core.UnitOfWork;

namespace LabBillingConsole
{
    public sealed class Utilities : MenuBase
    {

        public Utilities(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            
        }

        public override bool LaunchMenu()
        {
            StringBuilder menuText = new StringBuilder();
            menuText.AppendLine($"Database: {_appEnvironment.DatabaseName}\n\n");
            menuText.AppendLine("Choose an option:");
            menuText.AppendLine("2) Validate Accounts");
            menuText.AppendLine("3) Add/Edit Provider");
            menuText.AppendLine("4) Notes Import");
            menuText.AppendLine("5) Process Interface Messages");
            menuText.AppendLine("6) Reprint Invoice");
            menuText.AppendLine("7) Generate Statement");
            menuText.AppendLine("8) Regenerate Claim Batch");
            menuText.AppendLine("9) Fix Drug Screen Charges");
            menuText.AppendLine("10) Swap Insurances");
            menuText.AppendLine("12) Regenerate Collections File");
            menuText.AppendLine("13) Run Claims Batch");
            menuText.AppendLine("X) Exit");

            var panel = new Panel(menuText.ToString());
            panel.Header = new PanelHeader("Lab Patient Accounting Utility Menu");
            panel.Border = BoxBorder.Square;
            panel.Expand = true;
            panel.Padding = new Padding(2, 2, 2, 2);

            AnsiConsole.Write(panel);

            var selected = AnsiConsole.Ask<string>("Select an option: ");

            return ExecuteMenuSelection(selected);
        }

        public override bool ExecuteMenuSelection(string selection)
        {
            switch (selection)
            {
                case "2":
                    Console.Clear();
                    ValidateAccountsJob();
                    return true;
                case "3":
                    Console.Clear();
                    ProviderAddEdit();
                    return true;
                case "4":
                    Console.Clear();
                    NotesImport();
                    return true;
                case "5":
                    Console.Clear();
                    ProcessInterfaceMessages();
                    return true;
                case "6":
                    Console.Clear();
                    ReprintInvoice();
                    return true;
                case "7":
                    Console.Clear();
                    GenerateStatement();
                    return true;
                case "8":
                    Console.Clear();
                    RegenerateClaimBatch();
                    return true;
                case "9":
                    Console.Clear();
                    FixDrugScreenCharges();
                    return true;
                case "10":
                    Console.Clear();
                    SwapInsurance();
                    return true;
                case "12":
                    Console.Clear();
                    RegenerateCollectionsFile();
                    return true;
                case "13":
                    Console.Clear();
                    RunClaimsProcessing();
                    return true;
                case "X":
                    return false;
                case "x":
                    return false;
                default:
                    return true;
            }
        }

        public void RegenerateCollectionsFile()
        {
            PatientBillingService patientBilling = new PatientBillingService(_appEnvironment);

            DateTime tDate;

            tDate = AnsiConsole.Ask<DateTime>("Enter date of collection file to regenerate (mm/dd/yyyy): ");

            if(AnsiConsole.Confirm($"Confirm generating collection file for {tDate}? "))
            {
                //dateConfirmed = true;

                int processed = patientBilling.RegenerateCollectionsFile(tDate);

                Console.WriteLine($"{processed} records processed.");
                Console.Read();
            }
        }

        public void RegenerateClaimBatch()
        {
            ClaimGeneratorService claimGenerator = new ClaimGeneratorService(_appEnvironment);

            var response = AnsiConsole.Ask<int>("Enter batch number: ");

            if(AnsiConsole.Confirm($"Regenerate Claim batch {response}? "))
            {
                claimGenerator.RegenerateBatch(response);
            }            
        }

        public void FixDrugScreenCharges()
        {
            using UnitOfWorkMain unitOfWork = new(_appEnvironment, true);
            AccountService accountService = new(_appEnvironment);

            //get list of accounts
            var sql = Sql.Builder;
            sql.From("chrg");
            sql.Where("cdm = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "5362506" });
            sql.Where("account in (select account from acc where acc.trans_date > @0)", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(2023, 3, 1) });
            sql.Where("mod_date >= @0", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(2023, 3, 1) });
            sql.Where("credited = 0");

            var charges = unitOfWork.Context.Fetch<Chrg>(sql);
            //loop through accounts
            foreach (var chrg in charges)
            {
                //credit 5362506
                accountService.CreditCharge(chrg.ChrgId, "correct DAP7 cdm");
                Console.WriteLine($"Credited {chrg.AccountNo} {chrg.ChrgId} {chrg.Cdm}");
                //charge 5869007
                accountService.AddCharge(chrg.AccountNo, "5869007", chrg.Quantity, (DateTime)chrg.ServiceDate, "correct DAP7 cdm");
                Console.WriteLine($"Added {chrg.AccountNo} 5869007");

            }
            unitOfWork.Commit();
        }

        public void NotesImport()
        {
            Console.WriteLine("Beginning notes import.");
            NotesImportService notesImport = new(_appEnvironment);
            try
            {
                foreach (string filename in Directory.GetFiles(@"\\wthmclbill\shared\Billing\LIVE\claims\Notes", "*.exted"))
                {
                    notesImport.ImportNotes(filename);
                    Console.WriteLine($"Imported {filename}");
                }
                Console.WriteLine("Notes Import complete");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public void ValidateAccountsJob()
        {
            AccountService accountService = new(_appEnvironment);
            Console.WriteLine("In RunValidation() - Starting RunValidation job");
            accountService.ValidateUnbilledAccounts();
            Console.WriteLine("In RunValidation() - Finished RunValidation job");
        }

        public void ReprintInvoice()
        {


        }

        public void GenerateStatement()
        {

        }
        public void ProcessInterfaceMessages()
        {
            HL7ProcessorService hL7Processor = new HL7ProcessorService(_appEnvironment);
            hL7Processor.ProcessMessages();
            Console.WriteLine("Messages processed.");
        }

        public void SwapInsurance()
        {
            Console.Clear();
        }

        public void ProviderAddEdit()
        {
            Phy phy = new()
            {
                LastName = Prompt("Last Name: "),
                FirstName = Prompt("First Name: "),
                MiddleInitial = Prompt("Middle Initial: "),
                Address1 = Prompt("Address 1: "),
                Address2 = Prompt("Address 2: "),
                City = Prompt("City: "),
                State = Prompt("State: "),
                ZipCode = Prompt("Zip Code: "),
                Phone = Prompt("Phone: "),
                Credentials = Prompt("Credentials: "),
                DoctorNumber = Prompt("Doctor Number: "),
                NpiId = Prompt("NPI: "),
                BillingNpi = Prompt("Billing NPI: ")
            };

        }

        private string Prompt(string promptText)
        {
            var response = AnsiConsole
                .Prompt(new TextPrompt<string>(promptText)
                    .Validate(name => !string.IsNullOrEmpty(name))
                    .PromptStyle("grey"))
                .Trim();

            return response;
        }

        public void RunClaimsProcessing()
        {
            ClaimGeneratorService claimGenerator = new(_appEnvironment);

            CancellationToken cancellationToken = new CancellationToken();
            Progress<ProgressReportModel> progressReportModel = new Progress<ProgressReportModel>();

            Console.WriteLine("In RunClaimsProcessing() - Starting ClaimsProcessing job");
            Log.Instance.Info("In RunClaimsProcessing() - Starting ClaimsProcessing job");

            try
            {
                int claimsProcessed = 0;
                claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Institutional, progressReportModel, cancellationToken);

                if (claimsProcessed < 0)
                {
                    Log.Instance.Info("Error processing institutional claims. No file generated.");
                    Console.WriteLine("Error processing institutional claims. No file generated.");
                }
                else
                {
                    Log.Instance.Info($"Institutional claim file generated. {claimsProcessed} claims generated.");
                    Console.WriteLine($"Institutional claim file generated. {claimsProcessed} claims generated.");
                }
            }
            catch (TaskCanceledException tce)
            {
                Log.Instance.Error($"Institutional claim Batch cancelled by user", tce);
                Console.WriteLine("Institutional claim batch cancelled by user. No file was generated and batch has been rolled back.");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }

            try
            {
                int claimsProcessed = 0;

                claimsProcessed = claimGenerator.CompileBillingBatch(ClaimType.Professional, progressReportModel, cancellationToken);

                if (claimsProcessed < 0)
                {
                    Log.Instance.Info("Error processing professional claims. No file generated.");
                    Console.WriteLine("Error processing professional claims. No file generated.");
                }
                else
                {
                    Log.Instance.Info($"Professional file generated. {claimsProcessed} claims generated.");
                    Console.WriteLine($"Professional file generated. {claimsProcessed} claims generated.");
                }

            }
            catch (TaskCanceledException tce)
            {
                Log.Instance.Error($"Claim Batch cancelled by user", tce);
                Console.WriteLine("Professional claim batch cancelled by user. No file was generated and batch has been rolled back.");
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex.Message);
                Console.WriteLine(ex.Message);
            }

            Console.WriteLine("In RunClaimsProcessing() - Finished ClaimsProcessing job");
            Log.Instance.Info("In RunClaimsProcessing() - Finished ClaimsProcessing job");
        }

    }

}
