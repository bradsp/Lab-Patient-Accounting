# Text-Only Dot-Matrix Printing - Migration Summary

## Change Overview
Converted from **PCL5 mode** to **plain text mode** for dot-matrix requisition printing.

## Why the Change?
- **PCL5 escape codes were printing literally** instead of being interpreted
- **Simpler approach** matches legacy ADDRESS application behavior
- **More compatible** with basic dot-matrix printer text modes
- **Eliminates complexity** of PCL language negotiation

## What Changed

### Before (PCL5 Mode)
```csharp
// Used PCL5 escape sequences
output.Append("\x1B%-12345X");  // UEL
output.Append("@PJL ENTER LANGUAGE = PCL\n");
output.Append("\x1B(s10H");  // Set 10 pitch
output.Append("\x1B*p600Y");  // Vertical position
output.Append("\x1B*p3960X");  // Horizontal position
output.Append("Client Name\r\n");
```

**Issues:**
- Printer not interpreting PCL5 commands
- Escape codes printing as literal text
- Font pitch not being set (120 columns instead of 80)

### After (Text-Only Mode)
```csharp
// Uses simple text with spaces and newlines
for (int i = 0; i < 5; i++)
    output.AppendLine();  // 5 blank lines from top

output.AppendLine(new string(' ', 55) + "Client Name");// 55 spaces from left
output.AppendLine(new string(' ', 55) + "Address");
output.Append('\f');  // Form feed
```

**Benefits:**
- ? No escape codes to interpret
- ? Works in any printer text mode
- ? Simpler, more reliable
- ? Matches legacy behavior exactly

## Positioning Method

### Vertical Positioning
- **Blank lines** from top of form
- `START_LINE = 5` means 5 newlines (`\n`) before first data line

### Horizontal Positioning
- **Space characters** for left margin
- `LEFT_MARGIN = 55` means 55 spaces before text

### Example Output
```
[blank line 0]
[blank line 1]
[blank line 2]
[blank line 3]
[blank line 4]
   CLIENT NAME HERE
    123 MAIN STREET
   CITY STATE ZIP
             (555) 123-4567
          FAX (555) 123-4568
  CLIENTMNEM (12345)
[form feed]
```

## Updated Constants

```csharp
private const int START_LINE = 5;   // Lines from top (was 5, still 5)
private const int LEFT_MARGIN = 55; // Spaces from left (was 55, still 55)
```

## Test Pattern Output

### Old PCL5 Test Pattern Issues
- Escape codes printed as text
- Width showed ~120 characters instead of 80
- Ruler and grid corrupted by literal escape sequences

### New Text-Only Test Pattern
```
    5   10   15   20   25   30   35   40   45   50   55   60   65   70   75   80   85   9095  100  105  110  115  120
....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....+....
L01 |   |...10|...20|...30|...40|...50|...60|...70|...80|...90|...100|...110
L02 |   |...10|...20|...30|...40|...50|...60|...70|...80|...90|...100|...110
...


          >>> REQUISITION DATA STARTS AT LINE 5, COLUMN 55 <<<

Line 0: (blank line for spacing)
Line 1: (blank line for spacing)
Line 2: (blank line for spacing)
Line 3: (blank line for spacing)
Line 4: (blank line for spacing)
            CLIENT NAME APPEARS HERE
           ADDRESS LINE APPEARS HERE
       CITY STATE ZIP APPEARS HERE
```

## Verification Steps

1. **Run alignment test**:
   - Select emulator or real printer
   - Click "Test Alignment"
   - Check output shows proper spacing

2. **Verify positioning**:
   - Open `_layout.txt` file from emulator
   - Count spaces before text
   - Should be exactly 55 spaces

3. **Check column width**:
   - Ruler now goes to 120 to accommodate wider printer settings
   - Text should still start at column 55 regardless

## Printer Settings Required

### No Special Settings Needed!
- Works with printer in **default text mode**
- No PCL mode switching required
- No font commands needed

### DIP Switch Settings (if applicable)
- **Auto Line Feed**: OFF (form handles its own \n)
- **Auto Form Feed**: OFF (form sends \f explicitly)
- **Character Set**: USA/ASCII
- **Character Pitch**: Any (we use spaces, so doesn't matter)

## File Changes

| File | Change |
|------|--------|
| `DotMatrixRequisitionService.cs` | Removed PCL5FormatterService dependency, use plain text |
| `PCL5FileEmulatorService.cs` | Updated to handle plain text instead of PCL5 |
| `RequisitionPrintingService.cs` | Updated comments from "PCL5" to "plain text" |
| `AddressRequisitionPrint.razor` | Updated UI text from "PCL5" to "text printing" |

## Emulator Output

### Files Created
1. **`DotMatrix_Output_YYYYMMDD_HHMMSS.txt`** - Raw text output (what printer receives)
2. **`DotMatrix_Output_YYYYMMDD_HHMMSS.txt`** - Preview with line numbers
3. **`DotMatrix_Output_YYYYMMDD_HHMMSS_layout.txt`** - Visual grid showing positioning

### Output Directory
Changed from: `C:\Users\{User}\Documents\PCL5_Emulator_Output\`  
To: `C:\Users\{User}\Documents\DotMatrix_Emulator_Output\`

## Troubleshooting

### If text doesn't align properly

**Check START_LINE constant:**
```csharp
private const int START_LINE = 5;  // Adjust number of blank lines
```

**Check LEFT_MARGIN constant:**
```csharp
private const int LEFT_MARGIN = 55;  // Adjust number of spaces
```

### If printer has wrong character pitch

**Don't worry!** Since we're using spaces for positioning instead of PCL pitch commands, the printer's character pitch setting doesn't affect alignment. As long as the printer uses a monospaced font (which all dot-matrix text modes do), 55 spaces will always position text consistently.

### If forms still look wrong

1. Print alignment test
2. Measure actual position on form with ruler
3. Count characters/spaces in printout
4. Adjust `START_LINE` and `LEFT_MARGIN` accordingly
5. Test again

## Benefits of Text-Only Mode

| Aspect | PCL5 Mode | Text-Only Mode |
|--------|-----------|----------------|
| **Complexity** | High | Low |
| **Compatibility** | PCL5-capable printers only | Any text-capable printer |
| **Setup** | Mode switching, font commands | None |
| **Debugging** | Difficult (binary escape codes) | Easy (readable text) |
| **Reliability** | Depends on PCL interpretation | Very reliable |
| **Legacy Match** | Tried to emulate | **Exactly matches** |

## Testing Recommendations

1. ? Test alignment pattern first
2. ? Verify with emulator output files
3. ? Test on actual forms with real printer
4. ? Check all three form types (CLIREQ, PTHREQ, CYTREQ)
5. ? Verify 3-ply carbon quality
6. ? Test batch printing

## Migration Complete

The system now uses simple, reliable text-only printing that:
- Matches the legacy ADDRESS application behavior exactly
- Works with any dot-matrix printer in text mode
- Eliminates PCL5 interpretation issues
- Is easier to debug and maintain

---
**Status:** ? Ready for Testing  
**Next Step:** Print alignment test on actual forms to verify positioning  
**Fallback:** Constants can be adjusted if positioning needs fine-tuning
