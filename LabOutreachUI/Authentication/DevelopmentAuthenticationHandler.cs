using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LabOutreachUI.Authentication;

/// <summary>
/// Development authentication handler that bypasses Windows Authentication for local debugging
/// </summary>
public class DevelopmentAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<DevelopmentAuthenticationHandler> _logger;

    public DevelopmentAuthenticationHandler(
      IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
     UrlEncoder encoder,
    IConfiguration configuration)
        : base(options, logger, encoder)
    {
        _configuration = configuration;
   _logger = logger.CreateLogger<DevelopmentAuthenticationHandler>();
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
 {
            // Get the configured development user
    var developmentUser = _configuration.GetValue<string>("AppSettings:DevelopmentUser");

            if (string.IsNullOrEmpty(developmentUser))
            {
      _logger.LogWarning("DevelopmentUser not configured in appsettings");
    developmentUser = Environment.UserName;
       }

       _logger.LogInformation($"Development Authentication: Authenticating as {developmentUser}");

     // Create claims for the development user
            var claims = new[]
            {
          new Claim(ClaimTypes.Name, developmentUser),
  new Claim(ClaimTypes.NameIdentifier, developmentUser),
        new Claim("AuthenticationMethod", "Development")
            };

 var identity = new ClaimsIdentity(claims, "Development");
      var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Development");

         return Task.FromResult(AuthenticateResult.Success(ticket));
  }
catch (Exception ex)
     {
      _logger.LogError(ex, "Error in development authentication");
            return Task.FromResult(AuthenticateResult.Fail("Development authentication failed"));
        }
    }
}
