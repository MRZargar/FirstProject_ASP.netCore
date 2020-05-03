using Microsoft.AspNetCore.Mvc;
using DataLayer;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Mehr.ViewComponents
{
    [ViewComponent]
    public class DetailsColleagueTable : ViewComponent
    {
        private ISponsorTransactionRepository transactions;
        private IColleageRepository colleages;

        public DetailsColleagueTable(MyContext context)
        {
            transactions = new SponsorTransactionRepository(context);
            colleages = new ColleagueRepository(context);
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var colleague = await colleages.GetByIdAsync(id);

            List<SponsorTransaction> colleagusTransactios = new List<SponsorTransaction>();

            foreach (var sponsor in colleague.Sponsors)
            {
                colleagusTransactios.AddRange(await transactions.GetAllBySponsorIdAsync(sponsor.SponsorID));
            } 

            return View(colleagusTransactios);
        }
    }
}
