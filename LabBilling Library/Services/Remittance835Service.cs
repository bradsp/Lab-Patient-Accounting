using EdiTools;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace LabBilling.Core.Services;


/*

ClaimAdjustmentGroupCode
•	CO (Contractual Obligations): Adjustments due to the terms of the contract between the payer and the provider. These adjustments are not the patient's responsibility.
•	CR (Corrections and Reversals): Adjustments made to correct a previous claim or to reverse a previous payment.
•	OA (Other Adjustments): Adjustments that do not fall into the other categories. These can include various reasons such as policy limitations or other payer-specific reasons.
•	PI (Payer Initiated Reductions): Adjustments initiated by the payer, often due to reasons such as medical necessity or coding errors.
•	PR (Patient Responsibility): Adjustments that are the patient's responsibility, such as deductibles, co-payments, and co-insurance.

AdjustmentReasonCode

•	1 (Deductible Amount): The amount of the claim that is the patient's responsibility due to the deductible.
•	2 (Coinsurance Amount): The amount of the claim that is the patient's responsibility due to coinsurance.
•	3 (Co-payment Amount): The amount of the claim that is the patient's responsibility due to a co-payment.
•	4 (The procedure code is inconsistent with the modifier used or a required modifier is missing): The procedure code is inconsistent with the modifier used or a required modifier is missing.
•	5 (The procedure code/bill type is inconsistent with the place of service): The procedure code/bill type is inconsistent with the place of service.
•	6 (The procedure/revenue code is inconsistent with the patient's age): The procedure/revenue code is inconsistent with the patient's age.
•	7 (The procedure/revenue code is inconsistent with
•	8 (The procedure code is inconsistent with the provider type/specialty (taxonomy)): The procedure code is inconsistent with the provider type/specialty (taxonomy).
•	9 (The diagnosis is inconsistent with the patient's age): The diagnosis is inconsistent with the patient's age.
•	10 (The diagnosis is inconsistent with the procedure): The diagnosis is inconsistent with the procedure.
•	11 (The diagnosis is inconsistent with the provider type): The diagnosis is inconsistent with the provider type.
•	12 (The diagnosis is inconsistent with the provider specialty (taxonomy)): The diagnosis is inconsistent with the provider specialty (taxonomy).
•	13 (The date of birth follows the invalid format): The date of birth follows the invalid format.
•	14 (The date of birth follows the invalid format): The date of birth follows the invalid format.
•	15 (The date of birth follows the invalid format): The date of birth follows the invalid format.
•	16 (The date of birth follows the invalid format): The date of birth follows the invalid format.
    
•	45 (Charge exceeds fee schedule/maximum allowable or contracted/legislated fee arrangement): 
    This adjustment amount cannot equal the total service or claim charge amount and must not duplicate provider adjustment amounts 
    (payments and contractual reductions) that have resulted from prior payer(s) adjudication. This code is used to indicate that 
    the billed amount is higher than the allowed amount according to the payer's fee schedule or contracted rate.
*   253 (Sequestration - reduction in federal spending): This code is used to indicate that the payment was reduced due to sequestration.
 
 */
public sealed class Remittance835Service
{
    private readonly IAppEnvironment _appEnvironment;
    private readonly List<string> _errors = new();


    public List<string> Errors { get { return _errors; } }

    public Remittance835Service(IAppEnvironment appEnvironment)
    {
        _appEnvironment = appEnvironment;
    }

    public void UpdateRemittance(RemittanceFile remittanceFile)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        try
        {
            uow.RemittanceRepository.Update(remittanceFile);
            uow.Commit();
        }
        catch (Exception ex)
        {
            _errors.Add($"Error updating remittance: {ex.Message}");
        }
    }

    public bool DeleteRemittance(RemittanceFile remittance)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        try
        {
            foreach (var claim in remittance.Claims)
            {
                foreach (var detail in claim.ClaimDetails)
                {
                    foreach (var adj in detail.Adjustments)
                    {
                        uow.RemittanceClaimAdjustmentRepository.Delete(adj);
                    }
                    uow.RemittanceClaimDetailRepository.Delete(detail);
                }
                uow.RemittanceClaimRepository.Delete(claim);
            }
            uow.RemittanceRepository.Delete(remittance);
            return true;
        }
        catch (Exception ex)
        {
            _errors.Add($"Error deleting remittance: {ex.Message}");
            return false;
        }
    }

    public RemittanceFile GetRemittanceByFileName(string filename)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        return uow.RemittanceRepository.GetByFilename(filename);
    }

    public RemittanceData ReimportRemittance(int remittanceId)
    {
        //delete existing RemittanceFile records
        using UnitOfWorkMain uow = new(_appEnvironment, true);
        try
        {
            var remit = GetRemittance(remittanceId);
            var filename = Path.Combine(_appEnvironment.ApplicationParameters.RemitImportDirectory, "archive", remit.FileName);
            this.DeleteRemittance(remit);

            RemittanceData remittance = Load835(filename);

            uow.Commit();
            return remittance;
        }
        catch (Exception ex)
        {
            _errors.Add($"Error deleting existing remittance records: {ex.Message}");
            return null;
        }
    }

    public RemittanceData Load835(string fileName)
    {
        EdiDocument ediDocument;
        string logfile = "c:\\temp\\edi.log";

        try
        {
            // Determine the delimiter by reading the first line of the file
            string firstLine = File.ReadLines(fileName).First();
            char delimiter = firstLine.Contains('|') ? '|' : '*';
            // Configure EdiOptions with the appropriate delimiter
            EdiOptions options = new()
            {
                SegmentTerminator = '~',
                ElementSeparator = delimiter,
                ComponentSeparator = ':',
                RepetitionSeparator = '^',
            };

            ediDocument = EdiDocument.Load(fileName, options);
            File.WriteAllText(logfile, $"Loaded EDI document {fileName}\n");
        }
        catch (Exception ex)
        {
            // Log or handle the error when loading the EDI document
            _errors.Add($"Error loading EDI document: {ex.Message}");
            return null;
        }

        RemittanceData remittance = new();
        Loop2000 loop2000 = null;
        Loop2100 loop2100 = null;
        Loop2110 loop2110 = null;
        Loop2110Adj loop2110Adj = null;

        string currentLoop = null;
        string currN1type = null;

        foreach (var segment in ediDocument.Segments)
        {
            try
            {
                File.AppendAllText(logfile, $"{segment}\n");
                switch (segment.Id)
                {
                    case "ISA":
                        File.AppendAllText(logfile, $"Processing ISA segment\n");
                        remittance.InterchangeControlNumber = segment[13];
                        break;
                    case "GS":
                        File.AppendAllText(logfile, $"Processing GS segment\n");
                        remittance.GroupControlNumber = segment[6];
                        break;
                    case "ST":
                        File.AppendAllText(logfile, $"Starting new transaction {segment[1]}\n");
                        remittance.TransactionSetControlNumber = segment[2];
                        break;
                    case "BPR":
                        File.AppendAllText(logfile, $"Processed BPR segment\n");
                        remittance.TransactionHandlingCode = segment[1];
                        remittance.TotalPremiumPaymentAmount = segment[2];
                        remittance.CreditorDebitFlagCode = segment[3];
                        remittance.PaymentDate = DateTime.ParseExact(segment[16], "yyyyMMdd", null);
                        break;
                    case "TRN":
                        File.AppendAllText(logfile, $"Processing TRN segment\n");
                        remittance.CurrentTransactionTraceNumber = segment[2];
                        break;
                    case "N1":
                        if (segment[1] == "PR")
                        {
                            File.AppendAllText(logfile, $"Processing PR segment\n");
                            currentLoop = "1000A";
                            currN1type = segment[1];
                            remittance.PayerName = segment[2];
                        }
                        if (segment[1] == "PE")
                        {
                            File.AppendAllText(logfile, $"Processing PE segment\n");
                            currentLoop = "1000B";
                            currN1type = segment[1];
                            remittance.PayeeName = segment[2];
                            remittance.BankIdentifier = segment[4];
                        }
                        break;
                    case "N3":
                        if (currN1type == "PR")
                        {
                            File.AppendAllText(logfile, $"Processing PR segment\n");
                            remittance.PayerAddress = segment[1];
                            remittance.PayerAddress2 = segment[2];
                        }
                        if (currN1type == "PE")
                        {
                            File.AppendAllText(logfile, $"Processing PE segment\n");
                            remittance.PayeeAddress = segment[1];
                        }
                        break;
                    case "N4":
                        if (currN1type == "PR")
                        {
                            File.AppendAllText(logfile, $"Processing PR segment\n");
                            remittance.PayerCity = segment[1];
                            remittance.PayerState = segment[2];
                            remittance.PayerZip = segment[3];
                        }
                        if (currN1type == "PE")
                        {
                            File.AppendAllText(logfile, $"Processing PE segment\n");
                            remittance.PayeeCity = segment[1];
                            remittance.PayeeState = segment[2];
                            remittance.PayeeZip = segment[3];
                        }
                        break;
                    case "LX":
                        currentLoop = "2000";
                        if (loop2000 != null)
                            remittance.Loop2000s.Add(loop2000);
                        loop2000 = new Loop2000();
                        File.AppendAllText(logfile, $"Processing LX segment loop2000\n");
                        break;
                    case "TS3":
                        loop2000.TotalClaimCount = segment[4];
                        loop2000.TotalClaimChargeAmount = segment[5];
                        loop2000.TotalHCPCSReportedChargeAmount = segment[17];
                        loop2000.TotalHCPCSPayableAmount = segment[18];
                        File.AppendAllText(logfile, $"Processing TS3 segment\n");
                        break;
                    case "CLP":
                        File.AppendAllText(logfile, $"Processing CLP segment loop2100\n");
                        File.AppendAllText(logfile, $"AccountNo: {segment[1]}\n");

                        if (loop2100 != null)
                        {
                            loop2100.Loop2110s.Add(loop2110);
                            loop2000.Loop2100s.Add(loop2100);
                            File.AppendAllText(logfile, $"Adding loop2100 to loop2000\n");
                        }

                        loop2100 = new Loop2100();
                        currentLoop = "2100";
                        loop2100.AccountNo = segment[1];
                        loop2100.ClaimStatusCode = segment[2];
                        loop2100.ClaimChargeAmount = segment[3];
                        loop2100.ClaimPaymentAmount = segment[4];
                        loop2100.PatientResponsibilityAmount = segment[5];
                        loop2100.ClaimFilingIndicatorCode = segment[6];
                        loop2100.PayerClaimControlNumber = segment[7];
                        loop2100.FacilityTypeCode = segment[8];
                        loop2100.ClaimFrequencyCode = segment[9];
                        loop2110 = null; // Reset loop2110 to ensure it doesn't carry over to the new loop2100
                        break;
                    case "NM1":
                        if (currentLoop == "2100" && segment[1] == "QC")
                        {
                            loop2100.PatientLastName = segment[3];
                            loop2100.PatientFirstName = segment[4];
                            loop2100.PatientMiddleName = segment[5];
                        }
                        if (currentLoop == "1000A" && segment[1] == "PR")
                        {
                            remittance.PayerContactName = segment[3];
                        }
                        break;
                    case "DMG":
                        if (currentLoop == "2100")
                        {
                            loop2100.PatientDateOfBirth = DateTime.ParseExact(segment[2], "yyyyMMdd", null);
                            loop2100.PatientGender = segment[3];
                        }
                        break;
                    case "PER":
                        //need to specify contact function code for payer info
                        if (currentLoop == "1000A")
                        {
                            remittance.PayerContactPhone = segment[4];
                            remittance.PayerContactEmail = segment[6];
                        }
                        break;
                    case "AMT":
                        if (currentLoop == "2100")
                        {
                            if (segment[1] == "B6")
                            {
                                loop2100.PaidAmount = segment[2];
                            }
                            if (segment[1] == "AU")
                            {
                                loop2100.AllowedAmount = segment[2];
                            }
                            File.AppendAllText(logfile, currentLoop + " " + segment[1] + " " + segment[2] + "\n");
                        }
                        else if (currentLoop == "2110")
                        {
                            if (segment[1] == "B6")
                            {
                                loop2110.PaidAmount = segment[2];
                            }
                            if (segment[1] == "AU")
                            {
                                loop2110.AllowedAmount = segment[2];
                            }
                            File.AppendAllText(logfile, currentLoop + " " + segment[1] + " " + segment[2] + "\n");
                        }
                        break;
                    case "SVC":
                        if (loop2110 != null)
                            loop2100.Loop2110s.Add(loop2110);

                        currentLoop = "2110";
                        loop2110 = new()
                        {
                            ProcedureCode = segment.Element(1).Component(2).ToString(),
                            LineItemChargeAmount = segment[2],
                            MonetaryAmount = segment[3],
                            RevenueCode = segment[4]
                        };
                        File.AppendAllText(logfile, $"Processing SVC segment loop2110\n");
                        break;
                    case "CAS":
                        if (loop2110 == null)
                        {
                            loop2110 = new Loop2110();
                            loop2100.Loop2110s.Add(loop2110);
                        }

                        loop2110Adj = new Loop2110Adj
                        {
                            ClaimAdjustmentGroupCode = segment[1],
                            AdjustmentReasonCode = segment[2],
                            AdjustmentAmount = segment[3],
                            AdjustmentQuantity = segment[4]
                        };

                        loop2110.Adjustments.Add(loop2110Adj);

                        if (segment[5] != null)
                        {
                            loop2110Adj = new Loop2110Adj
                            {
                                ClaimAdjustmentGroupCode = segment[1],
                                AdjustmentReasonCode = segment[5],
                                AdjustmentAmount = segment[6],
                                AdjustmentQuantity = segment[7]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        if (segment[8] != null)
                        {
                            loop2110Adj = new Loop2110Adj
                            {
                                ClaimAdjustmentGroupCode = segment[1],
                                AdjustmentReasonCode = segment[8],
                                AdjustmentAmount = segment[9],
                                AdjustmentQuantity = segment[10]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        if (segment[11] != null)
                        {
                            loop2110Adj = new Loop2110Adj
                            {
                                ClaimAdjustmentGroupCode = segment[1],
                                AdjustmentReasonCode = segment[11],
                                AdjustmentAmount = segment[12],
                                AdjustmentQuantity = segment[13]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        if (segment[14] != null)
                        {
                            loop2110Adj = new Loop2110Adj
                            {
                                ClaimAdjustmentGroupCode = segment[1],
                                AdjustmentReasonCode = segment[14],
                                AdjustmentAmount = segment[15],
                                AdjustmentQuantity = segment[16]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        if (segment[17] != null)
                        {
                            loop2110Adj = new Loop2110Adj
                            {
                                ClaimAdjustmentGroupCode = segment[1],
                                AdjustmentReasonCode = segment[17],
                                AdjustmentAmount = segment[18],
                                AdjustmentQuantity = segment[19]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        File.AppendAllText(logfile, $"Processing CAS segment loop2110\n");
                        break;
                    case "SE":
                        if (loop2110 != null)
                        {
                            loop2100.Loop2110s.Add(loop2110);
                            loop2110 = null;
                        }
                        if (loop2100 != null)
                        {
                            loop2000.Loop2100s.Add(loop2100);
                            loop2100 = null;
                        }
                        if (loop2000 != null)
                        {
                            remittance.Loop2000s.Add(loop2000);
                            loop2000 = null;
                        }
                        File.AppendAllText(logfile, $"Processing SE segment\n");
                        break;
                    default:
                        // Handle unknown segment
                        _errors.Add($"Unknown segment ID: {segment.Id}");
                        File.AppendAllText(logfile, $"Unknown segment ID: {segment.Id}\n");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error for the specific segment
                _errors.Add($"Error processing segment {segment.Id}: {ex.Message}");
            }
        }

        // Add the last loop2110 to loop2100 and the last loop2100 to loop2000
        if (loop2110 != null)
        {
            loop2100.Loop2110s.Add(loop2110);
        }
        if (loop2100 != null)
        {
            loop2000.Loop2100s.Add(loop2100);
        }
        if (loop2000 != null)
        {
            remittance.Loop2000s.Add(loop2000);
        }

        StoreRemittanceData(remittance, fileName);
        return remittance;
    }


    public string ConvertRemittanceHeaderToHtml(RemittanceData remittanceData)
    {
        var html = new StringBuilder();

        html.Append("<html><head><style>");
        html.Append("body { font-family: Arial, sans-serif; }");
        html.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
        html.Append("th, td { padding: 8px; text-align: left; }");
        html.Append("th { background-color: #f2f2f2; font-weight: bold; }");
        html.Append("tr:nth-child(even) { background-color: #f9f9f9; }");
        html.Append(".label { font-size: smaller; color: gray; }");
        html.Append(".smalltext { font-size: smaller; color: black; }");
        html.Append(".section { border: 1px solid #ccc; display: flex; margin-bottom: 20px; }");
        html.Append(".section-header { writing-mode: vertical-rl; text-orientation: mixed; background-color: #e0e0e0; padding: 10px; font-weight: bold; display: flex; align-items: center; justify-content: center; }");
        html.Append(".section-content { padding: 10px; flex-grow: 1; }");
        html.Append("</style></head><body>");

        html.Append("<div class='section'>");
        html.Append("<div class='section-header'>Payer Information</div>");
        html.Append("<div class='section-content'>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayerName}<br><span class='label'>Payer Name</span></td>");
        html.Append($"<td>{remittanceData.PayerAddress} {remittanceData.PayerAddress2}<br><span class='label'>Payer Address</span></td>");
        html.Append($"<td>{remittanceData.PayerCity}<br><span class='label'>Payer City</span></td>");
        html.Append($"<td>{remittanceData.PayerState}<br><span class='label'>Payer State</span></td>");
        html.Append($"<td>{remittanceData.PayerZip}<br><span class='label'>Payer Zip</span></td>");
        html.Append("</tr>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayerContactName}<br><span class='label'>Payer Contact Name</span></td>");
        html.Append($"<td>{remittanceData.PayerContactPhone}<br><span class='label'>Payer Contact Phone</span></td>");
        html.Append($"<td>{remittanceData.PayerContactEmail}<br><span class='label'>Payer Contact Email</span></td>");
        html.Append("</tr>");
        html.Append("</table>");
        html.Append("</div>");
        html.Append("</div>");

        html.Append("<div class='section'>");
        html.Append("<div class='section-header'>Payee Information</div>");
        html.Append("<div class='section-content'>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayeeName}<br><span class='label'>Payee Name</span></td>");
        html.Append($"<td>{remittanceData.PayeeAddress}<br><span class='label'>Payee Address</span></td>");
        html.Append($"<td>{remittanceData.PayeeCity}<br><span class='label'>Payee City</span></td>");
        html.Append($"<td>{remittanceData.PayeeState}<br><span class='label'>Payee State</span></td>");
        html.Append($"<td>{remittanceData.PayeeZip}<br><span class='label'>Payee Zip</span></td>");
        html.Append("</tr>");
        html.Append("</table>");
        html.Append("</div>");
        html.Append("</div>");

        html.Append("<div class='section'>");
        html.Append("<div class='section-header'>Transaction Information</div>");
        html.Append("<div class='section-content'>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.TransactionHandlingCode}<br><span class='label'>Transaction Handling Code</span></td>");
        html.Append($"<td>{decimal.Parse(remittanceData.TotalPremiumPaymentAmount ?? "0"):F2}<br><span class='label'>Total Premium Payment Amount</span></td>");
        html.Append($"<td>{remittanceData.CreditorDebitFlagCode}<br><span class='label'>Creditor Debit Flag Code</span></td>");
        html.Append($"<td>{remittanceData.PaymentDate?.ToString("yyyy-MM-dd")}<br><span class='label'>Payment Date</span></td>");
        html.Append("</tr>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.InterchangeControlNumber}<br><span class='label'>Interchange Control Number</span></td>");
        html.Append($"<td>{remittanceData.GroupControlNumber}<br><span class='label'>Group Control Number</span></td>");
        html.Append($"<td>{remittanceData.TransactionSetControlNumber}<br><span class='label'>Transaction Set Control Number</span></td>");
        html.Append($"<td>{remittanceData.CurrentTransactionTraceNumber}<br><span class='label'>Current Transaction Trace Number</span></td>");
        html.Append("</tr>");
        html.Append("</table>");
        html.Append("</div>");
        html.Append("</div>");

        html.Append("</body></html>");

        return html.ToString();
    }

    public string ConvertRemittanceHeaderToRtf(RemittanceData remittanceData)
    {
        var rtf = new StringBuilder();

        rtf.Append(@"{\rtf1\ansi\deff0{\fonttbl{\f0 Arial;}}");
        rtf.Append(@"\viewkind4\uc1\pard\lang1033\f0\fs20");

        // Payer Information Section
        rtf.Append(@"\b Payer Information\b0\par");
        rtf.Append(@"\trowd\trgaph108\trleft-108");
        rtf.Append(@"\cellx3000\cellx6000");
        rtf.Append(@"\intbl Payer Name:\cell\b ");
        rtf.Append($@"{remittanceData.PayerName}\b0\cell\row");
        rtf.Append(@"\intbl Payer Address:\cell\b ");
        rtf.Append($@"{remittanceData.PayerAddress} {remittanceData.PayerAddress2}\b0\cell\row");
        rtf.Append(@"\intbl Payer City:\cell\b ");
        rtf.Append($@"{remittanceData.PayerCity}\b0\cell\row");
        rtf.Append(@"\intbl Payer State:\cell\b ");
        rtf.Append($@"{remittanceData.PayerState}\b0\cell\row");
        rtf.Append(@"\intbl Payer Zip:\cell\b ");
        rtf.Append($@"{remittanceData.PayerZip}\b0\cell\row");
        rtf.Append(@"\intbl Payer Contact Name:\cell\b ");
        rtf.Append($@"{remittanceData.PayerContactName}\b0\cell\row");
        rtf.Append(@"\intbl Payer Contact Phone:\cell\b ");
        rtf.Append($@"{remittanceData.PayerContactPhone}\b0\cell\row");
        rtf.Append(@"\intbl Payer Contact Email:\cell\b ");
        rtf.Append($@"{remittanceData.PayerContactEmail}\b0\cell\row");
        rtf.Append(@"\pard\par");

        // Transaction Information Section
        rtf.Append(@"\b Transaction Information\b0\par");
        rtf.Append(@"\trowd\trgaph108\trleft-108");
        rtf.Append(@"\cellx3000\cellx6000");
        rtf.Append(@"\intbl Transaction Handling Code:\cell\b ");
        rtf.Append($@"{remittanceData.TransactionHandlingCode}\b0\cell\row");
        rtf.Append(@"\intbl Total Premium Payment Amount:\cell\b ");
        rtf.Append($@"{decimal.Parse(remittanceData.TotalPremiumPaymentAmount ?? "0"):F2}\b0\cell\row");
        rtf.Append(@"\intbl Creditor Debit Flag Code:\cell\b ");
        rtf.Append($@"{remittanceData.CreditorDebitFlagCode}\b0\cell\row");
        rtf.Append(@"\intbl Payment Date:\cell\b ");
        rtf.Append($@"{remittanceData.PaymentDate?.ToString("yyyy-MM-dd")}\b0\cell\row");
        rtf.Append(@"\intbl Interchange Control Number:\cell\b ");
        rtf.Append($@"{remittanceData.InterchangeControlNumber}\b0\cell\row");
        rtf.Append(@"\intbl Group Control Number:\cell\b ");
        rtf.Append($@"{remittanceData.GroupControlNumber}\b0\cell\row");
        rtf.Append(@"\intbl Transaction Set Control Number:\cell\b ");
        rtf.Append($@"{remittanceData.TransactionSetControlNumber}\b0\cell\row");
        rtf.Append(@"\intbl Current Transaction Trace Number:\cell\b ");
        rtf.Append($@"{remittanceData.CurrentTransactionTraceNumber}\b0\cell\row");
        rtf.Append(@"\pard\par");

        // Payee Information Section
        rtf.Append(@"\b Payee Information\b0\par");
        rtf.Append(@"\trowd\trgaph108\trleft-108");
        rtf.Append(@"\cellx3000\cellx6000");
        rtf.Append(@"\intbl Payee Name:\cell\b ");
        rtf.Append($@"{remittanceData.PayeeName}\b0\cell\row");
        rtf.Append(@"\intbl Payee Address:\cell\b ");
        rtf.Append($@"{remittanceData.PayeeAddress}\b0\cell\row");
        rtf.Append(@"\intbl Payee City:\cell\b ");
        rtf.Append($@"{remittanceData.PayeeCity}\b0\cell\row");
        rtf.Append(@"\intbl Payee State:\cell\b ");
        rtf.Append($@"{remittanceData.PayeeState}\b0\cell\row");
        rtf.Append(@"\intbl Payee Zip:\cell\b ");
        rtf.Append($@"{remittanceData.PayeeZip}\b0\cell\row");
        rtf.Append(@"\pard\par");

        rtf.Append(@"}");

        return rtf.ToString();
    }


    public string ConvertRemittanceDataToHtml(RemittanceData remittanceData)
    {
        DictionaryService dictService = new(_appEnvironment);
        Dictionary<string, string> COadjustmentCodes = new();
        Dictionary<string, string> PRadjustmentCodes = new();

        COadjustmentCodes.Add("109", "Not medically necessary - The service was deemed unnecessary for the patient's condition.");
        COadjustmentCodes.Add("11", "Procedures not covered - The procedure performed is not covered under the policy.");
        COadjustmentCodes.Add("115", "Duplicate claim/service - The claim was denied because it is a duplicate of a previously processed claim.");
        COadjustmentCodes.Add("119", "Not a covered service - The service provided is not included in the benefits of the plan.");
        COadjustmentCodes.Add("131", "Patient not eligible - The patient was not eligible for coverage at the time of service.");
        COadjustmentCodes.Add("151", "Service not covered by the plan - Indicates that the specific service is not covered by the insurance plan.");
        COadjustmentCodes.Add("16", "Claim/service lacks information - The claim is missing necessary information for processing.");
        COadjustmentCodes.Add("167", "Service not authorized - The service required prior authorization, which was not obtained.");
        COadjustmentCodes.Add("18", "Duplicate service - The service is considered a duplicate of another service billed.");
        COadjustmentCodes.Add("190", "Non-covered charges - Charges related to the service are not covered by the insurance.");
        COadjustmentCodes.Add("204", "Service not covered - The service rendered is not included in the coverage terms.");
        COadjustmentCodes.Add("226", "Exceeds maximum allowable - The billed amount exceeds the maximum allowable amount for the service.");
        COadjustmentCodes.Add("231", "Service not performed - Indicates that the service billed was not actually performed.");
        COadjustmentCodes.Add("234", "Not a valid procedure code - The procedure code used on the claim is invalid.");
        COadjustmentCodes.Add("236", "Service not provided as billed - The service billed does not match what was provided.");
        COadjustmentCodes.Add("252", "Payment adjusted due to contractual agreement - Payment was adjusted based on the terms of the contract.");
        COadjustmentCodes.Add("253", "Adjustment for contractual obligation - Indicates an adjustment based on contractual obligations.");
        COadjustmentCodes.Add("273", "Adjustment for non-compliance - Indicates an adjustment due to non-compliance with policy guidelines.");
        COadjustmentCodes.Add("288", "No prior authorization - Indicates that prior authorization was not obtained for the service.");
        COadjustmentCodes.Add("29", "Coverage terminated - Coverage for the patient was terminated before the service date.");
        COadjustmentCodes.Add("4", "Service not covered under the plan - Indicates that the service is not covered by the insurance plan.");
        COadjustmentCodes.Add("45", "Charge exceeds fee schedule/maximum allowable or contracted/legislated fee arrangement");
        COadjustmentCodes.Add("50", "Service not provided - Indicates that the service was not provided as billed.");
        COadjustmentCodes.Add("94", "Non-covered service - The service is not covered under the policy.");
        COadjustmentCodes.Add("96", "Non-covered charges - Charges related to the service are not covered by the insurance.");
        COadjustmentCodes.Add("97", "Adjustment for non-compliance - Indicates an adjustment due to non-compliance with policy guidelines.");
        COadjustmentCodes.Add("B11", "Service not authorized - The service required prior authorization, which was not obtained.");
        COadjustmentCodes.Add("B13", "Service not covered - The service rendered is not included in the coverage terms.");
        COadjustmentCodes.Add("B15", "Not a valid procedure code - The procedure code used on the claim is invalid.");
        


        var html = new StringBuilder();

        html.Append("<html><head><style>");
        html.Append("body { font-family: Arial, sans-serif; }");
        html.Append("table { width: 100%; border-collapse: collapse; margin-bottom: 20px; }");
        html.Append("th, td { padding: 8px; text-align: left; }");
        html.Append("th { background-color: #f2f2f2; font-weight: bold; }");
        html.Append("tr:nth-child(even) { background-color: #f9f9f9; }");
        html.Append(".label { font-size: smaller; color: gray; }");
        html.Append(".smalltext { font-size: smaller; color: black; }");
        html.Append(".section-header { background-color: #e0e0e0; padding: 10px; margin-top: 20px; font-weight: bold; }");
        html.Append(".section-sub-header {background-color: #f0f0f0; padding: 10px; margin-top: 10px; font-weight: bold; }");
        html.Append(".claim-separator { margin-top: 20px; }");
        html.Append(".indent { padding-left: 20px; }");
        html.Append(".indent-2 { padding-left: 40px; }");
        html.Append(".indent-3 { padding-left: 60px; }");
        html.Append("</style></head><body>");

        html.Append("<h1>Remittance Data</h1>");

        html.Append("<div class='section-header'>Payer Information</div>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayerName}<br><span class='label'>Payer Name</span></td>");
        html.Append($"<td>{remittanceData.PayerAddress} {remittanceData.PayerAddress2}<br><span class='label'>Payer Address</span></td>");
        html.Append($"<td>{remittanceData.PayerCity}<br><span class='label'>Payer City</span></td>");
        html.Append($"<td>{remittanceData.PayerState}<br><span class='label'>Payer State</span></td>");
        html.Append($"<td>{remittanceData.PayerZip}<br><span class='label'>Payer Zip</span></td>");
        html.Append("</tr>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayerContactName}<br><span class='label'>Payer Contact Name</span></td>");
        html.Append($"<td>{remittanceData.PayerContactPhone}<br><span class='label'>Payer Contact Phone</span></td>");
        html.Append($"<td>{remittanceData.PayerContactEmail}<br><span class='label'>Payer Contact Email</span></td>");
        html.Append("</tr>");
        html.Append("</table>");

        html.Append("<div class='section-header'>Payee Information</div>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.PayeeName}<br><span class='label'>Payee Name</span></td>");
        html.Append($"<td>{remittanceData.PayeeAddress}<br><span class='label'>Payee Address</span></td>");
        html.Append($"<td>{remittanceData.PayeeCity}<br><span class='label'>Payee City</span></td>");
        html.Append($"<td>{remittanceData.PayeeState}<br><span class='label'>Payee State</span></td>");
        html.Append($"<td>{remittanceData.PayeeZip}<br><span class='label'>Payee Zip</span></td>");
        html.Append("</tr>");
        html.Append("</table>");

        html.Append("<div class='section-header'>Transaction Information</div>");
        html.Append("<table>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.TransactionHandlingCode}<br><span class='label'>Transaction Handling Code</span></td>");
        html.Append($"<td>{decimal.Parse(remittanceData.TotalPremiumPaymentAmount ?? "0"):F2}<br><span class='label'>Total Premium Payment Amount</span></td>");
        html.Append($"<td>{remittanceData.CreditorDebitFlagCode}<br><span class='label'>Creditor Debit Flag Code</span></td>");
        html.Append($"<td>{remittanceData.PaymentDate?.ToString("yyyy-MM-dd")}<br><span class='label'>Payment Date</span></td>");
        html.Append("</tr>");
        html.Append("<tr>");
        html.Append($"<td>{remittanceData.InterchangeControlNumber}<br><span class='label'>Interchange Control Number</span></td>");
        html.Append($"<td>{remittanceData.GroupControlNumber}<br><span class='label'>Group Control Number</span></td>");
        html.Append($"<td>{remittanceData.TransactionSetControlNumber}<br><span class='label'>Transaction Set Control Number</span></td>");
        html.Append($"<td>{remittanceData.CurrentTransactionTraceNumber}<br><span class='label'>Current Transaction Trace Number</span></td>");
        html.Append("</tr>");
        html.Append("</table>");

        foreach (var loop2000 in remittanceData.Loop2000s)
        {
            html.Append("<div class='section-header'>Claim Information</div>");
            html.Append("<table>");
            html.Append("<tr>");
            html.Append($"<td>{loop2000.TotalClaimCount}<br><span class='label'>Total Claim Count</span></td>");
            html.Append($"<td>{decimal.Parse(loop2000.TotalClaimChargeAmount ?? "0"):F2}<br><span class='label'>Total Claim Charge Amount</span></td>");
            html.Append("</tr>");
            html.Append("</table>");

            foreach (var loop2100 in loop2000.Loop2100s)
            {
                html.Append("<div class='claim-separator'><hr /></div>");
                html.Append("<div class='section-header'>Claim</div>");
                html.Append("<table>");
                html.Append("<tr>");
                html.Append($"<td>{loop2100.AccountNo}<br><span class='label'>Account No</span></td>");
                html.Append($"<td>{loop2100.ClaimStatusCode}<br><span class='label'>Claim Status Code</span></td>");
                html.Append($"<td>{decimal.Parse(loop2100.ClaimChargeAmount ?? "0"):F2}<br><span class='label'>Claim Charge Amount</span></td>");
                html.Append($"<td>{decimal.Parse(loop2100.ClaimPaymentAmount ?? "0"):F2}<br><span class='label'>Claim Payment Amount</span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append($"<td>{decimal.Parse(loop2100.PatientResponsibilityAmount ?? "0"):F2}<br><span class='label'>Patient Responsibility Amount</span></td>");
                html.Append($"<td>{loop2100.ClaimFilingIndicatorCode}<br><span class='label'>Claim Filing Indicator Code</span></td>");
                html.Append($"<td>{loop2100.PayerClaimControlNumber}<br><span class='label'>Payer Claim Control Number</span></td>");
                html.Append($"<td>{loop2100.FacilityTypeCode}<br><span class='label'>Facility Type Code</span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append($"<td>{loop2100.ClaimFrequencyCode}<br><span class='label'>Claim Frequency Code</span></td>");
                html.Append($"<td>{decimal.Parse(loop2100.PaidAmount ?? "0"):F2}<br><span class='label'>Paid Amount</span></td>");
                html.Append($"<td>{decimal.Parse(loop2100.AllowedAmount ?? "0"):F2}<br><span class='label'>Allowed Amount</span></td>");
                html.Append("</tr>");
                html.Append("<tr>");
                html.Append($"<td>{loop2100.PatientLastName}<br><span class='label'>Patient Last Name</span></td>");
                html.Append($"<td>{loop2100.PatientFirstName}<br><span class='label'>Patient First Name</span></td>");
                html.Append($"<td>{loop2100.PatientMiddleName}<br><span class='label'>Patient Middle Name</span></td>");
                html.Append($"<td>{loop2100.PatientGender}<br><span class='label'>Patient Gender</span></td>");
                html.Append($"<td>{loop2100.PatientDateOfBirth?.ToString("yyyy-MM-dd")}<br><span class='label'>Patient Date of Birth</span></td>");
                html.Append("</tr>");
                html.Append("</table>");


                foreach (var loop2110 in loop2100.Loop2110s)
                {
                    html.Append("<div class='indent'>");
                    html.Append("<div class='section-header'>Service Line Details</div>");
                    html.Append("<table>");
                    html.Append("<tr>");
                    html.Append($"<td>{loop2110.ProcedureCode}<br><span class='label'>Procedure Code</span></td>");
                    html.Append($"<td>{dictService.GetCptAmaDescription(loop2110.ProcedureCode)}<br><span class='label'>Procedure Description</span></td>");
                    html.Append($"<td>{decimal.Parse(loop2110.LineItemChargeAmount ?? "0"):F2}<br><span class='label'>Line Item Charge Amount</span></td>");
                    html.Append($"<td>{decimal.Parse(loop2110.MonetaryAmount ?? "0"):F2}<br><span class='label'>Monetary Amount</span></td>");
                    html.Append($"<td>{loop2110.RevenueCode}<br><span class='label'>Revenue Code</span></td>");
                    html.Append($"<td>{decimal.Parse(loop2110.PaidAmount ?? "0"):F2}<br><span class='label'>Paid Amount</span></td>");
                    html.Append($"<td>{decimal.Parse(loop2110.AllowedAmount ?? "0"):F2}<br><span class='label'>Allowed Amount</span></td>");
                    html.Append("</tr>");
                    html.Append("</table>");
                    html.Append("</div>");

                    html.Append("<div class='indent-2 smalltext'>");
                    html.Append("<div class='section-sub-header smalltext'>Adjustments</div>");
                    html.Append("<table class='smalltext'>");
                    html.Append("<tr><th>Claim Adjustment Group Code</th><th>Adjustment Reason Code</th><th>Explanation</th><th>Adjustment Amount</th><th>Adjustment Quantity</th></tr>");
                    foreach (var adjustment in loop2110.Adjustments)
                    {
                        string adjustmentDescription;
                        switch(adjustment.ClaimAdjustmentGroupCode)
                        {
                            case "CO":
                                COadjustmentCodes.TryGetValue(adjustment.AdjustmentReasonCode, out adjustmentDescription);
                                break;
                            case "PR":
                                PRadjustmentCodes.TryGetValue(adjustment.AdjustmentReasonCode, out adjustmentDescription);
                                break;
                            default:
                                adjustmentDescription = string.Empty;
                                break;
                        }

                        html.Append("<tr>");
                        html.Append($"<td>{adjustment.ClaimAdjustmentGroupCode}</td>");
                        html.Append($"<td>{adjustment.AdjustmentReasonCode}</td>");
                        html.Append($"<td>{adjustmentDescription}</td>");
                        html.Append($"<td>{decimal.Parse(adjustment.AdjustmentAmount ?? "0"):F2}</td>");
                        html.Append($"<td>{adjustment.AdjustmentQuantity}</td>");
                        html.Append("</tr>");
                    }
                    html.Append("</table>");
                    html.Append("</div>");
                }
                html.Append("</div>");
            }
        }

        html.Append("</body></html>");

        return html.ToString();
    }


    public bool IsRemittancePosted(int remittanceId)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        var remittance = uow.RemittanceRepository.GetByKey(remittanceId);
        //if there are Chk entries with the same check number, remittance has been posted.
        var payments = uow.ChkRepository.GetByCheckNo(remittance.TransactionTraceNumber);
        if (payments.Count > 0)
            return true;
        else
            return false;

    }

    private void StoreRemittanceData(RemittanceData remittanceData, string fileName)
    {
        try
        {
            var remittanceFile = new RemittanceFile
            {
                FileName = Path.GetFileName(fileName), // Set appropriate file name
                ProcessedDate = remittanceData.PaymentDate ?? DateTime.Now,
                Payer = remittanceData.PayerName,
                TransactionTraceNumber = remittanceData.CurrentTransactionTraceNumber,
                TotalPaymentAmount = decimal.Parse(remittanceData.TotalPremiumPaymentAmount ?? "0"),
                ClaimCount = remittanceData.Loop2000s.Sum(x => Convert.ToInt16(x.TotalClaimCount)),
                TotalAllowedAmount = remittanceData.Loop2000s.Sum(l => l.Loop2100s.Sum(c => decimal.Parse(c.AllowedAmount ?? "0"))),
                TotalChargeAmount = remittanceData.Loop2000s.Sum(l => Convert.ToDecimal(l.TotalClaimChargeAmount)),
                TotalPaidAmount = decimal.Parse(remittanceData.PaidAmount ?? "0"),
                RemittanceData = JsonConvert.SerializeObject(remittanceData),
                Claims = new List<RemittanceClaim>()
            };

            foreach (var loop2000 in remittanceData.Loop2000s)
            {
                foreach (var loop2100 in loop2000.Loop2100s)
                {
                    var remittanceClaim = new RemittanceClaim
                    {
                        AccountNo = loop2100.AccountNo,
                        ClaimStatusCode = loop2100.ClaimStatusCode,
                        ClaimChargeAmount = decimal.Parse(loop2100.ClaimChargeAmount ?? "0"),
                        ClaimPaymentAmount = decimal.Parse(loop2100.ClaimPaymentAmount ?? "0"),
                        PatientResponsibilityAmount = decimal.Parse(loop2100.PatientResponsibilityAmount ?? "0"),
                        ClaimFilingIndicatorCode = loop2100.ClaimFilingIndicatorCode,
                        PayerClaimControlNumber = loop2100.PayerClaimControlNumber,
                        FacilityTypeCode = loop2100.FacilityTypeCode,
                        ClaimFrequencyCode = loop2100.ClaimFrequencyCode,
                        PaidAmount = decimal.Parse(loop2100.PaidAmount ?? "0"),
                        AllowedAmount = decimal.Parse(loop2100.AllowedAmount ?? "0"),
                        ClaimDetails = new List<RemittanceClaimDetail>()
                    };

                    foreach (var loop2110 in loop2100.Loop2110s)
                    {
                        var remittanceClaimDetail = new RemittanceClaimDetail
                        {
                            ProcedureCode = loop2110.ProcedureCode,
                            LineItemChargeAmount = decimal.Parse(loop2110.LineItemChargeAmount ?? "0"),
                            MonetaryAmount = decimal.Parse(loop2110.MonetaryAmount ?? "0"),
                            RevenueCode = loop2110.RevenueCode,
                            PaidAmount = decimal.Parse(loop2110.PaidAmount ?? "0"),
                            AllowedAmount = decimal.Parse(loop2110.AllowedAmount ?? "0"),
                            Adjustments = new List<RemittanceClaimAdjustment>()
                        };

                        foreach (var adjustment in loop2110.Adjustments)
                        {
                            var claimAdjustment = new RemittanceClaimAdjustment
                            {
                                ClaimAdjustmentGroupCode = adjustment.ClaimAdjustmentGroupCode,
                                AdjustmentReasonCode = adjustment.AdjustmentReasonCode,
                                AdjustmentAmount = decimal.Parse(adjustment.AdjustmentAmount ?? "0"),
                                AdjustmentQuantity = int.Parse(adjustment.AdjustmentQuantity ?? "0")
                            };

                            remittanceClaimDetail.Adjustments.Add(claimAdjustment);
                        }

                        remittanceClaim.ClaimDetails.Add(remittanceClaimDetail);
                    }

                    remittanceFile.Claims.Add(remittanceClaim);
                }
            }

            //Save the remittance file and related data
            using UnitOfWorkMain uow = new(_appEnvironment);

            var remit = uow.RemittanceRepository.Add(remittanceFile);

            foreach (var claim in remittanceFile.Claims)
            {
                claim.RemittanceId = remit.RemittanceId;
                var newclaim = uow.RemittanceClaimRepository.Add(claim);
                foreach (var detail in claim.ClaimDetails)
                {
                    detail.RemittanceClaimId = newclaim.ClaimId;
                    var newdetail = uow.RemittanceClaimDetailRepository.Add(detail);
                    foreach (var adjustment in detail.Adjustments)
                    {
                        adjustment.RemittanceClaimDetailId = newdetail.Id;
                        uow.RemittanceClaimAdjustmentRepository.Add(adjustment);
                    }
                }
            }
            uow.Commit();
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error storing remittance data: {ex.Message}");
            throw;
        }
    }

    //add GetAllRemittancesAsync method that is a Task wrapper for GetAllRemittances
    public async Task<List<RemittanceFile>> GetAllRemittancesAsync()
    {
        return await Task.Run(() => GetAllRemittances());
    }

    public List<RemittanceFile> GetAllRemittances(bool includePosted = true)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);

        return uow.RemittanceRepository.GetRemittances(includePosted);
    }

    //add GetRemittanceAsync method that is a Task wrapper for GetRemittance
    public async Task<RemittanceFile> GetRemittanceAsync(int remittanceId)
    {
        return await Task.Run(() => GetRemittance(remittanceId));
    }

    public RemittanceFile GetRemittance(int remittanceId)
    {
        using UnitOfWorkMain uow = new(_appEnvironment);
        var remit = uow.RemittanceRepository.GetByKey(remittanceId);
        if (remit == null)
            return null;

        remit.Claims = uow.RemittanceClaimRepository.GetByRemitId(remittanceId).ToList();

        foreach (var claim in remit.Claims)
        {
            claim.ClaimDetails = uow.RemittanceClaimDetailRepository.GetByClaimId(claim.ClaimId).ToList();
            claim.PatientName = uow.AccountRepository.GetByAccount(claim.AccountNo)?.PatFullName;
            foreach (var detail in claim.ClaimDetails)
            {
                detail.Adjustments = uow.RemittanceClaimAdjustmentRepository.GetByClaimDetailId(detail.Id).ToList();
            }
        }

        return remit;
    }

    public class OperationResult
    {
        public int RemittanceId { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }

    public async Task<OperationResult> HandleRemittanceAsync(int remittanceId, bool isPosting, IProgress<ProgressReportModel> progress)
    {
        Log.Instance.Trace($"{(isPosting ? "Post" : "Unpost")}RemittanceAsync called with remittanceId {remittanceId}");
        AccountService accountService = new(_appEnvironment);
        var remittance = this.GetRemittance(remittanceId);
        if (remittance == null)
        {
            return new OperationResult { Success = false, ErrorMessage = $"Remittance {remittanceId} not found" };
        }

        if (isPosting && IsRemittancePosted(remittanceId))
        {
            Log.Instance.Error($"Remittance {remittanceId} has already been posted");
            return new OperationResult { Success = false, ErrorMessage = $"Remittance {remittanceId} has already been posted" };
        }

        if (!isPosting && !remittance.PostedDate.HasValue)
        {
            Log.Instance.Error($"Remittance {remittanceId} has not been posted.");
            return new OperationResult { Success = false, ErrorMessage = $"Remittance {remittanceId} has not been posted" };
        }

        int totalClaims = remittance.Claims.Count;
        int processedClaims = 0;

        try
        {
            using UnitOfWorkMain uow = new(_appEnvironment, true);
            double total_paid = 0;
            double total_contractual = 0;
            double total_patient_responsibility = 0;

            foreach (var claim in remittance.Claims)
            {
                if (!claim.ClaimDetails.Any())
                {
                    Chk chk = new()
                    {
                        ChkDate = DateTime.Today,
                        PaidAmount = isPosting ? Convert.ToDouble(claim.ClaimPaymentAmount) : -Convert.ToDouble(claim.ClaimPaymentAmount),
                        Source = remittance.Payer,
                        Comment = $"{(isPosting ? "Check" : "Offsetting check")} for claim {claim.AccountNo}",
                        CheckNo = remittance.TransactionTraceNumber,
                        AccountNo = claim.AccountNo,
                        Batch = remittance.RemittanceId,
                        ClaimAdjCode = claim.ClaimStatusCode,
                        PostingFile = Path.GetFileName(remittance.FileName),
                        EftNumber = remittance.TransactionTraceNumber,
                        ClaimNo = claim.PayerClaimControlNumber,
                        EftDate = remittance.ProcessedDate,
                        Status = "NEW"
                    };
                    total_paid += isPosting ? Convert.ToDouble(claim.ClaimPaymentAmount) : -Convert.ToDouble(claim.ClaimPaymentAmount);
                    total_contractual += isPosting ? Convert.ToDouble(claim.ClaimChargeAmount) - Convert.ToDouble(claim.ClaimPaymentAmount) : -(Convert.ToDouble(claim.ClaimChargeAmount) - Convert.ToDouble(claim.ClaimPaymentAmount));
                    total_patient_responsibility += isPosting ? Convert.ToDouble(claim.PatientResponsibilityAmount) : -Convert.ToDouble(claim.PatientResponsibilityAmount);

                    await uow.ChkRepository.AddAsync(chk);
                }
                else
                {
                    foreach (var detail in claim.ClaimDetails)
                    {
                        if (claim.ProcessStatus != ClaimProcessStatus.Process)
                            break;

                        Chk chk = new()
                        {
                            ChkDate = DateTime.Today,
                            PaidAmount = isPosting ? Convert.ToDouble(detail.MonetaryAmount) : -Convert.ToDouble(detail.MonetaryAmount),
                            Source = remittance.Payer,
                            Comment = $"{(isPosting ? "Check" : "Offsetting check")} for claim {claim.AccountNo} {detail.ProcedureCode}",
                            CheckNo = remittance.TransactionTraceNumber,
                            AccountNo = claim.AccountNo,
                            Batch = remittance.RemittanceId,
                            Cpt4Code = detail.ProcedureCode,
                            PostingFile = Path.GetFileName(remittance.FileName),
                            EftNumber = remittance.TransactionTraceNumber,
                            ClaimNo = claim.PayerClaimControlNumber,
                            EftDate = remittance.ProcessedDate,
                            Status = "NEW"
                        };
                        await uow.ChkRepository.AddAsync(chk);

                        foreach (var adj in detail.Adjustments)
                        {
                            if (adj.AdjustmentAmount == 0)
                                continue;

                            if (adj.ClaimAdjustmentGroupCode != "CO")
                                continue;

                            if (adj.AdjustmentReasonCode != "45" && adj.AdjustmentReasonCode != "253")
                                continue;

                            string comment;

                            switch (adj.AdjustmentReasonCode)
                            {
                                case "45":
                                    comment = $"{(isPosting ? "Contractual adjustment" : "Offsetting contractual adjustment")} for claim {claim.AccountNo} {detail.ProcedureCode}";
                                    break;
                                case "253":
                                    comment = $"{(isPosting ? "Sequestration - reduction in federal spending" : "Offsetting sequestration - reduction in federal spending")} for {claim.AccountNo} {detail.ProcedureCode}";
                                    break;
                                default:
                                    comment = $"{(isPosting ? "Adjustment" : "Offsetting adjustment")} for claim {claim.AccountNo} {detail.ProcedureCode}";
                                    break;
                            }

                            Chk adjchk = new()
                            {
                                ChkDate = DateTime.Today,
                                Source = remittance.Payer,
                                Comment = comment,
                                CheckNo = remittance.TransactionTraceNumber,
                                AccountNo = claim.AccountNo,
                                Batch = remittance.RemittanceId,
                                Cpt4Code = detail.ProcedureCode,
                                ContractualAmount = isPosting ? Convert.ToDouble(adj.AdjustmentAmount) : -Convert.ToDouble(adj.AdjustmentAmount),
                                ClaimAdjCode = adj.AdjustmentReasonCode,
                                ClaimAdjGroupCode = adj.ClaimAdjustmentGroupCode,
                                PostingFile = Path.GetFileName(remittance.FileName),
                                EftNumber = remittance.TransactionTraceNumber,
                                ClaimNo = claim.PayerClaimControlNumber,
                                EftDate = remittance.ProcessedDate,
                                Status = "NEW"
                            };
                            await uow.ChkRepository.AddAsync(adjchk);
                        }

                        total_paid += isPosting ? Convert.ToDouble(detail.MonetaryAmount) : -Convert.ToDouble(detail.MonetaryAmount);
                        total_contractual += isPosting ? Convert.ToDouble(detail.Adjustments.Where(x => x.ClaimAdjustmentGroupCode == "CO" && x.AdjustmentReasonCode == "45").Sum(y => y.AdjustmentAmount)) : -Convert.ToDouble(detail.Adjustments.Where(x => x.ClaimAdjustmentGroupCode == "CO" && x.AdjustmentReasonCode == "45").Sum(y => y.AdjustmentAmount));
                        total_patient_responsibility += isPosting ? Convert.ToDouble(detail.Adjustments.Where(x => x.ClaimAdjustmentGroupCode == "PR").Sum(y => y.AdjustmentAmount)) : -Convert.ToDouble(detail.Adjustments.Where(x => x.ClaimAdjustmentGroupCode == "PR").Sum(y => y.AdjustmentAmount));
                    }
                }

                if (isPosting)
                {
                    accountService.AddNote(claim.AccountNo, $"Remittance {remittanceId} posted for claim {claim.AccountNo} with total paid amount of {total_paid}, total contractual amount of {total_contractual}, and total patient responsibility of {total_patient_responsibility}");
                }
                else
                {
                    accountService.AddNote(claim.AccountNo, $"Remittance {remittanceId} unposted for claim {claim.AccountNo} with total paid amount of {total_paid}, total contractual amount of {total_contractual}, and total patient responsibility of {total_patient_responsibility}");
                }

                total_paid = 0;
                total_contractual = 0;
                total_patient_responsibility = 0;

                processedClaims++;
                progress?.Report(new ProgressReportModel
                {
                    PercentageComplete = (processedClaims * 100 / totalClaims),
                    RecordsProcessed = processedClaims,
                    TotalRecords = totalClaims,
                    StatusMessage = $"Processed {processedClaims} of {totalClaims} claims"
                });
            }

            if (isPosting)
            {
                remittance.PostedDate = DateTime.Now;
                remittance.PostingUser = _appEnvironment.UserName;
                remittance.PostingHost = Utilities.OS.GetMachineName();
            }
            else
            {
                remittance.PostedDate = null;
                remittance.PostingUser = null;
                remittance.PostingHost = null;
            }

            uow.RemittanceRepository.Update(remittance);
            uow.Commit();

            return new OperationResult { Success = true };
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex.Message);
            return new OperationResult { Success = false, ErrorMessage = ex.Message };
        }
    }

    // New method to reimport all unposted remittances
    public async Task<List<OperationResult>> ReimportUnpostedRemittancesAsync(IProgress<ProgressReportModel> progress = null)
    {
        var unpostedRemittances = GetAllRemittances(includePosted: false);
        var results = new List<OperationResult>();
        int total = unpostedRemittances.Count;
        int count = 0;

        foreach (var remittance in unpostedRemittances)
        {
            try
            {
                await Task.Run(() => ReimportRemittance(remittance.RemittanceId));
                results.Add(new OperationResult { Success = true });

                Log.Instance.Info($"Successfully reimported remittance {remittance.RemittanceId}");
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"Error reimporting remittance {remittance.RemittanceId}: {ex.Message}");
                results.Add(new OperationResult { Success = false, ErrorMessage = ex.Message });
            }

            count++;
            progress?.Report(new ProgressReportModel
            {
                TotalRecords = total,
                RecordsProcessed = count,
                PercentageComplete = (int)((double)count / total * 100),
                StatusMessage = $"Reimported {count} of {total} remittances"
            });
        }

        return results;
    }

}
