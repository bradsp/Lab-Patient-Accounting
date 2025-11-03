using LabBilling.Core.DataAccess;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using LabOutreachUI.Authentication;
using LabOutreachUI.Services;
using LabOutreachUI.Middleware;
using LabOutreachUI.Authorization;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using NLog;
using NLog.Web;

// Early init of NLog to allow startup and exception logging
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    logger.Info("Starting application configuration");

    // Add NLog for logging
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    logger.Info("Configuring IIS Integration");

    // Check if we should use Windows Authentication (Production) or Development Authentication
    var useWindowsAuth = builder.Configuration.GetValue<bool>("AppSettings:UseWindowsAuthentication", true);
    var isDevelopment = builder.Environment.IsDevelopment();

    logger.Info($"Environment: {builder.Environment.EnvironmentName}, UseWindowsAuth: {useWindowsAuth}");
    if (useWindowsAuth && !isDevelopment)
    {
        // Production mode with Windows Authentication
        logger.Info("Configuring Windows Authentication for Production");

        // Configure IIS Integration and Windows Authentication
        builder.Services.Configure<IISOptions>(options =>
        {
            options.AutomaticAuthentication = true;
            options.AuthenticationDisplayName = "Windows";
        });

        // Add authentication - use IIS default scheme
        builder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
    }
    else
    {
        // Development mode with simulated authentication
        logger.Info("Configuring Development Authentication");

        builder.Services.AddAuthentication("Development")
            .AddScheme<AuthenticationSchemeOptions, DevelopmentAuthenticationHandler>(
           "Development", options => { });
    }

    // Add authorization services with database user policy
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("DatabaseUser", policy =>
           policy.Requirements.Add(new DatabaseUserRequirement()));

        // Set as fallback policy - all pages require DatabaseUser by default
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
         .RequireAuthenticatedUser()
       .AddRequirements(new DatabaseUserRequirement())
        .Build();
    });

    // Register the authorization handler
    builder.Services.AddScoped<IAuthorizationHandler, DatabaseUserAuthorizationHandler>();

    logger.Info("Adding Razor Pages and Blazor services");

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor(options =>
    {
        options.DetailedErrors = true; // Enable for debugging
    });

    // Add HttpContextAccessor for accessing Windows Authentication context
    builder.Services.AddHttpContextAccessor();

    // DO NOT register custom authentication provider - use built-in
    // The middleware will handle Windows user validation

    // Configure HSTS options for production
    builder.Services.AddHsts(options =>
    {
        options.Preload = true;
        options.IncludeSubDomains = true;
        options.MaxAge = TimeSpan.FromDays(365);
    });

    // Configure AppEnvironment as SCOPED (per-request) to use the actual authenticated user
    builder.Services.AddScoped<IAppEnvironment>(sp =>
    {
        var config = sp.GetRequiredService<IConfiguration>();
        var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
        var log = sp.GetRequiredService<ILogger<Program>>();

        // Get authentication mode from configuration
        var authMode = config.GetValue<string>("AppSettings:AuthenticationMode") ?? "SqlServer";
        var useIntegratedAuth = authMode.Equals("Integrated", StringComparison.OrdinalIgnoreCase);

        // Get Windows user from HttpContext during initial connection
        var windowsUsername = httpContextAccessor.HttpContext?.User?.Identity?.Name
            ?? Environment.UserName;

        log.LogInformation("[AppEnvironment] Creating for user: {User}, AuthMode: {AuthMode}", windowsUsername, authMode);

        var appEnv = new AppEnvironment
        {
            DatabaseName = config.GetValue<string>("AppSettings:DatabaseName") ?? "LabBillingProd",
            ServerName = config.GetValue<string>("AppSettings:ServerName") ?? "localhost",
            LogDatabaseName = config.GetValue<string>("AppSettings:LogDatabaseName") ?? "NLog",
            IntegratedAuthentication = useIntegratedAuth,
            User = windowsUsername  // Store the Windows authenticated user for audit purposes
        };

        // For SQL Server authentication, set credentials from configuration
        if (!useIntegratedAuth)
        {
            appEnv.UserName = config.GetValue<string>("AppSettings:DatabaseUsername") ?? "";
            appEnv.Password = config.GetValue<string>("AppSettings:DatabasePassword") ?? "";

            if (string.IsNullOrEmpty(appEnv.UserName))
            {
                log.LogError("[AppEnvironment] DatabaseUsername not configured for SqlServer authentication mode");
                throw new InvalidOperationException("DatabaseUsername must be configured when AuthenticationMode is SqlServer");
            }
        }

        // Initialize with default empty parameters first to allow ConnectionString to work
        appEnv.ApplicationParameters = new LabBilling.Core.Models.ApplicationParameters();

        try
        {
            // Now try to load real parameters from database
            using var uow = new UnitOfWorkSystem(appEnv);
            var systemService = new SystemService(appEnv, uow);
            appEnv.ApplicationParameters = systemService.LoadSystemParameters();
        }
        catch (Exception ex)
        {
            // Log the error but continue with default parameters
            log.LogWarning(ex, "Could not load system parameters from database. Continuing with defaults.");
        }

        return appEnv;
    });

    // Register UnitOfWorkSystem for authentication (uses system-level connection)
    builder.Services.AddScoped<IUnitOfWorkSystem>(sp =>
    {
        var appEnv = sp.GetRequiredService<IAppEnvironment>();
        return new UnitOfWorkSystem(appEnv);
    });

    // Register services
    builder.Services.AddScoped<IRandomDrugScreenService, RandomDrugScreenService>();
    builder.Services.AddScoped<DictionaryService>();
    builder.Services.AddScoped<RequisitionPrintingService>();
    builder.Services.AddScoped<FormPrintService>();
    builder.Services.AddScoped<IUnitOfWork>(sp =>
    {
        var appEnv = sp.GetRequiredService<IAppEnvironment>();
        return new UnitOfWorkMain(appEnv);
    });

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }

    // Enable HTTPS redirection - IIS binding should be configured
    app.UseHttpsRedirection();

    app.UseStaticFiles();

    app.UseRouting();

    // Add authentication middleware
    app.UseAuthentication();
  
    // Add custom middleware to validate Windows user against database
    app.UseWindowsAuthenticationMiddleware();
    
    // Add status code pages to handle 403 (Forbidden)
    app.UseStatusCodePagesWithReExecute("/AccessDeniedPage");
    
    app.UseAuthorization();

    // Map Blazor Hub - authorization handled by fallback policy
    app.MapBlazorHub();
    app.MapFallbackToPage("/_Host");

    // Log startup info
    logger.Info("Application starting up");
    logger.Info("Environment: {Env}", app.Environment.EnvironmentName);

    if (app.Environment.IsProduction())
    {
        var config = app.Services.GetRequiredService<IConfiguration>();
        var authMode = config.GetValue<string>("AppSettings:AuthenticationMode") ?? "Integrated";

        if (authMode.Equals("Integrated", StringComparison.OrdinalIgnoreCase))
        {
            logger.Info("Production mode with Integrated Authentication enabled");
        }
    }

    app.MapGet("/auth-test", (HttpContext context) =>
    {
        return Results.Json(new
        {
            IsAuthenticated = context.User.Identity?.IsAuthenticated ?? false,
            Username = context.User.Identity?.Name ?? "Anonymous",
            AuthType = context.User.Identity?.AuthenticationType ?? "None"
        });
    });

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
