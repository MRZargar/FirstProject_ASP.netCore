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
        private readonly MyContext _context;

        public SponsorController(MyContext context)
        {
            _context = context;
        }

        // GET: App/Sponsor
        public async Task<IActionResult> Index()
        {
            var myContext = _context.Sponsors.Include(s => s.MyColleague);
            return View(await myContext.ToListAsync());
        }

        // GET: App/Sponsor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .Include(s => s.MyColleague)
                .FirstOrDefaultAsync(m => m.SponsorID == id);
            if (sponsor == null)
            {
                return NotFound();
            }

            return View(sponsor);
        }

        // GET: App/Sponsor/Create
        public IActionResult Create()
        {
            ViewData["ColleagueID"] = new SelectList(_context.Colleagues, "ColleagueID", "Name");
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
                _context.Add(sponsor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColleagueID"] = new SelectList(_context.Colleagues, "ColleagueID", "Name", sponsor.ColleagueID);
            return View(sponsor);
        }

        // GET: App/Sponsor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors.FindAsync(id);
            if (sponsor == null)
            {
                return NotFound();
            }
            ViewData["ColleagueID"] = new SelectList(_context.Colleagues, "ColleagueID", "Name", sponsor.ColleagueID);
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
                    _context.Update(sponsor);
                    await _context.SaveChangesAsync();
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
            ViewData["ColleagueID"] = new SelectList(_context.Colleagues, "ColleagueID", "Name", sponsor.ColleagueID);
            return View(sponsor);
        }

        // GET: App/Sponsor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsor = await _context.Sponsors
                .Include(s => s.MyColleague)
                .FirstOrDefaultAsync(m => m.SponsorID == id);
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
            var sponsor = await _context.Sponsors.FindAsync(id);
            _context.Sponsors.Remove(sponsor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorExists(int id)
        {
            return _context.Sponsors.Any(e => e.SponsorID == id);
        }
    }
}
