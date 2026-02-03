using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NPOI.XWPF.UserModel;
using PetaPoco.Core;
using PetaPoco.Providers;
using PetaPoco;
using LabBilling.Core.Models;
using LabBilling.Logging;
using System.Data;
using Microsoft.Data.SqlClient;

namespace LabBilling.Core.DataAccess
{
    public class LogRepository
    {
        protected readonly PetaPoco.Database dbConnection = null;
        protected readonly string _tableName;
        protected IList<string> _fields;
        protected TableInfo _tableInfo;
        /// <summary>
        /// Contains error messages as a result of actions.
        /// </summary>
        public string Errors { get; internal set; }

        public LogRepository(string connectionString)
        {
            Log.Instance.Trace("Entering");
            _tableInfo = GetTableInfo(typeof(Logs));
            _tableName = _tableInfo.TableName;
            // LogRepository connects to a separate NLog logging database (not the main LabBilling database).
            // It intentionally uses SqlServerMsDataDatabaseProvider instead of CustomSqlMsDatabaseProvider
            // because the CustomSqlMsDatabaseProvider workaround for SQL Error 334 is only needed for
            // the main application tables. MyMapper is also not required since Logs columns map directly
            // without custom attribute resolution.
            dbConnection = new PetaPoco.Database(connectionString, new SqlServerMsDataDatabaseProvider());
            Log.Instance.Debug(dbConnection.ConnectionString);
        }

        public List<Logs> GetAll()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName);

            var queryResult = dbConnection.Fetch<Logs>(sql);

            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return queryResult;
        }

        public List<Logs> GetDateRange(DateTime fromDate, DateTime thruDate)
        {
            Log.Instance.Trace("Entering");

            var sql = Sql.Builder
                .From(_tableName)
                .Where($"{nameof(Logs.CreatedOn)} between @0 and @1",
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = fromDate },
                    new SqlParameter() { SqlDbType = SqlDbType.DateTime, Value = thruDate });

            var queryResult = dbConnection.Fetch<Logs>(sql);

            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);

            return queryResult.ToList<Logs>();
        }

        public async Task<IEnumerable<Logs>> GetAllAsync()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName);

            var queryResult = await dbConnection.FetchAsync<Logs>(sql);

            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return queryResult.ToList<Logs>();
        }

        public object Add(Logs table)
        {
            Log.Instance.Trace("Entering");

            object identity = dbConnection.Insert(table);
            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return identity;
        }

        public bool Update(Logs table)
        {
            Log.Instance.Trace("Entering");

            dbConnection.Update(table);
            Log.Instance.Debug(dbConnection.LastSQL);
            return true;
        }

        public bool Update(Logs table, IEnumerable<string> columns)
        {
            Log.Instance.Trace("Entering");
            List<string> cColumns = new List<string>();

            foreach (string column in columns)
            {
                var pocoData = PocoData.ForType(table.GetType(), dbConnection.DefaultMapper);
                cColumns.Add(pocoData.GetColumnName(column));
            }

            dbConnection.Update(table, cColumns);
            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastCommand);
            return true;
        }

        public bool Save(Logs table)
        {
            Log.Instance.Trace("Entering");

            try
            {
                dbConnection.Save(table);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error saving record to database.", ex);
                return false;
            }
            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastCommand);
            return true;
        }

        public bool Delete(Logs table)
        {
            Log.Instance.Trace("Entering");

            var count = dbConnection.Delete(table);
            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return count > 0;
        }

        public string GetRealColumn(Type poco, string propertyName)
        {
            return this.GetRealColumn(poco.AssemblyQualifiedName, propertyName);
        }

        public string GetRealColumn(string propertyName)
        {
            return GetRealColumn(typeof(Logs), propertyName);
        }

        public string GetRealColumn(string objectName, string propertyName)
        {
            //this can throw if invalid type names are used, or return null of there is no such type
            Type t = Type.GetType(objectName);
            if (t == null)
                return null;
            //this will only find public instance properties, or return null if no such property is found
            PropertyInfo pi = t.GetProperty(propertyName);
            //this returns an array of the applied attributes (will be 0-length if no attributes are applied
            var attributes = pi.GetCustomAttributes(typeof(ColumnAttribute), false).ToArray();

            if (attributes.Length == 0)
                return propertyName;
            else
            {
                foreach (ColumnAttribute attribute in attributes)
                {
                    if (attribute.GetType() != typeof(ResultColumnAttribute))
                    {
                        return attribute.Name;
                    }
                }
            }

            return propertyName;
        }

        public static TableInfo GetTableInfo(Type t)
        {
            TableInfo tableInfo = new TableInfo();
            object[] customAttributes = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            tableInfo.TableName = (customAttributes.Length == 0) ? t.Name : (customAttributes[0] as TableNameAttribute).Value;
            customAttributes = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            tableInfo.PrimaryKey = (customAttributes.Length == 0) ? "ID" : (customAttributes[0] as PrimaryKeyAttribute).Value;
            tableInfo.SequenceName = (customAttributes.Length == 0) ? null : (customAttributes[0] as PrimaryKeyAttribute).SequenceName;
            tableInfo.AutoIncrement = customAttributes.Length != 0 && (customAttributes[0] as PrimaryKeyAttribute).AutoIncrement;
            return tableInfo;
        }

        public virtual void BeginTransaction()
        {
            BeginTransaction();
        }

        public virtual void CompleteTransaction()
        {
            CompleteTransaction();
        }
        public virtual void AbortTransaction()
        {
            AbortTransaction();
        }

        public IEnumerable<Logs> Find(Expression<Func<Logs, bool>> predicate)
        {

            return null;
        }
    }
}
