using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System.Reflection;
using PetaPoco;
using System.Linq.Expressions;
using Utilities;
using System.ComponentModel;
using LabBilling.Core.UnitOfWork;

namespace LabBilling.Core.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepositoryBase<TPoco> : RepositoryCoreBase<TPoco>, IRepositoryBase<TPoco> where TPoco : class, IBaseEntity
    {
        protected IList<string> _fields;

        /// <summary>
        /// Contains error messages as a result of actions.
        /// </summary>
        public string Errors { get; internal set; }

        public RepositoryBase(IAppEnvironment environment, IDatabase context) : base(environment, context)
        {
        }

        [Category("Action")] public event EventHandler<RepositoryEventArgs<TPoco>> RecordUpdated;
        [Category("Action")] public event EventHandler<RepositoryEventArgs<TPoco>> RecordAdded;
        [Category("Action")] public event EventHandler<RepositoryEventArgs<TPoco>> RecordDeleted;

        public virtual List<TPoco> GetAll()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = PetaPoco.Sql.Builder
                .From(_tableName);

            var queryResult = Context.Fetch<TPoco>(sql);

            Log.Instance.Debug(Context.LastSQL);
            Log.Instance.Debug(Context.LastArgs);
            return queryResult;
        }

        public virtual async Task<IEnumerable<TPoco>> GetAllAsync()
        {
            Log.Instance.Trace("Entering");

            PetaPoco.Sql sql = Sql.Builder
                .From(_tableName);

            var queryResult = await Context.FetchAsync<TPoco>(sql);

            Log.Instance.Debug(Context.LastSQL);
            Log.Instance.Debug(Context.LastArgs);
            return queryResult.ToList<TPoco>();
        }

        public virtual async Task<object> AddAsync(TPoco table) => await Task.Run(() => Add(table));

        public virtual object Add(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.UpdatedDate = DateTime.Now;
            table.UpdatedHost = Environment.MachineName;
            table.UpdatedApp = Utilities.OS.GetAppName();
            table.UpdatedUser = Environment.UserName.ToString();
            table.rowguid = Guid.NewGuid();
            try
            {
                object identity = Context.Insert(table);
                Log.Instance.Debug(Context.LastSQL.ToString());
                Log.Instance.Debug(Context.LastArgs.ToString());
                RecordAdded?.Invoke(this, new RepositoryEventArgs<TPoco>
                {
                    Record = table,
                    Action = "Add"
                });

                return identity;
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, "Exception encountered in RepositoryBase.Add");
                throw new ApplicationException("Exception encountered in RepositoryBase.Add", ex);
            }
        }

        public virtual bool Update(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.UpdatedDate = DateTime.Now;
            table.UpdatedHost = Environment.MachineName;
            table.UpdatedApp = OS.GetAppName();
            table.UpdatedUser = Environment.UserName.ToString();

            Context.Update(table);
            Log.Instance.Debug(Context.LastSQL.ToString());
            return true;
        }

        public virtual bool Update(TPoco table, IEnumerable<string> columns)
        {
            Log.Instance.Trace("Entering");
            List<string> cColumns = new();

            table.UpdatedDate = DateTime.Now;
            cColumns.Add(GetRealColumn(nameof(table.UpdatedDate)));
            table.UpdatedHost = Environment.MachineName;
            cColumns.Add(GetRealColumn(nameof(table.UpdatedHost)));
            table.UpdatedApp = Utilities.OS.GetAppName();
            cColumns.Add(GetRealColumn(nameof(table.UpdatedApp)));
            table.UpdatedUser = Environment.UserName.ToString();
            cColumns.Add(GetRealColumn(nameof(table.UpdatedUser)));

            foreach (string column in columns)
            {
                if (!cColumns.Contains(column))
                    cColumns.Add(GetRealColumn(column));
            }

            try
            {
                Context.Update(table, cColumns);
            }
            catch (Exception ex)
            {
                Log.Instance.Debug(Context.LastSQL.ToString());
                Log.Instance.Debug(Context.LastArgs.ToString());
                throw new ApplicationException("Error during database update.", ex);
            }

            RecordUpdated?.Invoke(this, new RepositoryEventArgs<TPoco>()
            {
                Record = table,
                Action = "update"
            });
            Log.Instance.Debug(Context.LastSQL.ToString());
            Log.Instance.Debug(Context.LastArgs.ToString());
            return true;
        }

        public virtual bool Save(TPoco table)
        {
            Log.Instance.Trace("Entering");

            table.UpdatedDate = DateTime.Now;
            table.UpdatedHost = Environment.MachineName;
            table.UpdatedApp = OS.GetAppName();
            table.UpdatedUser = Environment.UserName.ToString();
            try
            {
                Context.Save(table);
            }
            catch (Exception ex)
            {
                Log.Instance.Error("Error saving account validation record to database.", ex);
                return false;
            }
            Log.Instance.Debug(Context.LastSQL.ToString());
            Log.Instance.Debug(Context.LastCommand.ToString());

            return true;
        }

        public virtual bool Delete(TPoco table)
        {
            Log.Instance.Trace("Entering");

            var count = Context.Delete(table);
            RecordDeleted?.Invoke(this, new RepositoryEventArgs<TPoco>()
            {
                Record = table,
                Action = "deleted"
            });
            Log.Instance.Debug(Context.LastSQL.ToString());
            Log.Instance.Debug(Context.LastArgs.ToString());
            return count > 0;
        }

        //public virtual void BeginTransaction()
        //{
        //    Log.Instance.Debug("Begin Transaction");
        //    transactionStarted = true;
        //    Context.BeginTransaction();
        //}

        //public virtual void CompleteTransaction()
        //{
        //    Log.Instance.Debug("Complete Transaction");
        //    Context.CompleteTransaction();
        //    transactionStarted = false;
        //}

        //public virtual void AbortTransaction()
        //{
        //    Log.Instance.Debug("Abort Transaction");
        //    Context.AbortTransaction();
        //    transactionStarted = false;
        //}

        public IEnumerable<TPoco> Find(Expression<Func<TPoco, bool>> predicate)
        {

            return null;
        }

    }

    public class RepositoryEventArgs<T> : EventArgs
    {
        public string Action { get; set; }
        public T Record { get; set; }
    }

}
