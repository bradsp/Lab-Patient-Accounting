using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;
using PetaPoco.Core;


namespace LabBilling.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : IBaseEntity
    {
        protected readonly PetaPoco.Database dbConnection = null;
        protected readonly string _tableName;
        protected IList<string> _fields;
        /// <summary>
        /// Contains error messages as a result of actions.
        /// </summary>
        public string Errors { get; internal set; }

        public RepositoryBase(string tableName, string connectionString)
        {
            Log.Instance.Trace("Entering");
            _tableName = tableName;
            dbConnection = new PetaPoco.Database(connectionString, new CustomSqlDatabaseProvider());

            Log.Instance.Trace("Exiting");
        }

        /// <summary>
        /// Allows passing in an existing database connection to allow for connection pooling.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="connectionString"></param>
        /// <param name="db"></param>
        public RepositoryBase(string tableName, string connectionString, PetaPoco.Database db)
        {
            Log.Instance.Trace("Entering");
            _tableName = tableName;
            dbConnection = db;

            Log.Instance.Trace("Exiting");
        }

        public virtual IEnumerable<T> GetAll()
        {
            Log.Instance.Trace("Entering");

            string sql = $"SELECT * FROM {_tableName}";

            var queryResult = dbConnection.Fetch<T>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            Log.Instance.Trace("Entering");

            string sql = $"SELECT * FROM {_tableName}";

            var queryResult = await dbConnection.FetchAsync<T>(sql);

            Log.Instance.Trace("Exiting");
            return queryResult.ToList<T>();
        }

        public virtual object Add(T table)
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

        public abstract T GetById(int id);

        //public abstract T GetByPrimaryKey();

        public virtual bool Update(T table)
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

        public virtual bool Update(T table, IEnumerable<string> columns)
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

        public virtual bool Save(T table)
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

        public virtual bool Delete(T table)
        {
            Log.Instance.Trace("Entering");

            var count = dbConnection.Delete(table);
            Log.Instance.Trace("Exiting");
            return count > 0;
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
