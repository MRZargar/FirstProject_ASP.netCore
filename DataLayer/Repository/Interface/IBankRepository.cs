using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAllAsync();
        Task<Bank> GetByIdAsync(int bankID);
        Task<bool> InsertAsync(Bank bank);
        Task<bool> UpdateAsync(Bank bank);
        Task<bool> DeleteAsync(Bank bank);
        Task<bool> DeleteAsync(int bankID);
        Task<bool> IsExistAsync(Bank bank);
        Task<bool> saveAsync();
        void Dispose();

        Task<IEnumerable<BankTransaction>> GetAllTransactionAsync();
        Task<BankTransaction> GetTransactionByIdAsync(int bankTransactionID);
        Task<BankTransaction> GetTransactionAsync(BankTransaction bankTransaction);
        Task<IEnumerable<BankTransaction>> GetAllTransactionByBankIdAsync(int bankID);
        Task<IEnumerable<BankTransaction>> GetFromToTransactionByBankIdAsync(int bankID, DateTime From, DateTime To);
        Task<bool> InsertTransactionAsync(BankTransaction bankTransaction);
        Task<bool> UpdateTransactionAsync(BankTransaction bankTransaction);
        Task<bool> DeleteTransactionAsync(BankTransaction bankTransaction);
        Task<bool> DeleteTransactionAsync(int bankTransactionID);
        Task<bool> IsExistTransactionAsync(BankTransaction bankTransaction);
        Task<bool> IsExistTransactionAsync(SponsorTransaction st);
    }
}
