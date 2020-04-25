using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class BankDataRepository : IBankDataRepository
    {
        private MyContext db;

        public BankDataRepository(MyContext context)
        {
            this.db = context;
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
                    .FirstOrDefaultAsync(m => m.BankDataID == bankDataID);   
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(BankData bankData)
        {
            try
            {
                db.BankDatas.Add(bankData);     
                await saveAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(BankData bankData)
        {
            try
            {
                db.BankDatas.Update(bankData);
                await saveAsync();

                return true;    
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(BankData bankData)
        {
            try
            {
                db.BankDatas.Remove(bankData);
                await saveAsync();

                return true;    
            }
            catch (System.Exception)
            {
                return false;
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
                return false;
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
                return false;
            }
        }

    }
}
