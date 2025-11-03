# Requisition Form Layout Implementation Guide

## Overview
This document describes the implementation of laboratory requisition form layouts in the LabOutreach Blazor application, based on the legacy ADDRESS MFC application specifications.

## Component Structure

### 1. FormPrintService.cs
**Location:** `LabBilling Library/Services/FormPrintService.cs`

Service class that generates HTML for various form types with precise positioning matching the legacy application.

**Key Methods:**
- `GenerateRequisitionForm()` - CLIREQ, PTHREQ, CYTREQ forms
- `GenerateCustodyForm()` - Chain of Custody forms
- `GenerateLabOfficeForm()` - TOX LAB forms
- `GenerateEdLabForm()` - Emergency Department forms

### 2. Form Layout Components

#### RequisitionFormLayout.razor
**Location:** `LabOutreachUI/Components/Forms/RequisitionFormLayout.razor`

Handles CLIREQ, PTHREQ, and CYTREQ requisition forms.

**Layout Specifications:**
- 3 lines from top (`line-3` CSS class)
- 50 character left margin for all fields
- Fields displayed:
- Client Name
  - Full Address (combined addr_1 + addr_2)
  - City/State/ZIP
  - Phone
  - Fax (with "FAX " prefix)
  - Client Mnemonic + Code + EMR Type (if present)

**Usage:**
```razor
<RequisitionFormLayout Client="@client" FormType="CLIREQ" />
```

#### CustodyFormLayout.razor
**Location:** `LabOutreachUI/Components/Forms/CustodyFormLayout.razor`

Handles Chain of Custody forms with complex dual-column layout.

**Layout Specifications:**
- Client section starts 6 lines from top
- Dual-column format:
  - Left: Client information (50 char width)
  - Right: MRO information OR "X X X X NONE X X X X"
- Collection site section:
  - 10 lines spacing (or 7 if DAP enabled)
  - 3 character indent
  - Name field: 60 chars + Phone field: 40 chars
  - Address: 20 chars + City: 15 chars + State: 2 chars + ZIP: 9 chars
- Footer: "MCL Courier" centered with 60 char right margin

**Usage:**
```razor
<CustodyFormLayout 
    Client="@client" 
    AlternativeSite="@altSiteData" 
    IncludeDap="@true" />
```

#### LabOfficeFormLayout.razor
**Location:** `LabOutreachUI/Components/Forms/LabOfficeFormLayout.razor`

Handles TOX LAB forms.

**Layout Specifications:**
- Content starts 20 lines from top
- 3 character indent
- Line 1: "MCL" + 50 spaces + "731 541 7990"
- Line 2: Empty
- Line 3: "620 Skyline Drive, JACKSON, TN 38301" + 15 spaces + "731 541 7992"
- Footer: "TOX LAB" centered with 60 char right margin (13 lines spacing)

**Usage:**
```razor
<LabOfficeFormLayout Copies="@quantity" />
```

#### EdLabFormLayout.razor
**Location:** `LabOutreachUI/Components/Forms/EdLabFormLayout.razor`

Handles Emergency Department forms.

**Layout Specifications:**
- Content starts 20 lines from top
- 3 character indent
- Line 1: "JMCGH - ED LAB" + 40 spaces + "731 541 4833"
- Line 2: Empty
- Line 3: "620 Skyline Drive, JACKSON, TN 38301" (no fax)

**Usage:**
```razor
<EdLabFormLayout Copies="@quantity" />
```

### 3. Print Styles
**Location:** `LabOutreachUI/wwwroot/css/requisition-forms.css`

CSS file with precise spacing utilities matching legacy form layouts.

**Key Features:**
- Fixed-width Courier New font
- Character spacing utilities (`.char-1` through `.char-60`)
- Line spacing utilities (`.line-1` through `.line-20`)
- Print-specific media queries
- Form container sizing (8.5" x 11")

**Spacing Utilities:**
```css
.char-50 { margin-left: 50ch; }  /* 50 character indent */
.line-3 { margin-top: 3.6em; }   /* 3 lines spacing */
.line-6 { margin-top: 7.2em; }   /* 6 lines spacing */
.line-10 { margin-top: 12em; }   /* 10 lines spacing */
```

## Form Positioning Details

### Requisition Forms (CLIREQ, PTHREQ, CYTREQ)

```
|<-- 50 chars -->|
 CLIENT NAME
    ADDRESS LINE 1 ADDRESS LINE 2
  CITY STATE ZIP
          PHONE
        FAX 999-999-9999
      MNEM CODE (EMR)
```

### Chain of Custody Forms

```
Client Section (6 lines from top):
|<-- 50 chars -->|<-- Right Column -->|
CLIENT NAME       MRO NAME
ADDRESS         MRO ADDRESS 1
CITY STATE ZIP    MRO ADDRESS 2
PHONE FAX      MRO CITY STATE ZIP
MNEM (CODE)

Collection Site (10 lines spacing OR 7 if DAP):
|<-3->|<-- 60 chars -->|<-- 40 chars -->|
      SITE NAME         SITE PHONE
      ADDRESS     CITY   ST ZIP

Footer (13 lines spacing):
          MCL Courier
```

### Lab Office Forms (TOX LAB)

```
(20 lines from top)
|<-3->|<-- Content -->|
      MCL  731 541 7990

      620 Skyline Drive, JACKSON, TN 38301   731 541 7992

(13 lines spacing)
 TOX LAB
```

### ED Lab Forms

```
(20 lines from top)
|<-3->|<-- Content -->|
      JMCGH - ED LAB          731 541 4833

      620 Skyline Drive, JACKSON, TN 38301
```

## Print Workflow

### 1. Preview
User clicks "Preview" button:
1. Component validates form data
2. Parses selected form type
3. Renders appropriate form layout component
4. Displays in preview container with grey background

### 2. Print
User clicks "Print" button:
1. Component validates form data
2. Records print job in `rpt_track` table
3. Generates form HTML using FormPrintService (future enhancement)
4. Triggers browser print dialog

### 3. Audit Trail
Every print job is tracked with:
- Client name and mnemonic
- Form type
- Printer name
- Quantity printed
- User who printed
- Timestamp
- Application name

## CSS Class Reference

### Container Classes
- `.form-container` - Base form wrapper (8.5" x 11", 1" padding)
- `.requisition-form` - Requisition form specific styles
- `.custody-form` - Custody form specific styles
- `.lab-office-form` - Lab/ED form specific styles

### Content Classes
- `.client-info` - Client information section
- `.field-line` - Individual field line
- `.client-section` - Client section in custody forms
- `.client-line` - Client info line in custody forms
- `.collection-site` - Collection site section
- `.footer` - Form footer section
- `.lab-info` - Lab information section
- `.lab-line` - Lab info line

### Spacing Classes
- `.char-N` - N character left margin
- `.line-N` - N lines top margin

### Preview Classes
- `.print-preview-container` - Preview display container

## Database Schema

### rpt_track Table
Tracks all print jobs for audit purposes:

```sql
CREATE TABLE rpt_track (
    uri INT IDENTITY(1,1) PRIMARY KEY,
    cli_nme VARCHAR(255),   -- Client Name
  form_printed VARCHAR(50),  -- Form Type (CLIREQ, CUSTODY, etc.)
    printer_name VARCHAR(255),-- Printer Used
    qty_printed INT,      -- Quantity Printed
    mod_app VARCHAR(100),         -- Application Name
    mod_date DATETIME,  -- Print Timestamp
    mod_user VARCHAR(30),           -- User who printed
    mod_host VARCHAR(30)      -- Host machine
);
```

## Testing Checklist

### Visual Testing
- [ ] Requisition forms show correct 50-char indent
- [ ] Custody forms show dual-column layout
- [ ] Collection site has 3-char indent
- [ ] Lab office forms show correct spacing
- [ ] ED Lab forms display correctly
- [ ] DAP notation appears when enabled
- [ ] Fonts are Courier New monospace
- [ ] Line spacing matches specifications

### Functional Testing
- [ ] Form preview displays correctly
- [ ] All form types can be selected
- [ ] Alternative site data appears in custody forms
- [ ] Print button records audit trail
- [ ] Validation prevents invalid submissions
- [ ] Multi-copy printing works
- [ ] Printer selection functions
- [ ] User authentication captured correctly

### Print Testing
- [ ] Forms align on pre-printed stock
- [ ] Text positioning is accurate
- [ ] Page breaks work correctly
- [ ] Multiple copies print properly
- [ ] Browser print dialog appears
- [ ] Forms print in monospace font

## Future Enhancements

1. **Print Preview PDF Generation**
   - Generate PDF for preview instead of HTML
   - Allow save-to-file option

2. **Direct Printer Integration**
   - Bypass browser print dialog
   - Support raw printer commands
   - Implement form cutting for compatible printers

3. **Form Templates**
   - Allow customization of form layouts
   - Support client-specific form variations
   - Template management interface

4. **Batch Printing**
 - Print multiple clients at once
   - Queue management
   - Print job history

5. **Alternative Site Presets**
   - Save frequently used alternative sites
   - Quick-select dropdown
   - Site management interface

## Troubleshooting

### Forms Not Aligning
- Verify Courier New font is available
- Check browser print settings (margins, scaling)
- Ensure pre-printed forms are correct version
- Test with .form-container border visible

### Spacing Issues
- Review line-height calculations
- Check character width in browser
- Verify CSS is loaded
- Test in different browsers

### Print Dialog Not Appearing
- Check browser permissions
- Verify JavaScript is enabled
- Test print functionality independently
- Check browser console for errors

## References

- Legacy ADDRESS Application Specifications: `Address Requisition Specifications.md`
- Legacy C++ MFC Source Code Analysis
- Laboratory Form Templates and Pre-printed Stock Specifications
- Medical Center Laboratory (MCL) Printing Standards

---

**Last Updated:** 2025-11-03  
**Version:** 1.0  
**Author:** Lab Patient Accounting Development Team
