using LabBilling.Core;
using LabBilling.Core.BusinessLogic;
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
            PatientBilling patientBilling = new PatientBilling(_appEnvironment);

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
            ClaimGenerator claimGenerator = new ClaimGenerator(_appEnvironment);

            var response = AnsiConsole.Ask<int>("Enter batch number: ");

            if(AnsiConsole.Confirm($"Regenerate Claim batch {response}? "))
            {
                claimGenerator.RegenerateBatch(response);
            }            
        }

        public void FixDrugScreenCharges()
        {

            PetaPoco.Database dbConnection = new PetaPoco.Database(_appEnvironment.ConnectionString, new SqlServerMsDataDatabaseProvider());
            ChrgRepository chargeRepository = new ChrgRepository(_appEnvironment);
            AccountRepository accountRepository = new AccountRepository(_appEnvironment);
            //get list of accounts
            var sql = Sql.Builder;
            sql.From("chrg");
            sql.Where("cdm = @0", new SqlParameter() { SqlDbType = SqlDbType.VarChar, Value = "5362506" });
            sql.Where("account in (select account from acc where acc.trans_date > @0)", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(2023, 3, 1) });
            sql.Where("mod_date >= @0", new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = new DateTime(2023, 3, 1) });
            sql.Where("credited = 0");

            var charges = dbConnection.Fetch<Chrg>(sql);
            //loop through accounts
            foreach (var chrg in charges)
            {
                //credit 5362506
                chargeRepository.CreditCharge(chrg.ChrgId, "correct DAP7 cdm");
                Console.WriteLine($"Credited {chrg.AccountNo} {chrg.ChrgId} {chrg.Cdm}");
                //charge 5869007
                accountRepository.AddCharge(chrg.AccountNo, "5869007", chrg.Quantity, (DateTime)chrg.ServiceDate, "correct DAP7 cdm");
                Console.WriteLine($"Added {chrg.AccountNo} 5869007");

            }

        }

        public void NotesImport()
        {
            Console.WriteLine("Beginning notes import.");
            NotesImport notesImport = new NotesImport(_appEnvironment);
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
            AccountRepository accountRepository = new(_appEnvironment);
            Console.WriteLine("In RunValidation() - Starting RunValidation job");
            accountRepository.ValidateUnbilledAccounts();
            Console.WriteLine("In RunValidation() - Finished RunValidation job");
        }

        public void ReprintInvoice()
        {
            ClientInvoices clientInvoices = new ClientInvoices(_appEnvironment);

            //string filename = clientInvoices.PrintInvoice("78630");

            //System.Diagnostics.Process.Start(filename);
        }

        public void GenerateStatement()
        {
            ClientInvoices clientInvoice = new ClientInvoices(_appEnvironment);

            //string filename = clientInvoice.GenerateStatement("HESC", DateTime.Today.AddDays(-120));

            //System.Diagnostics.Process.Start(filename);
        }
        public void ProcessInterfaceMessages()
        {
            HL7Processor hL7Processor = new HL7Processor(_appEnvironment);
            hL7Processor.ProcessMessages();
            Console.WriteLine("Messages processed.");
        }

        public void SwapInsurance()
        {
            Console.Clear();

            var accountRepository = new AccountRepository(_appEnvironment);

            var accountNo = AnsiConsole.Ask<string>("Enter account number: ");

            var account = accountRepository.GetByAccount(accountNo);

            if(account == null)
            {
                AnsiConsole.Markup($"[red]Account {accountNo} not found.[/]  Enter a valid account. \n");

                var response = AnsiConsole.Ask<char>("Press any key to try again, or X to exit...");
                if (response == 'X' || response == 'x')
                    return;
                else
                {
                    SwapInsurance();
                    return;
                }
            }



            var swaps = AnsiConsole.Prompt(new MultiSelectionPrompt<InsCoverage>()
                .PageSize(10)
                .Title("Select insurances to swap: ")
                .InstructionsText("[grey](Press [blue][/] to toggle an insurance, [green][/] to accept)[/]")
                .AddChoices<InsCoverage>(new InsCoverage[] { InsCoverage.Primary, InsCoverage.Secondary, InsCoverage.Tertiary }));

            if(swaps.Count > 0)
            {

            }

        }

        public void ProviderAddEdit()
        {
            Phy phy = new Phy();

            phy.LastName = Prompt("Last Name: ");
            phy.FirstName = Prompt("First Name: ");
            phy.MiddleInitial = Prompt("Middle Initial: ");
            phy.Address1 = Prompt("Address 1: ");
            phy.Address2 = Prompt("Address 2: ");
            phy.City = Prompt("City: ");
            phy.State = Prompt("State: ");
            phy.ZipCode = Prompt("Zip Code: ");
            phy.Phone = Prompt("Phone: ");
            phy.Credentials = Prompt("Credentials: ");
            phy.DoctorNumber = Prompt("Doctor Number: ");
            phy.NpiId = Prompt("NPI: ");
            phy.BillingNpi = Prompt("Billing NPI: ");

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
            ClaimGenerator claimGenerator = new ClaimGenerator(_appEnvironment);

            CancellationToken cancellationToken = new CancellationToken();
            Progress<ProgressReportModel> progressReportModel = new Progress<ProgressReportModel>();

            Console.WriteLine("In RunClaimsProcessing() - Starting ClaimsProcessing job");
            Log.Instance.Info("In RunClaimsProcessing() - Starting ClaimsProcessing job");

            try
            {
                int claimsProcessed = 0;
                claimsProcessed = claimGenerator.CompileBillingBatch(LabBilling.Core.ClaimType.Institutional, progressReportModel, cancellationToken);

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

                claimsProcessed = claimGenerator.CompileBillingBatch(LabBilling.Core.ClaimType.Professional, progressReportModel, cancellationToken);

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
