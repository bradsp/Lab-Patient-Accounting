# Dot-Matrix Printer Emulation for Development

## Overview
This guide provides multiple approaches to emulate dot-matrix PCL5 printing without physical hardware, enabling faster development and testing of the requisition printing system.

## Option 1: PCL5 File Output + Viewer (RECOMMENDED)

### Implementation
Create a file-based printer emulator that captures PCL5 output to files that can be viewed.

### Step 1: Create PCL5 File Writer Service

```csharp
// LabBilling Library/Services/PCL5FileEmulatorService.cs
using System;
using System.IO;
using System.Text;
using LabBilling.Logging;

namespace LabBilling.Core.Services;

/// <summary>
/// Emulates dot-matrix printer by writing PCL5 output to files.
/// Useful for development and testing without physical hardware.
/// </summary>
public class PCL5FileEmulatorService
{
    private readonly string _outputDirectory;

    public PCL5FileEmulatorService(string outputDirectory = null)
{
        _outputDirectory = outputDirectory ?? Path.Combine(
Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
         "PCL5_Emulator_Output"
   );

        // Create output directory if it doesn't exist
        if (!Directory.Exists(_outputDirectory))
        {
      Directory.CreateDirectory(_outputDirectory);
 }
    }

  /// <summary>
    /// Writes PCL5 data to a file for emulation/testing.
    /// </summary>
    /// <param name="pcl5Data">Raw PCL5 string data</param>
    /// <param name="fileName">Optional filename (auto-generated if not provided)</param>
    /// <returns>Full path to created file</returns>
    public string WritePCL5ToFile(string pcl5Data, string fileName = null)
    {
        if (string.IsNullOrEmpty(fileName))
   {
   fileName = $"PCL5_Output_{DateTime.Now:yyyyMMdd_HHmmss}.pcl";
        }

    string fullPath = Path.Combine(_outputDirectory, fileName);

        try
        {
     // Write raw PCL5 data (binary mode to preserve escape sequences)
        File.WriteAllText(fullPath, pcl5Data, Encoding.ASCII);

            Log.Instance.Info($"PCL5 output written to: {fullPath}");

            // Also create a text preview file
    CreateTextPreview(pcl5Data, fullPath);

       return fullPath;
        }
        catch (Exception ex)
        {
            Log.Instance.Error($"Error writing PCL5 file: {ex.Message}", ex);
      throw;
        }
    }

    /// <summary>
 /// Creates a human-readable text preview of PCL5 output.
    /// Shows escape sequences and interprets positioning commands.
    /// </summary>
    private void CreateTextPreview(string pcl5Data, string originalPath)
    {
        string previewPath = Path.ChangeExtension(originalPath, ".txt");
    StringBuilder preview = new StringBuilder();

        preview.AppendLine("=== PCL5 OUTPUT PREVIEW ===");
        preview.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        preview.AppendLine($"Size: {pcl5Data.Length} bytes");
        preview.AppendLine(new string('=', 80));
        preview.AppendLine();

        // Parse and display PCL5 commands
        int lineNumber = 1;
        int column = 0;
        StringBuilder currentLine = new StringBuilder();

        for (int i = 0; i < pcl5Data.Length; i++)
        {
            char c = pcl5Data[i];

            if (c == '\x1B') // ESC character
          {
       // Parse PCL5 escape sequence
    string escapeSeq = ExtractEscapeSequence(pcl5Data, i);
                preview.Append($"[PCL: {EscapeSequenceToString(escapeSeq)}]");
  i += escapeSeq.Length - 1;
            }
       else if (c == '\n')
            {
         preview.AppendLine($"Line {lineNumber,3}: {currentLine}");
     currentLine.Clear();
    lineNumber++;
    column = 0;
            }
            else if (c == '\r')
  {
                column = 0;
            preview.Append("[CR]");
     }
else if (c == '\f')
{
  preview.AppendLine($"Line {lineNumber,3}: {currentLine}");
   preview.AppendLine();
        preview.AppendLine("=== FORM FEED (PAGE BREAK) ===");
      preview.AppendLine();
  currentLine.Clear();
          lineNumber = 1;
            column = 0;
  }
            else if (c >= 32 && c <= 126) // Printable ASCII
      {
      currentLine.Append(c);
      column++;
            }
            else
    {
        currentLine.Append($"[0x{(int)c:X2}]");
 }
        }

        if (currentLine.Length > 0)
        {
     preview.AppendLine($"Line {lineNumber,3}: {currentLine}");
        }

  File.WriteAllText(previewPath, preview.ToString());
      Log.Instance.Debug($"Text preview written to: {previewPath}");
    }

    /// <summary>
    /// Extracts a complete PCL5 escape sequence starting at the given position.
    /// </summary>
    private string ExtractEscapeSequence(string data, int startIndex)
    {
if (startIndex >= data.Length || data[startIndex] != '\x1B')
     return "";

        StringBuilder sequence = new StringBuilder();
        sequence.Append(data[startIndex]); // ESC
        int i = startIndex + 1;

        if (i < data.Length)
        {
     sequence.Append(data[i]); // Parameterized character

          i++;
            while (i < data.Length && (char.IsDigit(data[i]) || data[i] == '.' || data[i] == '-'))
    {
              sequence.Append(data[i]);
      i++;
  }

            if (i < data.Length)
  {
            sequence.Append(data[i]); // Terminating character
      }
        }

      return sequence.ToString();
    }

    /// <summary>
    /// Converts PCL5 escape sequence to human-readable description.
    /// </summary>
    private string EscapeSequenceToString(string sequence)
    {
   if (sequence.Length < 2) return sequence;

  char paramChar = sequence[1];
  string value = sequence.Length > 3 ? sequence.Substring(2, sequence.Length - 3) : "";
        char terminator = sequence[sequence.Length - 1];

        return sequence switch
        {
            "\x1BE" => "RESET",
    _ when paramChar == '&' && terminator == 'H' => $"SET_COL({value})",
            _ when paramChar == '*' && terminator == 'p' && sequence.Contains('Y') => $"SET_ROW({value})",
            _ when paramChar == '*' && terminator == 'p' && sequence.Contains('X') => $"SET_COL({value})",
         _ when paramChar == '(' && terminator == 's' => $"FONT_PITCH({value})",
            _ when paramChar == '(' && terminator == 'H' => $"FONT_SPACING({value})",
      _ when paramChar == '&' && terminator == 'l' => $"PAGE_LENGTH({value})",
         _ => sequence.Replace("\x1B", "ESC")
  };
    }

 /// <summary>
    /// Opens the emulator output directory in File Explorer.
    /// </summary>
    public void OpenOutputDirectory()
    {
        try
        {
System.Diagnostics.Process.Start("explorer.exe", _outputDirectory);
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
}
```

### Step 2: Update RequisitionPrintingService for Emulation

```csharp
// Add to RequisitionPrintingService.cs

/// <summary>
/// Prints to PCL5 file emulator for development/testing.
/// </summary>
public async Task<(bool success, string message, string filePath)> PrintToEmulatorAsync(
    string clientMnemonic,
    FormType formType = FormType.CLIREQ,
    int copies = 1,
    string userName = "System")
{
    try
    {
        using var uow = new UnitOfWorkMain(_appEnvironment);
        var (isValid, errors) = await ValidateClientForPrinting(clientMnemonic, uow);
        
        if (!isValid)
        {
            return (false, string.Join("; ", errors), null);
        }

        var client = uow.ClientRepository.GetClient(clientMnemonic);
        if (client == null)
{
    return (false, "Client not found", null);
        }

        // Generate PCL5 data
        string pcl5Data = _dotMatrixService.FormatRequisition(client, formType.ToString());

 // Write to emulator
 var emulator = new PCL5FileEmulatorService();
        string fileName = $"{formType}_{client.ClientMnem}_{DateTime.Now:yyyyMMdd_HHmmss}.pcl";
        string filePath = emulator.WritePCL5ToFile(pcl5Data, fileName);

        // Record print job
   await RecordPrintJobAsync(
   client.ClientMnem,
    client.Name,
   formType,
          copies,
            "PCL5_EMULATOR",
     userName,
 "EmulatorMode");

        Log.Instance.Info($"Emulated print: {filePath}");
return (true, $"PCL5 output saved to: {filePath}", filePath);
    }
    catch (Exception ex)
    {
Log.Instance.Error($"Error in emulator print: {ex.Message}", ex);
  return (false, $"Error: {ex.Message}", null);
    }
}
```

### Step 3: Update UI for Emulation Mode

```csharp
// Add to AddressRequisitionPrint.razor @code section

private bool emulatorMode = false; // Set to true for development

private async Task HandlePrint()
{
    if (!ValidateForm()) return;

    isProcessing = true;
validationErrors.Clear();
    successMessage = null;

    try
    {
        if (PrintingService == null || client == null) return;

        string userName = "Unknown";
        if (AuthenticationStateProvider != null)
        {
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        userName = authState.User?.Identity?.Name ?? "Unknown";
      }

        if (!Enum.TryParse<RequisitionPrintingService.FormType>(selectedFormType, out var formType))
     {
      validationErrors.Add("Invalid form type selected");
     return;
   }

        var (isValid, errors) = await PrintingService.ValidateClientForPrinting(ClientMnemonic, UnitOfWork);
        if (!isValid)
        {
    validationErrors.AddRange(errors);
            return;
        }

     if (emulatorMode || string.IsNullOrEmpty(selectedPrinter) || selectedPrinter == "EMULATOR")
     {
            // Use emulator mode
            var (success, message, filePath) = await PrintingService.PrintToEmulatorAsync(
       ClientMnemonic,
     formType,
    quantity,
       userName
        );

            if (success)
        {
 successMessage = $"{message}\n\nCheck the .txt file for readable preview.";
 }
       else
            {
        validationErrors.Add(message);
         }
     }
        else
        {
            // Use real printer
    var (success, message) = await PrintingService.PrintToDotMatrixAsync(
    ClientMnemonic,
         selectedPrinter!,
                formType,
         quantity,
       userName
            );

            if (success)
      {
       successMessage = message;
       }
         else
            {
        validationErrors.Add(message);
            }
        }
    }
    catch (Exception ex)
    {
   validationErrors.Add($"An error occurred: {ex.Message}");
    }
    finally
    {
        isProcessing = false;
    }
}
```

## Option 2: GhostPCL (PCL Interpreter)

### Download & Install
1. Download GhostPCL from: https://www.ghostscript.com/download/gpcldnld.html
2. Install to `C:\Program Files\ghostpcl\`

### Convert PCL to PDF
```powershell
# PowerShell script to convert PCL5 output to PDF
& "C:\Program Files\ghostpcl\gpcl6win64.exe" `
    -sDEVICE=pdfwrite `
    -sOutputFile="output.pdf" `
    -dNOPAUSE `
    -dBATCH `
    "C:\Users\bpowers\Documents\PCL5_Emulator_Output\PCL5_Output_20240101_120000.pcl"
```

### Automate Conversion
```csharp
// Add method to PCL5FileEmulatorService
public string ConvertPCLToPDF(string pclFilePath)
{
    string ghostPclPath = @"C:\Program Files\ghostpcl\gpcl6win64.exe";
    if (!File.Exists(ghostPclPath))
    {
   throw new FileNotFoundException("GhostPCL not found. Please install from ghostscript.com");
    }

    string pdfPath = Path.ChangeExtension(pclFilePath, ".pdf");
    
    var process = new System.Diagnostics.Process
    {
        StartInfo = new System.Diagnostics.ProcessStartInfo
     {
            FileName = ghostPclPath,
            Arguments = $"-sDEVICE=pdfwrite -sOutputFile=\"{pdfPath}\" -dNOPAUSE -dBATCH \"{pclFilePath}\"",
        UseShellExecute = false,
            RedirectStandardOutput = true,
         CreateNoWindow = true
        }
    };

    process.Start();
    process.WaitForExit();

    if (File.Exists(pdfPath))
    {
        Log.Instance.Info($"PDF created: {pdfPath}");
        return pdfPath;
  }

    return null;
}
```

## Option 3: Virtual Printer (RedMon + Ghostscript)

### Setup
1. Install Ghostscript: https://ghostscript.com/releases/gsdnld.html
2. Install RedMon (Print Redirector): http://pages.cs.wisc.edu/~ghost/redmon/
3. Create virtual printer that outputs to file

### Configuration
```
Printer Name: Virtual Dot-Matrix
Port: RPT1: (RedMon port)
Redirect to: C:\PCL_Output\output_%d.pcl
Run Program: gswin64c.exe
Arguments: -sDEVICE=txtwrite -sOutputFile="C:\PCL_Output\text_%d.txt" -
```

## Option 4: PCL5 to Text Converter (Quick & Simple)

### Implementation
```csharp
// LabBilling Library/Services/PCL5ToTextConverter.cs
using System;
using System.Text;

namespace LabBilling.Core.Services;

/// <summary>
/// Simple converter to strip PCL5 commands and show text content.
/// Useful for quick visual verification during development.
/// </summary>
public class PCL5ToTextConverter
{
    /// <summary>
    /// Converts PCL5 output to plain text by removing escape sequences.
    /// </summary>
    public string ConvertToText(string pcl5Data)
    {
        StringBuilder text = new StringBuilder();
        bool inEscapeSequence = false;

    foreach (char c in pcl5Data)
        {
   if (c == '\x1B') // ESC character
   {
                inEscapeSequence = true;
      continue;
  }

      if (inEscapeSequence)
{
                // Skip until we find a letter (end of escape sequence)
          if (char.IsLetter(c))
                {
       inEscapeSequence = false;
         }
     continue;
            }

   // Keep printable characters and newlines
   if (c == '\n' || c == '\r' || (c >= 32 && c <= 126))
            {
         text.Append(c);
       }
  else if (c == '\f')
        {
                text.AppendLine("\n\n[PAGE BREAK]\n\n");
  }
      }

        return text.ToString();
    }

    /// <summary>
 /// Creates a visual representation showing positioning.
  /// </summary>
    public string VisualizeLayout(string pcl5Data)
    {
        var lines = new System.Collections.Generic.List<StringBuilder>();
    int currentLine = 0;
        int currentColumn = 0;

        // Initialize grid (11" form, 80 columns wide, 66 lines tall)
        for (int i = 0; i < 66; i++)
        {
            lines.Add(new StringBuilder(new string('.', 80)));
        }

        bool inEscape = false;
        StringBuilder escapeSeq = new StringBuilder();

        foreach (char c in pcl5Data)
        {
            if (c == '\x1B')
          {
       inEscape = true;
      escapeSeq.Clear();
         escapeSeq.Append(c);
       continue;
 }

            if (inEscape)
            {
    escapeSeq.Append(c);
            if (char.IsLetter(c))
          {
         // Process escape sequence
 ProcessEscapeForVisualization(escapeSeq.ToString(), ref currentLine, ref currentColumn);
  inEscape = false;
                }
      continue;
       }

 // Place printable characters
   if (c >= 32 && c <= 126)
        {
      if (currentLine < lines.Count && currentColumn < 80)
    {
         lines[currentLine][currentColumn] = c;
      currentColumn++;
                }
    }
            else if (c == '\n')
            {
       currentLine++;
             currentColumn = 0;
            }
     else if (c == '\r')
      {
    currentColumn = 0;
}
        }

        // Build output with line numbers
        StringBuilder output = new StringBuilder();
        for (int i = 0; i < lines.Count; i++)
        {
            if (i == 0 || i % 5 == 0)
         {
           output.AppendLine($"{i,3}: {lines[i]}");
 }
        }

     return output.ToString();
    }

    private void ProcessEscapeForVisualization(string sequence, ref int line, ref int column)
 {
        // Simplified - just handle basic positioning
        if (sequence.Contains("*p") && sequence.Contains("Y"))
        {
     // Vertical position
          string numStr = sequence.Substring(4, sequence.IndexOf('Y') - 4);
            if (int.TryParse(numStr, out int row))
{
         line = row / 12; // Convert decipoints to lines (6 LPI)
            }
        }
        else if (sequence.Contains("*p") && sequence.Contains("X"))
    {
 // Horizontal position
            string numStr = sequence.Substring(4, sequence.IndexOf('X') - 4);
            if (int.TryParse(numStr, out int col))
{
                column = col / 12; // Convert decipoints to columns (10 CPI)
      }
    }
    }
}
```

## Option 5: Browser-Based Visualizer (HTML/JavaScript)

### Create Web Preview
```csharp
// Generate HTML preview of PCL5 output
public string GenerateHTMLPreview(string pcl5Data)
{
    var converter = new PCL5ToTextConverter();
  string textContent = converter.ConvertToText(pcl5Data);

    return $@"
<!DOCTYPE html>
<html>
<head>
    <title>PCL5 Preview</title>
    <style>
        body {{
         font-family: 'Courier New', monospace;
            font-size: 12px;
      background: #f0f0f0;
        margin: 20px;
 }}
        .page {{
            background: white;
 width: 8.5in;
          height: 11in;
 padding: 0.5in;
            margin: 20px auto;
  box-shadow: 0 0 10px rgba(0,0,0,0.3);
            white-space: pre;
         position: relative;
        }}
        .ruler {{
            font-size: 8px;
            color: #ccc;
          position: absolute;
            top: 0;
            left: 0;
     right: 0;
        }}
        .line-numbers {{
            position: absolute;
     left: 0;
 top: 0;
bottom: 0;
            width: 30px;
            background: #f8f8f8;
        font-size: 8px;
            color: #999;
        }}
        .content {{
          margin-left: 40px;
     }}
    </style>
</head>
<body>
    <div class='page'>
        <div class='ruler'>....5....10...15...20...25...30...35...40...45...50...55...60...65...70...75...80</div>
        <div class='content'>{System.Web.HttpUtility.HtmlEncode(textContent)}</div>
    </div>
</body>
</html>";
}
```

## Development Workflow

### Recommended Approach
1. **Development**: Use PCL5 File Emulator (Option 1)
2. **Visual Check**: Use Text Converter or HTML Preview (Options 4 & 5)
3. **Final Verification**: Use GhostPCL to PDF (Option 2)
4. **Production Testing**: Use actual dot-matrix printer

### Example Development Session
```csharp
// 1. Generate PCL5 to file
var emulator = new PCL5FileEmulatorService();
string pclFile = emulator.WritePCL5ToFile(pcl5Data, "test_requisition.pcl");

// 2. Create text preview (auto-created)
// Check: C:\Users\bpowers\Documents\PCL5_Emulator_Output\test_requisition.txt

// 3. Convert to PDF for visual check
string pdfFile = emulator.ConvertPCLToPDF(pclFile);
System.Diagnostics.Process.Start(pdfFile); // Open in default PDF viewer

// 4. Check positioning in visual grid
var converter = new PCL5ToTextConverter();
string visual = converter.VisualizeLayout(pcl5Data);
Console.WriteLine(visual);
```

## Verification Checklist

Using emulated output, verify:
- [ ] Client name appears at correct position (line 5, column 55)
- [ ] Address appears below name
- [ ] City/State/ZIP on one line
- [ ] Phone number included
- [ ] Fax with "FAX" prefix
- [ ] Mnemonic/code line at bottom
- [ ] Form feed at end
- [ ] No extra blank lines
- [ ] Character spacing correct (10 pitch)
- [ ] All text within 80 column width

## Tools Comparison

| Tool | Pros | Cons | Best For |
|------|------|------|----------|
| **File Emulator** | Fast, no install, detailed output | Manual viewing | Development |
| **GhostPCL** | Industry standard, PDF output | External dependency | Final verification |
| **Virtual Printer** | Acts like real printer | Complex setup | Integration testing |
| **Text Converter** | Instant feedback | No formatting | Quick checks |
| **HTML Preview** | Browser viewing, portable | Approximate layout | Sharing/documentation |

## Production Checklist

Before deploying to production with real printers:
1. ? Test all three form types (CLIREQ, PTHREQ, CYTREQ)
2. ? Verify alignment on actual forms
3. ? Check 3-ply carbon quality
4. ? Test with various client data (long names, multiple addresses)
5. ? Verify form feed works correctly
6. ? Test error handling (invalid printer, network issues)

---
**Recommendation**: Start with **Option 1 (File Emulator)** for fastest iteration, then validate with **Option 2 (GhostPCL to PDF)** before production testing.
