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
/// Authorization requirement that checks if user can access Random Drug Screen module
/// </summary>
public class RandomDrugScreenRequirement : IAuthorizationRequirement
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

/// <summary>
/// Authorization handler for Random Drug Screen access
/// </summary>
public class RandomDrugScreenAuthorizationHandler : AuthorizationHandler<RandomDrugScreenRequirement>
{
    private readonly ILogger<RandomDrugScreenAuthorizationHandler> _logger;

    public RandomDrugScreenAuthorizationHandler(ILogger<RandomDrugScreenAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
   AuthorizationHandlerContext context,
  RandomDrugScreenRequirement requirement)
    {
 var username = context.User.Identity?.Name ?? "Unknown";
        
        // First check if user is validated in database
   var dbValidatedClaim = context.User.FindFirst("DbUserValidated");
   
  _logger.LogInformation("[RDSAuthHandler] Checking RDS access for user: {Username}, DbValidated: {DbValidated}",
   username, dbValidatedClaim?.Value ?? "null");
      
 if (dbValidatedClaim?.Value != "true")
      {
    _logger.LogWarning("[RDSAuthHandler] User {Username} not validated in database", username);
  return Task.CompletedTask;
  }

 // Check for specific Random Drug Screen permission
    var rdsAccessClaim = context.User.FindFirst("CanAccessRandomDrugScreen");
   var isAdminClaim = context.User.FindFirst("IsAdministrator");

  _logger.LogInformation("[RDSAuthHandler] User {Username} - IsAdmin: {IsAdmin}, CanAccessRDS: {CanAccessRDS}",
  username, isAdminClaim?.Value ?? "null", rdsAccessClaim?.Value ?? "null");

        // Administrators always have access OR specific permission granted
   if (isAdminClaim?.Value == "True" || rdsAccessClaim?.Value == "True")
  {
   _logger.LogInformation("[RDSAuthHandler] ? Random Drug Screen access GRANTED for {Username} (IsAdmin={IsAdmin}, CanAccessRDS={CanAccessRDS})",
  username, isAdminClaim?.Value, rdsAccessClaim?.Value);
  context.Succeed(requirement);
 }
   else
   {
       _logger.LogWarning("[RDSAuthHandler] ? Random Drug Screen access DENIED for {Username} (IsAdmin={IsAdmin}, CanAccessRDS={CanAccessRDS})",
        username, isAdminClaim?.Value ?? "null", rdsAccessClaim?.Value ?? "null");
   
    // Important: We should NOT call context.Succeed() here
   // The requirement will fail naturally if we don't call Succeed
    }

   return Task.CompletedTask;
    }
}
