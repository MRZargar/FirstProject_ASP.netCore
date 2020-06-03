using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class ListColleague : ViewComponent
    {
        private IColleageRepository colleages;

        public ListColleague(MyContext context)
        {
            colleages = new ColleagueRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(string Mode = "Default")
        {
            var cllgs = await colleages.GetAllAsync();

            if (Mode == "Tops")
            {
                var temp = new List<Tuple<Colleague, double>>();
                foreach (Colleague cllg in cllgs)
                {
                    double sumAmounts = await colleages.GetSumOfAmountsAsync(cllg);
                    temp.Add(new Tuple<Colleague, double>(cllg, sumAmounts));
                }
                
                temp.Sort(delegate (Tuple<Colleague, double> x, Tuple<Colleague, double> y)
                {
                    return x.Item2 > y.Item2 ? -1 : 1;
                });

                cllgs = temp.Select(x => x.Item1).Take(5);
                ViewBag.Amounts = temp.Select(x => x.Item2).Take(5).ToList();
            }

            return View(Mode, cllgs);
        }
    }
}
