-- ============================================================================
-- Authentication Setup Script for Random Drug Screen UI
-- ============================================================================
-- This script helps set up users for authentication in the emp table.
-- It includes examples for both Windows Authentication and SQL Authentication.
-- ============================================================================

-- ============================================================================
-- CHECK EXISTING USERS
-- ============================================================================
-- View all users and their access levels
SELECT 
    name AS Username,
    full_name AS FullName,
    access AS AccessLevel,
    reserve4 AS IsAdministrator,
    access_edit_dictionary AS CanEditDictionary,
    CASE 
        WHEN password IS NULL OR LEN(password) = 0 THEN 'No Password (Windows Auth Only)'
  ELSE 'Password Set (SQL Auth Available)'
    END AS PasswordStatus
FROM emp
ORDER BY full_name;

-- ============================================================================
-- WINDOWS AUTHENTICATION SETUP
-- ============================================================================
-- For Windows Authentication, users just need an entry in the emp table
-- with their Windows username and appropriate access level.

-- Example: Add a new user for Windows Authentication
/*
INSERT INTO emp (name, full_name, access, reserve4, access_edit_dictionary)
VALUES 
    ('jsmith', 'John Smith', 'ENTER/EDIT', 0, 0),
    ('jadmin', 'Jane Admin', 'ENTER/EDIT', 1, 1);
*/

-- Update existing user for Windows Authentication (no password needed)
/*
UPDATE emp 
SET access = 'ENTER/EDIT',    -- Grant access
    password = NULL   -- Clear password (optional, Windows auth doesn't use it)
WHERE name = 'your_windows_username';
*/

-- ============================================================================
-- SQL AUTHENTICATION SETUP
-- ============================================================================
-- For SQL Authentication, users need a hashed password in addition to the entry.
-- Use the PasswordHasher utility to generate hashes.

-- Example: Set password for existing user
-- First, generate hash using PasswordHasher.HashPassword("your_password")
-- Then update the emp table:
/*
UPDATE emp 
SET password = 'wZadGvXKhPwJ1nYkKnKEjo6shfT3RTFG5t3O4TvDuJ0='  -- Replace with actual hash
WHERE name = 'username';
*/

-- Example: Add new user with password for SQL Authentication
/*
INSERT INTO emp (
    name, 
    full_name, 
    access, 
    password,
    reserve4,
    access_edit_dictionary,
    add_chrg,
    add_chk,
    add_chk_amt
)
VALUES (
    'testuser', -- Username
    'Test User',       -- Full name
    'ENTER/EDIT',      -- Access level
    'wZadGvXKhPwJ1nYkKnKEjo6shfT3RTFG5t3O4TvDuJ0=',         -- Password hash (use PasswordHasher)
    0,         -- Is Administrator (0=No, 1=Yes)
    0,            -- Can Edit Dictionary (0=No, 1=Yes)
    0,    -- Can Submit Charges
    0,  -- Can Add Adjustments
    0          -- Can Add Payments
);
*/

-- ============================================================================
-- ACCESS LEVELS
-- ============================================================================
-- Valid access levels in the emp table:
-- 'NONE'       - No access (user cannot log in)
-- 'VIEW'       - Read-only access
-- 'ENTER/EDIT' - Full access to create and modify records

-- Update user access level
/*
UPDATE emp 
SET access = 'ENTER/EDIT'  -- or 'VIEW' or 'NONE'
WHERE name = 'username';
*/

-- ============================================================================
-- PERMISSIONS
-- ============================================================================
-- Additional permissions are controlled by specific columns:

-- Grant administrator privileges
/*
UPDATE emp 
SET reserve4 = 1  -- IsAdministrator
WHERE name = 'username';
*/

-- Grant dictionary editing permission
/*
UPDATE emp 
SET access_edit_dictionary = 1  -- CanEditDictionary
WHERE name = 'username';
*/

-- Grant all permissions
/*
UPDATE emp 
SET reserve4 = 1,-- Administrator
    access_edit_dictionary = 1,       -- Can Edit Dictionary
    add_chrg = 1,              -- Can Submit Charges
add_chk = 1,        -- Can Add Adjustments
    add_chk_amt = 1     -- Can Add Payments
WHERE name = 'username';
*/

-- ============================================================================
-- TESTING QUERIES
-- ============================================================================

-- Check if a Windows username exists
/*
SELECT * FROM emp WHERE name = 'your_windows_username';
*/

-- Verify user has appropriate access
/*
SELECT 
    name,
    full_name,
    access,
    CASE 
        WHEN access = 'NONE' THEN 'BLOCKED - Cannot log in'
        WHEN access = 'VIEW' THEN 'OK - Read-only access'
        WHEN access = 'ENTER/EDIT' THEN 'OK - Full access'
        ELSE 'UNKNOWN - Check access value'
    END AS AccessStatus
FROM emp 
WHERE name = 'username';
*/

-- ============================================================================
-- PASSWORD HASH GENERATION (C# Code)
-- ============================================================================
-- Use this C# code to generate password hashes:
/*
using LabOutreachUI.Utilities;

var password = "mypassword123";
var hash = PasswordHasher.HashPassword(password);
Console.WriteLine($"Password hash: {hash}");

// Then use the hash in your SQL UPDATE statement
*/

-- Or use this SQL CLR function if available:
/*
SELECT CONVERT(VARCHAR(MAX), HASHBYTES('SHA2_256', 'mypassword123'), 2);
*/

-- Note: The application uses Base64 encoding, so you'll need to convert:
-- In C#: Convert.ToBase64String(hash)

-- ============================================================================
-- TROUBLESHOOTING
-- ============================================================================

-- Find users who cannot log in (access = 'NONE')
SELECT name, full_name, access 
FROM emp 
WHERE access = 'NONE';

-- Find users without passwords (Windows auth only)
SELECT name, full_name, access 
FROM emp 
WHERE password IS NULL OR LEN(password) = 0;

-- Find users with passwords (SQL auth available)
SELECT name, full_name, access 
FROM emp 
WHERE password IS NOT NULL AND LEN(password) > 0;

-- Check for duplicate usernames
SELECT name, COUNT(*) as Count
FROM emp
GROUP BY name
HAVING COUNT(*) > 1;

-- ============================================================================
-- CLEANUP / SECURITY
-- ============================================================================

-- Revoke access for a user (don't delete, just disable)
/*
UPDATE emp 
SET access = 'NONE'
WHERE name = 'username';
*/

-- Clear password (force Windows auth only)
/*
UPDATE emp 
SET password = NULL
WHERE name = 'username';
*/

-- Delete a user (not recommended, use 'NONE' access instead)
/*
DELETE FROM emp WHERE name = 'username';
*/

-- ============================================================================
-- EXAMPLE: Complete User Setup
-- ============================================================================
/*
-- 1. For Windows Authentication user:
INSERT INTO emp (name, full_name, access, reserve4, access_edit_dictionary)
VALUES ('bsmith', 'Bob Smith', 'ENTER/EDIT', 0, 0);

-- 2. For SQL Authentication user (after generating hash):
INSERT INTO emp (name, full_name, access, password, reserve4, access_edit_dictionary)
VALUES ('jdoe', 'John Doe', 'ENTER/EDIT', '<hash_from_passwordhasher>', 0, 0);

-- 3. For Administrator user:
INSERT INTO emp (name, full_name, access, reserve4, access_edit_dictionary)
VALUES ('admin', 'System Administrator', 'ENTER/EDIT', 1, 1);

-- 4. Verify the users:
SELECT name, full_name, access, reserve4 as IsAdmin, access_edit_dictionary as CanEditDict
FROM emp 
WHERE name IN ('bsmith', 'jdoe', 'admin');
*/

-- ============================================================================
-- NOTES
-- ============================================================================
-- * Windows username must match exactly (case-insensitive in SQL Server)
-- * Access level must be 'VIEW' or 'ENTER/EDIT' to allow login
-- * Passwords are SHA256 hashes, Base64-encoded
-- * Use PasswordHasher utility to generate hashes
-- * reserve4 column = IsAdministrator flag
-- * access_edit_dictionary column = CanEditDictionary flag
-- ============================================================================
