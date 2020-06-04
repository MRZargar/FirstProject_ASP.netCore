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
    public class DetailsBankTable : ViewComponent
    {
        private IBankRepository banks;

        public DetailsBankTable(MyContext context)
        {
            banks = new BankRepository(context);
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

            IEnumerable<BankTransaction> transactions = await banks.GetAllTransactionByBankIdAsync(id);

            TempData["maxAmount"] = 50000;
            if (transactions.Count() > 0)
            {
                double max = transactions.Select(x => x.Transaction.Amount).Max();
                double div = Math.Pow(10, max.ToString().Count() - 1);
                double round = Math.Ceiling(max / div) * div;
                TempData["maxAmount"] = round;
            }

            TempData["FromDate"] = From.ToShortDateString();
            TempData["ToDate"] = To.ToShortDateString();
            return View(transactions);
        }
    }
}
