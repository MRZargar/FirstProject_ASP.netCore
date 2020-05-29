using System;
using System.IO;
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
        public async Task<IActionResult> Details(int? id, string FromDate = "", string ToDate = "")
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            DateTime From = new DateTime();
            DateTime To = new DateTime();

            if (FromDate == "")
            {
                string temp = DateTime.Today.ToSolar();
                temp = temp.Substring(0, temp.Length - 2) + "01";
                From = Convert.ToDateTime(temp.ToAD());
            }
            else
            {
                try
                {
                    From = Convert.ToDateTime(FromDate.ToAD());
                }
                catch (Exception)
                {
                    ViewBag.err = new Exception("Invalid persian time format ...");
                    return View("Error");
                }
            }

            if (ToDate == "")
            {
                To = DateTime.Today;
            }
            else
            {
                try
                {
                    To = Convert.ToDateTime(ToDate.ToAD());
                }
                catch (Exception)
                {
                    ViewBag.err = new Exception("Invalid persian time format ...");
                    return View("Error");
                }
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
            
            ViewBag.FromDate = From.ToShortDateString();
            ViewBag.ToDate = To.ToShortDateString();
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
                saveProfile(ref colleague, profile);

                try
                {
                    await colleagues.InsertAsync(colleague);
                    await colleagues.saveAsync();  
                   this.SetViewMessage("New Colleague Created successfully." ,WebMessageType.Success);
           
                }
                catch (Exception ex)
                {
                    this.SetViewMessage( ex.Message , WebMessageType.Danger);
                }
             }
            else
            {
                this.SetViewMessage("Please Complete fields ..." , WebMessageType.Warning);
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
        public async Task<IActionResult> Edit(int id, [Bind("ColleagueID,Name,PhoneNumber,BirthDay,StartActivity,code,color,isMale")] Colleague colleague, IFormFile profile)
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
                    Colleague Edited = await colleagues.GetByIdAsync(id);
                    Edited.BirthDay = colleague.BirthDay;
                    Edited.code = colleague.code;
                    Edited.color = colleague.color;
                    Edited.isMale = colleague.isMale;
                    Edited.StartActivity = colleague.StartActivity;
                    Edited.Name = colleague.Name;

                    if (profile != null)
                    {
                        saveProfile(ref colleague, profile);
                        Edited.picName = colleague.picName;
                    }

                    await colleagues.UpdateAsync(Edited);
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
                deleteProfile(id);
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

        private void saveProfile(ref Colleague colleague, IFormFile profile)
        {
            if (profile != null)
            {
                colleague.picName = Guid.NewGuid() + Path.GetExtension(profile.FileName);
                string picPath = _hostingEnvironment.WebRootPath
                                + @"\images\Profiles\"
                                + colleague.picName;

                using (var stream = new FileStream(picPath, FileMode.Create))
                {
                    profile.CopyTo(stream);
                }
            }
        }

        private async void deleteProfile(int id)
        {
            var colleague = await colleagues.GetByIdAsync(id);

            if (colleague.picName != null)
            {
                string picPath = _hostingEnvironment.WebRootPath
                                + @"\images\Profiles\"
                                + colleague.picName;

                if (System.IO.File.Exists(picPath))
                {
                    System.IO.File.Delete(picPath);
                }
            }
        }
    }
}
