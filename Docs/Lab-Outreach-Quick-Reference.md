# Lab Outreach Application - Quick Reference Guide

## ?? Common Tasks

### Random Drug Screen Tasks

#### Perform a Random Selection
1. Navigate to **RDS Dashboard** ? **Random Selection**
2. Select **Client** from dropdown
3. Choose **Shift** (optional) or leave as "All Shifts"
4. Enter **Number to Select**
5. Click **"Generate Random Selection"**
6. Click **"Export to CSV"** or **"Print Results"**

#### Add a Single Candidate
1. Navigate to **Manage Candidates**
2. Select **Client**
3. Click **"Add New Candidate"**
4. Fill in: Name, Shift (optional), Last Test Date (optional)
5. Click **"Save"**

#### Import Multiple Candidates
1. Prepare CSV file with columns: `Client,Name,Shift,TestDate`
2. Navigate to **Import Candidates**
3. Click **"Choose File"** and select your CSV
4. Review preview
5. Click **"Import Candidates"**
6. Review results and download error report if needed

#### Generate a Report
1. Navigate to **Reports**
2. Select **Report Type**
3. Select **Client**
4. Add optional filters if available
5. Click **"Generate Report"**
6. Click **"Export CSV"** to download

#### Edit a Candidate
1. Navigate to **Manage Candidates**
2. Select **Client**
3. Find candidate in list
4. Click **Edit** button (pencil icon)
5. Modify information
6. Click **"Save"**

#### Deactivate a Candidate
1. Navigate to **Manage Candidates**
2. Select **Client**
3. Find candidate in list
4. Click **Delete** button (trash icon)
5. Confirm deletion

### Client Viewer Tasks

#### Look Up Client Information
1. Navigate to **Search Clients**
2. Type client name or code in search box
3. Select client from dropdown
4. View contact information displayed

---

## ?? Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| F5 | Refresh page |
| Ctrl+F5 | Hard refresh (clear cache) |
| Esc | Close modal/dialog |
| Tab | Navigate form fields |
| Enter | Submit form |

---

## ?? Troubleshooting Quick Fixes

| Problem | Quick Fix |
|---------|-----------|
| Can't see RDS module | Check permissions at `/auth-diagnostics` |
| Import fails | Verify CSV format: `Client,Name,Shift,TestDate` |
| No candidates available | Check if candidates are deleted or wrong client selected |
| Access Denied | Navigate to home page, use only visible modules |
| Selection won't export | Check browser download permissions |
| Page not updating | Press F5 to refresh |
| Changes don't save | Check for error messages, try again |

---

## ?? CSV Import Template

```csv
Client,Name,Shift,TestDate
CLIENTA,John Doe,Day,2024-01-15
CLIENTA,Jane Smith,Night,2024-02-20
CLIENTB,Bob Johnson,Evening,
```

**Rules:**
- Client = Client mnemonic (required)
- Name = Full name (required)
- Shift = Day/Night/Evening/etc. (optional)
- TestDate = YYYY-MM-DD format (optional)

---

## ?? User Permissions

| User Type | Client Viewer | RDS Module |
|-----------|--------------|------------|
| Standard User | ? Yes | ? No |
| RDS User | ? Yes | ? Yes |
| Administrator | ? Yes | ? Yes (auto) |

---

## ?? Getting Help

**IT Support**
- Email: [Support Email]
- Phone: [Support Phone]
- Include: Username, description, screenshot

**Permission Requests**
- Contact: System Administrator
- Provide: Windows username, needed access

**Diagnostics Page**
- URL: `/auth-diagnostics`
- Shows: Current permissions and authentication status

---

## ?? Security Reminders

- ? Lock computer when away
- ? Don't share credentials
- ? Log out when finished
- ? Report suspicious activity

---

## ?? Report Types

| Report | Purpose | Parameters |
|--------|---------|------------|
| Non-Selected Candidates | Find overdue candidates | Client, From Date (opt) |
| All Candidates | Complete roster | Client |
| Client Summary | Statistics by shift | Client |

---

## ?? Best Practices

### Random Selection
- ? Consistent intervals (weekly/monthly)
- ? Export results immediately
- ? Print backup copy
- ? Document selection date

### Candidate Management
- ? Keep information current
- ? Remove departed employees
- ? Update test dates
- ? Regular audits

### Data Import
- ? Backup before import
- ? Test with small sample
- ? Review error reports
- ? Keep original files

---

## ?? Quick Diagnostics

**Check Your Permissions:**
```
Navigate to: /auth-diagnostics
Look for: "Can Access RDS" row
Expected: "True" (if you should have access)
```

**Verify Client Exists:**
```
Navigate to: Search Clients
Search for: Client name or code
Result: Should appear in dropdown
```

**Check Import File:**
```
Required columns: Client,Name,Shift,TestDate
Header row: Must be present
Format: CSV (comma-separated)
Encoding: UTF-8
```

---

## ?? Selection Process Flow

```
Select Client ? Apply Filters ? Set Count ? Generate ? Export/Print
```

**Validation Checks:**
- ? Client selected
- ? Count ? 1
- ? Count ? Available candidates
- ? At least one candidate available

---

## ??? File Naming Conventions

**Import Files:**
- Format: `Candidates_ClientName_YYYYMMDD.csv`
- Example: `Candidates_CLIENTA_20241201.csv`

**Export Files:**
- Selection: `Selection_ClientName_YYYYMMDD_HHMMSS.csv`
- Report: `Report_ClientName_YYYYMMDD_HHMMSS.csv`

---

## ?? Time-Saving Tips

1. **Use Browser Bookmarks**
   - Bookmark frequently used pages
   - Example: `/rds/dashboard`, `/candidates`

2. **Learn Autocomplete**
   - Start typing in search boxes
   - Use arrow keys to navigate suggestions
   - Press Enter to select

3. **Export for Records**
   - Always export selections
   - Keep CSV files in shared folder
   - Name files consistently

4. **Batch Operations**
   - Import multiple candidates at once
   - Use CSV for bulk updates
   - Schedule regular selection times

5. **Check Dashboard First**
   - View statistics before selections
   - Identify trends
   - Plan selections accordingly

---

## ?? Common Error Messages

| Error Message | Meaning | Solution |
|---------------|---------|----------|
| "Client not found" | Invalid client code | Verify client mnemonic |
| "No candidates available" | No eligible candidates | Check candidate list |
| "Selection count exceeds available" | Too many requested | Reduce selection count |
| "Invalid date format" | Wrong date format | Use YYYY-MM-DD |
| "Access Denied" | Insufficient permissions | Request access from admin |
| "Database error" | System issue | Retry or contact IT |

---

## ?? Browser Compatibility

| Browser | Status | Notes |
|---------|--------|-------|
| Microsoft Edge | ? Recommended | Best performance |
| Google Chrome | ? Supported | Fully compatible |
| Mozilla Firefox | ? Supported | Fully compatible |
| Internet Explorer | ? Not Supported | Please upgrade |
| Safari | ?? Limited | May have issues |

---

## ?? Update Checklist

**After Permission Changes:**
- [ ] Close ALL browser windows
- [ ] Restart browser
- [ ] Navigate to application
- [ ] Verify new permissions at `/auth-diagnostics`

**After Candidate Changes:**
- [ ] Refresh page (F5)
- [ ] Verify changes appear
- [ ] Export updated list if needed

**Before Selection:**
- [ ] Verify candidate list is current
- [ ] Check for deleted candidates
- [ ] Update any test dates
- [ ] Review shift assignments

**After Selection:**
- [ ] Export results to CSV
- [ ] Print results (optional)
- [ ] Document selection in records
- [ ] Update last test dates after testing

---

*Version 1.0 - December 2024*  
*For detailed information, see the full User Guide*
