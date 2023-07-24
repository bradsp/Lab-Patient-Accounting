using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Reflection;
using PetaPoco;
using System.Linq.Expressions;

namespace LabBilling.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<TPoco> : IRepositoryBase<TPoco> where TPoco : IBaseEntity
    {
        protected PetaPoco.IDatabase dbConnection = null;
        protected string _tableName;
        protected IList<string> _fields;
        protected TableInfo _tableInfo;
        protected bool transactionStarted = false;
        /// <summary>
        /// Contains error messages as a result of actions.
        /// </summary>
        public string Errors { get; internal set; }
        protected IAppEnvironment _appEnvironment { get; set; }

        public RepositoryBase(IAppEnvironment environment)
        {
            Log.Instance.Trace("Entering");
            if (!environment.EnvironmentValid)
                throw new ApplicationException("AppEnvironment not valid.");
            
            _appEnvironment = environment;
            Initialize();
            
        }

        private void Initialize()
        {
            Log.Instance.Trace("Entering");
            _tableInfo = GetTableInfo(typeof(TPoco));
            _tableName = _tableInfo.TableName;
            dbConnection = _appEnvironment.Database;
        }

        public virtual List<TPoco> GetAll()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName);

            var queryResult = dbConnection.Fetch<TPoco>(sql);

            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return queryResult;
        }

        public virtual async Task<IEnumerable<TPoco>> GetAllAsync()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName);

            var queryResult = await dbConnection.FetchAsync<TPoco>(sql);

            Log.Instance.Debug(dbConnection.LastSQL);
            Log.Instance.Debug(dbConnection.LastArgs);
            return queryResult.ToList<TPoco>();
        }

        public virtual object Add(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.mod_date = DateTime.Now;
            table.mod_host = Environment.MachineName;
            table.mod_prg = RFClassLibrary.OS.GetAppName();
            table.mod_user = Environment.UserName.ToString();
            table.rowguid = Guid.NewGuid();

            object identity = dbConnection.Insert(table);
            Log.Instance.Debug(dbConnection.LastSQL.ToString());
            Log.Instance.Debug(dbConnection.LastArgs.ToString());

            return identity;
        }

        public virtual bool Update(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.mod_date = DateTime.Now;
            table.mod_host = Environment.MachineName;
            table.mod_prg = RFClassLibrary.OS.GetAppName();
            table.mod_user = Environment.UserName.ToString();

            dbConnection.Update(table);
            Log.Instance.Debug(dbConnection.LastSQL.ToString());
            return true;
        }

        public virtual bool Update(TPoco table, IEnumerable<string> columns)
        {
            Log.Instance.Trace("Entering");
            List<string> cColumns = new List<string>();

            table.mod_date = DateTime.Now;
            cColumns.Add(nameof(table.mod_date));
            table.mod_host = Environment.MachineName;
            cColumns.Add(nameof(table.mod_host));
            table.mod_prg = RFClassLibrary.OS.GetAppName();
            cColumns.Add(nameof(table.mod_prg));
            table.mod_user = Environment.UserName.ToString();
            cColumns.Add(nameof(table.mod_user));

            foreach (string column in columns)
            {
                if(!cColumns.Contains(column))
                    cColumns.Add(GetRealColumn(column));
            }

            try
            {
                dbConnection.Update(table, cColumns);
            }
            catch(Exception ex)
            {
                Log.Instance.Debug(dbConnection.LastSQL.ToString());
                Log.Instance.Debug(dbConnection.LastArgs.ToString());
                throw new ApplicationException("Error during database update.", ex);
            }

            Log.Instance.Debug(dbConnection.LastSQL.ToString());
            Log.Instance.Debug(dbConnection.LastArgs.ToString());
            return true;        
        }

        public virtual bool Save(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.mod_date = DateTime.Now;
            table.mod_host = Environment.MachineName;
            table.mod_prg = RFClassLibrary.OS.GetAppName();
            table.mod_user = Environment.UserName.ToString();
            try
            {
                dbConnection.Save(table);
            }
            catch(Exception ex)
            {
                Log.Instance.Error("Error saving account validation record to database.", ex);
                return false;
            }
            Log.Instance.Debug(dbConnection.LastSQL.ToString());
            Log.Instance.Debug(dbConnection.LastCommand.ToString());
            return true;
        }

        public virtual bool Delete(TPoco table)
        {
            Log.Instance.Trace("Entering");

            var count = dbConnection.Delete(table);
            Log.Instance.Debug(dbConnection.LastSQL.ToString());
            Log.Instance.Debug(dbConnection.LastArgs.ToString());
            return count > 0;
        }

        public string GetRealColumn(Type poco, string propertyName)
        {
            return this.GetRealColumn(poco.AssemblyQualifiedName, propertyName);
        }

        public string GetRealColumn(string propertyName)
        {
            return GetRealColumn(typeof(TPoco), propertyName);
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
                foreach(ColumnAttribute attribute in attributes)
                {
                    if(attribute.GetType() != typeof(ResultColumnAttribute))
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
            Log.Instance.Debug("Begin Transaction");
            transactionStarted = true;
            dbConnection.BeginTransaction();
        }

        public virtual void CompleteTransaction()
        {
            Log.Instance.Debug("Complete Transaction");
            dbConnection.CompleteTransaction();
            transactionStarted = false;
        }

        public virtual void AbortTransaction()
        {
            Log.Instance.Debug("Abort Transaction");
            dbConnection.AbortTransaction();
            transactionStarted = false;
        }

        public IEnumerable<TPoco> Find(Expression<Func<TPoco, bool>> predicate)
        {

            return null;
        }

    }
}
