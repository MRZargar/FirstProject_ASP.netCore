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
    }
}
