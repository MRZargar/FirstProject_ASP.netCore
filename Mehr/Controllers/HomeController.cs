using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mehr.Models;
using DataLayer;

namespace Mehr.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ISponsorRepository sponsors;
        private IColleageRepository colleagues;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            sponsors = new SponsorRepository(context);
            colleagues = new ColleagueRepository(context);
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.ColleaguesCount = colleagues.Count();
            ViewBag.SponsorsCount = sponsors.Count();
            ViewBag.ChartData = "[125, 200, 125, 225, 125, 200, 125, 225, 175, 275, 220]";
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
        public IActionResult Banks()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
