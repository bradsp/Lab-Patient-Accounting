using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using PetaPoco.Providers;

namespace LabBilling.Core.DataAccess
{
    public abstract class RepositoryBase<T> where T : IBaseEntity
    {
        protected PetaPoco.Database dbConnection = null; 
        protected readonly string _tableName;
        protected IList<string> _fields;

        public RepositoryBase(string tableName, string connectionString)
        {
            Log.Instance.Trace("Entering");
            _tableName = tableName;
            //dbConnection = new SqlConnection(Helper.ConnVal());
            dbConnection = new PetaPoco.Database(connectionString, new CustomSqlServerDatabaseProvider());

            //_fields = new List<string>();

            //Type type = typeof(T);
            //foreach (PropertyInfo propertyInfo in type.GetProperties())
            //{
            //    // Get name and add to field list. 
            //    _fields.Add(propertyInfo.Name.ToString());
            //}
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

            if (table.mod_date == null)
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
            dbConnection.Update(table);
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
    }
}
