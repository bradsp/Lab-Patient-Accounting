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
        int AddCharge(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00);
        int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00);
        Task<int> AddChargeAsync(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00);
        Task<int> AddChargeAsync(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null, double miscAmount = 0.00);
        bool AddNote(string account, string noteText);
        Task<bool> AddNoteAsync(string account, string noteText);
        void BundlePanels(Account account);
        void BundlePanels(string accountNo);
        Task BundlePanelsAsync(Account account);
        bool ChangeClient(Account table, string newClientMnem);
        Task<bool> ChangeClientAsync(Account table, string newClientMnem);
        bool ChangeDateOfService(Account table, DateTime newDate, string reason_comment);
        Task<bool> ChangeDateOfServiceAsync(Account table, DateTime newDate, string reason_comment);
        bool ChangeFinancialClass(Account table, string newFinCode);
        bool ChangeFinancialClass(string account, string newFinCode);
        Task<bool> ChangeFinancialClassAsync(string account, string newFinCode);
        Task<bool> ChangeFinancialClassAsync(Account table, string newFinCode);
        void ClearClaimStatus(Account account);
        Task ClearClaimStatusAsync(Account account);
        IEnumerable<ClaimItem> GetAccountsForClaims(AccountRepository.ClaimType claimType, int maxClaims = 0);
        Task<IEnumerable<ClaimItem>> GetAccountsForClaimsAsync(AccountRepository.ClaimType claimType, int maxClaims = 0);
        Account GetByAccount(string account, bool demographicsOnly = false);
        Task<Account> GetByAccountAsync(string account, bool demographicsOnly = false);
        IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate);
        Task<IEnumerable<InvoiceSelect>> GetInvoiceAccountsAsync(string clientMnem, DateTime thruDate);
        bool InsuranceSwap(string accountNo, InsCoverage swap1, InsCoverage swap2);
        Task<bool> InsuranceSwapAsync(string accountNo, InsCoverage swap1, InsCoverage swap2);
        void MoveCharge(string sourceAccount, string destinationAccount, int chrgId);
        Task MoveChargeAsync(string sourceAccount, string destinationAccount, int chrgId);
        (bool isSuccess, string error) MoveCharges(string sourceAccount, string destinationAccount);
        int ReprocessCharges(Account account, string comment);
        int ReprocessCharges(string account, string comment);
        Task<int> ReprocessChargesAsync(Account account, string comment);
        Task<int> ReprocessChargesAsync(string account, string comment);
        bool SetNoteAlert(string account, bool showAlert);
        Task<bool> SetNoteAlertAsync(string account, bool showAlert);
        Task UnbundlePanelsAsync(Account account);
        bool Update(Account table);
        bool Update(Account table, IEnumerable<string> columns);
        Task<bool> UpdateAsync(Account table);
        Task<bool> UpdateChargesClientAsync(string account, string clientMnem);
        Task<bool> UpdateChargesFinCodeAsync(string account, string finCode);
        bool UpdateDiagnoses(Account acc);
        Task<bool> UpdateDiagnosesAsync(Account acc);
        int UpdateStatus(string accountNo, string status);
        Task<int> UpdateStatusAsync(string accountNo, string status);
        bool Validate(Account account, bool reprint = false);
        Task<bool> ValidateAsync(Account account, bool reprint = false);
        void ValidateUnbilledAccounts();
        Task ValidateUnbilledAccountsAsync();
    }
}