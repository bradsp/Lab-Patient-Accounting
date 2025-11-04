# Lab Outreach Application - Administrator Guide

## Table of Contents
1. [Overview](#overview)
2. [User Management](#user-management)
3. [System Configuration](#system-configuration)
4. [Security & Permissions](#security--permissions)
5. [Troubleshooting](#troubleshooting)
6. [Maintenance](#maintenance)

---

## Overview

This guide provides information for system administrators managing the Lab Outreach Application, including user permissions, troubleshooting, and system maintenance.

### Administrator Responsibilities

- Grant and revoke user permissions
- Troubleshoot user access issues
- Monitor system usage
- Maintain data integrity
- Provide user support

### Required Tools

- Access to the WinForms User Security form
- SQL Server Management Studio (for direct database access)
- Application logs access
- Windows Active Directory access (for user verification)

---

## User Management

### Permission Model

The application uses a role-based permission system with the following permission flags:

| Permission | Database Column | Purpose |
|-----------|----------------|---------|
| Administrator | `reserve4` | Full system access, bypasses other checks |
| Can Access Random Drug Screen | `access_random_drug_screen` | Access to RDS module |
| Can Edit Dictionaries | `access_edit_dictionary` | Edit system dictionaries |
| Can Submit Billing | `access_billing` | Submit billing batches |
| Can Modify Bad Debt | `access_bad_debt` | Modify bad debt accounts |

### Granting RDS Access

#### Method 1: Using WinForms User Security Form

1. Open **Lab PA WinForms UI** application
2. Navigate to **User Security** form
3. Search for and select the user
4. Check the **"Can Access Random Drug Screen"** checkbox
5. Click **"Save"**

#### Method 2: Using SQL Server

```sql
-- Grant RDS access to a user
UPDATE emp 
SET access_random_drug_screen = 1
WHERE name = 'username'

-- Verify the change
SELECT 
    name,
    full_name,
    access,
    reserve4 AS IsAdministrator,
  access_random_drug_screen AS CanAccessRDS
FROM emp
WHERE name = 'username'
```

### Revoking RDS Access

```sql
-- Revoke RDS access from a user
UPDATE emp 
SET access_random_drug_screen = 0
WHERE name = 'username'
```

### Creating New Users

#### Using WinForms

1. Open **User Security** form
2. Click **"New User"** button
3. Fill in required fields:
   - **Username** - Windows domain username (without domain prefix)
   - **Full Name** - User's display name
   - **Access Level** - VIEW or ENTER/EDIT
   - **Password** - Initial password (if not using Windows auth)
4. Set appropriate permission checkboxes
5. Click **"Save"**

#### Using SQL

```sql
-- Create new user with RDS access
INSERT INTO emp (
    name,
    full_name,
    access,
    reserve4,
    access_random_drug_screen
)
VALUES (
    'username',
  'Full Name',
    'ENTER/EDIT',
    0,  -- 0 = not admin, 1 = admin
    1   -- 1 = has RDS access, 0 = no access
)
```

### Making a User an Administrator

```sql
-- Grant administrator rights
UPDATE emp 
SET reserve4 = 1
WHERE name = 'username'

-- Note: Administrators automatically have access to all modules
```

### Disabling User Access

```sql
-- Disable user (don't delete - preserves audit trail)
UPDATE emp 
SET access = 'NONE'
WHERE name = 'username'
```

---

## System Configuration

### Database Schema

#### Key Tables

**emp** - User accounts and permissions
```sql
CREATE TABLE emp (
    name VARCHAR(50) PRIMARY KEY,
    full_name VARCHAR(100),
  access VARCHAR(20),
 reserve4 BIT,  -- IsAdministrator
    access_random_drug_screen BIT,
    -- ... other columns
)
```

**random_drug_screen_person** - RDS candidates
```sql
CREATE TABLE random_drug_screen_person (
    id INT PRIMARY KEY IDENTITY,
    client_mnemonic VARCHAR(10),
    name VARCHAR(100),
    shift VARCHAR(50),
    test_date DATETIME,
    deleted BIT
)
```

**Client** - Client information
```sql
CREATE TABLE Client (
    cli_mnem VARCHAR(10) PRIMARY KEY,
    cli_nme VARCHAR(100),
    -- ... other columns
)
```

### Application Settings

Located in `appsettings.json` (or `appsettings.Development.json` for development):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=servername;Database=dbname;Integrated Security=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore.Authorization": "Warning",
      "LabOutreachUI.Middleware": "Information"
    }
  }
}
```

### Enabling Debug Logging

For troubleshooting authorization issues:

```json
{
  "Logging": {
  "LogLevel": {
      "Microsoft.AspNetCore.Authorization": "Debug",
      "LabOutreachUI.Middleware": "Debug",
   "LabOutreachUI.Authorization": "Debug"
    }
  }
}
```

---

## Security & Permissions

### Authorization Architecture

The application uses a multi-layer authorization approach:

```
Layer 1: Windows Authentication (IIS/HTTP)
    ?
Layer 2: Database Validation (Middleware)
    ?
Layer 3: Claims-Based Authorization (Authorization Handlers)
    ?
Layer 4: UI Visibility (AuthorizeView components)
    ?
Layer 5: Page-Level Authorization (Authorize attributes)
```

### Authorization Policies

#### DatabaseUser Policy
- **Requirement:** User exists in `emp` table with valid access level
- **Purpose:** Base authentication for application
- **Used By:** All pages except public pages

#### RandomDrugScreen Policy
- **Requirement:** User has `access_random_drug_screen = 1` OR is Administrator
- **Purpose:** Gate access to RDS module
- **Used By:** All RDS pages and components

### Security Best Practices

1. **Principle of Least Privilege**
   - Grant only necessary permissions
   - Review permissions regularly
   - Remove access for departed employees

2. **Administrator Accounts**
   - Limit number of administrators
   - Document why each admin needs elevated access
   - Review admin list quarterly

3. **Audit Trail**
   - Use soft deletes (mark deleted, don't remove)
   - Track who made changes (mod_user, mod_date columns)
   - Monitor access logs regularly

4. **Password Management**
   - Encourage use of Windows Authentication
   - If using database passwords, enforce complexity
   - Require regular password changes

---

## Troubleshooting

### User Can't See RDS Module

**Diagnostic Steps:**

1. **Check Database Permission**
   ```sql
   SELECT 
       name,
       full_name,
       access,
       reserve4 AS IsAdmin,
       access_random_drug_screen AS CanAccessRDS
   FROM emp
   WHERE name = 'username'
   ```

   Expected results:
- `access` = 'VIEW' or 'ENTER/EDIT' (not 'NONE')
   - `IsAdmin` = 1 OR `CanAccessRDS` = 1

2. **Have User Check Diagnostics**
   - Navigate to `/auth-diagnostics`
   - Verify claims are present:
     - `DbUserValidated` = "true"
     - `CanAccessRandomDrugScreen` = "True" (or `IsAdministrator` = "True")

3. **Clear Browser Cache**
   - Have user close ALL browser windows
   - Restart browser
   - Test again

4. **Check Application Logs**
   Look for these entries:
   ```
   [WindowsAuthMiddleware] Processing authenticated user: username
   [WindowsAuthMiddleware] User authorized: username, Access: ENTER/EDIT
   [WindowsAuthMiddleware] Setting claims - IsAdmin: False, CanAccessRDS: True
   ```

### User Gets "Access Denied" Page

**Possible Causes:**

1. **No Database Account**
   ```sql
   -- Check if user exists
   SELECT * FROM emp WHERE name = 'username'
   ```
   
   If no results, create user account

2. **Access Level = NONE**
   ```sql
   -- Check access level
   SELECT name, access FROM emp WHERE name = 'username'
   ```
   
   If 'NONE', update to appropriate level:
   ```sql
   UPDATE emp SET access = 'ENTER/EDIT' WHERE name = 'username'
   ```

3. **Domain Name Mismatch**
   - User authenticates as `DOMAIN\username`
   - Database has `username` (without domain)
   - Middleware handles this automatically, but verify:
   ```sql
   -- Try both variations
   SELECT * FROM emp WHERE name IN ('username', 'DOMAIN\username')
   ```

### Import Failures

**"Client not found" errors:**

1. Verify client exists:
   ```sql
   SELECT cli_mnem, cli_nme FROM Client WHERE cli_mnem = 'CLIENTCODE'
   ```

2. Check for case sensitivity:
   ```sql
-- Make search case-insensitive
   SELECT cli_mnem FROM Client WHERE UPPER(cli_mnem) = UPPER('clientcode')
   ```

**"Invalid date format" errors:**

- CSV must use `YYYY-MM-DD` format
- Excel may auto-format dates incorrectly
- Recommend opening CSV in text editor to verify

**"Duplicate candidate" warnings:**

- This is informational, not an error
- System updates existing candidate with new information
- Review to ensure intentional update

### Selection Generation Issues

**"No candidates available":**

1. Check for candidates:
   ```sql
   SELECT COUNT(*) 
   FROM random_drug_screen_person 
   WHERE client_mnemonic = 'CLIENTCODE'
   AND deleted = 0
   ```

2. If count = 0, candidates need to be added

3. Check if all deleted:
   ```sql
   SELECT COUNT(*) 
   FROM random_drug_screen_person 
   WHERE client_mnemonic = 'CLIENTCODE'
   ```

### Performance Issues

**Page Load Slow:**

1. Check database query performance
2. Review application logs for errors
3. Monitor server resources (CPU, Memory)
4. Consider database indexing on frequently queried columns:
   ```sql
   CREATE INDEX IX_RDS_Client ON random_drug_screen_person(client_mnemonic, deleted)
   ```

**Import Taking Long Time:**

- Large files (>1000 records) may take time
- This is normal, imports are processed in batches
- Monitor progress indicator

---

## Maintenance

### Regular Maintenance Tasks

#### Daily
- Monitor application logs for errors
- Review failed login attempts
- Check for stuck user sessions

#### Weekly
- Review new user access requests
- Verify system backups completed
- Check disk space on server

#### Monthly
- Audit user permissions
- Review and archive old selections
- Clean up deleted candidates (if policy allows)
- Update documentation as needed

#### Quarterly
- Review administrator list
- Analyze usage patterns
- Plan for capacity upgrades if needed
- Security audit

### Database Maintenance

#### Backup Strategy

```sql
-- Full backup (daily)
BACKUP DATABASE [LabBillingDatabase]
TO DISK = 'path\to\backup\LabBilling_Full.bak'
WITH INIT, COMPRESSION

-- Transaction log backup (hourly)
BACKUP LOG [LabBillingDatabase]
TO DISK = 'path\to\backup\LabBilling_Log.trn'
WITH INIT, COMPRESSION
```

#### Index Maintenance

```sql
-- Rebuild fragmented indexes (weekly)
ALTER INDEX ALL ON random_drug_screen_person REBUILD

-- Update statistics
UPDATE STATISTICS random_drug_screen_person
```

#### Data Cleanup

```sql
-- Archive old selections (older than 2 years)
-- First backup the data
SELECT * 
INTO random_drug_screen_selections_archive_2024
FROM random_drug_screen_selections
WHERE selection_date < DATEADD(YEAR, -2, GETDATE())

-- Then delete from main table
DELETE FROM random_drug_screen_selections
WHERE selection_date < DATEADD(YEAR, -2, GETDATE())
```

### Application Updates

#### Pre-Update Checklist
- [ ] Backup database
- [ ] Notify users of scheduled downtime
- [ ] Test update in development environment
- [ ] Document changes
- [ ] Prepare rollback plan

#### Update Process
1. Stop application (IIS App Pool or service)
2. Deploy new application files
3. Run any database migration scripts
4. Update configuration files if needed
5. Start application
6. Verify functionality
7. Notify users update is complete

#### Post-Update Verification
- [ ] Application starts successfully
- [ ] Users can log in
- [ ] RDS module accessible
- [ ] Selection process works
- [ ] Import/Export functions work
- [ ] Reports generate correctly

### Monitoring

#### Key Metrics to Monitor

1. **Authentication Failures**
   - Location: Application logs
   - Alert if: >10 failures per hour from single user

2. **Database Connection Errors**
   - Location: Application logs
   - Alert if: Any errors

3. **Selection Generation Time**
   - Expected: <5 seconds for typical selection
   - Alert if: >30 seconds

4. **Import Success Rate**
   - Expected: >95% success rate
   - Alert if: <90%

5. **Page Load Times**
   - Expected: <2 seconds
   - Alert if: >5 seconds

#### Log File Locations

**Application Logs:**
- Development: `Logs\` folder in application directory
- Production: Windows Event Log or configured log location

**IIS Logs:**
- `C:\inetpub\logs\LogFiles\`

**SQL Server Logs:**
- SQL Server Management Studio ? Management ? SQL Server Logs

### Disaster Recovery

#### Recovery Scenarios

**Scenario 1: Database Corruption**
1. Stop application
2. Restore from most recent full backup
3. Apply transaction log backups
4. Verify data integrity
5. Restart application

**Scenario 2: Lost User Permissions**
1. Locate last known good backup
2. Script out user permissions:
 ```sql
   SELECT 
       'UPDATE emp SET access_random_drug_screen = ' + 
    CAST(access_random_drug_screen AS VARCHAR) + 
       ' WHERE name = ''' + name + ''''
   FROM emp
   WHERE access_random_drug_screen = 1
   ```
3. Save script for reapplication
4. Restore or manually fix permissions

**Scenario 3: Accidental Candidate Deletion**
```sql
-- Restore deleted candidates (within grace period)
UPDATE random_drug_screen_person
SET deleted = 0
WHERE id IN (/* list of IDs */)
AND deleted = 1
```

---

## Support

### Creating Support Documentation

When documenting an issue for escalation:

1. **User Information**
   - Windows username
   - Full name
   - Department

2. **Issue Description**
   - What they were trying to do
   - What happened instead
   - Error messages (exact text)

3. **Diagnostic Information**
   - Database permission check results
   - `/auth-diagnostics` page screenshot
   - Relevant log entries
   - Steps to reproduce

4. **Actions Taken**
   - What troubleshooting steps were performed
   - Results of each step

### Escalation Path

1. **Tier 1:** Help Desk / User Support
   - Password resets
   - Basic troubleshooting
   - Permission requests routing

2. **Tier 2:** System Administrator (you)
   - Grant/revoke permissions
   - Database queries
   - Configuration changes
   - Standard troubleshooting

3. **Tier 3:** Database Administrator
   - Database performance issues
   - Backup/restore operations
   - Schema changes

4. **Tier 4:** Application Developer
   - Code bugs
   - Feature requests
   - System design issues

---

## Appendix

### Useful SQL Queries

**List All RDS Users:**
```sql
SELECT 
    name,
    full_name,
    access,
    CASE WHEN reserve4 = 1 THEN 'Admin' ELSE 'User' END AS UserType,
    CASE WHEN access_random_drug_screen = 1 THEN 'Yes' ELSE 'No' END AS RDSAccess
FROM emp
WHERE access <> 'NONE'
AND (reserve4 = 1 OR access_random_drug_screen = 1)
ORDER BY full_name
```

**Count Candidates by Client:**
```sql
SELECT 
    client_mnemonic,
    COUNT(*) AS TotalCandidates,
    SUM(CASE WHEN deleted = 0 THEN 1 ELSE 0 END) AS ActiveCandidates,
    SUM(CASE WHEN deleted = 1 THEN 1 ELSE 0 END) AS DeletedCandidates
FROM random_drug_screen_person
GROUP BY client_mnemonic
ORDER BY TotalCandidates DESC
```

**Recent Selections:**
```sql
SELECT TOP 50
    selection_date,
    client_mnemonic,
    candidate_name,
    selected_by
FROM random_drug_screen_selections
ORDER BY selection_date DESC
```

**Users Without RDS Access:**
```sql
SELECT 
    name,
    full_name,
    access
FROM emp
WHERE access <> 'NONE'
AND reserve4 = 0
AND access_random_drug_screen = 0
ORDER BY full_name
```

### Permission Matrix

| Feature | Standard User | RDS User | Administrator |
|---------|--------------|----------|---------------|
| Client Viewer | ? | ? | ? |
| RDS Dashboard | ? | ? | ? |
| View Candidates | ? | ? | ? |
| Add/Edit Candidates | ? | ? | ? |
| Delete Candidates | ? | ? | ? |
| Import Candidates | ? | ? | ? |
| Random Selection | ? | ? | ? |
| Generate Reports | ? | ? | ? |
| User Management | ? | ? | ? |

### Configuration Reference

**Authentication:**
- Type: Windows Authentication (Negotiate)
- Fallback: Development mode in appsettings

**Authorization:**
- Type: Policy-based
- Policies: DatabaseUser, RandomDrugScreen
- Claims: IsAdministrator, CanAccessRandomDrugScreen

**Database:**
- Type: SQL Server
- Authentication: Integrated Security
- Connection Pooling: Enabled

---

## Document Information

**Document Title:** Lab Outreach Application - Administrator Guide  
**Version:** 1.0  
**Last Updated:** December 2024  
**Intended Audience:** System Administrators, Database Administrators  
**Classification:** Internal - Confidential

---

*For application source code and technical architecture, contact the development team.*
