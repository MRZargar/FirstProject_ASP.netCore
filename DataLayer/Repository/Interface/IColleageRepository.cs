using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IColleageRepository
    {
        Task<IEnumerable<Colleague>> GetAllAsync();
        Task<Colleague> GetByIdAsync(int colleagueID);
        Task<bool> InsertAsync(Colleague colleague);
        Task<bool> UpdateAsync(Colleague colleague);
        Task<bool> DeleteAsync(Colleague colleague);
        Task<bool> DeleteAsync(int colleagueID);
        Task<bool> saveAsync();
    }
}
