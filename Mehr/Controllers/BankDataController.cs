using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using static DataLayer.BankDataRepository;

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

        public async Task<IActionResult> BankDetails(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            BankName bank = findBankName(id);
            if (bank == null)
            {
                return NotFound();
            }

            var bankData = bankDatas.GetAllByBankName(bank);

            ViewBag.BankName = bank;
            ViewBag.ChartData = "[125, 200, 125, 225, 125, 200, 125, 225, 175, 275, 220]";
            return View(bankData);
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

        private BankName? findBankName(string input)
        {
            BankName bankName;
            if (input.ToLower() == BankName.MELLI.ToString().ToLower())
                bankName = BankName.MELLI;
            else if (input.ToLower() == BankName.MELLAT.ToString().ToLower())
                bankName = BankName.MELLAT;
            else if (input.ToLower() == BankName.SADERAT.ToString().ToLower())
                bankName = BankName.SADERAT;
            else
                return null;

            return bankName;
        }
    }
}
