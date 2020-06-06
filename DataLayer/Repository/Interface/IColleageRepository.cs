using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface IColleageRepository
    {
        Task<IEnumerable<Colleague>> GetAllAsync();
        Task<Colleague> GetByIdAsync(int colleagueID);
        int Count();
        Task<Colleague> GetByPhoneNumberAsync(long phoneNmber);
        Task<double> GetSumOfAmountsAsync(int colleagueID);
        Task<double> GetSumOfAmountsAsync(Colleague colleague);
        Task<bool> InsertAsync(Colleague colleague);
        Task<bool> UpdateAsync(Colleague colleague);
        Task<bool> DeleteAsync(Colleague colleague);
        Task<bool> IsExistAsync(int ColleageID);
        Task<bool> IsExistAsync(Colleague colleague);
        Task<bool> IsExistAsync(long phoneNumber);
        Task<bool> DeleteAsync(int colleagueID);
        Task<bool> saveAsync();
        void Dispose();

        IEnumerable<SponsorTransaction> GetAllTransactionByColleagueIdAsync(int colleagueID);
        IEnumerable<SponsorTransaction> GetFromToTransactionByColleagueIdAsync(int colleagueID, DateTime From, DateTime To);

        Task<IEnumerable<SponsorTransactionError>> GetAllErrorsAsync();
        Task<SponsorTransactionError> GetErrorByIdAsync(int errorID);
        Task<bool> InsertErrorAsync(SponsorTransactionError error);
        Task<bool> InsertErrorAsync(DataRow row, ErrorMessage Message, int ColleagueID);
        Task<bool> InsertErrorAsync(SponsorTransaction st, ErrorMessage Message);
        Task<bool> UpdateErrorAsync(SponsorTransactionError error);
        Task<bool> DeleteErrorAsync(SponsorTransactionError error);
        Task<bool> IsExistErrorAsync(int errorID);
        Task<bool> IsExistErrorAsync(SponsorTransactionError error);
        Task<bool> DeleteErrorAsync(int errorID);
    }
}
