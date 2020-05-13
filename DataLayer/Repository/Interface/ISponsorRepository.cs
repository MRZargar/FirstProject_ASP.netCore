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
        Task<decimal> GetSumOfAmountsAsync(int sponsorID);
        Task<decimal> GetSumOfAmountsAsync(Sponsor sponsor);
        Task<bool> InsertAsync(Sponsor sponsor);
        Task<bool> UpdateAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(int sponsorID);
        Task<bool> saveAsync();
        void Dispose();
        Task<bool> IsExistAsync(int sponsorID);
        Task<bool> IsExistAsync(Sponsor sponsor);
        Task<bool> IsExistAsync(long phoneNumber);
    }
}
