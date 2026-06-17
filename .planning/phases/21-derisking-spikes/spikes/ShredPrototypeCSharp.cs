// ============================================================================
// PROTOTYPE B - C# / j4jayant.HL7.Parser HL7 shred  (Phase 21-03)
// ----------------------------------------------------------------------------
// Reproduces xml-case-source-tsql.sql (the SQL Agent "CERNER DAILY CDM" shred)
// projecting the SAME parity column set:
//     ACCOUNT, SET ID, CDM, CDM QTY, CERNER DESCRIPTION   (+ CLIENT header scalar)
//
// THROWAWAY SPIKE. Not wired into any project, not added to any .csproj. It is a
// console-style sketch using the REAL j4jayant.HL7.Parser types already referenced
// by `LabBilling Library` (see HL7ProcessorService.cs:1,19 and ParseFT1():1046).
//
// ---------------------------------------------------------------------------
// IMPORTANT - encoding note (the j4jayant "fit"):
//
//   The SQL Agent CDM step shreds the message from infce.messages_inbound.msgContent,
//   which is the HL7-as-XML envelope (<HL7Message><PV1.3.4>... </HL7Message>) -- see
//   spikes/sample-message.xml. j4jayant.HL7.Parser is a PIPE-DELIMITED HL7 parser:
//   Message.ValidateMessage() requires the text to start with "MSH" and splits on
//   the HL7 delimiters | ^ ~ \ & (Message.cs:23,162). It CANNOT parse the XML form.
//
//   That is fine -- and is in fact the whole point of moving this to the app tier.
//   The SAME logical message is ALSO persisted pipe-delimited in the
//   infce.messages_inbound.HL7Message column, and the production HL7ProcessorService
//   already parses THAT with j4jayant: ProcessMessage() -> ParseHL7(_currentMessage.HL7Message)
//   -> GetValue("PV1.3.4"), GetValue("PID.18.1"), Segments("FT1"), Fields(7).Components(1)
//   (HL7ProcessorService.cs:118,209,845,822,1048,1058). So the C# path does not shred
//   the XML at all -- it parses the canonical pipe-delimited HL7 that the app already
//   ingests, which is the natural and already-proven route.
//
//   The pipe-delimited message below is the exact equivalent of sample-message.xml.
// ---------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using j4jayant.HL7.Parser;

internal static class ShredPrototypeCSharp
{
    // One output row = one shredded FT1 charge line (the parity column set).
    private sealed record CdmShredRow(
        string Account,
        int SetId,
        string Cdm,
        int CdmQty,
        string CernerDescription);

    // Pipe-delimited equivalent of spikes/sample-message.xml (SYNTHETIC / PHI-FREE).
    //   PID-18.1 = 9000001234            -> ACCOUNT 'L' + value
    //   PV1-3.4  = CLNT01                -> CLIENT
    //   FT1-1.1  = set id, FT1-7.1 = CDM, FT1-7.2 = cerner desc, FT1-10.1 = qty
    // Segments are CR-separated (j4jayant default segment separator, Message.cs:9).
    private const string SamplePipeHl7 =
        "MSH|^~\\&|CERNER|SYNTH|LAB|SYNTH|20260616120000||DFT^P03|SYNTHETIC-CTRL-0001|P|2.3\r" +
        "PID|1||MRN0000001^^^||TESTPATIENT^ALPHA||||||||||||||9000001234\r" +
        "PV1|1|O|CLNT01^^^CLNT01^^^CLNT01\r" +
        "FT1|1||||20260616|||8000010^SYNTHETIC PANEL A|||1\r" +
        "FT1|2||||20260616|||8000020^SYNTHETIC TEST B|||2\r" +
        "FT1|3||||20260616|||8000099^SYNTHETIC UNMAPPED C|||1";

    private static void Main()
    {
        foreach (var row in Shred(SamplePipeHl7))
            Console.WriteLine($"{row.Account,-12}| {row.SetId,-6}| {row.Cdm,-8}| {row.CdmQty,-7}| {row.CernerDescription}");
    }

    /// <summary>
    /// Shreds one Cerner DFT-P03 charge message into the CDM parity column set,
    /// using the real j4jayant parser exactly as HL7ProcessorService does.
    /// </summary>
    public static IEnumerable<CdmShredRow> Shred(string pipeDelimitedHl7)
    {
        var msg = new Message(pipeDelimitedHl7);   // j4jayant.HL7.Parser.Message
        if (!msg.ParseMessage())                   // throws HL7Exception on bad input
            throw new InvalidOperationException("HL7 message failed to parse.");

        // Header scalars (mirror cteData STEP 2).
        // CLIENT: proc COALESCEs PV1.3.4 / .3.1 / .3.7 / .6.4 / .6.1 / .6.7 then maps
        //         CLIENT->CERNER (uow.MappingRepository.GetMappedValue in the app).
        //         Mapping omitted in the spike; raw resolved value shown.
        string client = FirstNonEmpty(
            msg.GetValue("PV1.3.4"), msg.GetValue("PV1.3.1"), msg.GetValue("PV1.3.7"),
            msg.GetValue("PV1.6.4"), msg.GetValue("PV1.6.1"), msg.GetValue("PV1.6.7")) ?? "K";

        // ACCOUNT: 'L' + PID-18.1  (HL7ProcessorService.cs:822 does the same prefix).
        string account = "L" + msg.GetValue("PID.18.1");

        // Per-charge shred (mirror xCDM STEP 3 / the app's ParseFT1() at :1046).
        foreach (Segment ft1 in msg.Segments("FT1"))
        {
            int setId = ToInt(ft1.Fields(1).Components(1).Value);     // FT1.1.1
            string cdm = ft1.Fields(7).Components(1).Value;           // FT1.7.1
            string cernerDesc = ft1.Fields(7).Components(2).Value;    // FT1.7.2
            int qty = ToInt(ft1.Fields(10).Components(1).Value);      // FT1.10.1

            yield return new CdmShredRow(account, setId, cdm, qty, cernerDesc);
        }

        // STEP 4 (dictionary join / SUM / "not in billing" filter) and HTML email
        // rendering live downstream -- in the app tier that is a DictionaryService
        // lookup + LINQ GroupBy + an SMTP email service (Quartz job), replacing the
        // LEFT JOIN cdm / FOR XML PATH in the agent step.
    }

    private static string? FirstNonEmpty(params string[] candidates)
    {
        foreach (var c in candidates)
            if (!string.IsNullOrWhiteSpace(c)) return c.Trim();
        return null;
    }

    private static int ToInt(string? s) =>
        int.TryParse(s, out var v) ? v : 0;
}

// ============================================================================
// EXPECTED OUTPUT (parity with the T-SQL reference and the PG prototype):
//
//   L9000001234 | 1     | 8000010 | 1      | SYNTHETIC PANEL A
//   L9000001234 | 2     | 8000020 | 2      | SYNTHETIC TEST B
//   L9000001234 | 3     | 8000099 | 1      | SYNTHETIC UNMAPPED C
//
// Real j4jayant API surface exercised (all already used by HL7ProcessorService):
//   new Message(string), Message.ParseMessage(), Message.GetValue("SEG.f.c"),
//   Message.Segments("FT1") -> List<Segment>, Segment.Fields(n) -> Field,
//   Field.Components(n).Value.
// ============================================================================
