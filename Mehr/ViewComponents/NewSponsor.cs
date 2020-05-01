using System.Threading.Tasks;
using DataLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class NewSponsor : ViewComponent
    {
        private IColleageRepository colleagues;

        public NewSponsor(MyContext context)
        {
            colleagues = new ColleagueRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewData["ColleagueID"] = new SelectList(await colleagues.GetAllAsync(), "ColleagueID", "Name");
            return View();
        }
    }
}
