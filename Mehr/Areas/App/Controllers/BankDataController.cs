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

        // GET: App/BankData/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: App/BankData/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: App/BankData/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BankDataID,BankName,TransactionDate,TrackingNumber,LateFourNumbersOfBankCard,Amount")] BankData bankData)
        {
            if (ModelState.IsValid)
            {
                await bankDatas.InsertAsync(bankData);
                await bankDatas.saveAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bankData);
        }

        // GET: App/BankData/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
                    bankDatas.Update(bankData);
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
