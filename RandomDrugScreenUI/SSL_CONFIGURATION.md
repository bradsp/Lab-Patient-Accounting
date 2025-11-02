# SSL/HTTPS Configuration Guide

## Overview
The RandomDrugScreenUI application is now configured to use HTTPS/SSL to ensure secure communication when handling patient information (PHI). This is required for HIPAA compliance.

## Development Environment

### Trust the Development Certificate
Before running the application in development, you need to trust the ASP.NET Core HTTPS development certificate:

```bash
dotnet dev-certs https --trust
```

This command will:
1. Generate a self-signed certificate if one doesn't exist
2. Add it to your certificate store
3. Configure your browser to trust it

### Verify Certificate
To check if the certificate exists:

```bash
dotnet dev-certs https --check
```

### Clean and Reinstall Certificate (if needed)
If you experience certificate issues:

```bash
# Remove existing certificates
dotnet dev-certs https --clean

# Create and trust new certificate
dotnet dev-certs https --trust
```

## Running the Application

### Development Mode
The application will run on:
- **HTTPS**: https://localhost:7063 (primary, secure)
- **HTTP**: http://localhost:5063 (redirects to HTTPS)

Launch profiles:
- **https** - Uses HTTPS (recommended)
- **http** - Uses HTTP but redirects to HTTPS
- **IIS Express** - Uses IIS Express with SSL

### Production Deployment

#### Option 1: IIS Deployment (Windows Server)

1. **Install SSL Certificate**
   - Obtain a valid SSL certificate from a Certificate Authority (CA) such as:
     - Let's Encrypt (free)
     - DigiCert
     - Comodo
     - GoDaddy
   
2. **Bind Certificate in IIS**
   - Open IIS Manager
   - Select your website
   - Click "Bindings" in the Actions pane
   - Add HTTPS binding on port 443
   - Select your SSL certificate
   - Set hostname (e.g., laboutreach.yourcompany.com)

3. **Configure Web.config**
   ```xml
   <system.webServer>
     <rewrite>
   <rules>
     <rule name="HTTPS Redirect" stopProcessing="true">
           <match url="(.*)" />
     <conditions>
       <add input="{HTTPS}" pattern="^OFF$" />
    </conditions>
 <action type="Redirect" url="https://{HTTP_HOST}/{R:1}" redirectType="Permanent" />
       </rule>
   </rules>
   </rewrite>
   </system.webServer>
   ```

4. **Update appsettings.Production.json**
   - Set appropriate database server
   - Configure authentication method
   - Remove Kestrel endpoints (IIS will handle)

#### Option 2: Kestrel Self-Hosted (Linux/Windows)

1. **Obtain SSL Certificate**
   - For production, use a valid certificate from a CA
   - Place certificate files in a secure location
   - Recommended: Use Let's Encrypt with automatic renewal

2. **Configure Kestrel in appsettings.Production.json**
   ```json
   {
     "Kestrel": {
  "Endpoints": {
         "Https": {
           "Url": "https://*:443",
     "Certificate": {
  "Path": "/path/to/certificate.pfx",
             "Password": "certificate-password"
        }
         }
}
     }
   }
   ```

3. **Using Environment Variables (More Secure)**
   ```bash
   export ASPNETCORE_URLS="https://*:443;http://*:80"
   export ASPNETCORE_Kestrel__Certificates__Default__Path="/path/to/cert.pfx"
   export ASPNETCORE_Kestrel__Certificates__Default__Password="cert-password"
   ```

4. **Using Azure Key Vault (Best Practice)**
   ```csharp
   builder.Configuration.AddAzureKeyVault(
    new Uri("https://your-keyvault.vault.azure.net/"),
   new DefaultAzureCredential());
   
   builder.WebHost.ConfigureKestrel(options =>
   {
       options.ConfigureHttpsDefaults(httpsOptions =>
  {
 httpsOptions.ServerCertificate = LoadCertificateFromKeyVault();
       });
   });
   ```

#### Option 3: Reverse Proxy (Nginx/Apache)

Use a reverse proxy to handle SSL termination:

**Nginx Example:**
```nginx
server {
    listen 443 ssl http2;
    server_name laboutreach.yourcompany.com;
  
    ssl_certificate /path/to/fullchain.pem;
    ssl_certificate_key /path/to/privkey.pem;
    ssl_protocols TLSv1.2 TLSv1.3;
ssl_ciphers HIGH:!aNULL:!MD5;
    
    location / {
        proxy_pass http://localhost:5063;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
  proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}

# Redirect HTTP to HTTPS
server {
    listen 80;
 server_name laboutreach.yourcompany.com;
    return 301 https://$server_name$request_uri;
}
```

## Security Features Enabled

### 1. HTTPS Redirection
- All HTTP requests automatically redirect to HTTPS
- Configured in `Program.cs` with `app.UseHttpsRedirection()`

### 2. HSTS (HTTP Strict Transport Security)
- Enabled in production mode
- **Max Age**: 365 days
- **Include Subdomains**: Yes
- **Preload**: Yes
- Forces browsers to always use HTTPS

### 3. Secure Cookies
ASP.NET Core automatically sets secure cookies when using HTTPS:
- Authentication cookies marked as Secure
- Session cookies marked as Secure
- SameSite policy enforced

## HIPAA Compliance Considerations

### Required for PHI Protection:
? **Transport Layer Security** - HTTPS/TLS 1.2 or higher  
? **HSTS Enabled** - Prevents protocol downgrade attacks  
? **Secure Cookies** - Authentication data encrypted in transit  
? **Certificate Validation** - Valid, non-expired certificates  

### Additional Recommendations:
- Use TLS 1.2 or TLS 1.3 minimum
- Disable older protocols (SSL 3.0, TLS 1.0, TLS 1.1)
- Implement certificate pinning for mobile apps
- Regular certificate renewal (automated with Let's Encrypt)
- Monitor certificate expiration
- Implement proper access controls
- Enable audit logging for security events

## Testing SSL Configuration

### 1. Local Testing
```bash
# Test HTTPS endpoint
curl -k https://localhost:7063

# Test HTTP redirect
curl -I http://localhost:5063
# Should return 307 redirect to HTTPS
```

### 2. Production Testing Tools
- **SSL Labs**: https://www.ssllabs.com/ssltest/
- **Security Headers**: https://securityheaders.com/
- **Mozilla Observatory**: https://observatory.mozilla.org/

### 3. Verify HSTS
```bash
curl -I https://your-domain.com
# Look for: Strict-Transport-Security header
```

## Troubleshooting

### Development Certificate Issues

**Problem**: Browser shows "Your connection is not private"  
**Solution**: Run `dotnet dev-certs https --trust`

**Problem**: Certificate trust fails  
**Solution**:
```bash
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

**Problem**: Certificate not found  
**Solution**: Check certificate exists in user store:
- Windows: `certmgr.msc` ? Personal ? Certificates
- macOS: Keychain Access ? System ? Certificates
- Linux: `~/.dotnet/corefx/cryptography/x509stores/my/`

### Production Certificate Issues

**Problem**: Certificate expired  
**Solution**: Renew certificate through your CA or Let's Encrypt

**Problem**: Mixed content warnings  
**Solution**: Ensure all resources (images, scripts, CSS) use HTTPS or relative URLs

**Problem**: WebSocket connection fails  
**Solution**: Ensure reverse proxy properly forwards WebSocket upgrade headers

## Certificate Renewal

### Let's Encrypt (Recommended for Production)
```bash
# Install certbot
apt-get install certbot

# Obtain certificate
certbot certonly --standalone -d laboutreach.yourcompany.com

# Auto-renewal (add to cron)
certbot renew --quiet
```

### Manual Renewal Reminder
Set calendar reminders 30 days before certificate expiration.

## Monitoring

### Health Checks
Implement SSL/TLS monitoring:
- Certificate expiration monitoring
- TLS protocol version verification
- Cipher suite validation
- HSTS header presence

### Logging
Monitor security-related events:
- Failed HTTPS connections
- Certificate validation errors
- Protocol downgrades
- Unusual access patterns

## Support

### Resources
- ASP.NET Core HTTPS: https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl
- Let's Encrypt: https://letsencrypt.org/
- HSTS Preload: https://hstspreload.org/

### Contact
For issues related to SSL configuration, contact your DevOps or Security team.

## Compliance Documentation

Document the following for HIPAA compliance audits:
- [ ] SSL/TLS version in use (TLS 1.2+)
- [ ] Certificate type and issuer
- [ ] Certificate expiration date
- [ ] HSTS implementation
- [ ] Encryption cipher suites
- [ ] Certificate renewal process
- [ ] Incident response for certificate issues

---

**Last Updated**: January 2025  
**Reviewed By**: [Your Security Team]  
**Next Review**: [Date]
