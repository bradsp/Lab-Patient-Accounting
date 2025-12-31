# Requisition Form Printing - User Guide

## Overview
The requisition form printing functionality allows you to print laboratory requisition forms directly from your browser to any configured printer.

---

## How Printing Works

### Browser-Based Printing
The application uses **browser-based printing** which means:

1. **Form Generation**: When you click Print, the application generates HTML content with precise positioning matching your pre-printed forms
2. **Browser Print Dialog**: Your browser's native print dialog opens automatically
3. **Printer Selection**: The printer you selected in the form is suggested, but you can choose a different printer in the dialog
4. **Audit Tracking**: Every print job is recorded in the database with timestamp, user, and details

### Print Flow
```
[Select Client] ? [Choose Form Type] ? [Configure Options] ? [Click Print]
        ?
[Generate HTML] ? [Record in Database] ? [Open Print Dialog] ? [Print to Printer]
```

---

## Step-by-Step Instructions

### 1. Navigate to Requisition Forms
- Go to **Client Viewer** (`/clients`)
- Search for and select your client
- Click **Requisition Forms** button

### 2. Configure Print Job
1. **Client Information**: Verify client details are correct
2. **Alternative Site** (Optional):
   - Check "Use Alternative Collection Site" if needed
   - Fill in site name, address, city, state, ZIP, phone
   - Click Clear to reset
3. **Form Type**: Select from dropdown:
   - Client Requisition Forms (CLIREQ)
   - Path Requisition Forms (PTHREQ)
   - Cytology Requisition Forms (CYTREQ)
   - Chain of Custody Forms
   - Lab Office Forms (TOX LAB)
   - ED Lab Forms
4. **Quantity**: Enter 1-999 (number of copies)
5. **Printer**: Select from available printers
6. **Special Options**:
- ? Add DAP11 ZT notation (for custody forms)
   - ? Cut forms after printing (if printer supports it)

### 3. Preview (Optional)
- Click **Preview** button to see form layout on screen
- Verify positioning and content
- Make adjustments if needed

### 4. Print
- Click **Print** button
- Wait for success message: "Print job recorded successfully!"
- Browser print dialog will open automatically
- **In the Print Dialog**:
  - Verify correct printer is selected
  - Check page orientation (should be Portrait)
  - Ensure margins are set correctly (typically 0.5")
  - Click **Print** to send to printer

---

## Browser Print Dialog Settings

### Chrome/Edge
```
Destination: [Your Selected Printer]
Pages: All
Layout: Portrait
Paper size: Letter (8.5 x 11 in)
Margins: Custom (0.5in all sides)
Scale: 100%
Options: ? Headers and footers
        ? Background graphics
```

### Firefox
```
Printer: [Your Selected Printer]
Page Size: Letter
Orientation: Portrait
Margins: Custom (0.5in)
Scale: 100%
Print Headers and Footers: Unchecked
Print Backgrounds: Unchecked
```

### Safari (Mac)
```
Printer: [Your Selected Printer]
Paper Size: US Letter
Orientation: Portrait
Scale: 100%
Margins: 0.5in
Print backgrounds: No
```

---

## Troubleshooting

### Print Dialog Doesn't Open
**Cause**: Browser may have blocked the print dialog

**Solutions**:
1. Check for popup blocker notification in address bar
2. Allow popups for this site
3. Click Print button again
4. Check browser console (F12) for JavaScript errors

### Forms Don't Align on Pre-Printed Stock
**Causes**:
- Incorrect printer margins
- Wrong paper size
- Browser scaling

**Solutions**:
1. **Check Printer Settings**:
   - Open print dialog
   - Click "More settings" or "Preferences"
   - Set margins to 0.5" on all sides
   - Ensure paper size is Letter (8.5" x 11")

2. **Verify Scale**:
   - Scale must be 100% (not "Fit to page")
   - Turn off "Shrink to fit"

3. **Test Print**:
- Print one copy first
   - Check alignment with pre-printed form
   - Adjust if necessary

### Text is Cut Off or Wrapping
**Cause**: Monospace font not rendering correctly

**Solutions**:
1. Check "Print backgrounds" option is OFF
2. Verify Courier New font is installed
3. Try different browser
4. Clear browser cache

### Printer Not Listed
**Cause**: Printer not configured on computer

**Solutions**:
1. Install printer drivers
2. Add printer in Windows Settings
3. Set printer as shared if on network
4. Refresh page to reload printer list

### Multiple Copies Don't Print
**Cause**: Quantity setting only tracked in database

**Solution**:
- In browser print dialog, set "Copies" to match your quantity
- The application records the quantity you selected
- Browser handles actual copy printing

---

## Features Explained

### Audit Trail
Every print job is automatically tracked:
- **Client Name**: Which client's forms were printed
- **Form Type**: What type of form (CLIREQ, CUSTODY, etc.)
- **Printer**: Which printer was used
- **Quantity**: How many copies printed
- **User**: Who printed (from Windows login)
- **Timestamp**: When printed
- **Application**: Source application name

### Form Types

#### Requisition Forms (CLIREQ, PTHREQ, CYTREQ)
- Client name, address, contact info
- 50-character left margin positioning
- 3 lines from top
- Prints client mnemonic and code
- Includes EMR type if applicable

#### Chain of Custody
- Dual-column layout
- Client info on left
- MRO info on right (or "NONE" markers)
- Collection site section
- DAP11 ZT notation option
- "MCL Courier" footer

#### Lab Office Forms (TOX LAB)
- MCL contact information
- Phone and fax numbers
- Jackson, TN address
- "TOX LAB" footer
- Simple layout for toxicology forms

#### ED Lab Forms
- Emergency Department specific
- JMCGH - ED LAB header
- Contact phone number
- Jackson, TN address

### Alternative Collection Sites
Use when specimen collection is at a different location:
- Employee Health Service locations
- Satellite clinics
- Mobile collection sites
- Temporary locations

### DAP11 ZT Notation
Special notation for Department of Transportation (DOT) testing:
- Only available for Chain of Custody forms
- Indicates DOT-regulated drug testing
- Positioned per DOT requirements

---

## Browser Compatibility

### Fully Supported
- ? Chrome/Edge (Recommended)
- ? Firefox
- ? Safari

### Known Issues
- Internet Explorer: Not supported (use Edge)
- Mobile browsers: Print functionality limited

---

## Best Practices

### Before Printing
1. Always preview first
2. Verify client information is current
3. Check printer has correct form stock loaded
4. Test with single copy before printing multiples

### During Printing
1. Don't close browser until print completes
2. Wait for success message
3. Verify print dialog opened
4. Check printer selection in dialog

### After Printing
1. Verify forms printed correctly
2. Check alignment on pre-printed stock
3. Inspect all copies for quality
4. Report any issues immediately

### Form Stock Management
- Keep pre-printed forms in original packaging until use
- Store in dry, room-temperature environment
- Load correct form type for printer
- Check form alignment periodically

---

## Keyboard Shortcuts

- **Tab**: Navigate between fields
- **Enter**: Submit form (when button focused)
- **Esc**: Close preview or validation messages
- **Ctrl+P**: May trigger print (depending on browser)

---

## Privacy & Security

### Data Protection
- All print jobs logged with user identification
- Audit trail cannot be modified
- PHI protection follows HIPAA guidelines
- Secure authentication required

### Access Control
- Only authorized users can print forms
- Database validation required
- Windows authentication enforced in production
- Print history viewable by administrators only

---

## Technical Details

### Print Format
- HTML/CSS based rendering
- Courier New monospace font
- Precise character positioning
- 8.5" x 11" page size
- Fixed line heights for alignment

### Browser Requirements
- JavaScript enabled
- Popups allowed for this site
- Print access not restricted
- Modern browser (last 2 versions)

### Network Printers
- Must be installed locally
- Share permissions configured
- Driver updated
- Test page prints successfully

---

## FAQ

**Q: Why does the browser print dialog open instead of printing directly?**  
A: Browser security requires user confirmation for printing. This also allows you to verify settings and choose alternatives.

**Q: Can I print to PDF?**  
A: Yes, select "Microsoft Print to PDF" or "Save as PDF" in the print dialog destination.

**Q: Does the quantity field control how many copies print?**  
A: The quantity field records how many you intend to print. Set copies in the browser print dialog to actually print multiple copies.

**Q: What if my printer supports raw commands?**  
A: Raw printer control is not implemented in the web version. Use browser print dialog settings instead.

**Q: Can I customize form layouts?**  
A: Form layouts match legacy specifications exactly. Contact IT for customization requests.

**Q: Why do I see "Print job recorded successfully" but nothing printed?**  
A: The application tracked the request. Check:
   - Browser print dialog may be hidden
   - Printer may be offline
   - Check printer queue
   - Try print preview first

**Q: How do I change the default printer?**  
A: The dropdown shows your Windows default printer. Change in Windows Settings ? Printers, or select different printer in the print dialog.

---

## Support

### Getting Help
- **Technical Issues**: Contact IT Support
- **Form Alignment**: Contact Lab Operations
- **Data Issues**: Contact Lab Billing
- **Training**: Request from supervisor

### Reporting Issues
Include in your support request:
1. Client name or mnemonic
2. Form type being printed
3. Error message (if any)
4. Browser and version
5. Printer name
6. Screenshot if applicable

---

**Version**: 1.0  
**Last Updated**: 2025-11-03  
**Application**: LabOutreach - Client Requisition Printing
