using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Utilities;

namespace LabBillingConsole;

public sealed class Testing : MenuBase
{
    private UnitOfWorkMain uow;

    public Testing(IAppEnvironment appEnvironment) : base(appEnvironment)
    {
        uow = new UnitOfWorkMain(appEnvironment);
    }

    public override bool LaunchMenu()
    {
        StringBuilder menuText = new();
        menuText.AppendLine($"Database:  {_appEnvironment.DatabaseName}\n\n");
        menuText.AppendLine("******** TESTING MENU ***********\n");
        menuText.AppendLine("Choose an option:");
        menuText.AppendLine("1) Remittance Test");
        menuText.AppendLine("3) Generate Claim Test");
        menuText.AppendLine("4) Populate Claim Detail Amount");
        menuText.AppendLine("5) Import ICD-10");
        menuText.AppendLine("6) Test send SFTP");
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
            case "4":
                Console.Clear();
                PopulateClaimDetailAmount();
                return false;
            case "5":
                Console.Clear();
                ImportICD10();
                return false;
            case "6":
                Console.Clear();
                TestSftp();
                return false;
            case "X":
                return false;
            case "x":
                return false;
            default:
                return true;
        }
    }

    private void TestSftp()
    {
        string filename = @"\\mclftp2\MCLFTP_E\MSCB\test\test.txt";

        SFTP.UploadSftp(filename, "test/" + Path.GetFileName(filename),
            "sftp.mscb-inc.com", 22,
            "CLIENT156",
            @"e?`5H,\*");

    }

    private void ImportICD10()
    {
        DictionaryImportService dictionaryImport = new(_appEnvironment);
        dictionaryImport.RecordProcessed += OnRecordProcessed;
        dictionaryImport.ImportICD(@"C:\Users\bpowers\Downloads\icd10cm-CodesDescriptions-2024\icd10cm-order-2024.txt", "2024", _appEnvironment);

    }

    private void OnRecordProcessed(object source, RecordProcessedArgs args)
    {
        Console.WriteLine($"ICD Code: {args.IcdCode}   Records processed: {args.RecordsProcessed}");
    }

    private void PopulateClaimDetailAmount()
    {
        using UnitOfWorkMain unitOfWork = new(_appEnvironment);
        unitOfWork.StartTransaction();

        var records = unitOfWork.BillingActivityRepository.GetByRunDate(new DateTime(2023, 7, 1), new DateTime(2023, 8, 2));
        int cnt = 0;
        foreach (var record in records)
        {
            //parse json from record.Text
            var claim = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimData>(record.Text);

            if (claim == null) continue;

            record.ClaimAmount = claim.TotalChargeAmount;

            unitOfWork.BillingActivityRepository.Update(record, new string[] { nameof(BillingActivity.ClaimAmount) });
            Console.WriteLine($"{++cnt} - Account {record.AccountNo} - Claim Amount {record.ClaimAmount}");

        }
        unitOfWork.Commit();
        Console.WriteLine($"Completed updating claim amount on {cnt} claims.");
        Console.ReadKey();
    }

    public void RemittanceTest()
    {
        string file = @"\\wthmclbill\shared\Billing\LIVE\claims\835\MCL_MCR_10937.IMTN1.283257.20241126.092201844.ERA.835";

        Remittance835Service remittance835 = new(_appEnvironment);

        var result = remittance835.Load835(file);

        if (result == null)
        {
            Console.WriteLine("Error loading 835 file");
            return;
        }

        Console.WriteLine($"Loaded 835 file.\nPress any key to continue...");
        Console.ReadKey();

        //write RemittanceData to JSON file
        string jsonFile = @"c:\temp\remittance.json";
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(jsonFile, json);
        Console.WriteLine("Remittance data written to JSON file.\nPress any key to exit...");
        Console.ReadKey();

    }

    public void TestClientInvoices()
    {
        ClientInvoicesService clientInvoices = new(_appEnvironment);

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
        //InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

        //invoicePrint.CreateInvoicePdf(invModel, filename);

        System.Diagnostics.Process.Start(filename);
    }

    public void GenerateClaimTest()
    {
        //ClaimGenerator claimGenerator = new ClaimGenerator(connectionString);

        //claimGenerator.CompileClaim("L17429213");

        Billing837Service billing837 = new Billing837Service(_appEnvironment.ApplicationParameters.GetProductionEnvironment());

    }


}
