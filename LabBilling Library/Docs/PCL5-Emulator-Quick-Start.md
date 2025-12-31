# PCL5 Emulator - Quick Start Guide

## ? Emulator Now Integrated!

The PCL5 File Emulator is now built into the application, allowing you to develop and test dot-matrix printing **without access to physical printers**.

## How to Use

### Option 1: Via UI (Development Mode)

When running in **DEBUG mode**, the printer dropdown includes:
```
*** PCL5 EMULATOR (Development) ***
```

**Steps:**
1. Run application in DEBUG mode
2. Navigate to Print Requisition Forms
3. Select client
4. Choose form type (CLIREQ, PTHREQ, or CYTREQ)
5. Select **"*** PCL5 EMULATOR (Development) ***"** from printer dropdown
6. Click "Print to Dot-Matrix"
7. Check output files in: `C:\Users\{YourUsername}\Documents\PCL5_Emulator_Output\`

### Option 2: Programmatically

```csharp
// In your test code
var printingService = new RequisitionPrintingService(appEnvironment);

// Print requisition to emulator
var (success, message, filePath) = await printingService.PrintToEmulatorAsync(
    "CLIENTMNEM",
    RequisitionPrintingService.FormType.CLIREQ,
    copies: 1,
    "TestUser"
);

if (success)
{
  Console.WriteLine(message);
    // Files created in Documents\PCL5_Emulator_Output\
}
```

### Option 3: Direct Emulator Use

```csharp
// Generate PCL5 data
var dotMatrixService = new DotMatrixRequisitionService();
string pcl5Data = dotMatrixService.FormatRequisition(client, "CLIREQ");

// Write to emulator
var emulator = new PCL5FileEmulatorService();
string filePath = emulator.WritePCL5ToFile(pcl5Data);

// Output directory auto-opens with:
emulator.OpenOutputDirectory();
```

## Output Files

For each print job, **three files** are created:

### 1. `*.pcl` - Raw PCL5 File
Binary file containing actual PCL5 escape sequences. Can be:
- Sent to real dot-matrix printer later
- Converted to PDF using GhostPCL
- Inspected with hex editor

### 2. `*.txt` - Text Preview
Human-readable preview showing:
- Line numbers
- Actual text content
- Form feed markers
- **This is your primary verification file**

Example output:
```
??????????????????????????????????????????????????????????????????????????????????
?       PCL5 OUTPUT TEXT PREVIEW           ?
??????????????????????????????????????????????????????????????????????????????????
? Generated: 2024-01-15 10:30:45        ?
? Size:  512 bytes         ?
??????????????????????????????????????????????????????????????????????????????????

  1: 
  2: 
  3: 
  4: 
  5:         ACME Medical Center
  6:  123 Main Street
  7:        Springfield IL 62701
  8:       (217) 555-1234
  9:  FAX (217) 555-1235
 10:            ACME (12345)

??????????????????????? FORM FEED (PAGE BREAK) ???????????????????????
```

### 3. `*_layout.txt` - Visual Layout
Grid-based visualization showing:
- Column ruler (0-80)
- Actual character positions
- Dots (`.`) for empty space
- **Use this to verify positioning**

Example output:
```
VISUAL LAYOUT PREVIEW (80 columns x 66 lines @ 10 pitch, 6 LPI)
Column ruler:
    5   10   15   20   25   30   35   40   45   50   55   60   65   70   75   80
....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....

 0: ................................................................................
 1: ................................................................................
 2: ................................................................................
 3: ................................................................................
 4: ................................................................................
 5: ......................................................ACME Medical Center......
 6: ......................................................123 Main Street...........
 7: ......................................................Springfield IL 62701.....
 8: ......................................................(217) 555-1234............
 9: ......................................................FAX (217) 555-1235.......
10: ......................................................ACME (12345)..............
```

## Verification Checklist

Using the output files, verify:

- [ ] **Line 5, Column 55** - Client name starts here
- [ ] **Subsequent lines** - Address, city/state/zip follow
- [ ] **Phone/Fax** - Fax has "FAX" prefix
- [ ] **Mnemonic line** - Format: `MNEM (CODE)` or `MNEM CODE (EMR)`
- [ ] **Form feed** - Present at end
- [ ] **No extra blanks** - Clean, compact output
- [ ] **Within 80 columns** - Nothing past column 80

## Troubleshooting

### Can't find output files
**Location**: `C:\Users\{YourUsername}\Documents\PCL5_Emulator_Output\`

Or programmatically:
```csharp
var emulator = new PCL5FileEmulatorService();
string directory = emulator.GetOutputDirectory();
emulator.OpenOutputDirectory(); // Opens in Explorer
```

### Text appears at wrong position
Check `_layout.txt` file:
- Should start at line 5 (index 5 in grid)
- Should start at column 55 (55 dots from left)

If wrong, adjust constants in `DotMatrixRequisitionService.cs`:
```csharp
private const int START_LINE = 5;   // Lines from top
private const int LEFT_MARGIN = 55; // Characters from left
```

### Want to clear old test files
```csharp
var emulator = new PCL5FileEmulatorService();
emulator.ClearOutputDirectory();
```

## Converting PCL to PDF (Optional)

For final visual verification, install GhostPCL and convert:

```powershell
# Install GhostPCL from: https://ghostscript.com/releases/gpcldnld.html

# Convert PCL to PDF
& "C:\Program Files\ghostpcl\gpcl6win64.exe" `
    -sDEVICE=pdfwrite `
    -sOutputFile="C:\Temp\output.pdf" `
    -dNOPAUSE `
    -dBATCH `
    "C:\Users\bpowers\Documents\PCL5_Emulator_Output\PCL5_Output_20240115_103045.pcl"

# Opens the PDF
Start-Process "C:\Temp\output.pdf"
```

## Development Workflow

**Recommended iteration cycle:**

1. **Make code changes** to `DotMatrixRequisitionService.cs`
2. **Run application** in DEBUG mode
3. **Select emulator** from printer dropdown
4. **Click print** - generates files
5. **Check `_layout.txt`** - verify positioning
6. **Check `.txt`** - verify content
7. **Repeat** until correct
8. **Test on real printer** when positioning looks good

## Production Mode

In **RELEASE builds**, the emulator option is **not shown**. Users only see actual configured printers from `ApplicationParameters`.

To force emulator in production (for testing):
```csharp
// Manually set printer name
selectedPrinter = "*** PCL5 EMULATOR (Development) ***";
```

## Integration with CI/CD

Emulator can be used in automated tests:

```csharp
[Test]
public async Task TestRequisitionPrinting()
{
    // Arrange
    var service = new RequisitionPrintingService(appEnvironment);
    
    // Act
    var (success, message, filePath) = await service.PrintToEmulatorAsync(
  "TESTCLIENT",
        RequisitionPrintingService.FormType.CLIREQ
  );
    
    // Assert
    Assert.IsTrue(success);
    Assert.IsTrue(File.Exists(filePath));
    
    // Verify text content
    string txtFile = Path.ChangeExtension(filePath, ".txt");
    string content = File.ReadAllText(txtFile);
    
 Assert.IsTrue(content.Contains("Test Client Name"));
    Assert.IsTrue(content.Contains("FORM FEED"));
}
```

## Benefits

? **Develop without hardware** - No dot-matrix printer needed  
? **Fast iteration** - Instant feedback via text files  
? **Version control** - PCL files can be committed for regression testing  
? **Team collaboration** - Share output files for review  
? **Automated testing** - Integration with unit/integration tests  
? **Documentation** - Output serves as examples for users  

## Next Steps

Once positioning looks good in emulator:
1. Save PCL file
2. Copy to machine with real dot-matrix printer
3. Use `RawPrinterHelper` to send to real printer:
   ```csharp
 byte[] pclData = File.ReadAllBytes("path/to/file.pcl");
   RawPrinterHelper.SendBytesToPrinter("\\\\SERVER\\DotMatrix", pclData, "Test");
   ```
4. Verify 3-ply carbon quality
5. Adjust if needed and repeat

---
**Pro Tip**: Keep a collection of `.pcl` files for different client scenarios (long addresses, multiple phone numbers, etc.) for regression testing!
