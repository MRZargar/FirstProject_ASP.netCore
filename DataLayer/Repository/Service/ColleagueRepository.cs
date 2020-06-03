using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataLayer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class ColleagueRepository : IColleageRepository
    {
        private MyContext db;

        public ColleagueRepository(MyContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<Colleague>> GetAllAsync()
        {
            try
            {
                return await db.Colleagues.ToListAsync(); 
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<Colleague> GetByIdAsync(int colleagueID)
        {
            try
            {
                return await db.Colleagues
                    .Include(s => s.Sponsors)
                    .FirstAsync(m => m.ColleagueID == colleagueID);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<Colleague> GetByPhoneNumberAsync(long phoneNmber)
        {
            try
            {
                return await db.Colleagues
                    .Include(s => s.Sponsors)
                    .FirstAsync(m => m.PhoneNumber == phoneNmber);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertAsync(Colleague colleague)
        {
            if (await IsExistAsync(colleague))
            {
                throw new DuplicatePhoneNumberException();
            }

            try
            {
                await db.Colleagues.AddAsync(colleague);     
                return true;
            }
            catch (Exception)
            {
                throw;   
            }
        }

        public async Task<bool> UpdateAsync(Colleague colleague)
        {
            if (!(await IsExistAsync(colleague)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Colleagues.Update(colleague);
                return true;    
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Colleague colleague)
        {
            if (!(await IsExistAsync(colleague)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Colleagues.Remove(colleague);
                return true;    
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int colleagueID)
        {
            try
            {
                var colleague = await GetByIdAsync(colleagueID);
                return await DeleteAsync(colleague);
            }
            catch (Exception)
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
            catch (Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<bool> IsExistAsync(int ColleageID)
        {
            try
            {
                Colleague c = await GetByIdAsync(ColleageID);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistAsync(Colleague colleague)
        {
            return await IsExistAsync(colleague.PhoneNumber) || await IsExistAsync(colleague.ColleagueID);
        }

        public async Task<bool> IsExistAsync(long phoneNumber)
        {
            try
            {
                Colleague c = await GetByPhoneNumberAsync(phoneNumber);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public int Count()
        {
            return db.Colleagues.Count();
        }

        public async Task<double> GetSumOfAmountsAsync(int colleagueID)
        {
            try
            {
                return await db.SponsorTransactions
                        .Where(x => x.ColleagueID == colleagueID)
                        .SumAsync(x => x.MyTransaction.Amount + x.MyReceipt.Amount);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<double> GetSumOfAmountsAsync(Colleague colleague)
        {
            if (!await IsExistAsync(colleague))
            {
                throw new NotFoundException();
            }

            try
            {
                return await GetSumOfAmountsAsync(colleague.ColleagueID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
