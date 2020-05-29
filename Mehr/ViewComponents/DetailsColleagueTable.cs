using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

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

            TempData["maxAmount"] = 50000;
            if (colleagusTransactios.Count() > 0)
            {
                double max = Convert.ToDouble(colleagusTransactios.Select(x => x.Amount).Max());
                double div = Math.Pow(10, max.ToString().Count() - 1);
                double round = Math.Ceiling(max / div) * div;
                TempData["maxAmount"] = round;
            }

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
