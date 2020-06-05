using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mehr.Models;
using DataLayer;
using Mehr.Classes;

namespace Mehr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISponsorRepository sponsors;
        private IColleageRepository colleagues;
        private IBankRepository banks;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            sponsors = new SponsorRepository(context);
            colleagues = new ColleagueRepository(context);
            banks = new BankRepository(context);
            _logger = logger;
        }

        public async Task<IActionResult> IndexAsync()
        {
            List<DateTime> months = this.GetFirstOfAllSolarMonth();
            string ChartData = "[";
            for (int i = 0; i < months.Count - 1; i++)
            {
                double sum = 0;
                foreach (Sponsor sponsor in await sponsors.GetAllAsync())
                {
                    var transactions = await sponsors.GetFromToTransactionBySponsorIdAsync(sponsor.SponsorID, months[i], months[i + 1]);
                    sum += transactions.Select(x => (x.MyTransaction?.Amount ?? 0) + (x.MyReceipt?.Amount ?? 0)).Sum();
                }
                ChartData += sum.ToString();
                ChartData += ", ";
            }
            ChartData = ChartData.Substring(0, ChartData.Length - 1) + "]";
            
            ViewBag.ChartData = ChartData;
            ViewBag.ColleaguesCount = colleagues.Count();
            ViewBag.SponsoqxrsCount = sponsors.Count();
            return View();
        }
        
        [Route("Colleagues")]
        public IActionResult Colleagues()
        {
            return View();
        }

        [Route("Sponsors")]
        public IActionResult Sponsors()
        {
            return View();
        }

        [Route("Banks")]
        public async Task<IActionResult> Banks()
        {
            var all = await banks.GetAllAsync();
            return View(all);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
