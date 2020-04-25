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
        bool Update(BankData bankData);
        bool Delete(BankData bankData);
        Task<bool> DeleteAsync(int bankDataID);
        Task<bool> saveAsync();
    }
}
