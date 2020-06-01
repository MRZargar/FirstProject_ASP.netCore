using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataLayer
{
    public class BankRepository : IBankRepository
    {
        private MyContext db;

        public BankRepository(MyContext context)
        {
            db = context;
        }

        public async Task<bool> DeleteAsync(Bank bank)
        {
            if (!(await IsExistAsync(bank)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Banks.Remove(bank);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int bankID)
        {
            try
            {
                var bank = await GetByIdAsync(bankID);
                return await DeleteAsync(bank);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<IEnumerable<Bank>> GetAllAsync()
        {
            try
            {
                return await db.Banks.ToListAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<Bank> GetByIdAsync(int bankID)
        {
            try
            {
                return await db.Banks
                    .Include(x => x.Transactions)
                    .FirstAsync(m => m.BankID == bankID);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertAsync(Bank bank)
        {
            if (await IsExistAsync(bank))
            {
                throw new DuplicatePhoneNumberException();
            }

            try
            {
                await db.Banks.AddAsync(bank);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistAsync(int BankID)
        {
            try
            {
                Bank c = await GetByIdAsync(BankID);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistAsync(string shebaNumber)
        {
            try
            {
                Bank c = await GetByShebaNumberAsync(shebaNumber);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistAsync(Bank bank)
        {
            return await IsExistAsync(bank.ShebaNumber) || await IsExistAsync(bank.BankID);
        }

        public async Task<Bank> GetByShebaNumberAsync(string shebaNmber)
        {
            try
            {
                return await db.Banks
                    .Include(s => s.Transactions)
                    .FirstAsync(m => m.ShebaNumber == shebaNmber);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> saveAsync()
        {
            try
            {
                await db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Bank bank)
        {
            if (!(await IsExistAsync(bank)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Banks.Update(bank);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
