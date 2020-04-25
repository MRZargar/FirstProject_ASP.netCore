using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ISponsorTransactionRepository
    {
        Task<IEnumerable<SponsorTransaction>> GetAllAsync();
        Task<SponsorTransaction> GetByIdAsync(int sponsorTransactionsID);
        Task<bool> InsertAsync(SponsorTransaction sponsorTransaction);
        bool Update(SponsorTransaction sponsorTransaction);
        bool Delete(SponsorTransaction sponsorTransaction);
        Task<bool> DeleteAsync(int sponsorTransactionsID);
        Task<bool> saveAsync();
    }
}
