# Lab Outreach Application - User Guide

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
3. [Random Drug Screen Module](#random-drug-screen-module)
4. [Client Viewer Module](#client-viewer-module)
5. [Dictionary Maintenance Module](#dictionary-maintenance-module)
6. [User Permissions](#user-permissions)
7. [Troubleshooting](#troubleshooting)

---

## Introduction

The **Lab Outreach Application** is a web-based management system designed to streamline laboratory outreach operations. The application provides these main modules:

- **Random Drug Screen (RDS) Module** - Comprehensive management of random drug screening programs
- **Client Viewer Module** - Quick access to client information and contacts
- **Dictionary Maintenance Module** - Edit the reference tables that drive billing, starting with client records (requires the "Can Edit Dictionary" permission)

### System Requirements

- **Browser:** Microsoft Edge, Google Chrome, or Mozilla Firefox (latest versions)
- **Network:** Must be connected to the internal network
- **Authentication:** Windows Authentication (automatic login with your domain credentials)
- **Permissions:** Access to modules is controlled by user permissions (see [User Permissions](#user-permissions))

---

## Getting Started

### Logging In

1. Open your web browser (Edge, Chrome, or Firefox)
2. Navigate to the Lab Outreach Application URL
3. You will be automatically authenticated using your Windows credentials
4. The home page displays available modules based on your permissions

### Home Page Overview

The home page provides:
- **Module Cards** - Large cards for each available module with feature descriptions
- **Quick Access Section** - Small cards providing direct links to frequently used features
- **System Information** - Current environment and version information

### Navigation

- **Side Navigation Menu** - Always accessible from the left side of the screen
  - Click the hamburger menu icon (?) to expand/collapse
- Shows available modules and their main features
- **Home Button** - Click the "Home" link or application logo to return to the home page

---

## Random Drug Screen Module

The Random Drug Screen (RDS) module provides comprehensive tools for managing random drug screening programs for clients.

> **Note:** Access to the RDS module requires the "Random Drug Screen" permission or Administrator rights.

### RDS Dashboard

**Location:** Home ? RDS Dashboard

The dashboard provides an overview of your random drug screening program:

#### Statistics Cards
- **Total Candidates** - Count of all active candidates across all clients
- **Active Clients** - Number of clients with active drug screening programs
- **Recent Selections** - Count of selections made in the past 30 days
- **Pending Tests** - Number of candidates selected but not yet tested

#### Recent Activity
- View the 10 most recent random selections
- See selection date, client, candidate name, and status
- Click "View All Selections" to see complete history

#### Quick Actions
- **New Random Selection** - Start a new random selection process
- **Manage Candidates** - Jump directly to candidate management
- **Import Candidates** - Bulk import candidates from CSV
- **View Reports** - Access reporting tools

### Candidate Management

**Location:** Home ? Manage Candidates

Manage the pool of candidates eligible for random drug screening.

#### Viewing Candidates

1. **Select a Client** 
   - Use the autocomplete search box to find a client
   - Type client name or mnemonic code
   - Select from the dropdown list

2. **View Candidate List**
   - After selecting a client, all candidates for that client are displayed
   - Table shows: Name, Shift, Last Test Date, Status

3. **Filter Options**
   - **Show Deleted Candidates** - Toggle to include deactivated candidates
   - **Search** - Type in the search box to filter by name

#### Adding a New Candidate

1. Click the **"Add New Candidate"** button
2. Fill in required information:
   - **Client** - Select from dropdown (pre-filled if client already selected)
 - **Full Name** - Enter candidate's full name
   - **Shift** - Enter shift assignment (optional)
   - **Test Date** - Last test date (optional, leave blank for new candidates)
3. Click **"Save"** to add the candidate

#### Editing a Candidate

1. Click the **"Edit"** button (pencil icon) next to a candidate
2. Modify the information as needed
3. Click **"Save"** to apply changes
4. Click **"Cancel"** to discard changes

#### Deleting (Deactivating) a Candidate

1. Click the **"Delete"** button (trash icon) next to a candidate
2. Confirm the deletion when prompted
3. The candidate is marked as deleted (soft delete)
   - They remain in the database for historical records
   - They will not appear in future random selections
   - Enable "Show Deleted" to view deactivated candidates

### Import Candidates

**Location:** Home ? Import Candidates

Bulk import candidates from a CSV file.

#### CSV File Format

Your CSV file must have these columns (in order):
```
Client,Name,Shift,TestDate
```

**Example:**
```
CLIENTA,John Doe,Day,2024-01-15
CLIENTA,Jane Smith,Night,2024-02-20
CLIENTB,Bob Johnson,Evening,
```

**Notes:**
- First row should contain column headers
- **Client** - Client mnemonic code (required)
- **Name** - Full name of candidate (required)
- **Shift** - Shift assignment (optional, can be blank)
- **TestDate** - Last test date in format YYYY-MM-DD (optional, can be blank)

#### Import Process

1. Click **"Choose File"** and select your CSV file
2. Review the file preview showing:
   - Number of records to be imported
   - First few rows of data
3. Click **"Import Candidates"** to start the import
4. Review the import results:
   - ? **Successful imports** - Candidates added
   - ?? **Warnings** - Candidates updated (if they already existed)
   - ? **Errors** - Records that failed with reason
5. Click **"Download Error Report"** to get details of any failures

#### Common Import Issues

- **"Client not found"** - Client mnemonic doesn't exist in system
- **"Invalid date format"** - Use YYYY-MM-DD format for dates
- **"Duplicate candidate"** - Candidate already exists (this is a warning, not an error - the existing record is updated)
- **"Missing required field"** - Client or Name is blank

### Random Selection

**Location:** Home ? RDS Dashboard ? Random Selection (or direct link from navigation)

Perform random selection of candidates for drug screening.

#### Selection Process

1. **Select Client**
   - Use autocomplete search to find client
   - Type name or mnemonic
   - Select from dropdown

2. **Apply Filters (Optional)**
   - **Shift Filter** - Select specific shift or leave as "All Shifts"
   - System shows number of available candidates

3. **Specify Selection Count**
   - Enter number of candidates to select
   - Must be ? available candidates
   - System validates your entry

4. **Review Selection Info**
   - Right panel shows:
   - Selected client
     - Applied filters
     - Number to select
     - Available candidates

5. **Generate Selection**
   - Click **"Generate Random Selection"**
   - System randomly selects candidates from eligible pool
   - Selection uses cryptographic randomization for fairness

#### Selection Results

After generating a selection:

- **Selection Summary** appears showing:
  - Success message with count selected
  - Table of selected candidates with:
    - Name
    - Client
 - Shift
    - Last test date
    - Selection date

- **Available Actions:**
  - **Print Results** - Print the selection list
  - **Export to CSV** - Download selection as CSV file
  - **Clear Results** - Clear the current selection to start a new one

#### Selection Rules

- Candidates are selected randomly from the eligible pool
- **Eligible candidates:**
  - Active (not deleted)
  - Match selected client
  - Match shift filter (if applied)
- Each candidate has equal probability of selection
- Selection is logged with timestamp

### Reports

**Location:** Home ? RDS Dashboard ? Reports

Generate various reports for drug screening programs.

#### Available Reports

##### 1. Non-Selected Candidates
Shows candidates who have not been selected in a specified timeframe.

**Use Case:** Identify candidates who are overdue for testing

**Parameters:**
- **Client** - Required
- **From Date** - Optional (show candidates not tested since this date)

**Output:** List of candidates with their last test date

##### 2. All Candidates
Complete list of all candidates for a client.

**Use Case:** Comprehensive candidate roster

**Parameters:**
- **Client** - Required

**Output:** All active and deleted candidates with:
- Name, Shift, Last test date, Status (Active/Deleted)

##### 3. Client Summary
Statistical summary of candidates by client.

**Use Case:** Program overview and planning

**Parameters:**
- **Client** - Required

**Output:** 
- Total candidate count
- Active vs. Deleted count
- Breakdown by shift
- Complete candidate list

#### Generating a Report

1. Select **Report Type** from dropdown
2. Select **Client** using autocomplete search
3. Apply any optional filters (if available)
4. Click **"Generate Report"**
5. Review results in the table

#### Exporting Reports

- Click **"Export CSV"** button
- File downloads with naming format: `ReportType_ClientName_YYYYMMDD_HHMMSS.csv`
- Open in Excel or other spreadsheet software

---

## Client Viewer Module

The Client Viewer module provides quick access to client information and contact details.

> **Note:** All authenticated users have access to the Client Viewer module.

### Search Clients

**Location:** Home ? Search Clients

#### Searching for a Client

1. Use the **autocomplete search box**
2. Type client name or mnemonic code
3. Select from the filtered dropdown list
4. Client details display automatically

#### Client Information Displayed

- **Client Code** - Mnemonic identifier
- **Full Name** - Official client name
- **Contact Information**
  - Address
  - City, State, ZIP
  - Phone number
  - Fax number
  - Email address
- **Account Details**
  - Account number
  - Billing method
  - Fee schedule
  - Status (Active/Inactive)

#### Quick Actions

- **Clear Selection** - Clear current client to search for another
- **Copy to Clipboard** - Copy client contact information (if available)

---

## Dictionary Maintenance Module

The Dictionary Maintenance module lets authorized users edit the reference ("dictionary") tables that drive billing. The first area available is **Client Maintenance**, where you can add, edit, and deactivate client records.

> **Note:** The Dictionary section is only visible to users with the **"Can Edit Dictionary"** permission or Administrator rights. If you do not see it in the navigation menu, see [User Permissions](#user-permissions). The Client Viewer module (read-only) remains available to all users; Client Maintenance is where changes are actually made.

### Client Maintenance

**Location:** Navigation menu → **Dictionary** → **Clients**

The Client Maintenance page opens with a list of all clients.

#### Finding a Client

1. **Filter box** - Type in the **Filter by Name or Mnemonic** box at the top of the list:
   - Entering part of a **name** narrows the list to clients whose name contains what you typed.
   - Entering an exact **mnemonic** shows that specific client.
   - The list updates a moment after you stop typing.
2. **Include inactive** - Check this box to also show deactivated (inactive) clients. They appear greyed out. Leave it unchecked to see active clients only.
3. A running count of the clients currently shown is displayed below the list.

#### The Client List

The list shows, for each client: **Mnem** (mnemonic code), **Name**, **Address**, **City**, **State**, **Zip**, **Facility No**, **Type**, and **Bill Method**. Inactive clients are shown greyed out.

- Click the **pencil (Edit)** button on a row to open that client for editing.
- Click **New Client** (top right) to create a new client record.

### Editing a Client

Click the **Edit** (pencil) button on any row to open the client edit page. Fields are grouped into cards:

#### Identity
- **Client Mnemonic** *(required)* - The client's short code. This **cannot be changed** once a client exists (it is read-only when editing an existing client).
- **Client Name** - The client's full name.
- **Facility No** - The facility number.
- **Active** - When checked, the client is active. Unchecking it deactivates the client (see [Deactivating a Client](#deactivating-and-reactivating-a-client)).

#### Address
- **Address 1**, **Address 2**, **City**, **State** (dropdown), **Zip**, and **County** (dropdown).

#### Contact
- **Phone**, **Fax**, **Email**, and a free-text **Contact** field.

#### Business Classification
These fields control how the client is billed. Several are required:
- **Client Type** *(required)* - Select the client category (for example, Affiliate Hospital, Owned Clinic Lab, Nursing Home).
- **Fee Schedule** *(required)* - The fee schedule applied to the client's charges.
- **EMR Type** - The client's integrated EMR system, if any.
- **Cost Center** *(required)* - The GL cost center the client rolls up to.
- **Bill Method** *(required)* - How the client is billed: **INVOICE**, **PATIENT**, or **PER ACCOUNT**.

#### Billing & Printing Preferences
- **Print Bills in Date Order** - Sort invoice lines by date.
- **Include on Charge Code Report** - Include this client on the charge code report.
- **Bill at Discount** - Show discounted amounts on the bill.
- **Do NOT Bill this Client** - Suppress billing for this client.
- **Print CPT on Invoice** - Include CPT codes on the client's invoices.
- **Default Discount %** - The default percentage discount applied to the client.

#### Medical Review Officer (MRO)
- **Name**, **Address 1**, **Address 2**, **City**, **State** (dropdown), and **Zip** for the client's Medical Review Officer.

#### Comments
- A free-text **Comments** field for notes about the client.

#### Saving Changes

1. Make your edits across the cards.
2. Click **Save**.
3. If any **required** field (Client Mnemonic, Client Type, Fee Schedule, Cost Center, Bill Method) is missing, the page highlights the field and shows a message; correct it and click Save again.
4. On a successful save, you are returned to the client list.
5. Click **Cancel** at any time to discard your changes and return to the list.

### Adding a New Client

1. On the Client Maintenance list, click **New Client**.
2. Enter the **Client Mnemonic** for the new client.
   - When you move out of the mnemonic field, the system checks whether that mnemonic already exists.
   - If it does, a warning appears with an **"Edit that client instead"** link so you don't accidentally create a duplicate. Click the link to open the existing client, or choose a different mnemonic.
3. Fill in the remaining fields, making sure the required ones are set: **Client Type**, **Fee Schedule**, **Cost Center**, and **Bill Method**.
4. Click **Save**. The new client is created and appears in the list.

> **Tip:** The mnemonic is permanent once the client is saved, so choose it carefully.

### Deactivating and Reactivating a Client

Clients are never permanently deleted; they are **deactivated** (a "soft delete") so historical records are preserved.

- **To deactivate:** Open the client, uncheck **Active**, and click **Save**. The client no longer appears in the default list.
- **To find deactivated clients:** On the list, check **Include inactive**. Inactive clients appear greyed out.
- **To reactivate:** Open a deactivated client (with **Include inactive** checked), re-check **Active**, and click **Save**.

---

## User Permissions

Access to features in the Lab Outreach Application is controlled by user permissions configured by system administrators.

### Permission Levels

#### Standard User
- Access to Client Viewer module
- Can search and view client information
- No access to Random Drug Screen module

#### Random Drug Screen User
- All Standard User permissions
- Access to Random Drug Screen module:
  - View RDS Dashboard
  - Manage candidates (add, edit, delete)
  - Import candidates from CSV
  - Perform random selections
  - Generate reports

#### Dictionary Editor ("Can Edit Dictionary")
- All Standard User permissions
- Access to the **Dictionary** section of the navigation menu
- Access to **Client Maintenance**:
  - Add new clients
  - Edit client details, billing settings, preferences, MRO, and comments
  - Deactivate and reactivate clients

#### Administrator
- Full access to all modules and features
- Access to the RDS and Dictionary modules regardless of the specific RDS or "Can Edit Dictionary" permission
- Can access administrative functions (if available)

### Requesting Access

To request access to the Random Drug Screen module:

1. Contact your system administrator
2. Provide your Windows username (e.g., DOMAIN\username)
3. Specify which module access you need
4. Await confirmation of permission grant

After permissions are granted:
1. Close all browser windows
2. Restart your browser
3. Log in to the application again
4. New modules should now be visible

### Checking Your Permissions

Navigate to the **Authentication Diagnostics** page:

**Location:** `/auth-diagnostics` (type in browser address bar)

This page shows:
- Your authentication status
- User name and Windows account
- Assigned permissions
- Expected access to each module

---

## Troubleshooting

### I can't see the Random Drug Screen module

**Possible Causes:**
1. You don't have the required permission
2. You are not an administrator
3. Your permissions were recently changed but browser hasn't refreshed

**Solutions:**
1. Check with your administrator about permissions
2. Navigate to `/auth-diagnostics` to view your current permissions
3. If permissions were just granted:
   - Close ALL browser windows
   - Restart your browser
   - Log in again

### I can't see the Dictionary section

**Cause:** The Dictionary section (Client Maintenance) requires the "Can Edit Dictionary" permission or Administrator rights.

**Solutions:**
1. Ask your administrator to grant the "Can Edit Dictionary" permission
2. Navigate to `/auth-diagnostics` to confirm your current permissions
3. If the permission was just granted, close all browser windows, restart your browser, and log in again

### I can't change a client's mnemonic

**Cause:** The Client Mnemonic is permanent once a client exists, so the field is read-only when editing.

**Solution:** If a client truly needs a different mnemonic, create a new client with the correct mnemonic and deactivate the old one. Contact your administrator if records need to be reassigned.

### Import fails with "Client not found"

**Cause:** Client mnemonic in CSV doesn't match system records

**Solution:**
1. Verify client mnemonic code is correct
2. Check spacing and capitalization (must match exactly)
3. Ask administrator to verify client exists in system

### Random selection shows "No candidates available"

**Possible Causes:**
1. No active candidates for selected client
2. Shift filter excludes all candidates
3. All candidates have been deleted

**Solutions:**
1. Verify candidates exist in Candidate Management
2. Try removing shift filter
3. Check "Show Deleted" to see if candidates were deactivated

### "Access Denied" page appears

**Cause:** You attempted to access a page or feature you don't have permission for

**Solution:**
1. Navigate back to the home page
2. Use only the modules visible in your navigation menu
3. Contact administrator if you believe you should have access

### Selection results don't export to CSV

**Possible Causes:**
1. Browser blocked the download
2. No candidates were selected

**Solutions:**
1. Check browser's download settings/permissions
2. Allow downloads from the application site
3. Try a different browser
4. Ensure you've completed a selection first

### Application is slow or unresponsive

**Solutions:**
1. Refresh the browser page (F5)
2. Close other browser tabs to free resources
3. Clear browser cache
4. Contact IT support if problem persists

### Changes to candidates don't appear

**Cause:** Browser cache showing old data

**Solution:**
1. Click browser refresh button (F5)
2. If problem persists, hard refresh (Ctrl+F5)
3. Navigate away and back to the page

---

## Best Practices

### Random Selection
- Perform selections at consistent intervals (weekly, monthly, etc.)
- Document your selection process
- Export results immediately after generating selection
- Print results before clearing for backup

### Candidate Management
- Keep candidate information up-to-date
- Remove candidates from pool when they leave
- Update last test dates after testing
- Regular audits of candidate lists

### Data Import
- Always backup existing data before bulk imports
- Test import file with a small sample first
- Review error report carefully after imports
- Keep original CSV files for records

### Security
- Don't share your login credentials
- Lock your computer when away from desk
- Log out when finished using application
- Report suspicious activity to IT immediately

---

## Support

### Getting Help

**For Technical Issues:**
- Contact IT Support
- Email: [IT Support Email]
- Phone: [IT Support Phone]
- Include:
  - Your username
  - Description of problem
  - Steps to reproduce
  - Screenshot if applicable

**For Permission Requests:**
- Contact your supervisor or system administrator
- Provide your Windows username
- Specify needed module access

**For Training:**
- Request training session from your supervisor
- Refer to this user guide
- Use Authentication Diagnostics to verify setup

---

## Appendix

### Glossary

- **Bill Method** - How a client is billed: INVOICE, PATIENT, or PER ACCOUNT
- **Candidate** - Individual eligible for random drug screening
- **Client** - Organization or facility using drug screening services
- **Client Type** - Category of a client (e.g., Affiliate Hospital, Nursing Home)
- **Cost Center** - The GL (general ledger) code a client's charges roll up to
- **Dictionary** - The reference tables (clients, fee schedules, etc.) that drive billing
- **Fee Schedule** - The pricing schedule applied to a client's charges
- **MRO (Medical Review Officer)** - The reviewing officer whose contact details are stored on a client record
- **Mnemonic** - Short code identifying a client (e.g., "CLIENTA")
- **Selection** - Process of randomly choosing candidates for testing
- **Shift** - Work schedule assignment (Day, Night, Evening, etc.)
- **Soft Delete** - Marking record as deleted (deactivated) without removing from database
- **Windows Authentication** - Automatic login using your domain credentials

### Keyboard Shortcuts

- **F5** - Refresh current page
- **Ctrl+F5** - Hard refresh (clear cache)
- **Escape** - Close open modals or dialogs
- **Tab** - Navigate between form fields
- **Enter** - Submit forms or confirm actions

### CSV Template

Save this as a template for candidate imports:

```csv
Client,Name,Shift,TestDate
CLIENTCODE,Full Name,Day,2024-01-01
CLIENTCODE,Another Name,Night,
CLIENTCODE,Third Person,Evening,2024-02-15
```

### Version Information

**Current Version:** 1.0.0  
**Last Updated:** July 2026  
**Platform:** ASP.NET Core 8.0 Blazor Server

---

## Document Information

**Document Title:** Lab Outreach Application - User Guide  
**Version:** 1.0  
**Last Updated:** July 2026  
**Intended Audience:** End Users, Supervisors, Administrators  
**Distribution:** Internal Use Only

---

*For the most current version of this guide, check the application's help section or contact IT Support.*
