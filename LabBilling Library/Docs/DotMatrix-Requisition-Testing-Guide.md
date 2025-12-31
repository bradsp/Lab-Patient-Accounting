# Dot-Matrix Requisition Printing - Testing Guide

## Overview
This document provides instructions for testing the optimized PCL5 requisition printing system for 3-ply pin-fed dot-matrix forms.

## Prerequisites
- Dot-matrix printer configured in Windows (e.g., Epson LQ-series, OKI Microline, etc.)
- PCL5-compatible printer driver installed
- 3-ply pin-fed requisition forms loaded in printer
- Printer configured for tractorfeed mode

## Printer Setup

### 1. Physical Setup
1. Load 3-ply pin-fed forms into tractor feed
2. Ensure forms are aligned at perforation (top-of-form position)
3. Set printer to **Tractor/Pin-Feed mode** (not friction feed)
4. Verify paper guide pins engage form perforations
5. Set printer DIP switches or software to:
   - **Auto Line Feed: OFF** (critical - prevents double-spacing)
   - **Auto Form Feed: OFF** (we control form feeds via PCL5)
   - **Skip Perforation: OFF** (important for continuous forms)

### 2. Windows Printer Configuration
1. Open **Devices and Printers**
2. Right-click your dot-matrix printer ? **Printer Properties**
3. **Device Settings** tab:
   - Set Paper Source to "Tractor Feeder" or "Pin Feed"
   - Set Form Size to match your forms (typically 11" length)
4. **Advanced** tab ? **Printing Defaults**:
   - Quality: Draft or Letter Quality (as needed)
   - Color: Black & White
5. **Apply** changes

### 3. Configure Application Settings
In your application configuration (database or settings file), set:

```sql
-- Example SQL to configure printer settings
UPDATE dbo.system_parms 
SET parm_value = 'Your_DotMatrix_Printer_Name'
WHERE key_name = 'DefaultClientRequisitionPrinter';

UPDATE dbo.system_parms 
SET parm_value = 'True'
WHERE key_name = 'UseDotMatrixRawPrinting';
```

## Test Procedures

### Test 1: Alignment Test Pattern
**Purpose:** Verify form positioning before printing actual data

```csharp
// Example C# code to run alignment test
var printService = new RequisitionPrintingService(appEnvironment);
bool success = printService.PrintAlignmentTest("Your_Printer_Name");

if (!success)
{
  Console.WriteLine($"Error: {RawPrinterHelper.GetLastErrorMessage()}");
}
```

**Expected Output:**
- Grid with column/line rulers
- ">>> REQUISITION DATA STARTS HERE <<<" at line 3, column 50
- Check if text aligns with pre-printed form fields

**If Misaligned:**
1. Adjust `START_LINE` constant in `DotMatrixRequisitionService.cs`
2. Adjust `LEFT_MARGIN` constant for horizontal alignment
3. Re-test until aligned

### Test 2: Single Requisition Print
**Purpose:** Print one client requisition

```csharp
var printService = new RequisitionPrintingService(appEnvironment);
var (success, message) = await printService.PrintToDotMatrixAsync(
    clientMnemonic: "TESTCLI",
    printerName: "Your_Printer_Name",
    formType: RequisitionPrintingService.FormType.CLIREQ,
    copies: 1,
    userName: "TestUser"
);

Console.WriteLine($"Success: {success}, Message: {message}");
```

**Verification:**
- Client name appears at correct position
- Address fields print at 50-character left margin
- All 3 plies produce clear, readable carbon copies
- Form ejects cleanly after printing (form feed works)

### Test 3: Batch Printing (Multiple Clients)
**Purpose:** Test continuous form feeding

```csharp
var clientList = new[] { "CLIENT1", "CLIENT2", "CLIENT3" };
var (successCount, failCount, errors) = await printService.PrintBatchToDotMatrixAsync(
    clientMnemonics: clientList,
    printerName: "Your_Printer_Name",
    formType: RequisitionPrintingService.FormType.CLIREQ,
    copiesPerClient: 1,
    userName: "TestUser"
);

Console.WriteLine($"Successful: {successCount}, Failed: {failCount}");
foreach (var error in errors)
{
    Console.WriteLine($"Error: {error}");
}
```

**Verification:**
- Forms advance automatically between requisitions
- No skipped forms or double-feeds
- Consistent positioning across all forms
- Print queue shows completed jobs

### Test 4: Carbon Copy Quality
**Purpose:** Verify all 3 plies are legible

**Procedure:**
1. Print a single requisition
2. Separate the 3-ply form
3. Check each ply:
   - **White (original):** Should be darkest
   - **Yellow (copy 1):** Should be clearly readable
   - **Pink (copy 2):** Should be readable (may be lighter)

**If carbon copies are faint:**
- Increase printer impact/darkness setting
- Check ribbon condition (replace if worn)
- Verify form plies are making proper contact

## Troubleshooting

### Problem: Print Queue Shows Error
**Causes:**
- Printer not online/ready
- Forms jammed
- Printer door open

**Solutions:**
1. Check printer display/lights for error indicators
2. Verify forms are properly loaded
3. Check `RawPrinterHelper.GetLastErrorMessage()` for details

### Problem: Text Not Positioned Correctly
**Horizontal Misalignment:**
- Adjust `LEFT_MARGIN` in `DotMatrixRequisitionService.cs`
- Verify printer is set to 10 pitch (10 characters per inch)

**Vertical Misalignment:**
- Adjust `START_LINE` constant
- Check printer line spacing (should be 6 lines/inch)
- Verify top-of-form alignment

### Problem: Forms Not Ejecting
**Causes:**
- Auto form feed enabled (should be OFF)
- Skip perforation enabled (should be OFF)

**Solutions:**
1. Check printer DIP switch settings
2. Verify printer driver settings in Windows
3. Ensure PCL5 form feed command (`\f`) is being sent

### Problem: Double Spacing
**Cause:** Auto line feed is enabled

**Solution:**
- Set printer Auto LF to OFF (DIP switch or control panel)
- Check printer manual for specific instructions

### Problem: Poor Print Quality on Carbon Copies
**Solutions:**
1. **Increase impact/darkness:**
   - Adjust printer control panel setting
   - May need to set to "Letter Quality" instead of "Draft"

2. **Replace ribbon:**
   - Worn ribbon produces faint copies
   - Use fabric ribbon (not film) for carbon copies

3. **Check form quality:**
   - Old or improperly stored forms may have degraded carbon
   - Test with fresh forms

### Problem: Forms Misalign After Several Prints
**Cause:** Cumulative positioning errors

**Solutions:**
1. Check tractor feed tension (too loose or too tight)
2. Verify forms are properly seated on pins
3. May need to pause between large batch jobs
4. Consider form feed calibration on printer

## Performance Notes

- **Speed:** Dot-matrix is slower than laser (typically 200-300 cps)
- **Batch printing:** Allow time between forms for mechanical movement
- **Ribbon life:** Fabric ribbons last 3-5 million characters
- **Noise:** Dot-matrix printers are loud - consider location

## Best Practices

1. **Daily:**
   - Check ribbon condition
   - Verify form alignment at start of day
   - Test print one form before batch

2. **Weekly:**
   - Clean print head with isopropyl alcohol
   - Check tractor feed mechanism
   - Verify form supply

3. **Monthly:**
   - Run full alignment test
   - Clean paper path
   - Lubricate tractor feed (if specified in manual)

4. **Quarterly:**
   - Replace ribbon proactively
   - Deep clean printer
   - Calibrate form feed if needed

## Additional Resources

- **PCL5 Reference:** [HP PCL 5 Technical Reference Manual](https://developers.hp.com/hp-labs/pcl)
- **Printer Manuals:** Consult manufacturer documentation for specific models
- **Form Specifications:** Verify form dimensions match printer capabilities

## Support Contacts

For assistance with requisition printing:
- **Internal IT Support:** [Your contact info]
- **Printer Vendor:** [Vendor support contact]
- **Forms Supplier:** [Supplier contact]

---
**Document Version:** 1.0  
**Last Updated:** {Current Date}  
**Author:** Lab Patient Accounting - Development Team
