using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class SponsorTransactionRepository : ISponsorTransactionRepository
    {
        private MyContext db;

        public SponsorTransactionRepository(MyContext context)
        {
            this.db = context;
        }

        public async Task<IEnumerable<SponsorTransaction>> GetAllAsync()
        {
            try
            {
                return await db.SponsorTransactions.Include(s => s.MySponsor).ToListAsync(); 
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public async Task<IEnumerable<SponsorTransaction>> GetAllBySponsorIdAsync(int sponsorID)
        {
            try
            {
                return db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .Where(m => m.SponsorID == sponsorID);
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public async Task<SponsorTransaction> GetByIdAsync(int sponsorTransactionID)
        {
            try
            {
                return await db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .FirstOrDefaultAsync(m => m.SponsorTransactionsID == sponsorTransactionID);   
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                await db.SponsorTransactions.AddAsync(sponsorTransaction);     

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Update(SponsorTransaction sponsorTransaction)
        {
            try
            {
                db.SponsorTransactions.Update(sponsorTransaction);

                return true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool Delete(SponsorTransaction sponsorTransaction)
        {
            try
            {
                db.SponsorTransactions.Remove(sponsorTransaction);

                return true;
            }
            catch (System.Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int sponsorTransactionID)
        {
            try
            {
                var sponsorTransaction = await GetByIdAsync(sponsorTransactionID);
                return Delete(sponsorTransaction);
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
