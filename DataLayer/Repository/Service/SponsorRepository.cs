using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Exceptions;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        private async Task<bool> InsertTransactionValidateAsync(SponsorTransaction sponsorTransaction)
        {
            if (await IsExistTransactionAsync(sponsorTransaction))
            {
                throw new DuplicateTransactionException();
            }

            if (await new BankRepository(db).IsExistTransactionAsync(sponsorTransaction))
            {
                throw new DoNotExistBankTransactionException();
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

        public async Task<bool> InsertTransactionAsync(DataRow row, int ColleagueID)
        {
            var st = new SponsorTransaction();
            
            if (row["Phone"].ToString() == string.Empty || row["Phone"].ToString().Length != 10)
            {
                await new ColleagueRepository(db)
                    .InsertErrorAsync(row, ErrorMessage.Phone_number_not_entered, ColleagueID);
            }

            long phoneNumber = long.Parse(row["Phone"].ToString());

            Sponsor mySponsor;
            try
            {
                mySponsor = await GetByPhoneNumberAsync(phoneNumber);

                if (mySponsor.ColleagueID != ColleagueID)
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.This_sponsor_is_related_to_another_colleague, ColleagueID);
                }
            }
            catch (NotFoundException)
            {
                mySponsor = new Sponsor();
                mySponsor.ColleagueID = ColleagueID;
                mySponsor.PhoneNumber = phoneNumber;
                mySponsor.Name = row["SponsorName"].ToString();
                if (mySponsor.Name == string.Empty)
                {
                    mySponsor.Name = "Undefine";
                }

                await InsertAsync(mySponsor);
                await saveAsync();

                mySponsor = await GetByPhoneNumberAsync(phoneNumber);
                if (mySponsor == null)
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.There_is_a_problem_when_adding_a_new_sponsor, ColleagueID);
                }
            }

            try
            {
                st.SponsorID = mySponsor.SponsorID;
                st.ColleagueID = ColleagueID;

                //DateTime date = Convert.ToDateTime(row["Date"].ToString().ToAD()).Date;
                DateTime date = Convert.ToDateTime(row["Date"].ToString()).Date;
                TimeSpan time;
                try
                {
                    time = TimeSpan.Parse(row["Time"].ToString());
                }
                catch (Exception)
                {
                    time = Convert.ToDateTime(row["Time"].ToString()).TimeOfDay;
                }

                if (row["CardNumber"].ToString() != "" && row["TrackingNumber"].ToString() != "")
                {
                    st.MyTransaction = new BankData();
                    st.MyTransaction.Amount = Convert.ToDouble(row["Amount"]);
                    st.MyTransaction.LastFourNumbersOfBankCard = Convert.ToInt16(row["CardNumber"]);
                    st.MyTransaction.TrackingNumber = row["TrackingNumber"].ToString();
                    st.MyTransaction.TransactionDate = date + time;
                }
                else if (row["ReceiptNumber"].ToString() != "")
                {
                    st.MyReceipt = new ReceiptData();
                    st.MyReceipt.Amount = Convert.ToDouble(row["Amount"]);
                    st.MyReceipt.TransactionDate = date + time;
                }
                else
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.No_transaction_information_entered, ColleagueID);
                }
            }
            catch (Exception)
            {
                await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.Correct_the_type_of_input_information, ColleagueID);
            }

            try
            {
                await InsertTransactionValidateAsync(st);
            }
            catch (DuplicateTransactionException)
            {
                await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.Duplicate, ColleagueID);

                return false;
            }
            catch (DoNotExistBankTransactionException)
            {
                await new ColleagueRepository(db)
                        .InsertErrorAsync(row, ErrorMessage.This_transaction_is_not_available_in_bank_transactions, ColleagueID);

                return false;
            }

            return true;
        }

        public async Task<bool> InsertTransactionAsync(SponsorTransactionError error)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("SponsorName");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Date");
            dt.Columns.Add("Time");
            dt.Columns.Add("ReceiptNumber");
            dt.Columns.Add("CardNumber");
            dt.Columns.Add("TrackingNumber");
            dt.Columns.Add("Amount");
            DataRow row = dt.NewRow();

            row["SponsorName"] = error.SponsorName;
            row["Phone"] = error.Phone;
            row["Date"] = error.Date;
            row["Time"] = error.Time;
            row["ReceiptNumber"] = error.ReceiptNumber;
            row["CardNumber"] = error.CardNumber;
            row["TrackingNumber"] = error.TrackingNumber;
            row["Amount"] = error.Amount;

            try
            {
                await new ColleagueRepository(db).DeleteErrorAsync(error.ErrorID);
            }
            catch (Exception)
            { }

            return await InsertTransactionAsync(row, error.ColleagueID);
        }

        public async Task<bool> InsertTransactionAsync(SponsorTransaction st)
        {
            Sponsor mySponsor;
            try
            {
                mySponsor = await GetByIdAsync(st.SponsorID);
                st.MySponsor = mySponsor;

                if (mySponsor.ColleagueID != st.ColleagueID)
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(st, ErrorMessage.This_sponsor_is_related_to_another_colleague);

                    return false;
                }
            }
            catch (NotFoundException ex)
            {
                await new ColleagueRepository(db)
                    .InsertErrorAsync(st, ErrorMessage.Phone_number_not_entered);

                return false;
            }

            if (st.MyTransaction != null)
            {
                bool valid = true;
                valid = valid && st.MyTransaction.Amount > 0;
                valid = valid && st.MyTransaction.LastFourNumbersOfBankCard.ToString().Length >= 4;
                valid = valid && st.MyTransaction.TrackingNumber.Length > 4;

                if (!valid)
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(st, ErrorMessage.Correct_the_type_of_input_information);

                    return false;
                }
            }
            else if (st.MyReceipt != null)
            {
                bool valid = true;
                valid = valid && st.MyReceipt.Amount > 0;
                valid = valid && st.MyReceipt.ReceiptNumber.ToString().Length > 2;

                if (!valid)
                {
                    await new ColleagueRepository(db)
                        .InsertErrorAsync(st, ErrorMessage.Correct_the_type_of_input_information);

                    return false;
                }
            }
            else
            {
                await new ColleagueRepository(db)
                    .InsertErrorAsync(st, ErrorMessage.No_transaction_information_entered);

                return false;
            }
            
            try
            {
                await InsertTransactionValidateAsync(st);
            }
            catch (DuplicateTransactionException)
            {
                await new ColleagueRepository(db)
                    .InsertErrorAsync(st, ErrorMessage.Duplicate);

                return false;
            }
            catch (DoNotExistBankTransactionException)
            {
                await new ColleagueRepository(db)
                    .InsertErrorAsync(st, ErrorMessage.This_transaction_is_not_available_in_bank_transactions);
                
                return false;
            }

            return true;
        }
    }
}
