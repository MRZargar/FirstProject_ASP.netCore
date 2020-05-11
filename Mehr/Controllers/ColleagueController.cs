using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Exceptions;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Mehr.Classes;

namespace Mehr.Controllers
{
    public class ColleagueController : Controller
    {
        public IColleageRepository colleagues;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ColleagueController(IHostingEnvironment hostingEnvironment, MyContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            this.colleagues = new ColleagueRepository(context);
        }

        // GET: App/Colleague/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            Colleague colleague;
            try
            {
                colleague = await colleagues.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            ViewBag.ChartData = "[125, 200, 125, 225, 125, 200, 125, 225, 175, 275, 220]";
            return View(colleague);
        }

        // POST: App/Colleague/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColleagueID,Name,PhoneNumber,BirthDay,StartActivity,code,color,isMale")] Colleague colleague, IFormFile profile)
        {
            if (ModelState.IsValid)
            {
                if (profile != null)
                {
                    colleague.picName = Guid.NewGuid() + Path.GetExtension(profile.FileName);
                    string picPath = _hostingEnvironment.WebRootPath 
                                    + @"\images\Profiles\" 
                                    + colleague.picName;

                    using (var stream = new FileStream(picPath, FileMode.Create))
                    {
                        await profile.CopyToAsync(stream);
                    }
                }

                try
                {
                    await colleagues.InsertAsync(colleague);
                    await colleagues.saveAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.alert = new Alert(false, ex.Message);
                }
                ViewBag.alert = new Alert(true, "New Colleague Created successfully.");
            }
            else
            {
                ViewBag.alert = new Alert(false, "Please Complete fields ...");
            }
            return RedirectToAction("Colleagues", "Home");
        }

        // GET: App/Colleague/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            Colleague colleague;
            try
            {
                colleague = await colleagues.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
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
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await colleagues.UpdateAsync(colleague);
                    await colleagues.saveAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.err = ex;
                    return View("Error");
                }
                return RedirectToAction("Details", new {id = colleague.ColleagueID}); //RedirectToAction(nameof(Index));
            }
            return View(colleague);
        }

        // GET: App/Colleague/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            Colleague colleague;
            try
            {
                colleague = await colleagues.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            return View(colleague);
        }

        // POST: App/Colleague/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await colleagues.DeleteAsync(id);
                await colleagues.saveAsync();
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }
            return RedirectToAction("Colleagues", "Home");
        }
    }
}
