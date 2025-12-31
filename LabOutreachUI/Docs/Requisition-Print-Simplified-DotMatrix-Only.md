# Requisition Print Simplified - Dot-Matrix Only

## Overview
The AddressRequisitionPrint component has been simplified to use **only dot-matrix PCL5 raw printing**. The browser print option has been completely removed to streamline the user experience and ensure consistent, precise printing on 3-ply pin-fed forms.

## What Was Removed

### Removed Features
- ? Browser print method selection toggle
- ? Print preview functionality
- ? Browser-based HTML/JavaScript printing
- ? Form print service integration for browser printing
- ? Alternative collection site fields (browser-print specific)
- ? DAP notation checkbox (browser-print specific)
- ? Cut forms checkbox (browser-print specific)
- ? Form HTML generation methods
- ? JavaScript interop dependency

### Removed Code Elements
- `printingMethod` variable
- `useAlternativeSite` variables
- `addDapNotation` variable
- `cutForms` variable
- `previewHtml` variable
- `showPreview` variable
- `FormPrintService` injection
- `IJSRuntime` injection
- `HandlePrintPreview()` method
- `HandleBrowserPrint()` method
- `GenerateFormHtml()` method
- `BuildCompleteHtmlDocument()` method
- `PrintFormHtml()` method
- `GetAlternativeSiteForService()` method
- `GetAlternativeSiteData()` method

## What Remains

### Core Functionality
? **Client Information Display** - Shows client details  
? **Form Type Selection** - CLIREQ, PTHREQ, CYTREQ only  
? **Automatic Printer Selection** - Based on form type  
? **Quantity Input** - Number of forms to print  
? **Printer Selection** - Shows network printers from configuration  
? **Alignment Test** - Verifies form positioning  
? **Print Button** - Sends raw PCL5 to dot-matrix printer  
? **Validation** - Client and printer validation  
? **Error Handling** - Displays errors and success messages  

### Simplified UI Flow

```
1. User lands on page
   ?
2. Client information automatically loaded
   ?
3. Shows info box: "Dot-Matrix PCL5 Raw Printing"
   ?
4. User selects form type (CLIREQ, PTHREQ, or CYTREQ)
   ?
5. Printer auto-selected based on ApplicationParameters
   ?
6. User sets quantity (default: 1)
   ?
7. (Optional) Click "Test Alignment"
   ?
8. Click "Print to Dot-Matrix"
   ?
9. Raw PCL5 commands sent to printer
   ?
10. Success message displayed
```

## User Interface

### Simplified Layout

```
???????????????????????????????????????
? Print Requisition Forms          ?
???????????????????????????????????????
? Client Information (Card)           ?
?  - Client Name & Mnemonic           ?
?  - Address?
?  - Phone/Fax   ?
???????????????????????????????????????

???????????????????????????????????????
? Print Configuration   ?
???????????????????????????????????????
? ? Dot-Matrix PCL5 Raw Printing      ?
?   (Info box explaining method)      ?
?            ?
? Form Type: [Select ?]      ?
?   Default printer: \\PC\Printer    ?
?       ?
? Quantity: [1]        ?
?              ?
? Printer: [\\PC\DotMatrix ?]        ?
?   [Test Alignment]          ?
?   ?
? ? Dot-Matrix Print Settings         ?
? - 10 pitch, 6 LPI    ?
?   - 3 lines from top, 50 char left  ?
?   - Automatic form feed      ?
???????????????????????????????????????
? [Print to Dot-Matrix] [Clear All]   ?
???????????????????????????????????????
```

## Benefits of Simplification

### For Users
? **Less Confusion** - No method selection needed  
? **Faster Workflow** - Fewer clicks and options  
? **Consistent Results** - Always uses precise PCL5 printing
? **Clear Purpose** - Obviously for dot-matrix forms  

### For Developers
? **Reduced Complexity** - ~50% less code  
? **Fewer Dependencies** - No JavaScript/HTML generation  
? **Easier Maintenance** - Single print path  
? **Better Performance** - No browser rendering overhead  

### For Operations
? **Reliable Printing** - Direct hardware control  
? **Consistent Positioning** - 3 lines/50 chars every time  
? **Better Carbon Copies** - Optimized for dot-matrix impact  
? **Network Ready** - Uses UNC printer paths from database  

## Configuration

### Database Setup (Unchanged)
```sql
-- Configure network printers in system_parms
UPDATE dbo.system_parms
SET parm_value = '\\CLIENT-PC\DotMatrixPrinter'
WHERE key_name = 'DefaultClientRequisitionPrinter';
```

### Supported Form Types
- **CLIREQ** - Client Requisition Forms
- **PTHREQ** - Pathology Requisition Forms
- **CYTREQ** - Cytology Requisition Forms

All other form types (CUSTODY, LABOFFICE, EDLAB, EHS) are **not available** in this simplified interface.

## Technical Details

### Print Process
1. **Validate** form inputs and client data
2. **Retrieve** client information from database
3. **Generate** PCL5 commands via `DotMatrixRequisitionService`
4. **Convert** to ASCII bytes
5. **Send** raw data via `RawPrinterHelper` (Win32 APIs)
6. **Record** print job in `rpt_track` table
7. **Display** success/error message

### Dependencies
- `RequisitionPrintingService` - Printer management and validation
- `DotMatrixRequisitionService` - PCL5 formatting
- `PCL5FormatterService` - Low-level PCL5 commands
- `RawPrinterHelper` - Win32 printer APIs
- `DictionaryService` - Client data retrieval
- `AuthenticationStateProvider` - User identification

### Error Handling
- Client validation (name, address required)
- Printer accessibility check
- Form type validation
- Quantity range validation (1-999)
- Win32 API error messages via `RawPrinterHelper.GetLastErrorMessage()`

## Migration Impact

### Breaking Changes
?? **No browser printing** - Users who relied on browser print must use dot-matrix  
?? **No custody/other forms** - Only CLIREQ, PTHREQ, CYTREQ supported  
?? **No preview** - Forms print immediately (use alignment test first)  

### Recommended Communication
```
Subject: Requisition Printing Now Dot-Matrix Only

The requisition print feature has been simplified to use only dot-matrix 
printers with PCL5 raw printing. This provides:

- More accurate form positioning (3 lines/50 chars)
- Better carbon copy quality on 3-ply forms
- Faster printing with fewer options

Please ensure your dot-matrix printer UNC paths are configured in 
System Parameters before printing.

Use the "Test Alignment" button to verify positioning before printing 
actual requisitions.
```

## Testing Checklist

Before deploying to production:

- [ ] Client information displays correctly
- [ ] Form type dropdown shows only CLIREQ, PTHREQ, CYTREQ
- [ ] Printer auto-selects based on form type
- [ ] Configured printer paths appear in dropdown
- [ ] Warning shows when no printers configured
- [ ] Test Alignment button sends pattern to printer
- [ ] Print button sends requisition to dot-matrix printer
- [ ] Forms position correctly (3 lines from top, 50 chars left)
- [ ] 3-ply carbon copies are readable
- [ ] Print jobs recorded in `rpt_track` table
- [ ] Error messages display clearly
- [ ] Success messages confirm print job
- [ ] Clear All button resets form

## Troubleshooting

### "No printers configured" Message
**Cause:** System parameters not set  
**Fix:** Configure in database:
```sql
UPDATE dbo.system_parms
SET parm_value = '\\YOUR-PC\PrinterName'
WHERE key_name IN (
 'DefaultClientRequisitionPrinter',
    'DefaultPathologyReqPrinter',
    'DefaultCytologyRequisitionPrinter'
);
```

### Printer Auto-Selection Not Working
**Cause:** Form type to printer mapping issue  
**Fix:** Verify `GetPrinterForFormType()` returns correct UNC path

### Forms Print with Wrong Positioning
**Cause:** Printer settings or alignment issue  
**Fix:** 
1. Run Test Alignment
2. Verify printer DIP switches (Auto LF: OFF, Auto FF: OFF)
3. Check tractor feed alignment

## Future Enhancements

Potential improvements (if needed):
- [ ] Support for CUSTODY forms in dot-matrix mode
- [ ] Batch printing (multiple clients at once)
- [ ] Print queue status display
- [ ] Printer configuration wizard
- [ ] Form positioning adjustment UI
- [ ] Print history/reprint functionality

## Related Documentation

- **Network-Printer-Configuration-IIS.md** - Network printer setup
- **DotMatrix-Requisition-Testing-Guide.md** - Testing procedures
- **ApplicationParameters-Printer-Configuration.md** - Database configuration
- **PCL5FormatterService.cs** - PCL5 command reference

---
**Document Version:** 1.0  
**Change Type:** Simplification - Removed browser print option  
**Status:** Production Ready  
**Last Updated:** {Current Date}
