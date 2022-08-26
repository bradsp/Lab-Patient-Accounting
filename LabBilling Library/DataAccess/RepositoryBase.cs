using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using PetaPoco.Core;
using System.Reflection;
using PetaPoco;

namespace LabBilling.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<Tpoco> : IRepositoryBase<Tpoco> where Tpoco : IBaseEntity
    {
        protected readonly PetaPoco.Database dbConnection = null;
        protected readonly string _tableName;
        protected IList<string> _fields;
        protected TableInfo _tableInfo;
        /// <summary>
        /// Contains error messages as a result of actions.
        /// </summary>
        public string Errors { get; internal set; }

        public RepositoryBase(string connectionString)
        {
            Log.Instance.Trace("Entering");
            _tableInfo = GetTableInfo(typeof(Tpoco));
            _tableName = _tableInfo.TableName;
            dbConnection = new PetaPoco.Database(connectionString, new CustomSqlDatabaseProvider());

            Log.Instance.Trace("Exiting");
        }

        public RepositoryBase(Database db)
        {
            Log.Instance.Trace("Entering");
            _tableInfo = GetTableInfo(typeof(Tpoco));
            _tableName = _tableInfo.TableName;
            dbConnection = db;

            Log.Instance.Trace("Exiting");
        }

        public virtual List<Tpoco> GetAll()
        {
            Log.Instance.Trace("Entering");

            string sql = $"SELECT * FROM {_tableName}";

            var queryResult = dbConnection.Fetch<Tpoco>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult;
        }

        public virtual async Task<IEnumerable<Tpoco>> GetAllAsync()
        {
            Log.Instance.Trace("Entering");

            string sql = $"SELECT * FROM {_tableName}";

            var queryResult = await dbConnection.FetchAsync<Tpoco>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult.ToList<Tpoco>();
        }

        public virtual object Add(Tpoco table)
        {
            Log.Instance.Trace("Entering");

            if (table.mod_date == null || table.mod_date == DateTime.MinValue)
                table.mod_date = DateTime.Now;
            if (table.mod_host == "" || table.mod_host == null)
                table.mod_host = Environment.MachineName;
            if (table.mod_prg == "" || table.mod_prg == null)
                table.mod_prg = System.AppDomain.CurrentDomain.FriendlyName;
            if (table.mod_user == "" || table.mod_user == null)
                table.mod_user = Environment.UserName.ToString();
            if (table.rowguid == Guid.Empty)
                table.rowguid = Guid.NewGuid();

            object identity = dbConnection.Insert(table);
            Log.Instance.Trace("Exiting");
            return identity;
        }

        public abstract Tpoco GetById(int id);

        //public abstract Tpoco GetByPrimaryKey();

        public virtual bool Update(Tpoco table)
        {
            Log.Instance.Trace("Entering");

            if (table.mod_date == null)
                table.mod_date = DateTime.Now;
            if (table.mod_host == "" || table.mod_host == null)
                table.mod_host = Environment.MachineName;
            if (table.mod_prg == "" || table.mod_prg == null)
                table.mod_prg = System.AppDomain.CurrentDomain.FriendlyName;
            if (table.mod_user == "" || table.mod_user == null)
                table.mod_user = Environment.UserName.ToString();

            dbConnection.Update(table);
            Log.Instance.Trace("Exiting");
            return true;
        }

        public virtual bool Update(Tpoco table, IEnumerable<string> columns)
        {
            Log.Instance.Trace("Entering");

            if (table.mod_date == null)
                table.mod_date = DateTime.Now;
            if (table.mod_host == "" || table.mod_host == null)
                table.mod_host = Environment.MachineName;
            if (table.mod_prg == "" || table.mod_prg == null)
                table.mod_prg = System.AppDomain.CurrentDomain.FriendlyName;
            if (table.mod_user == "" || table.mod_user == null)
                table.mod_user = Environment.UserName.ToString();

            List<string> cColumns = new List<string>();
            foreach (string column in columns)
            {
                var pocoData = PocoData.ForType(table.GetType(), dbConnection.DefaultMapper);
                cColumns.Add(pocoData.GetColumnName(column));
            }

            dbConnection.Update(table, cColumns);
            Log.Instance.Trace("Exiting");
            return true;        
        }

        public virtual bool Save(Tpoco table)
        {
            Log.Instance.Trace("Entering");

            if (table.mod_date == null)
                table.mod_date = DateTime.Now;
            if (table.mod_host == "" || table.mod_host == null)
                table.mod_host = Environment.MachineName;
            if (table.mod_prg == "" || table.mod_prg == null)
                table.mod_prg = System.AppDomain.CurrentDomain.FriendlyName;
            if (table.mod_user == "" || table.mod_user == null)
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
            Log.Instance.Trace("Exiting");
            return true;
        }

        public virtual bool Delete(Tpoco table)
        {
            Log.Instance.Trace("Entering");

            var count = dbConnection.Delete(table);
            Log.Instance.Trace("Exiting");
            return count > 0;
        }

        public string GetRealColumn(Type poco, string propertyName)
        {
            return this.GetRealColumn(poco.AssemblyQualifiedName, propertyName);
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
            dbConnection.BeginTransaction();
        }

        public virtual void CompleteTransaction()
        {
            dbConnection.CompleteTransaction();
        }

        public virtual void AbortTransaction()
        {
            dbConnection.AbortTransaction();
        }

    }
}
