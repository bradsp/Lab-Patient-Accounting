using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core;
using System.IO;
using RFClassLibrary;

namespace LabBillingConsole
{
    internal class Program
    {
        public const string connectionString = "Server=WTHMCLBILL;Database=LabBillingTest;Trusted_Connection=True;";

        static void Main(string[] args)
        {

            bool showMenu = true;

            while(showMenu)
            {
                showMenu = MainMenu();
            }

        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("******** TESTING MENU ***********\n");
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Remittance Test");
            Console.WriteLine("2) Validate Accounts");
            Console.WriteLine("3) Generate Claim Test");
            Console.WriteLine("4) Notes Import");
            Console.WriteLine("5) Process Interface Messages");
            Console.WriteLine("6) Reprint Invoice");
            Console.WriteLine("7) Generate Statement");
            Console.WriteLine("X) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    RemittanceTest();
                    return true;
                case "2":
                    ValidateAccountsJob();
                    return true;
                case "3":
                    GenerateClaimTest();
                    return false;
                case "4":
                    NotesImport();
                    return false;
                case "5":
                    ProcessInterfaceMessages();
                    return false;
                case "6":
                    ReprintInvoice();
                    return false;
                case "7":
                    GenerateStatement();
                    return false;
                case "X":
                    return false;
                case "x":
                    return false;
                default:
                    return true;
            }
        }

        public static void NotesImport()
        {
            Console.WriteLine("Beginning notes import.");
            NotesImport notesImport = new NotesImport(connectionString);
            //string filename = @"\\wthmclbill\shared\Billing\LIVE\claims\Notes\510051_20230215_ExportNotes_424.exted";
            try
            {
                foreach (string filename in Directory.GetFiles(@"\\wthmclbill\shared\Billing\LIVE\claims\Notes", "*.exted"))
                {
                    notesImport.ImportNotes(filename);
                    Console.WriteLine($"Imported {filename}");
                }
                Console.WriteLine("Notes Import complete");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }

        public static void GenerateClaimTest()
        {
            //ClaimGenerator claimGenerator = new ClaimGenerator(connectionString);

            //claimGenerator.CompileClaim("L17429213");

            Billing837 billing837 = new Billing837(connectionString, "");

        }

        public static void ValidateAccountsJob()
        {
            AccountRepository accountRepository = new AccountRepository(connectionString);
            Console.WriteLine("In RunValidation() - Starting RunValidation job");
            //log.Info("In RunValidation() - Starting RunValidation job");
            accountRepository.ValidateUnbilledAccounts();
            Console.WriteLine("In RunValidation() - Finished RunValidation job");
            //log.Info("In RunValidation() - Finished RunValidation job");
        }

        public static void AutomatationTest()
        {

        }

        public static void ReprintInvoice()
        {
            ClientInvoices clientInvoices = new ClientInvoices(connectionString);

            string filename = clientInvoices.PrintInvoice("77834");

            System.Diagnostics.Process.Start(filename);
        }

        public static void GenerateStatement()
        {
            ClientInvoices clientInvoice = new ClientInvoices(connectionString);

            string filename = clientInvoice.GenerateStatement("HESC", DateTime.Today.AddDays(-120));

            System.Diagnostics.Process.Start(filename);
        }


        public static void TestClientInvoices()
        {
            ClientInvoices clientInvoices = new ClientInvoices(connectionString);

            //clientInvoices.GenerateInvoice("WTCC", new DateTime(2022, 10, 31));

            //clientInvoices.ReprintInvoice("12345");

            InvoiceModel invModel = new InvoiceModel
            {
                StatementType = InvoiceModel.StatementTypeEnum.Invoice,
                ClientName = "WTTC",
                Address1 = "597 WEST FOREST COVE",
                City = "JACKSON",
                State = "TN",
                ZipCode = "38301",
                InvoiceNo = "75338",
                InvoiceDate = DateTime.Parse("02/02/2021"),
                InvoiceTotal = 21.00,
                BillingCompanyAddress = "PO Box 3099",
                BillingCompanyName = "Medical Center Laboratory",
                BillingCompanyCity = "Jackson",
                BillingCompanyState = "TN",
                BillingCompanyZipCode = "38303",
                BillingCompanyPhone = "731.541.7300 / 866.396.8537",
                ImageFilePath = @"\\wthmclbill\shared\billing\test\mcl-logo-horiz-300x60.jpg",
                InvoiceDetails = new List<InvoiceDetailModel>
                {
                    new InvoiceDetailModel
                    {
                        Account="L17138937",
                        PatientName = "MOUSE,MARION",
                        ServiceDate = DateTime.Parse("12/12/2020"),
                        AccountTotal = 13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = 1,
                                Amount = 13.54
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17158137",
                        PatientName = "DOE,JUNELEE",
                        ServiceDate = DateTime.Parse("01/09/2021"),
                        AccountTotal = 3.17 + 5.17,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "6127072",
                                CPT = "81001",
                                Description = "URINALYSIS W/MIC, C&S IF INDICATED",
                                Qty = 1,
                                Amount = 3.17
                            },
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5545154",
                                CPT = "88143",
                                Description = "CBC w/Auto Diff",
                                Qty = 1,
                                Amount = 5.17
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17158136",
                        PatientName = "DOE,MARCUS",
                        ServiceDate = DateTime.Parse("01/09/2021"),
                        AccountTotal = 13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = 1,
                                Amount = 13.54
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17163700",
                        PatientName = "DEER,MARGO",
                        ServiceDate = DateTime.Parse("01/17/2021"),
                        AccountTotal = 4.29,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5565120",
                                CPT = "85610",
                                Description = "PROTHROMBIN TIME",
                                Qty = 1,
                                Amount = 4.29
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="WTTC",
                        PatientName = "WEST TENNESSEE TRANSITIONAL CARE",
                        ServiceDate = DateTime.Parse("12/29/2020"),
                        AccountTotal = -13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = -1,
                                Amount = -13.54
                            }
                        }
                    },
                }
            };

            string filename = @"c:\temp\demo.pdf";
            //InvoicePrint.CreatePDF(invModel, filename);
            InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            invoicePrint.CreateInvoicePdf(invModel, filename);

            System.Diagnostics.Process.Start(filename);
        }

        public static void ProcessInterfaceMessages()
        {
            HL7Processor hL7Processor = new HL7Processor(connectionString);
            hL7Processor.ProcessMessages();
            Console.WriteLine("Messages processed.");
        }

        public static void SwapInsurance()
        {

            var accountRepository = new AccountRepository(connectionString);

            accountRepository.InsuranceSwap("L17436110", InsCoverage.Primary, InsCoverage.Secondary);

        }

        public static void RemittanceTest()
        {
            string file = @"\\wthmclbill\shared\Billing\TEST\Posting835Remit\MCL_NC_MCR_1093705428_835_11119267.RMT";

            Remittance835 remittance835 = new Remittance835(connectionString);

            remittance835.Load835(file);

        }

    }


}
