using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IBankDataRepository
    {
        Task<IEnumerable<BankData>> GetAllAsync();
        Task<BankData> GetByIdAsync(int bankDataID);
        Task<bool> InsertAsync(BankData bankData);
        Task<bool> UpdateAsync(BankData bankData);
        Task<bool> DeleteAsync(BankData bankData);
        Task<bool> DeleteAsync(int bankDataID);
        Task<bool> saveAsync();
    }
}
