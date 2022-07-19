using LabBilling.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public interface IRepositoryBase<T> where T : IBaseEntity
    {
        string Errors { get; }

        void AbortTransaction();
        object Add(T table);
        void BeginTransaction();
        void CompleteTransaction();
        bool Delete(T table);
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        T GetById(int id);
        bool Update(T table);
        bool Update(T table, IEnumerable<string> columns);
    }
}