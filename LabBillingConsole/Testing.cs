using LabBilling.Core;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabBillingConsole
{
    public sealed class Testing : MenuBase
    {
        public Testing(IAppEnvironment appEnvironment) : base(appEnvironment)
        {
            
        }

        public override bool LaunchMenu()
        {
            StringBuilder menuText = new StringBuilder();
            menuText.AppendLine($"Database:  {_appEnvironment.DatabaseName}\n\n");
            menuText.AppendLine("******** TESTING MENU ***********\n");
            menuText.AppendLine("Choose an option:");
            menuText.AppendLine("1) Remittance Test");
            menuText.AppendLine("3) Generate Claim Test");
            menuText.AppendLine("X) Exit");

            var panel = new Panel(menuText.ToString());
            panel.Header = new PanelHeader("Lab Patient Accounting Testing Menu");
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
                case "1":
                    Console.Clear();
                    RemittanceTest();
                    return true;
                case "3":
                    Console.Clear();
                    GenerateClaimTest();
                    return false;
                case "X":
                    return false;
                case "x":
                    return false;
                default:
                    return true;
            }
        }

        public void RemittanceTest()
        {
            string file = @"\\wthmclbill\shared\Billing\TEST\Posting835Remit\MCL_NC_MCR_1093705428_835_11119267.RMT";

            Remittance835 remittance835 = new Remittance835(_appEnvironment);

            remittance835.Load835(file);

        }

        public void TestClientInvoices()
        {
            ClientInvoices clientInvoices = new ClientInvoices(_appEnvironment);

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

        public void GenerateClaimTest()
        {
            //ClaimGenerator claimGenerator = new ClaimGenerator(connectionString);

            //claimGenerator.CompileClaim("L17429213");

            Billing837 billing837 = new Billing837(_appEnvironment.ConnectionString, "");

        }


    }
}
