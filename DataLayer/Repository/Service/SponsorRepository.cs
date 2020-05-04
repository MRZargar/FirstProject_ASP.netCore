using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    .FirstOrDefaultAsync(m => m.SponsorID == sponsorID);   
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Sponsor sponsor)
        {
            try
            {
                await db.Sponsors.AddAsync(sponsor);     

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Sponsor sponsor)
        {
            try
            {
                db.Sponsors.Update(sponsor);

                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool Delete(Sponsor sponsor)
        {
            try
            {
                db.Sponsors.Remove(sponsor);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int sponsorID)
        {
            try
            {
                var sponsor = await GetByIdAsync(sponsorID);
                return Delete(sponsor);
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

        public async Task<Sponsor> GetByPhoneNumberAsync(long phoneNumber)
        {
            try
            {
                return await db.Sponsors
                    .Include(s => s.MyColleague)
                    .FirstOrDefaultAsync(m => m.PhoneNumber == phoneNumber);   
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
