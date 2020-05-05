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
        IEnumerable<BankData> GetAllByBankName(BankName bankname);
        Task<bool> InsertAsync(BankData bankData);
        bool Update(BankData bankData);
        bool Delete(BankData bankData);
        Task<bool> DeleteAsync(int bankDataID);
        Task<bool> saveAsync();
        void Dispose();
    }
}
