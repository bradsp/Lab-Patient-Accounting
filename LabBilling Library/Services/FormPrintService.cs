using LabBilling.Core.Models;
using System;
using System.Linq;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Service for generating formatted requisition forms with precise positioning
/// Implements legacy ADDRESS application form layouts
/// </summary>
public class FormPrintService
{
    /// <summary>
    /// Alternative collection site data
    /// </summary>
    public class AlternativeSite
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Zip { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    /// <summary>
    /// Generates HTML for requisition forms (CLIREQ, PTHREQ, CYTREQ)
    /// Legacy format: 3 lines from top, 50 character left margin
    /// </summary>
    public string GenerateRequisitionForm(Client client, int copies, string formType)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<div class='form-container requisition-form'>");
        sb.AppendLine("  <div class='client-info line-3'>");

        // Client Name - 50 spaces from left
        sb.AppendLine($"    <div class='field-line'>{FormatWithSpacing(client.Name ?? "", 50)}</div>");

        // Full Address - 50 spaces from left
        var fullAddress = BuildFullAddress(client.StreetAddress1, client.StreetAddress2);
        sb.AppendLine($"    <div class='field-line'>{FormatWithSpacing(fullAddress, 50)}</div>");

        // City/State/ZIP - 50 spaces from left
        var cityStateZip = BuildCityStateZip(client.City, client.State, client.ZipCode);
        sb.AppendLine($"    <div class='field-line'>{FormatWithSpacing(cityStateZip, 50)}</div>");

        // Phone - 50 spaces from left
        if (!string.IsNullOrWhiteSpace(client.Phone))
        {
            sb.AppendLine($"  <div class='field-line'>{FormatWithSpacing(client.Phone, 50)}</div>");
        }

        // Fax with FAX prefix - 50 spaces from left
        if (!string.IsNullOrWhiteSpace(client.Fax))
        {
            sb.AppendLine($"    <div class='field-line'>{FormatWithSpacing($"FAX {client.Fax}", 50)}</div>");
        }

        // Client Mnemonic and Code with EMR if present
        var mnemLine = BuildMnemonicLine(client);
        sb.AppendLine($"    <div class='field-line'>{FormatWithSpacing(mnemLine, 50)}</div>");

        sb.AppendLine("  </div>");
        sb.AppendLine("</div>");

        return sb.ToString();
    }

    /// <summary>
    /// Generates HTML for chain of custody forms
    /// Legacy format: Complex layout with client info, MRO info, and collection site
    /// </summary>
    public string GenerateCustodyForm(Client client, AlternativeSite? altSite, bool includeDap, int copies)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<div class='form-container custody-form'>");

        // Client Information Section - starts 6 lines down
        sb.AppendLine("  <div class='client-section line-6'>");

        var hasMro = !string.IsNullOrWhiteSpace(client.MroName);

        if (!hasMro)
        {
            // No MRO - use "X X X X NONE X X X X" on right side
            var noneMarker = "X X X X NONE X X X X";
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(client.Name ?? "", noneMarker, 50)}</div>");

            var fullAddress = BuildFullAddress(client.StreetAddress1, client.StreetAddress2);
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(fullAddress, noneMarker, 50)}</div>");

            var cityStateZip = BuildCityStateZip(client.City, client.State, client.ZipCode);
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(cityStateZip, noneMarker, 50)}</div>");

            var phoneFax = $"{client.Phone,-20}{FormatFax(client.Fax),-30}";
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(phoneFax, noneMarker, 50)}</div>");
        }
        else
        {
            // With MRO information
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(client.Name ?? "", client.MroName ?? "", 50)}</div>");

            var fullAddress = BuildFullAddress(client.StreetAddress1, client.StreetAddress2);
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(fullAddress, client.MroStreetAddress1 ?? "", 50)}</div>");

            var cityStateZip = BuildCityStateZip(client.City, client.State, client.ZipCode);
            var mroAddr2 = client.MroStreetAddress2 ?? "";
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(cityStateZip, mroAddr2, 50)}</div>");

            var phoneFax = $"{client.Phone,-20}{FormatFax(client.Fax),-30}";
            var mroCityStateZip = BuildCityStateZip(client.MroCity, client.MroState, client.MroZipCode);
            sb.AppendLine($"    <div class='client-line'>{FormatDualColumn(phoneFax, mroCityStateZip, 50)}</div>");
        }

        // Client Mnemonic line
        var mnemLine = $"{client.ClientMnem} ({client.FacilityNo ?? ""})";
        sb.AppendLine($"    <div class='client-line'>{mnemLine}</div>");

        sb.AppendLine("  </div>");

        // Collection Site Section - starts 10 lines down (or 7 if DAP)
        var siteMargin = includeDap ? "line-7" : "line-10";
        sb.AppendLine($"  <div class='collection-site {siteMargin}'>");

        if (includeDap)
        {
            sb.AppendLine($"    <div class='dap-notation'>{FormatDapNotation()}</div>");
        }

        // Use alternative site or client location based on prn_loc flag
        var useCollectionSite = altSite != null || client.prn_loc == "Y";
        if (useCollectionSite)
        {
            var siteName = altSite?.Name ?? client.Name ?? "";
            var sitePhone = altSite?.Phone ?? client.Phone ?? "";
            sb.AppendLine($"    <div class='site-name'>   {siteName,-60} {sitePhone,-40}</div>");

            var siteAddress = altSite?.Address ?? client.StreetAddress1 ?? "";
            var siteCity = altSite?.City ?? client.City ?? "";
            var siteState = altSite?.State ?? client.State ?? "";
            var siteZip = altSite?.Zip ?? client.ZipCode ?? "";
            sb.AppendLine($"    <div class='site-address'>   {siteAddress,-20} {siteCity,-15} {siteState,-2} {siteZip,-9}</div>");
        }

        sb.AppendLine("  </div>");

        // Footer - MCL Courier
        sb.AppendLine("  <div class='footer line-13'>MCL Courier</div>");

        sb.AppendLine("</div>");

        return sb.ToString();
    }

    /// <summary>
    /// Generates HTML for lab office forms (TOX LAB)
    /// Legacy format: MCL info 20 lines down with footer
    /// </summary>
    public string GenerateLabOfficeForm(int copies)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<div class='form-container lab-office-form'>");
        sb.AppendLine("  <div class='lab-info line-20'>");

        // Line 1: MCL with phone
        sb.AppendLine($"    <div class='lab-line'>   MCL{GenerateSpaces(50)}731 541 7990</div>");

        // Line 2: Empty
        sb.AppendLine("    <div class='lab-line'></div>");

        // Line 3: Address with fax
        sb.AppendLine($"    <div class='lab-line'>   620 Skyline Drive, JACKSON, TN 38301{GenerateSpaces(15)}731 541 7992</div>");

        sb.AppendLine("  </div>");

        // Footer
        sb.AppendLine("  <div class='lab-footer line-13'>TOX LAB</div>");

        sb.AppendLine("</div>");

        return sb.ToString();
    }

    /// <summary>
    /// Generates HTML for ED Lab forms
    /// Legacy format: ED Lab info 20 lines down
    /// </summary>
    public string GenerateEdLabForm(int copies)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<div class='form-container lab-office-form'>");
        sb.AppendLine("  <div class='lab-info line-20'>");

        // Line 1: ED Lab with phone
        sb.AppendLine($"    <div class='lab-line'>   JMCGH - ED LAB{GenerateSpaces(40)}731 541 4833</div>");

        // Line 2: Empty
        sb.AppendLine("    <div class='lab-line'></div>");

        // Line 3: Address (no fax per specs)
        sb.AppendLine("    <div class='lab-line'>   620 Skyline Drive, JACKSON, TN 38301</div>");

        sb.AppendLine("  </div>");
        sb.AppendLine("</div>");

        return sb.ToString();
    }

    // Helper Methods

    private string FormatWithSpacing(string text, int leftSpaces)
    {
        return $"{GenerateSpaces(leftSpaces)}{text}";
    }

    private string FormatDualColumn(string leftText, string rightText, int leftWidth)
    {
        return $"{leftText,-50}{rightText}";
    }

    private string GenerateSpaces(int count)
    {
        return new string(' ', count);
    }

    private string BuildFullAddress(string? addr1, string? addr2)
    {
        if (string.IsNullOrWhiteSpace(addr1) && string.IsNullOrWhiteSpace(addr2))
            return "";

        if (string.IsNullOrWhiteSpace(addr2))
            return addr1?.Trim() ?? "";

        if (string.IsNullOrWhiteSpace(addr1))
            return addr2?.Trim() ?? "";

        return $"{addr1.Trim()} {addr2.Trim()}";
    }

    private string BuildCityStateZip(string? city, string? state, string? zip)
    {
        var parts = new[] { city?.Trim(), state?.Trim(), zip?.Trim() }
            .Where(p => !string.IsNullOrWhiteSpace(p));

        return string.Join(" ", parts);
    }

    private string FormatFax(string? fax)
    {
        if (string.IsNullOrWhiteSpace(fax))
            return "";

        return $"FAX {fax}";
    }

    private string BuildMnemonicLine(Client client)
    {
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

    private string FormatDapNotation()
    {
        // 13 characters from left + "X" + 20 characters + "DAP11 ZT"
        return $"{GenerateSpaces(13)}X{GenerateSpaces(20)}DAP11 ZT";
    }
}
