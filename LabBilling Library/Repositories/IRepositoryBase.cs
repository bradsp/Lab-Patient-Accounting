using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace LabBilling.Core.DataAccess
{
    public interface IRepositoryBase<TPoco> where TPoco : IBaseEntity
    {
        string Errors { get; }
        TPoco Add(TPoco table);
        bool Delete(TPoco table);
        List<TPoco> GetAll();
        Task<IEnumerable<TPoco>> GetAllAsync();
        TPoco Update(TPoco table);
        TPoco Update(TPoco table, IEnumerable<string> columns);
        IEnumerable<TPoco> Find(Expression<Func<TPoco, bool>> predicate);
    }
}