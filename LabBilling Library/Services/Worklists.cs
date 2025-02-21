using System.Collections.Generic;

namespace LabBilling.Core.Services;

public static class Worklists
{

    public const string MedicareCigna = "Medicare/Cigna";
    public const string BlueCross = "BlueCross";
    public const string Champus = "Champus";
    public const string TenncareBCBS = "Tenncare BC/BS";
    public const string CommercialInst = "Commercial Institutional";
    public const string CommercialProf = "Commercial Professional";
    public const string UHCCommunityPlan = "UHC Community Plan";
    public const string PathwaysTNCare = "Pathways TNCare";
    public const string Wellpoint = "Wellpoint";
    public const string SelfPay = "Self Pay";
    public const string ManualHold = "Manual Hold";
    public const string InitialHold = "Initial Hold";
    public const string ErrorFinCode = "Error Fin Code";
    public const string ClientBill = "Client Bill";
    public const string SubmittedInstitutional = "Submitted Institutional";
    public const string SubmittedProfessional = "Submitted Professional";
    public const string SubmittedOtherClaim = "Submitted Other";
    public const string ReceivingStatements = "Receiving Statements";
    public const string CreditBalance = "Credit Balance";


    public static List<string> ToList()
    {
        List<string> values = new();
        foreach (var prop in typeof(Worklists).GetFields())
        {
            values.Add(prop.GetValue(null) as string);
        }

        return values;
    }


}
