using System;
using System.Collections.Generic;
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
                return await db.SponsorTransactions.ToListAsync(); 
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
                db.SponsorTransactions.Add(sponsorTransaction);     
                await saveAsync();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                db.SponsorTransactions.Update(sponsorTransaction);
                await saveAsync();

                return true;    
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                db.SponsorTransactions.Remove(sponsorTransaction);
                await saveAsync();

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
                return await DeleteAsync(sponsorTransaction);
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
