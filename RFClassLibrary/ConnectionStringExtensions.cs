using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFClassLibrary
{
    /// <summary>
    /// Helper utilities for connection strings.
    /// </summary>
    public class ConnectionString 
    {
        private readonly string _value;

        public string DatabaseName { get; }
        public string ServerName { get; }

        /// <summary>
        /// Constructs the connection string. 
        /// </summary>
        /// <param name="connectionString"></param>
        public ConnectionString(string connectionString)
        {
            _value = connectionString;

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = _value;

            ServerName = (string)dbConnectionStringBuilder["Server"];
            DatabaseName = (string)dbConnectionStringBuilder["Database"];
        }

        public static implicit operator ConnectionString(string connectionString)
        {
            return new ConnectionString(connectionString);
        }

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
}
