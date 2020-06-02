using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Exceptions;
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
                    .FirstAsync(m => m.SponsorTransactionsID == sponsorTransactionID);   
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<SponsorTransaction> GetAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                return await db.SponsorTransactions
                    .FirstAsync(x => x.TransactionDate == sponsorTransaction.TransactionDate
                                  && x.TrackingNumber == sponsorTransaction.TrackingNumber
                                  && x.MySponsor == sponsorTransaction.MySponsor);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertAsync(SponsorTransaction sponsorTransaction)
        {
            if (await IsExistAsync(sponsorTransaction))
            {
                throw new DuplicateTransactionException();
            }

            try
            {
                await db.SponsorTransactions.AddAsync(sponsorTransaction);     
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateAsync(SponsorTransaction sponsorTransaction)
        {
            if (!(await IsExistAsync(sponsorTransaction)))
            {
                throw new NotFoundException();
            }

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

        public async Task<bool> DeleteAsync(SponsorTransaction sponsorTransaction)
        {
            if (!(await IsExistAsync(sponsorTransaction)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.SponsorTransactions.Remove(sponsorTransaction);

                return true;
            }
            catch (System.Exception)
            {
                throw;
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
            catch (System.Exception ex)
            {
                throw;
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

        public async Task<bool> IsExistAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                SponsorTransaction b = await GetAsync(sponsorTransaction);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<SponsorTransaction>> GetFromToBySponsorIdAsync(int sponsorID, DateTime From, DateTime To )
        {
            try
            {
                return db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .Where(m => m.SponsorID == sponsorID 
                            && m.TransactionDate >= From
                            && m.TransactionDate <= To);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
