using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LabBilling.Core.DataAccess
{
    public interface IAccountRepository
    {
        Account Add(Account table);
        Task<Account> AddAsync(Account table);
        Account GetByAccount(string account);
        Task<Account> GetByAccountAsync(string account);
        Account Update(Account table);        
        Account Update(Account table, IEnumerable<string> columns);
        Task<Account> UpdateAsync(Account table);
        int UpdateStatus(string accountNo, string status);
        Task<int> UpdateStatusAsync(string accountNo, string status);
    }
}