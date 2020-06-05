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

        public async Task<IEnumerable<SponsorTransaction>> GetAllTransactionAsync()
        {
            try
            {
                return await db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .Include(s => s.MyReceipt)
                    .Include(s => s.MyTransaction).ToListAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SponsorTransaction>> GetAllTransactionBySponsorIdAsync(int sponsorID)
        {
            try
            {
                return db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .Include(s => s.MyReceipt)
                    .Include(s => s.MyTransaction)
                    .Where(m => m.SponsorID == sponsorID);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<SponsorTransaction> GetTransactionByIdAsync(int sponsorTransactionID)
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

        public async Task<SponsorTransaction> GetTransactionAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                var x = (await GetAllTransactionBySponsorIdAsync(sponsorTransaction.SponsorID)).ToList();

                if (sponsorTransaction.MyTransaction != null)
                {
                    x = x.Where(m => m.MyTransaction != null).ToList();
                    x = x.Where(m => m.MyTransaction.TransactionDate == sponsorTransaction.MyTransaction.TransactionDate
                            &&
                            m.MyTransaction.Amount == sponsorTransaction.MyTransaction.Amount
                            &&
                            m.MyTransaction.LastFourNumbersOfBankCard == sponsorTransaction.MyTransaction.LastFourNumbersOfBankCard
                            &&
                            m.MyTransaction.TrackingNumber == sponsorTransaction.MyTransaction.TrackingNumber).ToList();
                }
                else if (sponsorTransaction.MyReceipt != null)
                {
                    x = x.Where(m => m.MyReceipt != null).ToList();
                    x = x.Where(m => m.MyReceipt.Amount == sponsorTransaction.MyReceipt.Amount
                            &&
                            m.MyReceipt.ReceiptNumber == sponsorTransaction.MyReceipt.ReceiptNumber
                            &&
                            m.MyReceipt.TransactionDate == sponsorTransaction.MyReceipt.TransactionDate).ToList();
                }
                else
                {
                    throw new NotFoundException();
                }

                return x.First();
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertTransactionAsync(SponsorTransaction sponsorTransaction)
        {
            if (await IsExistTransactionAsync(sponsorTransaction))
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

        public async Task<bool> UpdateTransactionAsync(SponsorTransaction sponsorTransaction)
        {
            if (!(await IsExistTransactionAsync(sponsorTransaction)))
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

        public async Task<bool> DeleteTransactionAsync(SponsorTransaction sponsorTransaction)
        {
            if (!(await IsExistTransactionAsync(sponsorTransaction)))
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

        public async Task<bool> DeleteTransactionAsync(int sponsorTransactionID)
        {
            try
            {
                var sponsorTransaction = await GetTransactionByIdAsync(sponsorTransactionID);
                return await DeleteTransactionAsync(sponsorTransaction);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistTransactionAsync(SponsorTransaction sponsorTransaction)
        {
            try
            {
                SponsorTransaction b = await GetTransactionAsync(sponsorTransaction);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<IEnumerable<SponsorTransaction>> GetFromToTransactionBySponsorIdAsync(int sponsorID, DateTime From, DateTime To)
        {
            try
            {
                var x = await GetAllTransactionBySponsorIdAsync(sponsorID);

                var xx = x.Where(m => m.MyTransaction != null).ToList();
                xx = xx.Where(m => m.MyTransaction.TransactionDate >= From).ToList();
                xx = xx.Where(m => m.MyTransaction.TransactionDate <= To).ToList();

                var xxx = x.Where(m => m.MyReceipt != null).ToList();
                xxx = xxx.Where(m => m.MyReceipt.TransactionDate >= From).ToList();
                xxx = xxx.Where(m => m.MyReceipt.TransactionDate <= To).ToList();
                
                xx.AddRange(xxx);

                return xx;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<SponsorTransaction>> GetFromToTransactionAsync(DateTime From, DateTime To)
        {
            try
            {
                var x = await GetAllTransactionAsync();

                var xx = x.Where(m => m.MyTransaction != null).ToList();
                xx = xx.Where(m => m.MyTransaction.TransactionDate >= From).ToList();
                xx = xx.Where(m => m.MyTransaction.TransactionDate <= To).ToList();

                var xxx = x.Where(m => m.MyReceipt != null).ToList();
                xxx = xxx.Where(m => m.MyReceipt.TransactionDate >= From).ToList();
                xxx = xxx.Where(m => m.MyReceipt.TransactionDate <= To).ToList();

                xx.AddRange(xxx);

                return xx;
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
