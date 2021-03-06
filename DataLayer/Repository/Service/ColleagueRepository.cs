using System;
using System.Collections.Generic;
using System.Data;
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

        public IEnumerable<SponsorTransaction> GetAllTransactionByColleagueIdAsync(int colleagueID)
        {
            try
            {
                return db.SponsorTransactions
                    .Include(s => s.MySponsor)
                    .Include(s => s.MyReceipt)
                    .Include(s => s.MyTransaction)
                    .Where(m => m.ColleagueID == colleagueID);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IEnumerable<SponsorTransaction> GetFromToTransactionByColleagueIdAsync(int colleagueID, DateTime From, DateTime To)
        {
            try
            {
                var x = GetAllTransactionByColleagueIdAsync(colleagueID);

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

        public async Task<IEnumerable<SponsorTransactionError>> GetAllErrorsAsync()
        {
            try
            {
                return await db.Errors.ToListAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<SponsorTransactionError> GetErrorByIdAsync(int errorID)
        {
            try
            {
                return await db.Errors
                    .FirstAsync(m => m.ErrorID == errorID);
            }
            catch (System.Exception)
            {
                throw new NotFoundException();
            }
        }

        public async Task<bool> InsertErrorAsync(SponsorTransactionError error)
        {
            if (await IsExistErrorAsync(error))
            {
                throw new DuplicatePhoneNumberException();
            }

            try
            {
                await db.Errors.AddAsync(error);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateErrorAsync(SponsorTransactionError error)
        {
            if (!(await IsExistErrorAsync(error)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Errors.Update(error);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteErrorAsync(SponsorTransactionError error)
        {
            if (!(await IsExistErrorAsync(error)))
            {
                throw new NotFoundException();
            }

            try
            {
                db.Errors.Remove(error);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> IsExistErrorAsync(int errorID)
        {
            try
            {
                var e = await GetErrorByIdAsync(errorID);
            }
            catch (NotFoundException)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsExistErrorAsync(SponsorTransactionError error)
        {
            return await IsExistErrorAsync(error.ErrorID);
        }

        public async Task<bool> DeleteErrorAsync(int errorID)
        {
            try
            {
                var error = await GetErrorByIdAsync(errorID);
                return await DeleteErrorAsync(error);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> InsertErrorAsync(DataRow row, ErrorMessage Message, int ColleagueID)
        {
            var err = new SponsorTransactionError();

            err.SponsorName = row["SponsorName"].ToString();
            err.Phone = row["Phone"].ToString();
            err.Date = row["Date"].ToString();
            err.Time = row["Time"].ToString();
            err.ReceiptNumber = row["ReceiptNumber"].ToString();
            err.CardNumber = row["CardNumber"].ToString();
            err.TrackingNumber = row["TrackingNumber"].ToString();
            err.Amount = row["Amount"].ToString();
            err.ErrorMessage = Message.ToString().Replace('_', ' ');
            err.ColleagueID = ColleagueID;

            await InsertErrorAsync(err);
            return true;
        }

        public async Task<bool> InsertErrorAsync(SponsorTransaction st, ErrorMessage Message)
        {
            var err = new SponsorTransactionError();

            err.SponsorName = st.MySponsor?.Name.ToString() ?? "";
            err.Phone = st.MySponsor?.PhoneNumber.ToString() ?? "";
            err.Date = st.MyTransaction?.TransactionDate.Date.ToString() ?? st.MyReceipt?.TransactionDate.Date.ToString() ?? "";
            err.Time = st.MyTransaction?.TransactionDate.TimeOfDay.ToString() ?? st.MyReceipt?.TransactionDate.TimeOfDay.ToString() ?? "";
            err.ReceiptNumber = st.MyReceipt?.ReceiptNumber.ToString() ?? "";
            err.CardNumber = st.MyTransaction?.LastFourNumbersOfBankCard.ToString() ?? "";
            err.TrackingNumber = st.MyTransaction?.TrackingNumber.ToString() ?? "";
            err.Amount = st.MyTransaction?.Amount.ToString() ?? st.MyReceipt?.Amount.ToString() ?? "";
            err.ErrorMessage = Message.ToString().Replace('_', ' ');
            err.ColleagueID = st.ColleagueID;

            await InsertErrorAsync(err);
            return true;
        }

        public async Task<IEnumerable<SponsorTransactionError>> GetAllErrorsByColleagueIDAsync(int ColleagueID)
        {
            try
            {
                return await db.Errors
                    .Where(x => x.ColleagueID == ColleagueID)
                    .ToListAsync();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
