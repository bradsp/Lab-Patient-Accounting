using Microsoft.Data.SqlClient;

namespace Utilities;

/// <summary>
/// Helper utilities for connection strings.
/// </summary>
public sealed class ConnectionString
{
    private readonly string _value;
    private SqlConnectionStringBuilder dbConnectionStringBuilder;

    /// <summary>
    /// Returns the extracted database name from the connection string.
    /// </summary>
    public string DatabaseName
    {
        get
        {
            return dbConnectionStringBuilder.InitialCatalog;
        }
    }

    /// <summary>
    /// Returns the extracted servername from the connection string.
    /// </summary>
    public string ServerName
    {
        get
        {
            return dbConnectionStringBuilder.DataSource;
        }
    }
    /// <summary>
    /// Constructs the connection string. 
    /// </summary>
    /// <param name="connectionString"></param>
    public ConnectionString(string connectionString)
    {
        _value = connectionString;

        dbConnectionStringBuilder = new SqlConnectionStringBuilder();
        dbConnectionStringBuilder.ConnectionString = _value;
    }

    /// <summary>
    /// Initialize ConnectionString by assigning a connection string.
    /// </summary>
    /// <param name="cs"></param>
    public static implicit operator ConnectionString(string cs) => new ConnectionString(cs);

    /// <summary>
    /// Returns a string array with the servername and database name.
    /// </summary>
    /// <returns></returns>
    public string[] ToArray()
    {

        string[] args = new string[]
        {
            ServerName,
            DatabaseName,
        };

        return args;
    }

}
