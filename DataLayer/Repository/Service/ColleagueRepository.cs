using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
                    .FirstOrDefaultAsync(m => m.ColleagueID == colleagueID);   
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(Colleague colleague)
        {
            try
            {
                await db.Colleagues.AddAsync(colleague);     

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(Colleague colleague)
        {
            try
            {
                db.Colleagues.Update(colleague);

                return true;    
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool Delete(Colleague colleague)
        {
            try
            {
                db.Colleagues.Remove(colleague);

                return true;    
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int colleagueID)
        {
            try
            {
                var colleague = await GetByIdAsync(colleagueID);
                return Delete(colleague);
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

        public void Dispose()
        {
            db.Dispose();
        }
    }
}
