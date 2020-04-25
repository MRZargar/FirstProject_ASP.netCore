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
    public class ColleagueController : Controller
    {
        private readonly MyContext _context;

        public ColleagueController(MyContext context)
        {
            _context = context;
        }

        // GET: App/Colleague
        public async Task<IActionResult> Index()
        {
            return View(await _context.Colleagues.ToListAsync());
        }

        // GET: App/Colleague/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colleague = await _context.Colleagues
                .FirstOrDefaultAsync(m => m.ColleagueID == id);
            if (colleague == null)
            {
                return NotFound();
            }

            return View(colleague);
        }

        // GET: App/Colleague/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: App/Colleague/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColleagueID,Name,PhoneNumber,BirthDay,StartActivity,code")] Colleague colleague)
        {
            if (ModelState.IsValid)
            {
                _context.Add(colleague);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(colleague);
        }

        // GET: App/Colleague/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colleague = await _context.Colleagues.FindAsync(id);
            if (colleague == null)
            {
                return NotFound();
            }
            return View(colleague);
        }

        // POST: App/Colleague/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColleagueID,Name,PhoneNumber,BirthDay,StartActivity,code")] Colleague colleague)
        {
            if (id != colleague.ColleagueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colleague);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColleagueExists(colleague.ColleagueID))
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
            return View(colleague);
        }

        // GET: App/Colleague/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colleague = await _context.Colleagues
                .FirstOrDefaultAsync(m => m.ColleagueID == id);
            if (colleague == null)
            {
                return NotFound();
            }

            return View(colleague);
        }

        // POST: App/Colleague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var colleague = await _context.Colleagues.FindAsync(id);
            _context.Colleagues.Remove(colleague);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColleagueExists(int id)
        {
            return _context.Colleagues.Any(e => e.ColleagueID == id);
        }
    }
}
