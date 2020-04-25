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
                return await db.Sponsors.ToListAsync(); 
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
                db.Sponsors.Add(sponsor);     
                await saveAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Sponsor sponsor)
        {
            try
            {
                db.Sponsors.Update(sponsor);
                await saveAsync();

                return true;    
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Sponsor sponsor)
        {
            try
            {
                db.Sponsors.Remove(sponsor);
                await saveAsync();

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
                return false;
            }
        }

    }
}
