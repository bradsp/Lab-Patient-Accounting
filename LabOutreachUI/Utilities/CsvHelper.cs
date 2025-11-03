using System.Text;

namespace LabOutreachUI.Utilities;

/// <summary>
/// Utility class for generating CSV files with proper formatting and escaping.
/// </summary>
public static class CsvHelper
{
    /// <summary>
    /// Escapes a CSV field value by wrapping it in quotes and escaping internal quotes.
    /// </summary>
    /// <param name="value">The value to escape</param>
    /// <returns>Properly escaped CSV field value</returns>
    public static string EscapeField(string? value)
    {
   if (string.IsNullOrEmpty(value))
     return "\"\"";

        // If the value contains quotes, double them
        if (value.Contains('"'))
        {
            value = value.Replace("\"", "\"\"");
  }

        // Always wrap in quotes to handle commas, newlines, etc.
return $"\"{value}\"";
    }

    /// <summary>
    /// Converts a collection of strings into a CSV row.
    /// </summary>
    /// <param name="fields">The field values</param>
    /// <returns>A properly formatted CSV row</returns>
    public static string ToCsvRow(params string?[] fields)
    {
        return string.Join(",", fields.Select(EscapeField));
    }

    /// <summary>
    /// Creates a CSV string from a collection of data with custom column mapping.
    /// </summary>
    /// <typeparam name="T">The type of objects to convert</typeparam>
    /// <param name="data">The collection of objects</param>
    /// <param name="headers">Column headers</param>
    /// <param name="valueSelectors">Functions to extract values for each column</param>
    /// <returns>Complete CSV string with headers and data</returns>
    public static string ToCsv<T>(
        IEnumerable<T> data, 
   string[] headers, 
   params Func<T, string?>[] valueSelectors)
    {
        var csv = new StringBuilder();

        // Add headers
        csv.AppendLine(ToCsvRow(headers));

    // Add data rows
      foreach (var item in data)
        {
            var values = valueSelectors.Select(selector => selector(item)).ToArray();
     csv.AppendLine(ToCsvRow(values));
        }

     return csv.ToString();
    }

    /// <summary>
    /// Generates a safe filename for CSV export with timestamp.
    /// </summary>
    /// <param name="baseName">Base name for the file (will be sanitized)</param>
    /// <returns>Safe filename with .csv extension</returns>
    public static string GenerateFileName(string baseName)
    {
        // Remove invalid filename characters
        var invalidChars = Path.GetInvalidFileNameChars();
        var safeName = string.Join("_", baseName.Split(invalidChars));
        
   // Replace spaces and dashes
 safeName = safeName.Replace(" ", "_").Replace("-", "_");
        
        // Add timestamp
        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        
return $"{safeName}_{timestamp}.csv";
    }

    /// <summary>
    /// Formats a DateTime as a CSV-safe string.
    /// </summary>
    /// <param name="date">The date to format</param>
    /// <param name="defaultValue">Default value if date is null (default: "Never")</param>
    /// <returns>Formatted date string</returns>
    public static string FormatDate(DateTime? date, string defaultValue = "Never")
    {
        return date.HasValue ? date.Value.ToShortDateString() : defaultValue;
    }

    /// <summary>
  /// Formats a boolean as a CSV-friendly string.
    /// </summary>
    /// <param name="value">The boolean value</param>
    /// <param name="trueText">Text for true (default: "Yes")</param>
    /// <param name="falseText">Text for false (default: "No")</param>
    /// <returns>Formatted string</returns>
    public static string FormatBoolean(bool value, string trueText = "Yes", string falseText = "No")
  {
        return value ? trueText : falseText;
    }
}

// Example usage:
/*
// Simple field escaping
var escapedName = CsvHelper.EscapeField("Smith, John");

// Create a row
var row = CsvHelper.ToCsvRow("Name", "Date", "Status");

// Generate CSV from collection
var csv = CsvHelper.ToCsv(
    candidates,
    new[] { "Name", "Shift", "Test Date" },
    c => c.Name,
    c => c.Shift ?? "Unassigned",
    c => CsvHelper.FormatDate(c.TestDate)
);

// Generate filename
var filename = CsvHelper.GenerateFileName("Client Report");
// Result: "Client_Report_20240125_143022.csv"
*/
