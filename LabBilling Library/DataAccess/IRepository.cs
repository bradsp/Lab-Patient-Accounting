using LabBilling.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public interface IRepositoryBase<Tpoco> where Tpoco : IBaseEntity
    {
        string Errors { get; }

        void AbortTransaction();
        object Add(Tpoco table);
        void BeginTransaction();
        void CompleteTransaction();
        bool Delete(Tpoco table);
        List<Tpoco> GetAll();
        Task<IEnumerable<Tpoco>> GetAllAsync();
        Tpoco GetById(int id);
        bool Update(Tpoco table);
        bool Update(Tpoco table, IEnumerable<string> columns);
    }
}