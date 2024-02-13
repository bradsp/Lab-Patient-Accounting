using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public interface IAccountRepository
    {
        object Add(Account table);
        void AddAccount(Account acc);
        Task AddAccountAsync(Account acc);
        Task<object> AddAsync(Account table);
        Account GetByAccount(string account, bool demographicsOnly = false);
        Task<Account> GetByAccountAsync(string account, bool demographicsOnly = false);
        bool Update(Account table);
        bool Update(Account table, IEnumerable<string> columns);
        Task<bool> UpdateAsync(Account table);
        int UpdateStatus(string accountNo, string status);
        Task<int> UpdateStatusAsync(string accountNo, string status);
    }
}