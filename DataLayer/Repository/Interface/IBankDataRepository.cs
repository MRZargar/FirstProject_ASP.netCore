using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static DataLayer.BankDataRepository;

namespace DataLayer
{
    public interface IBankDataRepository
    {
        Task<IEnumerable<BankData>> GetAllAsync();
        Task<BankData> GetByIdAsync(int bankDataID);
        Task<BankData> GetAsync(BankData bankData);
        IEnumerable<BankData> GetAllByBankName(BankName bankname);
        Task<bool> InsertAsync(BankData bankData);
        Task<bool> UpdateAsync(BankData bankData);
        Task<bool> DeleteAsync(BankData bankData);
        Task<bool> DeleteAsync(int bankDataID);
        Task<bool> IsExistAsync(BankData bankdata);
        Task<bool> saveAsync();
        void Dispose();
    }
}
