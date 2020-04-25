using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;

namespace Mehr.Areas.App.Controllers
{
    [Area("App")]
    public class SponsorController : Controller
    {
        private ISponsorRepository sponsors;
        private IColleageRepository colleagues;

        public SponsorController(MyContext context)
        {
            sponsors = new SponsorRepository(context);
            colleagues = new ColleagueRepository(context);
        }

        // GET: App/Sponsor
        public async Task<IActionResult> Index()
        {
            return View(await sponsors.GetAllAsync());
        }

        // GET: App/Sponsor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await sponsors.GetByIdAsync(id.Value);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // GET: App/Sponsor/Create
        public async Task<IActionResult> Create()
        {
            ViewData["ColleagueID"] = new SelectList(await colleagues.GetAllAsync(), "ColleagueID", "Name");
            return View();
        }

        // POST: App/Sponsor/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SponsorID,ColleagueID,Name,PhoneNumber,CauseOfSupport,OtherSupport,isValid")] Sponsor sponsor)
        {
            if (ModelState.IsValid)
            {
                await sponsors.InsertAsync(sponsor);
                await sponsors.saveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColleagueID"] = new SelectList(await colleagues.GetAllAsync(), "ColleagueID", "Name", sponsor.ColleagueID);
            return View(sponsor);
        }

        // GET: App/Sponsor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await sponsors.GetByIdAsync(id.Value);
            if (sponsor == null)
            {
                return NotFound();
            }
            ViewData["ColleagueID"] = new SelectList(await colleagues.GetAllAsync(), "ColleagueID", "Name", sponsor.ColleagueID);
            return View(sponsor);
        }

        // POST: App/Sponsor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorID,ColleagueID,Name,PhoneNumber,CauseOfSupport,OtherSupport,isValid")] Sponsor sponsor)
        {
            if (id != sponsor.SponsorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sponsors.Update(sponsor);
                    await sponsors.saveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorExists(sponsor.SponsorID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColleagueID"] = new SelectList(await colleagues.GetAllAsync(), "ColleagueID", "Name", sponsor.ColleagueID);
            return View(sponsor);
        }

        // GET: App/Sponsor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await sponsors.GetByIdAsync(id.Value);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // POST: App/Sponsor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await sponsors.DeleteAsync(id);
            await sponsors.saveAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return sponsors.GetByIdAsync(id) != null;
        }
    }
}
