# SSL Quick Start Guide

## ?? Quick Setup (5 minutes)

### Step 1: Trust the Development Certificate
Open PowerShell or Command Prompt and run:
```bash
dotnet dev-certs https --trust
```

Click **"Yes"** when prompted to trust the certificate.

### Step 2: Run the Application
In Visual Studio:
1. Select **"https"** profile from the dropdown (next to the Run button)
2. Press **F5** to run

Or from command line:
```bash
cd LabOutreachUI
dotnet run --launch-profile https
```

### Step 3: Access the Application
Open your browser and navigate to:
- **https://localhost:7063** ? Use this (secure)
- ~~http://localhost:5063~~ (will redirect to HTTPS)

You should see a padlock icon ?? in the address bar.

## ? Verification

### Quick Test
1. Open https://localhost:7063
2. Look for padlock icon in browser address bar
3. Click padlock ? should show "Connection is secure"

### If You See "Not Secure" Warning:
Run this command again:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

Then restart your browser and try again.

## ?? Troubleshooting

### Problem: Certificate Trust Failed on Windows
**Solution:**
1. Open `certmgr.msc` (Windows Key + R, type `certmgr.msc`)
2. Navigate to: Personal ? Certificates
3. Delete any "ASP.NET Core HTTPS development certificate"
4. Run: `dotnet dev-certs https --trust`

### Problem: Certificate Trust Failed on macOS
**Solution:**
1. Open Keychain Access
2. Search for "localhost"
3. Delete ASP.NET Core certificates
4. Run: `dotnet dev-certs https --trust`
5. Enter your password when prompted

### Problem: "ERR_CERT_AUTHORITY_INVALID" Error
**Solution:**
This is normal for development certificates. Run:
```bash
dotnet dev-certs https --trust
```

### Problem: Application Won't Start on HTTPS
**Check:**
1. Port 7063 is not in use: `netstat -ano | findstr :7063`
2. Certificate exists: `dotnet dev-certs https --check`
3. Try different profile: Use "http" profile temporarily

## ?? Development Notes

### Available Launch Profiles
1. **https** (Recommended) - Uses HTTPS on port 7063
2. **http** - Uses HTTP on port 5063, redirects to HTTPS
3. **IIS Express** - Uses IIS Express with SSL on port 44363

### Ports Used
- **7063** - HTTPS (Development)
- **5063** - HTTP (Development, redirects)
- **44363** - HTTPS (IIS Express)

### Environment Variables
You can also set these in your shell:
```bash
$env:ASPNETCORE_URLS = "https://localhost:7063;http://localhost:5063"
```

## ?? Security Features Enabled

When you run with HTTPS:
- ? All traffic encrypted with TLS 1.2+
- ? Secure cookies enabled automatically
- ? HTTP requests redirect to HTTPS
- ? HSTS enabled (browser remembers to use HTTPS)

## ?? More Information

For detailed information about:
- Production deployment
- Certificate management
- HIPAA compliance
- Troubleshooting

See: `SSL_CONFIGURATION.md` and `SSL_IMPLEMENTATION_SUMMARY.md`

## ?? Important Notes

### For Developers:
- Always use the **"https"** profile when running locally
- Development certificate is self-signed (safe for local dev)
- Don't commit certificate files to source control
- Production uses different, trusted certificates

### Before Committing Code:
- ? Verify application runs on HTTPS
- ? Check no mixed content warnings
- ? Test HTTP redirects to HTTPS
- ? Ensure no hardcoded HTTP URLs in code

## ?? Need Help?

If you're still having issues:
1. Check `SSL_CONFIGURATION.md` for detailed troubleshooting
2. Search: "ASP.NET Core dev certificate [your specific error]"
3. Contact the DevOps/Security team

---

**Last Updated**: January 2025  
**Quick Reference Version**: 1.0
