using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface ISponsorRepository
    {
        Task<IEnumerable<Sponsor>> GetAllAsync();
        Task<Sponsor> GetByIdAsync(int sponsorID);
        Task<bool> InsertAsync(Sponsor sponsor);
        Task<bool> UpdateAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(Sponsor sponsor);
        Task<bool> DeleteAsync(int sponsorID);
        Task<bool> saveAsync();
    }
}
