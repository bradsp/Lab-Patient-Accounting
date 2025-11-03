using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LabOutreachUI.Authorization;

/// <summary>
/// Authorization requirement that checks if user exists in database
/// </summary>
public class DatabaseUserRequirement : IAuthorizationRequirement
{
}

/// <summary>
/// Authorization handler that validates user against database
/// </summary>
public class DatabaseUserAuthorizationHandler : AuthorizationHandler<DatabaseUserRequirement>
{
    private readonly ILogger<DatabaseUserAuthorizationHandler> _logger;

    public DatabaseUserAuthorizationHandler(ILogger<DatabaseUserAuthorizationHandler> logger)
    {
     _logger = logger;
    }

    protected override Task HandleRequirementAsync(
     AuthorizationHandlerContext context,
        DatabaseUserRequirement requirement)
    {
   // Check if user has been validated against database
   var dbValidatedClaim = context.User.FindFirst("DbUserValidated");

        if (dbValidatedClaim?.Value == "true")
        {
        _logger.LogDebug("[DatabaseAuthHandler] User authorized via database");
         context.Succeed(requirement);
     }
     else
        {
            var username = context.User.Identity?.Name ?? "Unknown";
    var reason = context.User.FindFirst("UnauthorizedReason")?.Value ?? "Unknown";
            
    _logger.LogWarning("[DatabaseAuthHandler] User NOT authorized: {Username}, Reason: {Reason}", 
     username, reason);
            
      // Don't call context.Fail() - let it fail naturally
        }

        return Task.CompletedTask;
    }
}
