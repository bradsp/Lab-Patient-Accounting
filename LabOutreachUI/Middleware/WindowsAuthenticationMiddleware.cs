using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using System.Security.Claims;

namespace LabOutreachUI.Middleware;

/// <summary>
/// Middleware that captures Windows authenticated user and validates against database.
/// Adds custom claims for authorization.
/// </summary>
public class WindowsAuthenticationMiddleware
{
  private readonly RequestDelegate _next;
    private readonly ILogger<WindowsAuthenticationMiddleware> _logger;

    public WindowsAuthenticationMiddleware(RequestDelegate next, ILogger<WindowsAuthenticationMiddleware> logger)
    {
      _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAppEnvironment appEnvironment)
    {
  // Only process if user is authenticated
    if (context.User?.Identity?.IsAuthenticated == true)
        {
      var authType = context.User.Identity.AuthenticationType;
     var windowsUsername = context.User.Identity.Name;
     
            _logger.LogInformation("[WindowsAuthMiddleware] Processing authenticated user: {Username}, AuthType: {AuthType}", 
        windowsUsername, authType);

      // Check if we've already processed this user (avoid redundant DB queries)
         if (!context.User.HasClaim(c => c.Type == "DbUserValidated"))
      {
  try
              {
  // Query database to validate user
       using var uow = new UnitOfWorkSystem(appEnvironment);
   var authService = new AuthenticationService(uow);
 var dbUser = authService.AuthenticateIntegrated(windowsUsername);

   if (dbUser != null && dbUser.Access != UserStatus.None)
       {
      _logger.LogInformation("[WindowsAuthMiddleware] User authorized: {Username}, Access: {Access}", 
          dbUser.UserName, dbUser.Access);

    // Create new ClaimsIdentity with database user info
    var claims = new List<System.Security.Claims.Claim>
 {
          new System.Security.Claims.Claim(ClaimTypes.Name, dbUser.UserName),
    new System.Security.Claims.Claim(ClaimTypes.GivenName, dbUser.FullName ?? dbUser.UserName),
      new System.Security.Claims.Claim("Access", dbUser.Access ?? UserStatus.None),
     new System.Security.Claims.Claim("DbUserValidated", "true"),
          new System.Security.Claims.Claim("DbUserName", dbUser.UserName)
       };

             if (dbUser.IsAdministrator)
           {
   claims.Add(new System.Security.Claims.Claim(ClaimTypes.Role, "Administrator"));
          }

               if (dbUser.CanEditDictionary)
          {
      claims.Add(new System.Security.Claims.Claim("Permission", "EditDictionary"));
           }

             // Append claims to existing identity
          var identity = new ClaimsIdentity(claims, "DatabaseAuthorization");
            context.User.AddIdentity(identity);

 _logger.LogInformation("[WindowsAuthMiddleware] Added database claims for {Username}", dbUser.UserName);
        }
         else
       {
      _logger.LogWarning("[WindowsAuthMiddleware] User not authorized: {Username}", windowsUsername);
      
          // Add claim indicating user is NOT authorized
 var identity = new ClaimsIdentity(new[]
   {
              new System.Security.Claims.Claim("DbUserValidated", "false"),
      new System.Security.Claims.Claim("UnauthorizedReason", "NotInDatabase")
   }, "DatabaseAuthorization");
        
      context.User.AddIdentity(identity);
}
       }
        catch (Exception ex)
    {
   _logger.LogError(ex, "[WindowsAuthMiddleware] Error validating user: {Username}", windowsUsername);
        
    // Add error claim
  var identity = new ClaimsIdentity(new[]
         {
  new System.Security.Claims.Claim("DbUserValidated", "false"),
   new System.Security.Claims.Claim("UnauthorizedReason", "DatabaseError")
     }, "DatabaseAuthorization");
      
   context.User.AddIdentity(identity);
      }
        }
        }
        else
    {
       _logger.LogDebug("[WindowsAuthMiddleware] No authentication detected or already processed");
        }

        await _next(context);
    }
}

/// <summary>
/// Extension method to register the middleware
/// </summary>
public static class WindowsAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseWindowsAuthenticationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<WindowsAuthenticationMiddleware>();
    }
}
