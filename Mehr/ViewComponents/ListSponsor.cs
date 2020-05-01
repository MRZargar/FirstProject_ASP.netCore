using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;

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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await colleages.GetAllAsync());
        }
    }
}
