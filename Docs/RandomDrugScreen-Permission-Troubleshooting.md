# Random Drug Screen Permission Troubleshooting Guide

## Issue
User with `CanAccessRandomDrugScreen = 0 (False)` can still access Random Drug Screen pages.

## Diagnostic Steps

### 1. Verify Database Value
First, confirm the permission is actually set to `0` in the database:

```sql
SELECT 
    name,
    full_name,
    access,
    reserve4 AS IsAdministrator,
 access_random_drug_screen AS CanAccessRandomDrugScreen
FROM emp
WHERE name = 'your_username'
```

**Expected Result:** `CanAccessRandomDrugScreen` should be `0` and `IsAdministrator` should be `0`

### 2. Check Authentication Diagnostics Page
Navigate to: `/auth-diagnostics`

Look for these specific claims:
- `DbUserValidated` = should be "true"
- `IsAdministrator` = should be "False"  
- `CanAccessRandomDrugScreen` = should be "False"

**Expected Behavior:** The page should show "? DENIED" for Random Drug Screen access

### 3. Check Application Logs
Look in the application logs for these specific log entries when trying to access `/rds/dashboard`:

```
[WindowsAuthMiddleware] Processing authenticated user: {Username}
[WindowsAuthMiddleware] User authorized: {Username}, Access: {Access}
[WindowsAuthMiddleware] Added database claims for {Username}
[RDSAuthHandler] Checking RDS access for user: {Username}
[RDSAuthHandler] User {Username} - IsAdmin: {IsAdmin}, CanAccessRDS: {CanAccessRDS}
[RDSAuthHandler] ? Random Drug Screen access DENIED for {Username}
```

### 4. Test Access Scenarios

| Scenario | IsAdministrator | CanAccessRandomDrugScreen | Expected Result |
|----------|----------------|---------------------------|-----------------|
| Test 1   | False   | False   | ? DENIED     |
| Test 2   | False          | True        | ? GRANTED    |
| Test 3   | True     | False       | ? GRANTED      |
| Test 4   | True         | True          | ? GRANTED      |

### 5. Verify Authorization Handler Registration
Check `Program.cs` for proper registration:

```csharp
// Should have both handlers registered
builder.Services.AddScoped<IAuthorizationHandler, DatabaseUserAuthorizationHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RandomDrugScreenAuthorizationHandler>();

// Should have RandomDrugScreen policy
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RandomDrugScreen", policy =>
   policy.Requirements.Add(new RandomDrugScreenRequirement()));
});
```

## Common Issues & Solutions

### Issue 1: Claims Not Being Set
**Symptom:** Auth diagnostics page shows claims as "null" or missing

**Solution:**
1. Check that middleware is registered in correct order in `Program.cs`:
   ```csharp
   app.UseAuthentication();
   app.UseWindowsAuthenticationMiddleware(); // Must be after UseAuthentication
   app.UseAuthorization();
   ```

2. Verify database connection is working in middleware

### Issue 2: Authorization Handler Not Running
**Symptom:** No log entries from `[RDSAuthHandler]`

**Solution:**
1. Verify handler is registered as `Scoped` not `Singleton`
2. Check that the policy name matches exactly: `"RandomDrugScreen"`
3. Verify `@attribute [Authorize(Policy = "RandomDrugScreen")]` is on all RDS pages

### Issue 3: Boolean Comparison Issues
**Symptom:** Claims are being set but comparison fails

**Solution:**
The issue might be boolean string comparison. In the authorization handler, we're comparing:
```csharp
if (isAdminClaim?.Value == "True" || rdsAccessClaim?.Value == "True")
```

Boolean values in C# serialize as "True" or "False" (capital T/F), but SQL Server bit fields might come through differently.

**Test this:**
```csharp
// In middleware, explicitly log the exact values:
_logger.LogInformation("IsAdmin value: '{Value}' (Type: {Type})", 
    dbUser.IsAdministrator, dbUser.IsAdministrator.GetType());
_logger.LogInformation("CanAccessRDS value: '{Value}' (Type: {Type})", 
    dbUser.CanAccessRandomDrugScreen, dbUser.CanAccessRandomDrugScreen.GetType());
```

### Issue 4: Caching in Blazor Server
**Symptom:** Changes to permissions don't take effect until restart

**Solution:**
Blazor Server maintains a SignalR connection. Authorization is checked:
1. On initial HTTP request to `_Host`
2. When navigating between pages (if using `AuthorizeRouteView`)

Force re-authentication by:
1. Closing browser completely (not just tab)
2. Restarting the application
3. Clearing browser cache

### Issue 5: Page Rendered Before Authorization Check
**Symptom:** Page content flashes before redirect to Access Denied

**Solution:**
This is expected behavior in Blazor Server. The `AuthorizeRouteView` checks authorization and will redirect, but there may be a brief flash. This is normal and doesn't indicate a security issue.

## Testing Procedure

1. **Set up test user:**
   ```sql
   UPDATE emp 
 SET access_random_drug_screen = 0, reserve4 = 0 
   WHERE name = 'testuser'
   ```

2. **Clear browser cache and restart app**

3. **Login as test user**

4. **Check diagnostics:**
   - Navigate to `/auth-diagnostics`
   - Verify claims show False/False
   - Take screenshot

5. **Attempt access:**
   - Try to navigate to `/rds/dashboard`
   - **Expected:** Should redirect to `/AccessDeniedPage`
   - Take screenshot

6. **Check logs:**
   - Look for authorization denial message
   - Should see: `[RDSAuthHandler] ? Random Drug Screen access DENIED`

7. **Grant permission:**
   ```sql
UPDATE emp 
   SET access_random_drug_screen = 1 
 WHERE name = 'testuser'
   ```

8. **Re-test:**
   - Close ALL browser windows
   - Restart application
   - Login again
   - Try `/rds/dashboard` - should now work

## Verification Checklist

- [ ] Database column exists and is populated
- [ ] UserAccount model has property mapped
- [ ] Middleware adds claims correctly
- [ ] Authorization handler is registered
- [ ] Policy is defined with correct name
- [ ] All RDS pages have `[Authorize(Policy = "RandomDrugScreen")]`
- [ ] Logs show authorization checks happening
- [ ] Test with user having False/False works (denies access)
- [ ] Test with user having False/True works (grants access)
- [ ] Test with admin works (grants access)

## Build Verification

Run this command to verify no compilation errors:
```powershell
dotnet build
```

**Expected:** Build succeeds with 0 errors

## Additional Debugging

If the issue persists, add temporary debugging code to the authorization handler:

```csharp
protected override Task HandleRequirementAsync(
    AuthorizationHandlerContext context,
    RandomDrugScreenRequirement requirement)
{
  // TEMPORARY DEBUG CODE
  var allClaims = string.Join(", ", context.User.Claims.Select(c => $"{c.Type}={c.Value}"));
    _logger.LogWarning("[DEBUG] All claims: {Claims}", allClaims);
    
    // ... rest of handler code
}
```

This will help identify if claims are being set at all, and what their exact values are.

## Expected Log Output (Success Case - Denial)

```
[WindowsAuthMiddleware] Processing authenticated user: testuser, AuthType: Negotiate
[WindowsAuthMiddleware] User authorized: testuser, Access: ENTER/EDIT
[WindowsAuthMiddleware] Added database claims for testuser
[RDSAuthHandler] Checking RDS access for user: testuser, DbValidated: true
[RDSAuthHandler] User testuser - IsAdmin: False, CanAccessRDS: False
[RDSAuthHandler] ? Random Drug Screen access DENIED for testuser (IsAdmin=False, CanAccessRDS=False)
```

## Contact
If issue persists after following all steps, provide:
1. Screenshot of `/auth-diagnostics` page
2. Relevant log entries
3. SQL query results showing user permissions
4. Description of observed vs expected behavior
