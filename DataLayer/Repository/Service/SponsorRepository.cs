using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class SponsorRepository : ISponsorRepository
    {
        private MyContext db;

        public SponsorRepository(MyContext context)
        {
            db = context;
        }
        
        public async Task<IEnumerable<Sponsor>> GetAllAsync()
        {
            try
            {
                return await db.Sponsors.Include(s => s.MyColleague).ToListAsync(); 
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public async Task<Sponsor> GetByIdAsync(int sponsorID)
        {
            try
            {
                return await db.Sponsors
                    .Include(s => s.MyColleague)
                    .FirstAsync(m => m.SponsorID == sponsorID);   
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertAsync(Sponsor sponsor)
        {
            if (await IsExistAsync(sponsor))
            {
                throw new DuplicatePhoneNumberException();
            }

            try
            {
                await db.Sponsors.AddAsync(sponsor);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(Sponsor sponsor)
        {
            if (!(await IsExistAsync(sponsor)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Sponsors.Update(sponsor);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Sponsor sponsor)
        {
            if (!(await IsExistAsync(sponsor)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Sponsors.Remove(sponsor);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int sponsorID)
        {
            try
            {
                var sponsor = await GetByIdAsync(sponsorID);
                return await DeleteAsync(sponsor);
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
                throw;
            }
        }

        public async Task<Sponsor> GetByPhoneNumberAsync(long phoneNumber)
        {
            try
            {
                return await db.Sponsors
                    .Include(s => s.MyColleague)
                    .FirstAsync(m => m.PhoneNumber == phoneNumber);   
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<bool> IsExistAsync(int sponsorID)
        {
            try
            {
                Sponsor s = await GetByIdAsync(sponsorID);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistAsync(Sponsor sponsor)
        {
            return await IsExistAsync(sponsor.PhoneNumber) || await IsExistAsync(sponsor.ColleagueID);
        }

        public async Task<bool> IsExistAsync(long phoneNumber)
        {
            try
            {
                Sponsor s = await GetByPhoneNumberAsync(phoneNumber);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public int Count()
        {
            return db.Sponsors.Count();
        }

        public async Task<double> GetSumOfAmountsAsync(int sponsorID)
        {
            try
            {
                return await db.SponsorTransactions
                        .Where(x => x.SponsorID == sponsorID)
                        .SumAsync(x => 
                            (x.MyTransaction == null ? 0 : x.MyTransaction.Amount) 
                            +
                            (x.MyReceipt == null ? 0 : x.MyReceipt.Amount)
                        );
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<double> GetSumOfAmountsAsync(Sponsor sponsor)
        {
            if (!await IsExistAsync(sponsor))
            {
                throw new NotFoundException();
            }

            try
            {
                return await GetSumOfAmountsAsync(sponsor.SponsorID);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
