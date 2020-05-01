using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;

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

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await sponsors.GetAllAsync());
        }
    }
}
