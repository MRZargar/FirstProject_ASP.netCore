using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using System.Data;
using Mehr.Classes;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Mehr.Controllers
{
    public class SponsorTransactionController : Controller
    {
        private ISponsorTransactionRepository transactions;
        private ISponsorRepository sponsors;
        private IColleageRepository colleages;

        public SponsorTransactionController(MyContext context)
        {
            transactions = new SponsorTransactionRepository(context);
            sponsors = new SponsorRepository(context);
            colleages = new ColleagueRepository(context);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createzar(string BankName, int ColleagueID, IFormFile XLSXfile)
        {
            if (XLSXfile == null)
            {
                return RedirectToAction("Home", "Index");
            }

            using (var stream = new MemoryStream())
            {
                await XLSXfile.CopyToAsync(stream);

                DataTable dt = Excel.Read(stream);
                foreach (DataRow row in dt.Rows)
                {
                    var st = new SponsorTransaction();

                    long phoneNumber = long.Parse(row["Phone"].ToString());
                    Sponsor? mySponsor = await sponsors.GetByPhoneNumberAsync(phoneNumber);

                    if (mySponsor == null)
                    {
                        mySponsor = new Sponsor();
                        mySponsor.ColleagueID = ColleagueID;
                        mySponsor.PhoneNumber = phoneNumber;
                        mySponsor.Name = row["SponsorName"].ToString();
                        mySponsor.MyColleague = await colleages.GetByIdAsync(ColleagueID);

                        await sponsors.InsertAsync(mySponsor);
                        await sponsors.saveAsync();

                        mySponsor = await sponsors.GetByPhoneNumberAsync(phoneNumber);
                        if (mySponsor == null)
                        {
                            continue;
                        }
                    }
                    
                    st.isValid = false;
                    st.Amount = Convert.ToDecimal(row["Amount"]);
                    st.LastFourNumbersOfBankCard = Convert.ToInt16(row["CardNumber"]);
                    st.TrackingNumber = row["TrackingNumber"].ToString();
                    DateTime date = Convert.ToDateTime(row["Date"].ToString());
                    TimeSpan time = TimeSpan.Parse(row["Time"].ToString());
                    st.TransactionDate = date + time;
                    st.SponsorID = mySponsor.SponsorID;
                    await transactions.InsertAsync(st);
                }
            }
            await transactions.saveAsync();
            return RedirectToAction( "Details","Colleague", new {id = ColleagueID});
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
                    await transactions.UpdateAsync(sponsorTransaction);
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
