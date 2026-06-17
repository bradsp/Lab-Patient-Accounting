using LabBilling.Core.Models;
using LabBilling.Logging;
using Npgsql;
using System;
using System.Drawing;
using System.IO;

namespace LabBilling.Core.DataAccess;

public class AppEnvironment : IAppEnvironment
{
    public string DatabaseName { get; set; }
    public string ServerName { get; set; }
    public string LogDatabaseName { get; set; }
    public string Environment { get; set; }

    public string ServiceUsername { get; set; }
    public string ServicePassword { get; set; }

    public bool IntegratedAuthentication { get; set; }

    public UserAccount UserAccount { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }

    public bool RunAsService { get; set; } = false;

    public Color WindowBackgroundColor { get; set; } = Color.White;
    public Color WindowTextColor { get; set; } = Color.Black;
    public Color MenuBackgroundColor { get; set; } = Color.White;
    public Color MenuTextColor { get; set; } = Color.Black;

    public Color ButtonBackgroundColor { get; set; } = Color.LightCyan;
    public Color ButtonTextColor { get; set; } = Color.Black;

    public string TempFilePath => Path.GetTempPath() + @"LABPA\";

    // Default PostgreSQL port for local docker dev (Phase 22 container maps 5434).
    private const int _defaultPostgresPort = 5434;

    /// <summary>
    /// TLS policy for the PostgreSQL connection. Defaults to a dev-safe value for the
    /// local docker instance. The managed/prod target (AWS RDS) must override this to
    /// <see cref="SslMode.VerifyFull"/> (with a <see cref="RootCertificate"/>) to close the
    /// HIPAA in-transit gap (FINDINGS R6). Set from config; a code default is fine for now.
    /// </summary>
    public SslMode SslMode { get; set; } = SslMode.Prefer;

    /// <summary>
    /// Path to the CA root certificate bundle used when <see cref="SslMode"/> is
    /// <see cref="SslMode.VerifyCA"/>/<see cref="SslMode.VerifyFull"/> (e.g. the RDS CA bundle).
    /// </summary>
    public string RootCertificate { get; set; }

    public AppEnvironment()
    {
        // Initialize any necessary properties or fields here
    }

    /// <summary>
    /// Splits a configured server value into host + port. Accepts a bare host, or
    /// "host:port" / "host,port" (legacy SQL Server style). Falls back to the local
    /// PostgreSQL dev port when none is supplied.
    /// </summary>
    private (string Host, int Port) ResolveHostAndPort()
    {
        string raw = ServerName?.Trim() ?? string.Empty;
        char[] separators = { ',', ':' };
        int idx = raw.IndexOfAny(separators);

        if (idx >= 0 && int.TryParse(raw[(idx + 1)..].Trim(), out int parsedPort))
        {
            return (raw[..idx].Trim(), parsedPort);
        }

        return (raw, _defaultPostgresPort);
    }

    private NpgsqlConnectionStringBuilder BuildConnection(string database)
    {
        (string host, int port) = ResolveHostAndPort();

        NpgsqlConnectionStringBuilder builder = new()
        {
            Host = host,
            Port = port,
            Database = database,
            ApplicationName = Utilities.OS.GetAppName(),
            SslMode = SslMode,
            Timeout = 30,
            MinPoolSize = 10,
            MaxPoolSize = 200,
            Pooling = true,
        };

        if (!string.IsNullOrEmpty(RootCertificate))
        {
            builder.RootCertificate = RootCertificate;
        }

        // Npgsql has no MARS and (as of v10) no builder-level IntegratedSecurity flag.
        // For SQL-auth dev we supply Username/Password. Kerberos/integrated security against a
        // Windows-hosted PostgreSQL would be plumbed via the "Integrated Security=true" keyword
        // later; not needed for the local docker dev target.
        if (!IntegratedAuthentication)
        {
            builder.Username = UserName;
            builder.Password = Password;
        }

        return builder;
    }

    public bool EnvironmentValid
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                return false;
            }

            if (ApplicationParameters == null)
            {
                return false;
            }

            return true;
        }
    }

    public string ConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
            {
                return string.Empty;
            }

            try
            {
                return BuildConnection(DatabaseName).ConnectionString;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error building connection string.", ex);
                return string.Empty;
            }
        }
    }

    public string ConnectionStringService => ConnectionString;

    public string User { get; set; }

    private ApplicationParameters _appParms;
    public ApplicationParameters ApplicationParameters { get; set; }

    public string LogConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(LogDatabaseName))
            {
                return string.Empty;
            }

            try
            {
                return BuildConnection(LogDatabaseName).ConnectionString;
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error building log connection string.", ex);
                return string.Empty;
            }
        }
    }

    public string[] GetArgs()
    {
        string[] args = new string[2];

        args[0] = ServerName;
        args[1] = DatabaseName;

        return args;
    }
}
