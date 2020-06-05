using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ISponsorRepository
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor> GetByIdAsync(int sponsorID);
        int Count();
        Task<Sponsor> GetByPhoneNumberAsync(long phoneNumber);
        Task<double> GetSumOfAmountsAsync(int sponsorID);
        Task<double> GetSumOfAmountsAsync(Sponsor sponsor);
        Task<bool> InsertAsync(Sponsor sponsor);
        Task<bool> UpdateAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(int sponsorID);
        Task<bool> saveAsync();
        void Dispose();
        Task<bool> IsExistAsync(int sponsorID);
        Task<bool> IsExistAsync(Sponsor sponsor);
        Task<bool> IsExistAsync(long phoneNumber);

        Task<IEnumerable<SponsorTransaction>> GetAllTransactionAsync();
        Task<SponsorTransaction> GetTransactionByIdAsync(int sponsorTransactionsID);
        Task<SponsorTransaction> GetTransactionAsync(SponsorTransaction sponsorTransaction);
        Task<IEnumerable<SponsorTransaction>> GetAllTransactionBySponsorIdAsync(int sponsorID);
        Task<IEnumerable<SponsorTransaction>> GetFromToTransactionBySponsorIdAsync(int sponsorID, DateTime From, DateTime To);
        Task<IEnumerable<SponsorTransaction>> GetFromToTransactionAsync(DateTime From, DateTime To);
        Task<bool> InsertTransactionAsync(SponsorTransaction sponsorTransaction);
        Task<bool> UpdateTransactionAsync(SponsorTransaction sponsorTransaction);
        Task<bool> DeleteTransactionAsync(SponsorTransaction sponsorTransaction);
        Task<bool> DeleteTransactionAsync(int sponsorTransactionsID);
        Task<bool> IsExistTransactionAsync(SponsorTransaction sponsorTransaction);
    }
}
