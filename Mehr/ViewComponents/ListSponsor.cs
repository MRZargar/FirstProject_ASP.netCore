using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class ListSponsor : ViewComponent
    {
        private ISponsorRepository sponsors;

        public ListSponsor(MyContext context)
        {
            sponsors = new SponsorRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(string Mode = "Default")
        {
            var spnsrs = await sponsors.GetAllAsync();

            if (Mode == "Tops")
            {
                var temp = new List<Tuple<Sponsor, decimal>>();
                foreach (Sponsor spnsr in spnsrs)
                {
                    decimal sumAmounts = await sponsors.GetSumOfAmountsAsync(spnsr);
                    temp.Add(new Tuple<Sponsor, decimal>(spnsr, sumAmounts));
                }

                temp.Sort(delegate (Tuple<Sponsor, decimal> x, Tuple<Sponsor, decimal> y)
                {
                    return x.Item2 > y.Item2 ? -1 : 1;
                });

                spnsrs = temp.Select(x => x.Item1).Take(5);
                ViewBag.Amounts = temp.Select(x => x.Item2).Take(5).ToList();
            }

            return View(Mode, spnsrs);
        }
    }
}
