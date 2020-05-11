using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ISponsorTransactionRepository
    {
        Task<IEnumerable<SponsorTransaction>> GetAllAsync();
        Task<SponsorTransaction> GetByIdAsync(int sponsorTransactionsID);
        Task<SponsorTransaction> GetAsync(SponsorTransaction sponsorTransaction);
        Task<IEnumerable<SponsorTransaction>> GetAllBySponsorIdAsync(int sponsorID);
        Task<bool> InsertAsync(SponsorTransaction sponsorTransaction);
        Task<bool> UpdateAsync(SponsorTransaction sponsorTransaction);
        Task<bool> DeleteAsync(SponsorTransaction sponsorTransaction);
        Task<bool> DeleteAsync(int sponsorTransactionsID);
        Task<bool> IsExistAsync(SponsorTransaction sponsorTransaction);
        Task<bool> saveAsync();
        void Dispose();
    }
}
