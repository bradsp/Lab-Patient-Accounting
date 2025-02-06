using LabBilling.Core.UnitOfWork;
using PetaPoco;
using System;
using LabBilling.Logging;
using System.Reflection;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Hosting;

namespace LabBilling.Core.DataAccess
{
    public abstract class RepositoryCoreBase<TPoco> where TPoco : class
    {
        protected IAppEnvironment AppEnvironment { get; set; }
        protected readonly PetaPoco.Database Context;
        protected bool transactionStarted = false;
        protected TableInfo _tableInfo;
        protected string _tableName;

        public TableInfo TableInfo => _tableInfo;
        public string TableName => _tableName;

        protected virtual bool RequireValidEnvironment => true;

        private void DbConnection_ConnectionOpened(object sender, DbConnectionEventArgs e)
        {
            //Log.Instance.Trace($"Connected to Database {e.Connection.Database}");
        }

        private void DbConnection_ExceptionThrown(object sender, ExceptionEventArgs e)
        {
            throw new ApplicationException("Error with database connection", e.Exception);
        }

        public RepositoryCoreBase(IAppEnvironment environment, IDatabase context)
        {
            Log.Instance.Trace("Entering");


            AppEnvironment = environment;
            Context = (PetaPoco.Database)context;
            Initialize();
        }

        private void Initialize()
        {
            Log.Instance.Trace("Entering");
            if (RequireValidEnvironment && !AppEnvironment.EnvironmentValid)
                throw new InvalidOperationException("AppEnvironment not valid.");
            _tableInfo = GetTableInfo(typeof(TPoco));
            _tableName = _tableInfo.TableName;
        }

        public static TableInfo GetTableInfo(Type t)
        {
            TableInfo tableInfo = new();
            object[] customAttributes = t.GetCustomAttributes(typeof(TableNameAttribute), true);
            tableInfo.TableName = (customAttributes.Length == 0) ? t.Name : (customAttributes[0] as TableNameAttribute).Value;
            customAttributes = t.GetCustomAttributes(typeof(PrimaryKeyAttribute), true);
            tableInfo.PrimaryKey = (customAttributes.Length == 0) ? "ID" : (customAttributes[0] as PrimaryKeyAttribute).Value;
            tableInfo.SequenceName = (customAttributes.Length == 0) ? null : (customAttributes[0] as PrimaryKeyAttribute).SequenceName;
            tableInfo.AutoIncrement = customAttributes.Length != 0 && (customAttributes[0] as PrimaryKeyAttribute).AutoIncrement;
            return tableInfo;
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
                foreach (ColumnAttribute attribute in attributes.Cast<ColumnAttribute>())
                {
                    if (attribute.GetType() != typeof(ResultColumnAttribute))
                    {
                        return attribute.Name;
                    }
                }
            }

            return propertyName;
        }

    }
}
