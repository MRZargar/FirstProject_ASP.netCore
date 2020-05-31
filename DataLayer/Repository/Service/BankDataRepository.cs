using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer
{
    public class BankDataRepository : IBankDataRepository
    {
        private MyContext db;

        public BankDataRepository(MyContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<BankData>> GetAllAsync()
        {
            try
            {
                return await db.BankDatas.ToListAsync(); 
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public async Task<BankData> GetByIdAsync(int bankDataID)
        {
            try
            {
                return await db.BankDatas
                    .FirstAsync(m => m.BankDataID == bankDataID);   
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<BankData> GetAsync(BankData bankData)
        {
            try
            {
                return await db.BankDatas
                    .FirstAsync(x => x.BankID == bankData.BankID
                                  && x.TransactionDate == bankData.TransactionDate
                                  && x.TrackingNumber == bankData.TrackingNumber);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public IEnumerable<BankData> GetAllByBankName(Bank bank)
        {
            try
            {
                return db.BankDatas
                       .Where(x => x.MyBank == bank);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(BankData bankData)
        {
            if (await IsExistAsync(bankData))
            {
                throw new DuplicateTransactionException();
            }

            try
            {
                await db.BankDatas.AddAsync(bankData);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(BankData bankData)
        {
            if (!(await IsExistAsync(bankData)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.BankDatas.Update(bankData);
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(BankData bankData)
        {
            if (!(await IsExistAsync(bankData)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.BankDatas.Remove(bankData);
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int bankDataID)
        {
            try
            {
                var bankData = await GetByIdAsync(bankDataID);
                return await DeleteAsync(bankData);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> saveAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<bool> IsExistAsync(BankData bankData)
        {
            try
            {
                BankData b = await GetAsync(bankData);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }
    }
}
