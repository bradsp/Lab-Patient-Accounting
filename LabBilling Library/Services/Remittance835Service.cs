using System;
using System.Collections.Generic;
using EdiTools;
using LabBilling.Core.DataAccess;
using System.IO;
using LabBilling.Core.Models;
using LabBilling.Core.Repositories;
using LabBilling.Core.UnitOfWork;

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

    public RemittanceData Load835(string fileName)
    {
        EdiDocument ediDocument;
        try
        {
            ediDocument = EdiDocument.Load(fileName);
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

        foreach (var segment in _ediDocument.Segments)
        {
            try
            {
                switch (segment.Id)
                {
                    case "ST":
                        break;
                    case "BPR":
                        remittance.TransactionHandlingCode = segment[1];
                        remittance.TotalPremiumPaymentAmount = segment[2];
                        remittance.CreditorDebitFlagCode = segment[3];
                        break;
                    case "N1":
                        if (segment[1] == "PR")
                        {
                            currentLoop = "1000A";
                            currN1type = segment[1];
                            remittance.PayerName = segment[2];
                        }
                        if (segment[1] == "PE")
                        {
                            currentLoop = "1000B";
                            currN1type = segment[1];
                            remittance.PayeeName = segment[2];
                            remittance.BankIdentifier = segment[4];
                        }
                        break;
                    case "N3":
                        if (currN1type == "PR")
                        {
                            remittance.PayerAddress = segment[1];
                            remittance.PayerAddress2 = segment[2];
                        }
                        if (currN1type == "PE")
                        {
                            remittance.PayeeAddress = segment[1];
                        }
                        break;
                    case "N4":
                        if (currN1type == "PR")
                        {
                            remittance.PayerCity = segment[1];
                            remittance.PayerState = segment[2];
                            remittance.PayerZip = segment[3];
                        }
                        if (currN1type == "PE")
                        {
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
                        break;
                    case "TS3":
                        loop2000.TotalClaimCount = segment[4];
                        loop2000.TotalClaimChargeAmount = segment[5];
                        loop2000.TotalHCPCSReportedChargeAmount = segment[17];
                        loop2000.TotalHCPCSPayableAmount = segment[18];
                        break;
                    case "CLP":
                        if (loop2100 != null)
                        {
                            loop2000.Loop2100s.Add(loop2100);
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
                        }
                        break;
                    case "SVC":
                        if (loop2110 != null)
                            loop2100.Loop2110s.Add(loop2110);

                        currentLoop = "2110";
                        loop2110 = new ()
                        {
                            ProcedureCode = segment[1],
                            LineItemChargeAmount = segment[2],
                            MonetaryAmount = segment[3],
                            RevenueCode = segment[4]
                        };
                        break;
                    case "CAS":
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
                                AdjustmentReasonCode = segment[17],
                                AdjustmentAmount = segment[18],
                                AdjustmentQuantity = segment[19]
                            };
                            loop2110.Adjustments.Add(loop2110Adj);
                        }
                        break;
                    case "SE":
                        remittance.Loop2000s.Add(loop2000);
                        break;
                    default:
                        // Handle unknown segment
                        _errors.Add($"Unknown segment ID: {segment.Id}");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log or handle the error for the specific segment
                _errors.Add($"Error processing segment {segment.Id}: {ex.Message}");
            }
        }
        StoreRemittanceData(remittance);
        return remittance;
    }

    public void StoreRemittanceData(RemittanceData remittanceData)
    {
        var remittanceFile = new RemittanceFile
        {
            FileName = "exampleFileName", // Set appropriate file name
            ProcessedDate = DateTime.Now,
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
                    ClaimChargeAmount = decimal.Parse(loop2100.ClaimChargeAmount),
                    ClaimPaymentAmount = decimal.Parse(loop2100.ClaimPaymentAmount),
                    PatientResponsibilityAmount = decimal.Parse(loop2100.PatientResponsibilityAmount),
                    ClaimFilingIndicatorCode = loop2100.ClaimFilingIndicatorCode,
                    PayerClaimControlNumber = loop2100.PayerClaimControlNumber,
                    FacilityTypeCode = loop2100.FacilityTypeCode,
                    ClaimFrequencyCode = loop2100.ClaimFrequencyCode,
                    PaidAmount = decimal.Parse(loop2100.PaidAmount),
                    AllowedAmount = decimal.Parse(loop2100.AllowedAmount),
                    ClaimDetails = new List<RemittanceClaimDetail>()
                };

                foreach (var loop2110 in loop2100.Loop2110s)
                {
                    var remittanceClaimDetail = new RemittanceClaimDetail
                    {
                        ProcedureCode = loop2110.ProcedureCode,
                        LineItemChargeAmount = decimal.Parse(loop2110.LineItemChargeAmount),
                        MonetaryAmount = decimal.Parse(loop2110.MonetaryAmount),
                        RevenueCode = loop2110.RevenueCode,
                        PaidAmount = decimal.Parse(loop2110.PaidAmount),
                        AllowedAmount = decimal.Parse(loop2110.AllowedAmount),
                        Adjustments = new List<ClaimAdjustment>()
                    };

                    foreach (var adjustment in loop2110.Adjustments)
                    {
                        var claimAdjustment = new ClaimAdjustment
                        {
                            ClaimAdjustmentGroupCode = adjustment.ClaimAdjustmentGroupCode,
                            AdjustmentReasonCode = adjustment.AdjustmentReasonCode,
                            AdjustmentAmount = decimal.Parse(adjustment.AdjustmentAmount),
                            AdjustmentQuantity = int.Parse(adjustment.AdjustmentQuantity)
                        };

                        remittanceClaimDetail.Adjustments.Add(claimAdjustment);
                    }

                    remittanceClaim.ClaimDetails.Add(remittanceClaimDetail);
                }

                remittanceFile.Claims.Add(remittanceClaim);
            }
        }

        //Save the remittance file and related data
        using UnitOfWorkMain uow = new(_appEnvironment, true);
        uow.RemittanceRepository.Save(remittanceFile);
        foreach (var claim in remittanceFile.Claims)
        {
            uow.RemittanceClaimRepository.Save(claim);
            foreach (var detail in claim.ClaimDetails)
            {
                uow.RemittanceClaimDetailRepository.Save(detail);
                foreach (var adjustment in detail.Adjustments)
                {
                    uow.RemittanceClaimAdjustmentRepository.Save(adjustment);
                }
            }
        }
        uow.Commit();
    }
}
