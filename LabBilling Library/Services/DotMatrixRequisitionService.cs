using LabBilling.Core.Models;
using LabBilling.Logging;
using System;
using System.Linq;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for formatting requisition forms for dot-matrix printing on pin-fed forms.
/// Generates PCL5 commands for precise positioning on pre-printed 3-ply forms.
/// </summary>
public class DotMatrixRequisitionService
{
    private readonly PCL5FormatterService _pcl5;

    public DotMatrixRequisitionService()
 {
        _pcl5 = new PCL5FormatterService();
    }

    /// <summary>
    /// Legacy format constants matching ADDRESS application specifications.
    /// 3 lines from top, 50 character left margin.
/// </summary>
    private const int START_LINE = 3;  // Lines from top of form
    private const int LEFT_MARGIN = 50;  // Characters from left edge

    /// <summary>
    /// Generates PCL5-formatted requisition form data for a client.
    /// Format matches legacy ADDRESS application: 3 lines from top, 50 char left margin.
    /// </summary>
    /// <param name="client">Client information to print</param>
  /// <param name="formType">Form type (CLIREQ, PTHREQ, CYTREQ)</param>
    /// <returns>PCL5-formatted string ready for raw printing</returns>
    public string FormatRequisition(Client client, string formType = "CLIREQ")
    {
        try
    {
    StringBuilder output = new StringBuilder();

            // Initialize printer for pin-fed forms
            output.Append(_pcl5.InitializeForPinFedForms());

  // Position at line 3, column 0 (absolute positioning for accuracy)
   output.Append(_pcl5.SetPosition(START_LINE, 0));

            // Client Name - 50 spaces from left
            output.Append(_pcl5.FormatLine(client.Name ?? "", LEFT_MARGIN));

      // Full Address - 50 spaces from left
      string fullAddress = BuildFullAddress(client);
          if (!string.IsNullOrEmpty(fullAddress))
    {
     output.Append(_pcl5.FormatLine(fullAddress, LEFT_MARGIN));
            }

  // City/State/ZIP - 50 spaces from left
          string cityStateZip = BuildCityStateZip(client);
            if (!string.IsNullOrEmpty(cityStateZip))
            {
   output.Append(_pcl5.FormatLine(cityStateZip, LEFT_MARGIN));
    }

       // Phone - 50 spaces from left (only if present)
            if (!string.IsNullOrWhiteSpace(client.Phone))
    {
          output.Append(_pcl5.FormatLine(client.Phone, LEFT_MARGIN));
            }

        // Fax with FAX prefix - 50 spaces from left (only if present)
      if (!string.IsNullOrWhiteSpace(client.Fax))
   {
    output.Append(_pcl5.FormatLine($"FAX {client.Fax}", LEFT_MARGIN));
            }

          // Client Mnemonic and Code with optional EMR
  string mnemonicLine = BuildMnemonicLine(client);
if (!string.IsNullOrEmpty(mnemonicLine))
     {
         output.Append(_pcl5.FormatLine(mnemonicLine, LEFT_MARGIN));
     }

            // Eject page (form feed) - critical for pin-fed forms
   output.Append(_pcl5.EjectPage());

            Log.Instance.Debug($"Generated {formType} requisition for client {client.ClientMnem}");

         return output.ToString();
     }
        catch (Exception ex)
     {
    Log.Instance.Error($"Error formatting requisition for client {client?.ClientMnem}: {ex.Message}", ex);
throw;
        }
    }

    /// <summary>
    /// Generates PCL5 data for multiple requisitions (batch printing).
    /// </summary>
    /// <param name="clients">Collection of clients</param>
    /// <param name="formType">Form type</param>
    /// <param name="copiesPerClient">Number of copies to print per client</param>
    /// <returns>PCL5-formatted string for all requisitions</returns>
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
  /// Prints a grid with line/column markers.
    /// </summary>
    /// <returns>PCL5 test pattern</returns>
    public string GenerateAlignmentTestPattern()
 {
        StringBuilder test = new StringBuilder();

        // Initialize
     test.Append(_pcl5.InitializeForPinFedForms());

        // Print column ruler at top
      test.Append(_pcl5.SetPosition(0, 0));
     test.Append("....5....10...15...20...25...30...35...40...45...50...55...60...65...70...75...80");
    test.Append(_pcl5.NextLine());

        // Print line numbers and markers
        for (int line = 1; line <= 10; line++)
        {
        test.Append($"L{line:D2} ");
            
 // Mark every 10 columns
            for (int col = 0; col < 80; col += 10)
  {
             test.Append($"|...{col,2}...");
 }
       
        test.Append(_pcl5.NextLine());
        }

        // Highlight the requisition start position (line 3, column 50)
        test.Append(_pcl5.NextLine());
      test.Append(_pcl5.FormatLine(">>> REQUISITION DATA STARTS HERE <<<", LEFT_MARGIN));

    test.Append(_pcl5.EjectPage());

        return test.ToString();
    }
}
