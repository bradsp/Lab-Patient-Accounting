# CSV Export - Quick Reference Card

## ?? Exporting Reports

### Step 1: Generate Report
1. Navigate to **Reports** page
2. Select **Report Type**:
   - Non-Selected Candidates
   - All Candidates  
   - Client Summary
3. Select **Client** from dropdown
4. Click **Generate Report**

### Step 2: Export to CSV
1. Click **Export CSV** button (top right of report)
2. File downloads automatically
3. Open in Excel or Google Sheets

---

## ?? Exporting Selection Results

### Step 1: Perform Selection
1. Navigate to **Candidate Management** page
2. Select **Client**
3. Configure selection (shift, count)
4. Click **Generate Random Selection**

### Step 2: Export Results
1. After selection completes, scroll to results
2. Click **Export to CSV** button
3. File downloads automatically

---

## ?? File Format

### Filename Pattern:
```
ReportType_ClientName_YYYYMMDD_HHMMSS.csv
```

### Example:
```
NonSelectedCandidates_CLIENT01_20240125_143022.csv
```

---

## ?? Tips

? **Files won't overwrite** - Each has unique timestamp  
? **Excel compatible** - Opens directly in Microsoft Excel  
? **Special characters handled** - Commas and quotes work fine  
? **Date formatting** - Shows "Never" if not tested yet  

---

## ??? Troubleshooting

| Issue | Solution |
|-------|----------|
| Export button disabled | Generate report first |
| File won't download | Allow popups in browser |
| Data looks wrong in Excel | Use "Text Import Wizard" |
| Filename too long | Will be automatically shortened |

---

## ?? Where Files Download

Files save to your **browser's default download folder**:

- **Windows**: `C:\Users\[YourName]\Downloads\`
- **Mac**: `/Users/[YourName]/Downloads/`
- **Linux**: `/home/[yourname]/Downloads/`

---

## ?? Need Help?

Contact your system administrator or refer to the full documentation:
- `CSV_EXPORT_FEATURE.md` - Complete guide
- `CSV_EXPORT_IMPLEMENTATION.md` - Technical details

---

## ? Quick Facts

- ? Works in all modern browsers
- ? No limit on number of exports
- ? No special software required
- ? Data matches what you see on screen
- ? Safe and secure - no data stored on server

---

**Version 1.0** | Random Drug Screen Application
