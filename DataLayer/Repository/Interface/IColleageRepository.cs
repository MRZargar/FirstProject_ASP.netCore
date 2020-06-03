using System;
using System.Collections.Generic;
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
    }
}
