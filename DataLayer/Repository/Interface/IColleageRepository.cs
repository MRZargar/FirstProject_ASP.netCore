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
        bool Update(Colleague colleague);
        bool Delete(Colleague colleague);
        Task<bool> DeleteAsync(int colleagueID);
        Task<bool> saveAsync();
        void Dispose();
    }
}
