using Microsoft.AspNetCore.Mvc;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class NewColleague : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
