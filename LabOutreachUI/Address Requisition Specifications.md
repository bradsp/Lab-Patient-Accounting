# ADDRESS Application Analysis and Modern Replacement Specifications

**Analysis Date:** November 2, 2025  
**Legacy Application:** C++ MFC Windows Application (circa 1999-2015)  
**Purpose:** Client address printing on laboratory requisition forms

---

## Executive Summary

This document provides a comprehensive analysis of the legacy ADDRESS MFC application and specifications for developing a modern replacement. The application serves as a specialized printing utility for Medical Center Laboratory (MCL), enabling the printing of client information on various types of blank requisition and custody forms.

---

## Legacy Application Analysis

### Core Purpose
The ADDRESS application prints client name and address information on blank requisition forms of different types for a medical laboratory (MCL - Medical Center Laboratory). It serves as a specialized printing utility for laboratory forms with direct database integration and specialized printer control.

### Key Features Identified

#### 1. Client Management
- **Client Database Integration**: Connects to MCL's SQL database (`client` table)
- **Client Search/Selection**: Dropdown combo box with all active clients (filtered to exclude deleted clients)
- **Client Information Display**:
  - Client name, address, city/state/zip
  - Phone number and fax number
  - Client mnemonic code
  - Client facility code (for Cerner integration)
  - Medical Review Officer (MRO) information
  - Electronic Medical Record (EMR) type
  - Print location flag

#### 2. Form Types & Printing
The application supports multiple form types:
- **CLIREQ** (Client Requisition Forms)
- **PTHREQ** (Path Requisition Forms) 
- **CYTREQ** (Cytology Requisition Forms)
- **Chain of Custody Forms** (for specimen tracking)
- **Lab Office Forms** (TOX LAB)
- **ED Lab Forms** (Emergency Department)
- **EHS Forms** (Employee Health Service)

#### 3. Specialized Printing Features
- **Multi-copy printing**: Specify number of copies (1-999)
- **Form cutting**: Automatic form cutting for Printek printers
- **Printer-specific form loading**: Different forms loaded based on printer name
- **Raw printer control**: Bypasses Windows drivers for direct printer communication
- **Form positioning**: Precise positioning of text on pre-printed forms

#### 4. Alternative Collection Site Management
- Optional alternative collection site information
- Editable fields for different collection locations
- Special handling for Employee Health Service sites
- DAP11 ZT notation option for specific forms
- Toggle between standard client address and alternative site

#### 5. Audit Trail & Tracking
- **Usage Tracking**: Records all print jobs in `rpt_track` table
- **Audit Information**: Client name, form type, printer, quantity, timestamp
- **Error Logging**: Comprehensive error handling and logging
- **Version Tracking**: Application version recorded with each print job

---

## Database Schema Analysis

Based on the code analysis, the application requires these database tables:

### CLIENT Table Structure
```sql
-- Primary client information table
CREATE TABLE client (
    cli_nme VARCHAR(255) PRIMARY KEY,    -- Client Name
    addr_1 VARCHAR(255),                 -- Address Line 1
    addr_2 VARCHAR(255),                 -- Address Line 2
    city VARCHAR(100),                   -- City
    st VARCHAR(10),                      -- State
    zip VARCHAR(20),                     -- ZIP Code
    phone VARCHAR(50),                   -- Phone Number
    fax VARCHAR(50),                     -- Fax Number
    cli_mnem VARCHAR(20),                -- Client Mnemonic
    facilityNo VARCHAR(50),              -- Client Code for Cerner
    mro_name VARCHAR(255),               -- Medical Review Officer Name
    mro_addr1 VARCHAR(255),              -- MRO Address Line 1
    mro_addr2 VARCHAR(255),              -- MRO Address Line 2
    mro_city VARCHAR(100),               -- MRO City
    mro_st VARCHAR(10),                  -- MRO State
    mro_zip VARCHAR(20),                 -- MRO ZIP Code
    prn_loc CHAR(1),                     -- Print Location Flag (Y/N)
    electronic_billing_type VARCHAR(50), -- EMR Type
    deleted BIT DEFAULT 0                -- Soft Delete Flag
);
```

### RPT_TRACK Table Structure
```sql
-- Print job tracking table
CREATE TABLE rpt_track (
    uri INT IDENTITY(1,1) PRIMARY KEY,   -- Unique Record ID
    cli_nme VARCHAR(255),                -- Client Name
    form_printed VARCHAR(50),            -- Form Type Printed
    printer_name VARCHAR(255),           -- Printer Used
    qty_printed INT,                     -- Quantity Printed
    mod_app VARCHAR(100),                -- Application/Version
    print_timestamp DATETIME DEFAULT GETDATE(), -- Print Date/Time
    user_name VARCHAR(100)               -- User who printed (new field)
);
```

---

## Modern Application Specifications

### Technology Stack Recommendations

#### Option 1: Web-Based Application
- **Frontend**: React.js or Angular with TypeScript
- **Backend**: ASP.NET Core Web API
- **Database**: SQL Server or PostgreSQL
- **Printing**: Browser Print API with custom CSS for form layouts
- **Authentication**: Azure AD or OAuth 2.0

#### Option 2: Desktop Application
- **Framework**: .NET MAUI or WPF (.NET 8+)
- **Database**: SQL Server with Entity Framework Core
- **Printing**: .NET PrintDocument with custom form renderers
- **Deployment**: ClickOnce or MSIX packaging

#### Option 3: Hybrid Approach
- **Desktop**: Electron with React/Vue.js
- **Backend**: Node.js with Express or .NET Core
- **Database**: PostgreSQL or SQL Server
- **Printing**: Node.js printing libraries or browser print APIs

### Core Functional Requirements

#### FR-1: Client Management
- **FR-1.1**: Search and filter clients by name with autocomplete
- **FR-1.2**: Display complete client information in read-only fields
- **FR-1.3**: Real-time validation of client data integrity
- **FR-1.4**: Handle missing client codes with visual warnings
- **FR-1.5**: Support for client status indicators (active/inactive)

#### FR-2: Form Printing System
- **FR-2.1**: Support multiple form types with different layouts:
  - Requisition forms (CLIREQ, PTHREQ, CYTREQ)
  - Chain of custody forms
  - Lab office forms
  - Emergency department forms
- **FR-2.2**: Print specified quantities (1-999 copies) with validation
- **FR-2.3**: Auto-format client information for proper form positioning
- **FR-2.4**: Support alternative collection site data overlay
- **FR-2.5**: Print preview functionality for all form types

#### FR-3: Printer Management
- **FR-3.1**: Modern printer selection interface with printer status
- **FR-3.2**: Support various printer types and drivers
- **FR-3.3**: Handle form cutting/separation commands where supported
- **FR-3.4**: Print job queue management and status monitoring
- **FR-3.5**: Printer-specific configuration profiles

#### FR-4: Alternative Collection Sites
- **FR-4.1**: Toggle between standard client address and alternative site
- **FR-4.2**: Editable alternative site information fields
- **FR-4.3**: Preset configurations for common alternative sites (EHS, etc.)
- **FR-4.4**: Validation of alternative site data completeness
- **FR-4.5**: Save/recall frequently used alternative sites

#### FR-5: Data Validation & Error Handling
- **FR-5.1**: Validate all required fields before printing
- **FR-5.2**: Check client existence and active status
- **FR-5.3**: Validate quantity limits and printer capabilities
- **FR-5.4**: Comprehensive error handling with user-friendly messages
- **FR-5.5**: Data integrity checks and warnings

#### FR-6: Audit Trail & Reporting
- **FR-6.1**: Log all print jobs with complete details
- **FR-6.2**: Track usage by client, form type, user, and date
- **FR-6.3**: Generate usage reports and analytics
- **FR-6.4**: Maintain comprehensive audit trail for compliance
- **FR-6.5**: Export audit data in various formats (CSV, PDF, Excel)

### User Interface Specifications

```
┌─────────────────────────────────────────────────────────────┐
│ MCL Client Address Printing System                         │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│ Client Selection                                            │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ Search Client: [________________] 🔍                    │ │
│ │ [Dropdown List of Matching Clients]                    │ │
│ └─────────────────────────────────────────────────────────┘ │
│                                                             │
│ Client Information                                          │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ Name: [____________________________________]            │ │
│ │ Address: [_________________________________]            │ │
│ │ City/State/ZIP: [_________________________]             │ │
│ │ Phone: [________________] Fax: [___________]             │ │
│ │ Client Code: [__________] MRO: [___________]             │ │
│ │ EMR Type: [_____________]                               │ │
│ └─────────────────────────────────────────────────────────┘ │
│                                                             │
│ Alternative Collection Site                                 │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ ☐ Use Alternative Site  [Presets ▼] [Edit] [Clear]     │ │
│ │ Name: [____________________________________]            │ │
│ │ Address: [_________________________________]            │ │
│ │ City: [____________] State: [__] ZIP: [_____]           │ │
│ │ Phone: [________________]                               │ │
│ └─────────────────────────────────────────────────────────┘ │
│                                                             │
│ Print Configuration                                         │
│ ┌─────────────────────────────────────────────────────────┐ │
│ │ Form Type:                                              │ │
│ │ ○ Requisition Forms    ○ Chain of Custody              │ │
│ │ ○ Lab Office Forms     ○ ED Lab Forms                  │ │
│ │                                                         │ │
│ │ Quantity: [___] (1-999)                                │ │
│ │ Printer: [Select Printer ▼] [Status: Ready]           │ │
│ │                                                         │ │
│ │ Special Options:                                        │ │
│ │ ☐ Add DAP11 ZT notation                               │ │
│ │ ☐ Cut forms after printing (if supported)             │ │
│ └─────────────────────────────────────────────────────────┘ │
│                                                             │
│ [Preview] [Print] [Clear All] [Help]                      │ │
│                                                             │
│ Status: Ready to print                                      │
└─────────────────────────────────────────────────────────────┘
```

### Technical Architecture

#### System Architecture
```
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│   Client App    │◄──►│   Web API/       │◄──►│    Database     │
│  (Desktop/Web)  │    │   Service Layer  │    │  (SQL Server)   │
└─────────────────┘    └──────────────────┘    └─────────────────┘
         │                       │                       │
         ▼                       ▼                       ▼
┌─────────────────┐    ┌──────────────────┐    ┌─────────────────┐
│  Print Manager  │    │  Audit Service   │    │   Report Engine │
│    Service      │    │                  │    │                 │
└─────────────────┘    └──────────────────┘    └─────────────────┘
```

#### Security Requirements
- **Authentication**: Multi-factor authentication support
- **Authorization**: Role-based access control (RBAC)
- **Data Security**: Encrypted connections and data at rest
- **Audit Security**: Tamper-proof audit logging
- **Compliance**: HIPAA compliance for medical data handling

#### Performance Requirements
- **Response Time**: Client search results within 200ms
- **Print Speed**: Form generation within 1 second
- **Concurrent Users**: Support 20+ simultaneous users
- **Database**: Optimized queries with proper indexing
- **Memory Usage**: Efficient memory management for large client lists

### Migration Strategy

#### Phase 1: Assessment & Planning (2-4 weeks)
- **Data Assessment**: Analyze current database structure and data quality
- **Hardware Audit**: Assess current printer infrastructure
- **User Requirements**: Gather detailed requirements from current users
- **Risk Analysis**: Identify potential migration risks and mitigation strategies

#### Phase 2: Development (8-12 weeks)
- **Database Migration**: Design and implement new database schema
- **Core Application**: Develop main application functionality
- **Print System**: Implement modern printing capabilities
- **Testing**: Comprehensive testing with real data and forms

#### Phase 3: Deployment & Training (4-6 weeks)
- **Pilot Deployment**: Deploy to limited user group for testing
- **User Training**: Train staff on new system
- **Data Migration**: Migrate production data
- **Go-Live Support**: Provide intensive support during transition

#### Phase 4: Post-Implementation (2-4 weeks)
- **Performance Monitoring**: Monitor system performance and usage
- **User Feedback**: Collect and address user feedback
- **Optimization**: Fine-tune performance and features
- **Documentation**: Complete system documentation

### Data Migration Specifications

#### Client Data Migration
```sql
-- Sample migration script for client data
INSERT INTO new_client_table (
    client_name, address_line1, address_line2, 
    city, state, zip_code, phone_number, fax_number,
    client_mnemonic, facility_number, mro_name,
    mro_address1, mro_address2, mro_city, mro_state, mro_zip,
    print_location_flag, emr_type, is_active
)
SELECT 
    cli_nme, addr_1, addr_2,
    city, st, zip, phone, fax,
    cli_mnem, facilityNo, mro_name,
    mro_addr1, mro_addr2, mro_city, mro_st, mro_zip,
    CASE WHEN prn_loc = 'Y' THEN 1 ELSE 0 END,
    electronic_billing_type,
    CASE WHEN deleted = 0 THEN 1 ELSE 0 END
FROM legacy_client_table
WHERE cli_nme IS NOT NULL;
```

#### Audit Trail Preservation
```sql
-- Preserve existing audit trail
INSERT INTO new_audit_table (
    client_name, form_type, printer_name,
    quantity_printed, application_version,
    print_date, legacy_record_id
)
SELECT 
    cli_nme, form_printed, printer_name,
    qty_printed, mod_app, 
    COALESCE(print_timestamp, GETDATE()),
    uri
FROM legacy_rpt_track_table;
```

### Testing Strategy

#### Unit Testing
- **Data Access Layer**: Test all database operations
- **Business Logic**: Test client validation and form generation
- **Print Services**: Test print formatting and job management
- **User Interface**: Test all UI components and interactions

#### Integration Testing
- **Database Integration**: Test with real legacy data
- **Printer Integration**: Test with actual printers and forms
- **System Integration**: Test complete workflows end-to-end
- **Performance Testing**: Load testing with multiple concurrent users

#### User Acceptance Testing
- **Workflow Testing**: Test all current user workflows
- **Form Quality**: Verify print quality and positioning
- **Data Accuracy**: Verify all client data displays correctly
- **Error Handling**: Test error conditions and recovery

### Maintenance & Support

#### Ongoing Maintenance
- **Database Maintenance**: Regular backup and optimization
- **Application Updates**: Security patches and feature updates
- **Printer Driver Updates**: Maintain compatibility with printer updates
- **Performance Monitoring**: Continuous performance monitoring

#### Support Structure
- **User Documentation**: Comprehensive user manuals and help system
- **Technical Documentation**: System architecture and maintenance guides
- **Training Materials**: Video tutorials and quick reference guides
- **Help Desk**: Support structure for user issues and questions

### Success Metrics

#### Technical Metrics
- **System Uptime**: 99.9% availability target
- **Performance**: Sub-second response times for all operations
- **Print Quality**: Zero positioning errors on forms
- **Data Integrity**: 100% data accuracy during migration

#### Business Metrics
- **User Adoption**: 100% user migration within 30 days
- **Print Volume**: Maintain or increase current print volumes
- **Error Reduction**: 50% reduction in print errors
- **User Satisfaction**: 90%+ satisfaction rating in post-implementation survey

---

## Conclusion

The legacy ADDRESS application serves a critical function in the laboratory workflow, and its replacement must maintain all current functionality while providing modern usability and maintainability. The specifications outlined in this document provide a roadmap for developing a robust, secure, and user-friendly replacement that will serve the organization's needs for the next decade and beyond.

The modern replacement should focus on:
1. **Maintaining Core Functionality**: All current printing capabilities must be preserved
2. **Improving User Experience**: Modern interface with better usability
3. **Enhancing Security**: Modern authentication and audit capabilities
4. **Ensuring Scalability**: Architecture that can grow with organizational needs
5. **Providing Flexibility**: Easy configuration and customization options

By following these specifications, the new application will provide a solid foundation for laboratory form printing operations while positioning the organization for future technological advances.

---

## Form Layout Specifications for Blazor Implementation

### Overview
The legacy ADDRESS application prints to four distinct form types, each with specific positioning requirements. The application uses a file-based printing approach where content is formatted to temporary files and then sent to printers using raw printer commands. For Blazor implementation, these will be converted to HTML/CSS layouts with precise positioning.

### 1. Requisition Forms Layout (OnPrintReqForm)

#### Form Types Supported:
- **CLIREQ** (Client Requisition Forms) - Form 0
- **PTHREQ** (Path Requisition Forms) - Form 1  
- **CYTREQ** (Cytology Requisition Forms) - Form 2
- **3PLY** (3-Ply Tractor Forms) - Form 0

#### Print Layout Structure:
```css
/* Form positioning starts 3 lines down from top */
.requisition-form {
    margin-top: 3em; /* Equivalent to "\n\n\n" */
    font-family: 'Courier New', monospace; /* Fixed-width font for alignment */
    font-size: 10pt;
}

.client-info {
    margin-left: 50ch; /* 50 character spaces from left margin */
    line-height: 1.2em;
}
```

#### Data Fields Positioning:
1. **Client Name**: 50 spaces from left margin
   ```
   Format: "%50.50s %s" (50 spaces + client name)
   ```

2. **Client Address**: 50 spaces from left margin
   ```
   Format: "%50.50s %s" (50 spaces + full address)
   ```

3. **City/State/ZIP**: 50 spaces from left margin
   ```
   Format: "%50.50s %s" (50 spaces + city_st_zip)
   ```

4. **Phone Number**: 50 spaces from left margin
   ```
   Format: "%50.50s %s" (50 spaces + phone)
   ```

5. **Fax Number**: 50 spaces from left margin with "FAX" prefix
   ```
   Format: "%50.50s FAX %s" (50 spaces + "FAX " + fax number)
   ```

6. **Client Mnemonic & Code**: 50 spaces from left margin
   ```
   Format: "%50.50s %s (%s)" (50 spaces + mnemonic + client_code)
   With EMR: "%50.50s %s %s (%s)" (50 spaces + mnemonic + client_code + EMR_type)
   ```

#### Blazor Implementation:
```html
<div class="requisition-form">
    <div class="client-info">
        <div class="field-line">@($"{new string(' ', 50)} {ClientName}")</div>
        <div class="field-line">@($"{new string(' ', 50)} {ClientAddress}")</div>
        <div class="field-line">@($"{new string(' ', 50)} {CityStateZip}")</div>
        <div class="field-line">@($"{new string(' ', 50)} {Phone}")</div>
        <div class="field-line">@($"{new string(' ', 50)} FAX {Fax}")</div>
        <div class="field-line">
            @if (string.IsNullOrEmpty(EmrType))
            {
                @($"{new string(' ', 50)} {ClientMnemonic} ({ClientCode})")
            }
            else
            {
                @($"{new string(' ', 50)} {ClientMnemonic} {ClientCode} ({EmrType})")
            }
        </div>
    </div>
</div>
```

### 2. Chain of Custody Forms Layout (OnPrint_CUSTODY)

#### Form Structure:
The custody form has two main sections:
1. **Client Information Section** (top of form)
2. **Collection Site Information Section** (middle of form)
3. **Footer Section** with "MCL Courier" (bottom)

#### Print Layout Positioning:
```css
.custody-form {
    margin-top: 6em; /* Equivalent to 6 newlines */
    font-family: 'Courier New', monospace;
    font-size: 10pt;
}

.client-section {
    width: 100%;
}

.mro-section {
    margin-top: 2em;
}

.collection-site {
    margin-top: 10em; /* Large gap to collection site section */
    margin-left: 3ch; /* 3 character indent */
}

.footer {
    margin-top: 13em;
    text-align: center;
    margin-right: 60ch; /* Right-aligned with 60 char margin */
}
```

#### Client Information Section:
**When MRO is Empty:**
```
Line 1: Client Name (left-justified, 50 chars) + "X X X X NONE X X X X" (right side)
Line 2: Address (left-justified, 50 chars) + "X X X X NONE X X X X" (right side)  
Line 3: City/State/ZIP (left-justified, 50 chars) + "X X X X NONE X X X X" (right side)
Line 4: Phone (20 chars) + Fax (30 chars) + "X X X X NONE X X X X" (50 chars)
Line 5: Client Mnemonic + " (" + Client Code + ")"
```

**When MRO is Present:**
```
Line 1: Client Name (left-justified, 50 chars) + MRO Name (right side)
Line 2: Address (left-justified, 50 chars) + MRO Address 1 (right side)
Line 3: City/State/ZIP (left-justified, 50 chars) + MRO Address 2 (right side)  
Line 4: Phone (20 chars) + Fax (30 chars) + MRO City/State/ZIP (50 chars)
Line 5: Client Mnemonic + " (" + Client Code + ")"
```

#### Collection Site Section:
**Alternative Site Mode:**
- 10 newlines spacing (or 7 + DAP11 ZT if checked)
- Name field: 3 spaces indent + 60 character field + 40 character phone field
- Address line: 3 spaces indent + 20 char address + 15 char city + 2 char state + 9 char zip

**Standard Client Location (when prn_loc = "Y"):**
- Same formatting as alternative site but uses client data
- Supports override with alternative site data if provided

#### DAP11 ZT Notation:
When DAP checkbox is enabled:
```
Position: 13 characters from left + "X" + 20 characters + "DAP11 ZT"
Format: "%13.13s %20.25s\n" ("X", "DAP11 ZT")
```

#### Blazor Implementation:
```html
<div class="custody-form">
    <!-- Client Information Section -->
    <div class="client-section">
        @if (string.IsNullOrEmpty(MroName))
        {
            <div class="client-line">@($"{ClientName,-50}{"X X X X NONE X X X X"}")</div>
            <div class="client-line">@($"{ClientAddress,-50}{"X X X X NONE X X X X"}")</div>
            <div class="client-line">@($"{CityStateZip,-50}{"X X X X NONE X X X X"}")</div>
            <div class="client-line">@($"{Phone,-20}{FaxDisplay,-30}{"X X X X NONE X X X X",-50}")</div>
        }
        else
        {
            <div class="client-line">@($"{ClientName,-50}{MroName}")</div>
            <div class="client-line">@($"{ClientAddress,-50}{MroAddress1}")</div>
            <div class="client-line">@($"{CityStateZip,-50}{MroAddress2}")</div>
            <div class="client-line">@($"{Phone,-20}{FaxDisplay,-30}{MroCityStateZip,-50}")</div>
        }
        <div class="client-line">@($"{ClientMnemonic} ({ClientCode})")</div>
    </div>

    <!-- Collection Site Section -->
    <div class="collection-site">
        @if (IsDapEnabled)
        {
            <div class="dap-notation">@($"{"X",13} {"DAP11 ZT",25}")</div>
        }
        
        @if (UseAlternativeSite || (PrintLocation == "Y"))
        {
            <div class="site-name">@($"   {SiteName,-60} {SitePhone,-40}")</div>
            <div class="site-address">@($"   {SiteAddress,-20} {SiteCity,-15} {SiteState,-2} {SiteZip,-9}")</div>
        }
    </div>

    <!-- Footer -->
    <div class="footer">MCL Courier</div>
</div>
```

### 3. Lab Office Forms Layout (OnBtnLabofficeForm)

#### Form Structure:
This form prints MCL contact information for toxicology lab forms.

#### Print Layout:
```css
.lab-office-form {
    margin-top: 20em; /* 20 newlines from top */
    font-family: 'Courier New', monospace;
    font-size: 10pt;
}

.lab-info {
    margin-left: 3ch; /* 3 character indent */
    line-height: 1.5em;
}

.lab-footer {
    margin-top: 13em;
    text-align: center;
    margin-right: 60ch;
}
```

#### Content Structure:
```
Line 1: "   MCL" + (right-aligned) "731 541 7990"
Line 2: Empty line
Line 3: "   620 Skyline Drive, JACKSON, TN 38301" + (right-aligned) "731 541 7992"  
Line 4-16: Empty lines (13 newlines)
Line 17: "TOX LAB" (right-aligned, 60 characters from right)
```

#### Blazor Implementation:
```html
<div class="lab-office-form">
    <div class="lab-info">
        <div class="lab-line">   MCL@(new string(' ', 50))731 541 7990</div>
        <div class="lab-line"></div>
        <div class="lab-line">   620 Skyline Drive, JACKSON, TN 38301@(new string(' ', 15))731 541 7992</div>
    </div>
    <div class="lab-footer">TOX LAB</div>
</div>
```

### 4. ED Lab Forms Layout (OnButtonEdLab)

#### Form Structure:
Similar to Lab Office forms but with Emergency Department specific information.

#### Print Layout:
Same CSS structure as Lab Office forms.

#### Content Structure:
```
Line 1: "   JMCGH - ED LAB" + (right-aligned) "731 541 4833"
Line 2: Empty line  
Line 3: "   620 Skyline Drive, JACKSON, TN 38301" + (spaces for fax - left blank per requirements)
```

#### Blazor Implementation:
```html
<div class="lab-office-form">
    <div class="lab-info">
        <div class="lab-line">   JMCGH - ED LAB@(new string(' ', 40))731 541 4833</div>
        <div class="lab-line"></div>
        <div class="lab-line">   620 Skyline Drive, JACKSON, TN 38301</div>
    </div>
</div>
```

### 5. CSS Framework for Blazor Implementation

```css
/* Base form styles */
.form-container {
    font-family: 'Courier New', monospace;
    font-size: 10pt;
    line-height: 1.2em;
    white-space: pre;
    background: white;
    color: black;
    padding: 1in;
    width: 8.5in;
    min-height: 11in;
    box-sizing: border-box;
}

/* Print-specific styles */
@media print {
    .form-container {
        margin: 0;
        padding: 0.5in;
        page-break-after: always;
    }
    
    body {
        margin: 0;
        padding: 0;
    }
}

/* Character spacing utilities */
.char-1 { margin-left: 1ch; }
.char-3 { margin-left: 3ch; }
.char-13 { margin-left: 13ch; }
.char-20 { margin-left: 20ch; }
.char-50 { margin-left: 50ch; }
.char-60 { margin-left: 60ch; }

/* Line spacing utilities */
.line-1 { margin-top: 1em; }
.line-2 { margin-top: 2em; }
.line-3 { margin-top: 3em; }
.line-6 { margin-top: 6em; }
.line-7 { margin-top: 7em; }
.line-10 { margin-top: 10em; }
.line-13 { margin-top: 13em; }
.line-20 { margin-top: 20em; }

/* Field width utilities for text formatting */
.w-2 { width: 2ch; }
.w-9 { width: 9ch; }
.w-15 { width: 15ch; }
.w-20 { width: 20ch; }
.w-25 { width: 25ch; }
.w-30 { width: 30ch; }
.w-40 { width: 40ch; }
.w-50 { width: 50ch; }
.w-60 { width: 60ch; }
```

### 6. Blazor Component Structure Recommendations

#### FormPrintService.cs
```csharp
public class FormPrintService
{
    public async Task<string> GenerateRequisitionForm(ClientData client, int copies, string formType)
    public async Task<string> GenerateCustodyForm(ClientData client, AlternativeSite altSite, bool includeDap, int copies)
    public async Task<string> GenerateLabOfficeForm(int copies)
    public async Task<string> GenerateEdLabForm(int copies)
    
    private string FormatWithSpacing(string text, int width, bool leftAlign = true)
    private string GenerateSpaces(int count)
}
```

#### Print Component Structure
```razor
@page "/print-forms"
@inject FormPrintService PrintService

<div class="print-container">
    @if (ShowPreview)
    {
        <div class="form-preview">
            @((MarkupString)GeneratedFormHtml)
        </div>
    }
    
    <div class="print-controls">
        <button @onclick="PrintForm">Print</button>
        <button @onclick="ShowPreview">Preview</button>
    </div>
</div>
```

This specification provides the exact positioning and formatting requirements needed to replicate the legacy form layouts in a modern Blazor Server application while maintaining compatibility with the existing pre-printed forms.

---

## Conclusion

The legacy ADDRESS application serves a critical function in the laboratory workflow, and its replacement must maintain all current functionality while providing modern usability and maintainability. The specifications outlined in this document provide a roadmap for developing a robust, secure, and user-friendly replacement that will serve the organization's needs for the next decade and beyond.

The modern replacement should focus on:
1. **Maintaining Core Functionality**: All current printing capabilities must be preserved
2. **Improving User Experience**: Modern interface with better usability
3. **Enhancing Security**: Modern authentication and audit capabilities
4. **Ensuring Scalability**: Architecture that can grow with organizational needs
5. **Providing Flexibility**: Easy configuration and customization options

By following these specifications, the new application will provide a solid foundation for laboratory form printing operations while positioning the organization for future technological advances.