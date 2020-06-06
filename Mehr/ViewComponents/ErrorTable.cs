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
    public class ErrorTable : ViewComponent
    {
        private IColleageRepository colleages;

        public ErrorTable(MyContext context)
        {
            colleages = new ColleagueRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            return View(await colleages.GetAllErrorsByColleagueIDAsync(id));
        }
    }
}
