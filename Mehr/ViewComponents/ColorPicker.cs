using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class ColorPicker : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
