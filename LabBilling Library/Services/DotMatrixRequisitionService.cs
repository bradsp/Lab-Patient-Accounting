using LabBilling.Core.Models;
using LabBilling.Logging;
using System;
using System.Linq;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for formatting requisition forms for dot-matrix printing on pin-fed forms.
/// Uses simple text-only mode with spaces for positioning (compatible with legacy ADDRESS application).
/// </summary>
public class DotMatrixRequisitionService
{
    /// <summary>
    /// Legacy format constants matching ADDRESS application specifications.
    /// 5 lines from top, 55 character left margin.
    /// </summary>
    private const int START_LINE = 5;  // Lines from top of form
    private const int LEFT_MARGIN = 60;  // Characters from left edge

    /// <summary>
    /// Generates text-formatted requisition form data for a client.
    /// Format matches legacy ADDRESS application: plain text with spacing for positioning.
    /// </summary>
    /// <param name="client">Client information to print</param>
    /// <param name="formType">Form type (CLIREQ, PTHREQ, CYTREQ)</param>
    /// <returns>Plain text string ready for printing</returns>
    public string FormatRequisition(Client client, string formType = "CLIREQ")
    {
        try
        {
            StringBuilder output = new StringBuilder();

            // Add blank lines to position at START_LINE
            for (int i = 0; i < START_LINE; i++)
            {
                output.AppendLine();
            }

            // Client Name - 55 spaces from left
            output.AppendLine(FormatLine(client.Name ?? "", LEFT_MARGIN));

            // Full Address - 55 spaces from left
            string fullAddress = BuildFullAddress(client);
            if (!string.IsNullOrEmpty(fullAddress))
            {
                output.AppendLine(FormatLine(fullAddress, LEFT_MARGIN));
            }

            // City/State/ZIP - 55 spaces from left
            string cityStateZip = BuildCityStateZip(client);
            if (!string.IsNullOrEmpty(cityStateZip))
            {
                output.AppendLine(FormatLine(cityStateZip, LEFT_MARGIN));
            }

            // Phone - 55 spaces from left (only if present)
            if (!string.IsNullOrWhiteSpace(client.Phone))
            {
                output.AppendLine(FormatLine(client.Phone, LEFT_MARGIN));
            }

            // Fax with FAX prefix - 55 spaces from left (only if present)
            if (!string.IsNullOrWhiteSpace(client.Fax))
            {
                output.AppendLine(FormatLine($"FAX {client.Fax}", LEFT_MARGIN));
            }

            // Client Mnemonic and Code with optional EMR
            string mnemonicLine = BuildMnemonicLine(client);
            if (!string.IsNullOrEmpty(mnemonicLine))
            {
                output.AppendLine(FormatLine(mnemonicLine, LEFT_MARGIN));
            }

            // Form feed - eject page for pin-fed forms
            output.Append('\f');

            Log.Instance.Debug($"Generated {formType} text requisition for client {client.ClientMnem}");

            return output.ToString();
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error formatting requisition for client {client?.ClientMnem}: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Generates plain text data for multiple requisitions (batch printing).
    /// </summary>
    /// <param name="clients">Collection of clients</param>
    /// <param name="formType">Form type</param>
    /// <param name="copiesPerClient">Number of copies to print per client</param>
    /// <returns>Plain text string for all requisitions</returns>
    public string FormatRequisitionBatch(System.Collections.Generic.IEnumerable<Client> clients, string formType = "CLIREQ", int copiesPerClient = 1)
    {
        StringBuilder batch = new StringBuilder();

        foreach (var client in clients)
        {
            for (int copy = 0; copy < copiesPerClient; copy++)
            {
                batch.Append(FormatRequisition(client, formType));
            }
        }

        return batch.ToString();
    }

    /// <summary>
    /// Formats a text line with left margin spacing.
    /// </summary>
    /// <param name="text">Text to print</param>
    /// <param name="leftMargin">Number of spaces to indent from left edge</param>
    /// <returns>Formatted string with leading spaces (no newline)</returns>
    private string FormatLine(string text, int leftMargin)
    {
        return new string(' ', leftMargin) + text;
    }

    /// <summary>
    /// Builds full address from client address fields.
    /// </summary>
    private string BuildFullAddress(Client client)
    {
        if (client == null) return "";

        var addr1 = client.StreetAddress1?.Trim() ?? "";
        var addr2 = client.StreetAddress2?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(addr1) && string.IsNullOrWhiteSpace(addr2))
            return "";

        if (string.IsNullOrWhiteSpace(addr2))
            return addr1;

        if (string.IsNullOrWhiteSpace(addr1))
            return addr2;

        return $"{addr1} {addr2}";
    }

    /// <summary>
    /// Builds city, state, zip line from client fields.
    /// </summary>
    private string BuildCityStateZip(Client client)
    {
        if (client == null) return "";

        var parts = new[] {
   client.City?.Trim(),
      client.State?.Trim(),
            client.ZipCode?.Trim()
        }.Where(p => !string.IsNullOrWhiteSpace(p));

        return string.Join(" ", parts);
    }

    /// <summary>
    /// Builds mnemonic line with client code and optional EMR type.
    /// </summary>
    private string BuildMnemonicLine(Client client)
    {
        if (client == null) return "";

        var mnem = client.ClientMnem ?? "";
        var code = client.FacilityNo ?? "";
        var emr = client.ElectronicBillingType ?? "";

        if (string.IsNullOrWhiteSpace(emr))
        {
            return $"{mnem} ({code})";
        }
        else
        {
            return $"{mnem} {code} ({emr})";
        }
    }

    /// <summary>
    /// Generates a test pattern to verify printer alignment on pin-fed forms.
    /// Prints a grid with line/column markers using plain text.
    /// </summary>
    /// <returns>Plain text test pattern</returns>
    public string GenerateAlignmentTestPattern()
    {
        StringBuilder test = new StringBuilder();

        // Print column ruler at top (line 0)
        test.AppendLine("    5   10   15   20   25   30   35   40   45 50   55   60   65   70   75   80   85   90   95  100  105  110  115  120");
        test.AppendLine("....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....");

        // Print line numbers and markers for lines 1-10
        for (int line = 1; line <= 10; line++)
        {
            test.Append($"L{line:D2} ");

            // Mark every 10 columns up to 120
            for (int col = 0; col < 120; col += 10)
            {
                if (col == 0)
                    test.Append("|");
                else
                    test.Append($"|...{col,3}");
            }

            test.AppendLine();
        }

        // Add some blank lines
        test.AppendLine();
        test.AppendLine();

        // Highlight the requisition start position (line 5, column 55)
        test.AppendLine(FormatLine(">>> REQUISITION DATA STARTS AT LINE 5, COLUMN 55 <<<", LEFT_MARGIN));

        // Show actual positioning
        test.AppendLine();
        for (int i = 0; i < START_LINE; i++)
        {
            test.AppendLine($"Line {i}: (blank line for spacing)");
        }
        test.AppendLine(FormatLine("CLIENT NAME APPEARS HERE", LEFT_MARGIN));
        test.AppendLine(FormatLine("ADDRESS LINE APPEARS HERE", LEFT_MARGIN));
        test.AppendLine(FormatLine("CITY STATE ZIP APPEARS HERE", LEFT_MARGIN));

        // Form feed
        test.Append('\f');

        return test.ToString();
    }
}
