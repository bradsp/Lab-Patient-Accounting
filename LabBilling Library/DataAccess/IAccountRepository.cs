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
        int AddCharge(Account accData, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null);
        int AddCharge(string account, string cdm, int qty, DateTime serviceDate, string comment = null, string refNumber = null);
        bool AddNote(string account, string noteText);
        void BundlePanels(Account account);
        void BundlePanels(string accountNo);
        bool ChangeClient(ref Account table, string newClientMnem);
        bool ChangeDateOfService(ref Account table, DateTime newDate, string reason_comment);
        bool ChangeFinancialClass(ref Account table, string newFinCode);
        bool ChangeFinancialClass(string account, string newFinCode);
        void ClearClaimStatus(Account account);
        IEnumerable<ClaimItem> GetAccountsForClaims(AccountRepository.ClaimType claimType, int maxClaims = 0);
        Account GetByAccount(string account, bool demographicsOnly = false);
        IEnumerable<InvoiceSelect> GetInvoiceAccounts(string clientMnem, DateTime thruDate);
        bool InsuranceSwap(string accountNo, InsCoverage swap1, InsCoverage swap2);
        void MoveCharge(string sourceAccount, string destinationAccount, int chrgId);
        (bool isSuccess, string error) MoveCharges(string sourceAccount, string destinationAccount);
        int ReprocessCharges(Account account, string comment);
        int ReprocessCharges(string account, string comment);
        bool SetNoteAlert(string account, bool showAlert);
        bool Update(Account table);
        bool Update(Account table, IEnumerable<string> columns);
        bool UpdateDiagnoses(Account acc);
        int UpdateStatus(string accountNo, string status);
        bool Validate(ref Account account, bool reprint = false);
        void ValidateUnbilledAccounts();
        Task ValidateUnbilledAccountsAsync();
    }
}