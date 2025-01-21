using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using Microsoft.Extensions.Logging;

public class AppEnvironmentInitializationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AppEnvironmentInitializationMiddleware> _logger;

    public AppEnvironmentInitializationMiddleware(RequestDelegate next, ILogger<AppEnvironmentInitializationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IAppEnvironment appEnvironment, IServiceProvider serviceProvider)
    {
        // Exclude static files and SignalR hub endpoints
        var path = context.Request.Path;

        if (path.StartsWithSegments("/_blazor") ||
            path.StartsWithSegments("/_framework") ||
            path.StartsWithSegments("/css") ||
            path.StartsWithSegments("/js") ||
            path.StartsWithSegments("/images"))
        {
            // Skip initialization and proceed to the next middleware
            await _next(context);
            return;
        }

        if (!appEnvironment.EnvironmentValid)
        {

            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var username = context.User.Identity.Name.Split('\\')[1]; // Extract the username
                _logger.LogInformation("Authenticated user: {Username}", username);

                appEnvironment.UserName = username;
                appEnvironment.IntegratedAuthentication = true;
                appEnvironment.DatabaseName = "LabBillingTest";
                appEnvironment.ServerName = "wthmclbill";
                appEnvironment.LogDatabaseName = "NLog";

                _logger.LogInformation("AppEnvironment initialized with ServerName: {ServerName}, DatabaseName: {DatabaseName}", appEnvironment.ServerName, appEnvironment.DatabaseName);

                var systemService = new SystemService(appEnvironment);

                appEnvironment.UserAccount = systemService.GetUser(username);
                appEnvironment.ApplicationParameters = systemService.LoadSystemParameters();
            }
            else
            {
                _logger.LogWarning("Unauthenticated user");
            }
        }
        await _next(context);
    }
}
