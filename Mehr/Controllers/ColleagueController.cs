using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;

namespace Mehr.Controllers
{
    public class ColleagueController : Controller
    {
        public IColleageRepository colleagues;

        public ColleagueController(MyContext context)
        {
            this.colleagues = new ColleagueRepository(context);
        }

        // GET: App/Colleague
        public async Task<IActionResult> Index()
        {
            return View(await colleagues.GetAllAsync());
        }

        // GET: App/Colleague/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colleague = await colleagues.GetByIdAsync(id.Value);
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
                await colleagues.InsertAsync(colleague);
                await colleagues.saveAsync();
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

            var colleague = await colleagues.GetByIdAsync(id.Value);
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
                    colleagues.Update(colleague);
                    await colleagues.saveAsync();
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

            var colleague = await colleagues.GetByIdAsync(id.Value);
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
            await colleagues.DeleteAsync(id);
            await colleagues.saveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ColleagueExists(int id)
        {
            return colleagues.GetByIdAsync(id) != null;
        }
    }
}
