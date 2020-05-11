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
        public class BankName
        {
            private readonly string Name;
            private readonly string Image;

            public static readonly BankName MELLI = new BankName("Melli", "Melli.png");
            public static readonly BankName MELLAT = new BankName("Mellat", "Mellat.png");
            public static readonly BankName SADERAT = new BankName("Saderat", "Saderat.png");

            private BankName(string name, string image)
            {
                Name = name;
                Image = image;
            }

            public override string ToString()
            {
                return Name;
            }

            public string image()
            {
                return Image;
            }
        }

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
                    .FirstAsync(x => x.BankName == bankData.BankName
                                  && x.TransactionDate == bankData.TransactionDate
                                  && x.TrackingNumber == bankData.TrackingNumber);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public IEnumerable<BankData> GetAllByBankName(BankName bankname)
        {
            try
            {
                return db.BankDatas
                       .Where(x => x.BankName == bankname.ToString());
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
