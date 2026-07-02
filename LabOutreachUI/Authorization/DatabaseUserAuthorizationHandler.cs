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
/// Authorization requirement that checks if user can edit dictionary/reference data
/// </summary>
public class EditDictionaryRequirement : IAuthorizationRequirement
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

/// <summary>
/// Authorization handler for Dictionary edit access ("Can Edit Dictionary").
/// The "EditDictionary" permission is delivered as a claim of type "Permission" with value
/// "EditDictionary" by WindowsAuthenticationMiddleware when emp.access_edit_dictionary is set.
/// </summary>
public class EditDictionaryAuthorizationHandler : AuthorizationHandler<EditDictionaryRequirement>
{
    private readonly ILogger<EditDictionaryAuthorizationHandler> _logger;

    public EditDictionaryAuthorizationHandler(ILogger<EditDictionaryAuthorizationHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        EditDictionaryRequirement requirement)
    {
        var username = context.User.Identity?.Name ?? "Unknown";

        // First check if user is validated in database
        var dbValidatedClaim = context.User.FindFirst("DbUserValidated");

        if (dbValidatedClaim?.Value != "true")
        {
            _logger.LogWarning("[EditDictionaryAuthHandler] User {Username} not validated in database", username);
            return Task.CompletedTask;
        }

        // Administrators always have access OR the EditDictionary permission claim is present
        var isAdminClaim = context.User.FindFirst("IsAdministrator");
        var canEditDictionary = context.User.HasClaim("Permission", "EditDictionary");

        if (isAdminClaim?.Value == "True" || canEditDictionary)
        {
            _logger.LogInformation("[EditDictionaryAuthHandler] Dictionary edit access GRANTED for {Username} (IsAdmin={IsAdmin}, CanEditDictionary={CanEdit})",
                username, isAdminClaim?.Value ?? "null", canEditDictionary);
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning("[EditDictionaryAuthHandler] Dictionary edit access DENIED for {Username} (IsAdmin={IsAdmin}, CanEditDictionary={CanEdit})",
                username, isAdminClaim?.Value ?? "null", canEditDictionary);

            // Do NOT call context.Succeed() - the requirement fails naturally
        }

        return Task.CompletedTask;
    }
}
