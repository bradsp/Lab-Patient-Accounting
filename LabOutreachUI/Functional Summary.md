# RDS Legacy Application - Functional Summary

## Application Overview
RDS (Random Drug Screen) is a Windows MFC application developed by Rick Crone (2000-2012) for Medical Center Laboratory (MCL) personnel to manage candidate lists for random drug screening across multiple clients.

## Core Functionality
1. Candidate Management
Add/Edit/Delete candidates with the following data fields:
* Name/Candidate Identifier
* Shift information
* Client mnemonic (cli_mnem)
* Test date tracking
* URI (unique identifier)
* Deletion flag (soft delete)
2. Random Selection Engine
Algorithm: Uses C srand() and rand() for pseudorandom selection
Input parameters:
Number of candidates to select (validated against available pool)
Client selection
Optional shift filtering
Output: Generates randomized list of selected candidates
3. Multi-Client Support
Client management via dropdown selection
Special clients:
RTS (Real-Time Systems) - has dedicated import functionality
BTM - has dedicated import functionality
Client-specific data isolation using cli_mnem field
4. Data Import Capabilities
RTS Import: Bulk import with data validation and cleanup of old records
BTM Import: Employee data synchronization from text files
Format handling: Text file parsing with custom format detection
5. Reporting & Output
Print functionality:
Formatted reports with headers, pagination
Uses Windows GDI for print rendering
Fixed-width font layouts
Email integration:
Exports reports to text files
Integrates with system email via OnFileSendMail()
Report types:
Selected candidates list
Non-selected candidates report (with date filtering)
6. User Interface Features
Main Form (IDD_RDS_FORM):

Client selection dropdown
Number input validation
Shift filtering
Action buttons (Generate, Edit, Import, Email, Print)
Candidate List Management (IDD_DDISP_CANIDATES):

MSFlexGrid for data display
Add/Edit/Delete operations
Filtering capabilities
Bulk operations
Date Entry Dialog (IDD_DDATE_ENTRY):

Date validation using custom functions
Formatted date handling
Technical Architecture
Framework:
Microsoft Foundation Classes (MFC)
Document/View architecture
Windows GDI printing
Database Layer:
Table: rds (main candidate table)
Key fields: cli_mnem, name, shift, uri, test_date, deleted
Operations: SELECT, INSERT, UPDATE, DELETE with filtering
Custom recordset class: RRDS for data access
Key Classes:
CRDSApp: Application entry point
CRDSDoc: Document class (data container)
CRDSView: Main form view
DISP_CANIDATES: Candidate management dialog
DSEL_FOR_EDIT: Selection filter dialog
DDATE_ENTRY: Date input dialog
Business Rules & Logic
Selection Validation:
Cannot select more candidates than available in pool
Must select at least 1 candidate
Excludes deleted records from selection pool
Shift filtering is optional
Data Integrity:
Soft delete implementation (deleted flag)
Date validation for test dates
Duplicate prevention via URI field
Client isolation via mnemonic codes
Import Logic:
RTS: Complete data replacement (DELETE then INSERT)
BTM: Differential sync (mark missing as deleted, add new)
File format detection and parsing
Recommended Modern Architecture
Technology Stack Options:
Web Application: React/Angular + Node.js/ASP.NET Core + SQL Server
Desktop Application: Electron + React + Node.js or WPF/WinUI 3 + C#
Cross-Platform: Flutter/Xamarin with cloud backend
Key Modernization Areas:
Database Design:

Normalize client data into separate table
Add proper indexing and constraints
Implement audit trails
Use GUID for better unique identifiers
Security & Authentication:

User authentication and role-based access
Audit logging for compliance
Data encryption for sensitive information
Random Selection Algorithm:

Cryptographically secure random number generation
Reproducible selection with seed tracking
Advanced filtering options
Reporting Engine:

Modern report generators (Crystal Reports, SSRS, or web-based)
PDF generation
Email templates and scheduling
User Experience:

Responsive design for mobile/tablet access
Real-time data validation
Batch operations and bulk import/export
Search and advanced filtering
Integration:

REST APIs for system integration
Email service integration
File import via drag-and-drop or API
Export to multiple formats (Excel, CSV, PDF)
This legacy system provides a solid foundation for understanding the business requirements and can be modernized to meet current technology standards while preserving the core functionality that users depend on.