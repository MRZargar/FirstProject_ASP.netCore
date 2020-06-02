using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;
using Mehr.Classes;

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

        public async Task<IViewComponentResult> InvokeAsync(int id, string FromDate, string ToDate)
        {
            DateTime From = new DateTime();
            DateTime To = new DateTime();

            if (FromDate == "")
            {
                From = Convert.ToDateTime(new DateTime().getFirstSolarMonth().ToAD());
            }
            else
            {
                try
                {
                    From = Convert.ToDateTime(FromDate.ToAD());
                }
                catch (Exception ex)
                {
                    ViewBag.err = ex;
                    return View("Error");
                }
            }

            if (ToDate == "")
            {
                To = DateTime.Today;
            }
            else
            {
                try
                {
                    To = Convert.ToDateTime(ToDate.ToAD());
                }
                catch (Exception ex)
                {
                    ViewBag.err = ex;
                    return View("Error");
                }
            }

            var colleague = await colleages.GetByIdAsync(id);

            List<SponsorTransaction> colleagusTransactios = new List<SponsorTransaction>();
            decimal sumAmounts = 0;

            foreach (Sponsor sponsor in colleague.Sponsors)
            {   
                var sponsorTransactions = await transactions.GetFromToBySponsorIdAsync(sponsor.SponsorID, From, To.AddDays(1));
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
            TempData["FromDate"] = From.ToShortDateString();
            TempData["ToDate"] = To.ToShortDateString();
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
