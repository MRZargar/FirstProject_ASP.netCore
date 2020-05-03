using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class DetailsColleagueTable : ViewComponent
    {
        private ISponsorTransactionRepository transactions;
        private IColleageRepository colleages;

        public DetailsColleagueTable(MyContext context)
        {
            transactions = new SponsorTransactionRepository(context);
            colleages = new ColleagueRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var colleague = await colleages.GetByIdAsync(id);

            List<SponsorTransaction> colleagusTransactios = new List<SponsorTransaction>();
            decimal sumAmounts = 0;

            foreach (Sponsor sponsor in colleague.Sponsors)
            {   
                var sponsorTransactions = await transactions.GetAllBySponsorIdAsync(sponsor.SponsorID);
                colleagusTransactios.AddRange(sponsorTransactions);
                sumAmounts += sumAmountsTransactions(sponsorTransactions);
            } 

            ViewBag.sumAmounts = sumAmounts;

            return View(colleagusTransactios);
        }

        public decimal sumAmountsTransactions(IEnumerable<SponsorTransaction> transactions)
        {
            decimal sum = 0;

            foreach (SponsorTransaction transaction in transactions)
            {
                sum += transaction.Amount;
            }

            return sum;
        }
    }
}
