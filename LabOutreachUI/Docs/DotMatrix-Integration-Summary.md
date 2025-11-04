# Dot-Matrix Printing Integration for AddressRequisitionPrint Component

## Summary
Successfully integrated dot-matrix PCL5 printing functionality into the `AddressRequisitionPrint.razor` Blazor component. Users can now choose between standard browser printing and direct dot-matrix printing for 3-ply pin-fed requisition forms.

## Changes Made

### 1. **Printing Method Selection**
Added a toggle to choose between two printing methods:
- **Browser Print (Standard)**: Traditional web-based printing using the browser's print dialog
- **Dot-Matrix (3-Ply Pin-Fed)**: Direct PCL5 commands sent to dot-matrix printer, bypassing browser/GDI

### 2. **Form Type Filtering**
When dot-matrix mode is selected:
- Only displays form types compatible with dot-matrix printing (CLIREQ, PTHREQ, CYTREQ)
- Shows warning message if user selects incompatible form type
- Automatically filters the form type dropdown based on selected printing method

### 3. **Alignment Test Feature**
Added "Test Alignment" button (visible only in dot-matrix mode):
- Prints a test pattern to verify form positioning
- Displays grid with line/column markers
- Helps users verify 3-line/50-character positioning before printing actual forms

### 4. **Dual Print Handlers**
Implemented separate print logic for each method:

#### **HandleDotMatrixPrint()**
- Validates client data
- Calls `PrintingService.PrintToDotMatrixAsync()`
- Sends raw PCL5 commands directly to printer via Win32 APIs
- Records print job in audit trail

#### **HandleBrowserPrint()**
- Generates HTML form content
- Uses JavaScript interop for browser printing
- Records print job in audit trail

### 5. **UI Enhancements**
- Dynamic button labels based on printing method
- Contextual help text explaining each mode
- Dot-matrix specific settings display (pitch, spacing, margins)
- Visual indicators for dot-matrix requirements

### 6. **Helper Methods**
Added supporting methods:
- `IsDotMatrixSupported()` - Checks if form type works with dot-matrix
- `GetSupportedFormTypes()` - Returns filtered form types based on method
- `HandleAlignmentTest()` - Triggers alignment test pattern print
- Preserved existing methods for browser printing compatibility

## How It Works

### Dot-Matrix Print Flow
1. User selects "Dot-Matrix (3-Ply Pin-Fed)" method
2. Chooses compatible form type (CLIREQ, PTHREQ, or CYTREQ)
3. Selects dot-matrix printer from dropdown
4. (Optional) Clicks "Test Alignment" to verify positioning
5. Clicks "Print to Dot-Matrix"
6. System:
   - Validates client information
   - Generates PCL5-formatted data using `DotMatrixRequisitionService`
   - Converts to ASCII bytes
   - Sends directly to printer using `RawPrinterHelper` (Win32 APIs)
   - Records print job in database

### Browser Print Flow (Unchanged)
1. User selects "Browser Print (Standard)" method
2. Chooses any form type
3. Can preview before printing
4. Clicks "Print"
5. System:
   - Generates HTML form
   - Uses JavaScript to trigger browser print dialog
   - Records print job in database

## Technical Implementation

### PCL5 Generation
The dot-matrix printing leverages these services:
- **`DotMatrixRequisitionService`**: Formats client data into PCL5 commands
- **`PCL5FormatterService`**: Generates low-level PCL5 escape sequences
- **`RawPrinterHelper`**: Win32 API wrapper for direct printer communication

### Form Positioning
- **Start Line**: 3 lines from top of form
- **Left Margin**: 50 characters from left edge
- **Font**: 10 pitch Courier (10 characters per inch)
- **Line Spacing**: 6 lines per inch
- **Form Feed**: Automatic after each page

## Configuration Requirements

### Application Parameters
The system uses `ApplicationParameters.UseDotMatrixRawPrinting` flag to enable/disable feature globally.

### Printer Setup
For dot-matrix printing to work properly:
1. Printer must support PCL5
2. Configured for pin-fed/tractor feed mode
3. DIP switches set correctly:
   - Auto Line Feed: OFF
   - Auto Form Feed: OFF
   - Skip Perforation: OFF
4. Installed in Windows with "RAW" data type support

## Benefits

### Accuracy
- Precise positioning using PCL5 commands (decipoint resolution)
- Consistent output across all print jobs
- No browser rendering variations

### Reliability
- Direct hardware control
- No GDI/printer driver interference
- Works with legacy dot-matrix printers

### Carbon Copy Quality
- Optimized impact settings for 3-ply forms
- Proper line spacing for readable copies
- Compatible with continuous forms

## User Experience

### Visual Feedback
- Clear method selection with icons
- Context-sensitive help text
- Warning messages for incompatible configurations
- Success/error messages with detailed information

### Ease of Use
- One-click method switching
- Automatic form type filtering
- Built-in alignment testing
- Preserved familiar browser print option

## Testing Recommendations

Before production use:
1. Run alignment test on each dot-matrix printer
2. Verify 3-ply carbon copy quality
3. Test batch printing (multiple copies)
4. Confirm form positioning on actual forms
5. Validate audit trail recording

## Troubleshooting

### If alignment is off:
1. Run alignment test
2. Adjust `START_LINE` and `LEFT_MARGIN` constants in `DotMatrixRequisitionService`
3. Verify printer DIP switch settings

### If print fails:
1. Check printer is online/ready
2. Verify printer supports PCL5
3. Ensure "RAW" data type is configured
4. Review error message from `RawPrinterHelper.GetLastErrorMessage()`

### If carbon copies are faint:
1. Increase printer impact/darkness setting
2. Replace printer ribbon
3. Check form quality (old forms may have degraded carbon)

## Future Enhancements

Potential improvements:
- Support for CUSTODY forms in dot-matrix mode
- Configurable positioning (via app settings)
- Batch printing multiple clients
- Print queue status monitoring
- Printer capability detection

## Documentation

See also:
- `DotMatrix-Requisition-Testing-Guide.md` - Complete testing procedures
- `PCL5FormatterService.cs` - PCL5 command reference
- `RawPrinterHelper.cs` - Win32 API documentation

---
**Component**: `LabOutreachUI/Components/Clients/AddressRequisitionPrint.razor`  
**Last Updated**: {Current Date}  
**Author**: Lab Patient Accounting Development Team
