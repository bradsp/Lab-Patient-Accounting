using System;
using System.IO;
using System.Text;
using LabBilling.Logging;

namespace LabBilling.Core.Services;

/// <summary>
/// Emulates dot-matrix printer by writing text output to files.
/// Useful for development and testing without physical hardware.
/// Creates both raw text files and human-readable text previews.
/// </summary>
public class PCL5FileEmulatorService
{
    private readonly string _outputDirectory;

    public PCL5FileEmulatorService(string outputDirectory = null)
    {
        _outputDirectory = outputDirectory ?? Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "DotMatrix_Emulator_Output"
        );

     // Create output directory if it doesn't exist
    if (!Directory.Exists(_outputDirectory))
   {
    Directory.CreateDirectory(_outputDirectory);
     Log.Instance.Info($"Created dot-matrix emulator output directory: {_outputDirectory}");
   }
    }

    /// <summary>
 /// Writes plain text data to a file for emulation/testing.
  /// Also creates a human-readable text preview.
    /// </summary>
    /// <param name="textData">Plain text string data</param>
    /// <param name="fileName">Optional filename (auto-generated if not provided)</param>
    /// <returns>Full path to created text file</returns>
    public string WritePCL5ToFile(string textData, string fileName = null)
    {
        if (string.IsNullOrEmpty(fileName))
     {
 fileName = $"DotMatrix_Output_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
  }

      string fullPath = Path.Combine(_outputDirectory, fileName);

      try
        {
       // Write plain text data
    File.WriteAllText(fullPath, textData, Encoding.ASCII);

          Log.Instance.Info($"Dot-matrix text output written to: {fullPath}");

      // Also create a text preview file
        CreateTextPreview(textData, fullPath);
     
   // Create visual layout file
 CreateVisualLayout(textData, fullPath);

      return fullPath;
  }
        catch (Exception ex)
   {
 Log.Instance.Error($"Error writing text file: {ex.Message}", ex);
    throw;
        }
    }

    /// <summary>
    /// Creates a human-readable text preview of plain text output.
    /// Shows line numbers and actual content.
    /// </summary>
    private void CreateTextPreview(string textData, string originalPath)
    {
        string previewPath = Path.ChangeExtension(originalPath, ".txt");
        StringBuilder preview = new StringBuilder();

        preview.AppendLine("??????????????????????????????????????????????????????????????????????????????????");
        preview.AppendLine("?        TEXT OUTPUT PREVIEW      ?");
        preview.AppendLine("??????????????????????????????????????????????????????????????????????????????????");
        preview.AppendLine($"? Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}        ?");
        preview.AppendLine($"? Size: {textData.Length,6} bytes    ?");
        preview.AppendLine("??????????????????????????????????????????????????????????????????????????????????");
        preview.AppendLine();

   // Parse and display text with line numbers
   int lineNumber = 0;
  var lines = textData.Split('\n');

 foreach (var line in lines)
        {
        // Remove carriage return if present
      string cleanLine = line.TrimEnd('\r');
            
       if (cleanLine == "\f" || cleanLine.Contains('\f'))
  {
   preview.AppendLine();
     preview.AppendLine("??????????????????????? FORM FEED (PAGE BREAK) ???????????????????????");
            preview.AppendLine();
         lineNumber = 0;
       continue;
   }

     preview.AppendLine($"{lineNumber,3}: {cleanLine}");
            lineNumber++;
     }

     preview.AppendLine();
        preview.AppendLine("??????????????????????????????? END ???????????????????????????????");

    File.WriteAllText(previewPath, preview.ToString());
        Log.Instance.Debug($"Text preview written to: {previewPath}");
    }

    /// <summary>
    /// Creates a visual layout showing positioning with grid overlay.
    /// </summary>
    private void CreateVisualLayout(string textData, string originalPath)
 {
   string layoutPath = originalPath.Replace(".txt", "_layout.txt");
        StringBuilder layout = new StringBuilder();

        layout.AppendLine("VISUAL LAYOUT PREVIEW (120 columns x 66 lines)");
        layout.AppendLine("Column ruler:");
        layout.AppendLine("    5   10   15   20   25   30   35   40   45   50   55   60   65   70   75   80   85   90   95  100  105  110  115  120");
        layout.AppendLine("....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....");

        // Parse text and display with line numbers
        var lines = textData.Split('\n');
   int lineNumber = 0;

      foreach (var line in lines)
   {
  // Remove carriage return and form feed
     string cleanLine = line.TrimEnd('\r', '\f');
        
    if (line.Contains('\f'))
    {
            layout.AppendLine();
    layout.AppendLine("??????????????????????? FORM FEED ???????????????????????");
                layout.AppendLine();
      lineNumber = 0;
     continue;
          }

    // Pad line to 120 characters to show full width
            string paddedLine = cleanLine.PadRight(120, '.');
     
      // Only show lines with content or first 20 lines
      if (!string.IsNullOrWhiteSpace(cleanLine) || lineNumber < 20)
          {
      layout.AppendLine($"{lineNumber,2}: {paddedLine}");
       }

      lineNumber++;
  
            // Stop after reasonable number of lines to keep file manageable
    if (lineNumber > 66)
            {
          layout.AppendLine("... (remaining lines truncated)");
        break;
            }
        }

        layout.AppendLine();
     layout.AppendLine("Note: '.' represents space beyond actual text");
        layout.AppendLine("      Actual data should start at column 55 (marked by position ruler above)");

        File.WriteAllText(layoutPath, layout.ToString());
        Log.Instance.Debug($"Visual layout written to: {layoutPath}");
    }

    /// <summary>
    /// Processes escape sequences - simplified for text-only mode.
    /// </summary>
 private void ProcessEscapeSequence(string sequence, ref int row, ref int col)
    {
        // Not used in text-only mode, but kept for compatibility
        // Text positioning is now done with spaces and newlines
    }

    /// <summary>
    /// Extracts numeric value - not used in text-only mode.
    /// </summary>
    private string ExtractNumber(string sequence)
    {
        // Not used in text-only mode, but kept for compatibility
        return string.Empty;
    }

    /// <summary>
    /// Opens the emulator output directory in File Explorer.
    /// </summary>
    public void OpenOutputDirectory()
 {
        try
        {
 System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
         FileName = _outputDirectory,
       UseShellExecute = true
   });
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error opening output directory: {ex.Message}", ex);
     }
    }

    /// <summary>
    /// Gets the path to the output directory.
    /// </summary>
    public string GetOutputDirectory() => _outputDirectory;

    /// <summary>
    /// Clears all files from the output directory.
    /// </summary>
    public void ClearOutputDirectory()
    {
        try
      {
          foreach (var file in Directory.GetFiles(_outputDirectory))
            {
    File.Delete(file);
  }
         Log.Instance.Info("Emulator output directory cleared");
        }
    catch (Exception ex)
    {
        Log.Instance.Error($"Error clearing output directory: {ex.Message}", ex);
        }
    }

    /// <summary>
  /// Gets count of files in output directory.
    /// </summary>
    public int GetFileCount()
    {
   try
    {
       return Directory.GetFiles(_outputDirectory).Length;
  }
        catch
{
         return 0;
}
    }
}
