using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Exceptions;
using Mehr.Classes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mehr.Controllers
{
    public class BankController : Controller
    {
        public IBankRepository banks;
        private readonly IHostingEnvironment _hostingEnvironment;

        public BankController(IHostingEnvironment hostingEnvironment, MyContext context)
        {
            _hostingEnvironment = hostingEnvironment;
            banks = new BankRepository(context);
        }


        // GET: App/Bank/Details/5
        public async Task<IActionResult> Details(int? id, string FromDate = "", string ToDate = "")
        {
            if (id == null)
            {
                return NotFound();
            }

            Bank bank;
            try
            {
                bank = await banks.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }


            List<DateTime> months = this.GetFirstOfAllSolarMonth();
            string ChartData = "[";
            for (int i = 0; i < months.Count - 1; i++)
            {
                ChartData += bank.Transactions
                    .Where(x => x.Transaction.TransactionDate >= months[i]
                            && x.Transaction.TransactionDate <= months[i+1])
                    .Select(x => x.Transaction.Amount)
                    .Sum()
                    .ToString();
                ChartData += ", ";
            }
            ChartData = ChartData.Substring(0, ChartData.Length - 1) + "]";
            ViewBag.ChartData = ChartData;

            ViewBag.FromDate = FromDate;
            ViewBag.ToDate = ToDate;
            return View(bank);
        }


        // GET: Bank/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bank/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync([Bind("BankID,BankName,Owner,AccountNumber,CardNumber,ShebaNumber")] Bank bank, IFormFile img)
        {
            if (ModelState.IsValid)
            {
                saveImgBank(ref bank, img);

                try
                {
                    await banks.InsertAsync(bank);
                    await banks.saveAsync();
                    this.SetViewMessage("New Bank Created successfully.", WebMessageType.Success);

                }
                catch (Exception ex)
                {
                    this.SetViewMessage(ex.Message, WebMessageType.Danger);
                }
            }
            else
            {
                this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
            }
            return RedirectToAction("Banks", "Home");
        }

        // GET: Bank/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            Bank bank;
            try
            {
                bank = await banks.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            return View(bank);
        }

        // POST: Bank/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("BankID,BankName,Owner,AccountNumber,CardNumber,ShebaNumber")] Bank bank, IFormFile img)
        {
            if (id != bank.BankID)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Bank Edited = await banks.GetByIdAsync(id);
                    Edited.BankName = bank.BankName;
                    Edited.CardNumber = bank.CardNumber;
                    Edited.Owner = bank.Owner;
                    Edited.ShebaNumber = bank.ShebaNumber;
                    Edited.AccountNumber = bank.AccountNumber;

                    if (img != null)
                    {
                        deleteImgBank(id);
                        saveImgBank(ref Edited, img);
                    }

                    await banks.UpdateAsync(Edited);
                    await banks.saveAsync();
                }
                catch (Exception ex)
                {
                    ViewBag.err = ex;
                    return View("Error");
                }
                return RedirectToAction("Details", new { id = bank.BankID });
            }
            return View(bank);
        }

        // GET: Bank/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            Bank bank;
            try
            {
                bank = await banks.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            return View(bank);
        }

        // POST: Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            try
            {
                deleteImgBank(id);
                await banks.DeleteAsync(id);
                await banks.saveAsync();
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }
            return RedirectToAction("Banks", "Home");
        }

        private void saveImgBank(ref Bank bank, IFormFile img)
        {
            if (img != null)
            {
                bank.pic = Guid.NewGuid() + Path.GetExtension(img.FileName);
                string picPath = _hostingEnvironment.WebRootPath
                                + @"\images\Banks\"
                                + bank.pic;

                using (var stream = new FileStream(picPath, FileMode.Create))
                {
                    img.CopyTo(stream);
                }
            }
        }

        private async void deleteImgBank(int id)
        {
            var bank = await banks.GetByIdAsync(id);

            if (bank.pic != null)
            {
                string picPath = _hostingEnvironment.WebRootPath
                                + @"\images\Banks\"
                                + bank.pic;

                if (System.IO.File.Exists(picPath))
                {
                    System.IO.File.Delete(picPath);
                }
            }
        }
    }
}