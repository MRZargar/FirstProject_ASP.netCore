using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using static DataLayer.BankDataRepository;
using Mehr.Classes;
using DataLayer.Exceptions;

namespace Mehr.Controllers
{
    public class BankDataController : Controller
    {

        private IBankDataRepository bankDatas;
        public BankDataController(MyContext context)
        {
            bankDatas = new BankDataRepository(context);
        }

        // GET: App/BankData
        public async Task<IActionResult> Index()
        {
            return View(await bankDatas.GetAllAsync());
        }


        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            ViewBag.BankID = id;
            return View();
        }

        // POST: App/BankData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BankDataID,BankID,TrackingNumber,LastFourNumbersOfBankCard,Amount")] BankData bankData, string TransactionDate)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bankData.TransactionDate = Convert.ToDateTime(TransactionDate.ToAD());
                }
                catch (Exception ex)
                {
                    this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
                    return RedirectToAction("Details", "Bank", new { id = bankData.BankID });
                }

                try
                {
                    await bankDatas.InsertAsync(bankData);
                    await bankDatas.saveAsync();
                    this.SetViewMessage("A new transaction was registered successfully.", WebMessageType.Success);

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
            return RedirectToAction("Details", "Bank", new { id = bankData.BankID });
        }

        // GET: App/BankData/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            BankData bankData;
            try
            {
                bankData = await bankDatas.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }
            
            return View(bankData);
        }

        // POST: App/BankData/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BankDataID,BankName,TransactionDate,TrackingNumber,LateFourNumbersOfBankCard,Amount")] BankData bankData)
        {
            if (id != bankData.BankDataID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await bankDatas.UpdateAsync(bankData);
                    await bankDatas.saveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BankDataExists(bankData.BankDataID))
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
            return View(bankData);
        }

        // GET: App/BankData/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bankData = await bankDatas.GetByIdAsync(id.Value);
            if (bankData == null)
            {
                return NotFound();
            }

            return View(bankData);
        }

        // POST: App/BankData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await bankDatas.DeleteAsync(id);
            await bankDatas.saveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BankDataExists(int id)
        {
            return bankDatas.GetByIdAsync(id) != null;
        }

        
    }
}
