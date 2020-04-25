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
                db.Colleagues.Add(colleague);     
                await saveAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Colleague colleague)
        {
            try
            {
                db.Colleagues.Update(colleague);
                await saveAsync();

                return true;    
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(Colleague colleague)
        {
            try
            {
                db.Colleagues.Remove(colleague);
                await saveAsync();

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
                return await DeleteAsync(colleague);
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
