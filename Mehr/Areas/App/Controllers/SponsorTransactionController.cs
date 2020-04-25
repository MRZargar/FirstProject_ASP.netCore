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
    public class SponsorTransactionController : Controller
    {
        private ISponsorTransactionRepository transactions;
        private ISponsorRepository sponsors;

        public SponsorTransactionController(MyContext context)
        {
            transactions = new SponsorTransactionRepository(context);
            sponsors = new SponsorRepository(context);
        }

        // GET: App/SponsorTransaction
        public async Task<IActionResult> Index()
        {
            return View(await transactions.GetAllAsync());
        }

        // GET: App/SponsorTransaction/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorTransaction = await transactions.GetByIdAsync(id.Value);
            if (sponsorTransaction == null)
            {
                return NotFound();
            }

            return View(sponsorTransaction);
        }

        // GET: App/SponsorTransaction/Create
        public async Task<IActionResult> CreateAsync()
        {
            ViewData["SponsorID"] = new SelectList(await sponsors.GetAllAsync(), "SponsorID", "Name");
            return View();
        }

        // POST: App/SponsorTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SponsorTransactionsID,SponsorID,BankName,TransactionDate,TrackingNumber,LateFourNumbersOfBankCard,Amount")] SponsorTransaction sponsorTransaction)
        {
            if (ModelState.IsValid)
            {
                await transactions.InsertAsync(sponsorTransaction);
                await transactions.saveAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SponsorID"] = new SelectList(await sponsors.GetAllAsync(), "SponsorID", "Name", sponsorTransaction.SponsorID);
            return View(sponsorTransaction);
        }

        // GET: App/SponsorTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorTransaction = await transactions.GetByIdAsync(id.Value);
            if (sponsorTransaction == null)
            {
                return NotFound();
            }
            ViewData["SponsorID"] = new SelectList(await sponsors.GetAllAsync(), "SponsorID", "Name", sponsorTransaction.SponsorID);
            return View(sponsorTransaction);
        }

        // POST: App/SponsorTransaction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SponsorTransactionsID,SponsorID,BankName,TransactionDate,TrackingNumber,LateFourNumbersOfBankCard,Amount")] SponsorTransaction sponsorTransaction)
        {
            if (id != sponsorTransaction.SponsorTransactionsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    transactions.Update(sponsorTransaction);
                    await transactions.saveAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SponsorTransactionExists(sponsorTransaction.SponsorTransactionsID))
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
            ViewData["SponsorID"] = new SelectList(await sponsors.GetAllAsync(), "SponsorID", "Name", sponsorTransaction.SponsorID);
            return View(sponsorTransaction);
        }

        // GET: App/SponsorTransaction/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorTransaction = await transactions.GetByIdAsync(id.Value);
            if (sponsorTransaction == null)
            {
                return NotFound();
            }

            return View(sponsorTransaction);
        }

        // POST: App/SponsorTransaction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await transactions.DeleteAsync(id);
            await transactions.saveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorTransactionExists(int id)
        {
            return transactions.GetByIdAsync(id) != null;
        }
    }
}
