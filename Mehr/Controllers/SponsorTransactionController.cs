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
using Microsoft.AspNetCore.Hosting;
using DataLayer.Exceptions;
using System.Transactions;

namespace Mehr.Controllers
{
    public class SponsorTransactionController : Controller
    {
        private ISponsorTransactionRepository transactions;
        private ISponsorRepository sponsors;
        private IColleageRepository colleages;
        private readonly IHostingEnvironment _hostingEnvironment;


        public SponsorTransactionController(IHostingEnvironment hostingEnvironment, MyContext context)
        {
            transactions = new SponsorTransactionRepository(context);
            sponsors = new SponsorRepository(context);
            colleages = new ColleagueRepository(context);
            _hostingEnvironment = hostingEnvironment;
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
        public async Task<IActionResult> Create(string id, [Bind("SponsorTransactionsID,SponsorID,TrackingNumber,LastFourNumbersOfBankCard,Amount,CauseOfSupport,OtherSupport")] SponsorTransaction sponsorTransaction,
                                                string TransactionDate)
        {
            RedirectToActionResult view;
            if (id == "Colleague")
            {
                Sponsor s = await sponsors.GetByIdAsync(sponsorTransaction.SponsorID);
                view = RedirectToAction("Details", "Colleague", new { id = s.ColleagueID });
            }
            else if (id == "Sponsor")
            {
                view = RedirectToAction("Details", "Sponsor", new { id = sponsorTransaction.SponsorID });
            }
            else
            {
                ViewBag.err = new NotFoundException();
                return View("Error");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    sponsorTransaction.TransactionDate = Convert.ToDateTime(TransactionDate.ToAD());
                }
                catch (Exception)
                {
                    this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
                    return view;
                }

                try
                {
                    await transactions.InsertAsync(sponsorTransaction);
                    await transactions.saveAsync();
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
            return view;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(int ColleagueID, IFormFile XLSXfile)
        {
            if (XLSXfile == null)
            {
                return RedirectToAction("Details", "Colleague", new { id = ColleagueID });
            }

            string path = saveFile(XLSXfile);

            using (DataTable dt = Excel.Read(path))
            {
                // Phone
                // SponsorName
                // Amount
                // CardNumber
                // TrackingNumber
                // Date
                // Time
                foreach (DataRow row in dt.Rows)
                {
                    if (row["Phone"].ToString() == string.Empty)
                    {
                        continue;
                    }

                    var st = new SponsorTransaction();

                    long phoneNumber = long.Parse(row["Phone"].ToString());

                    Sponsor mySponsor;
                    try
                    {
                        mySponsor = await sponsors.GetByPhoneNumberAsync(phoneNumber);
                    }
                    catch (NotFoundException)
                    {
                        mySponsor = new Sponsor();
                        mySponsor.ColleagueID = ColleagueID;
                        mySponsor.PhoneNumber = phoneNumber;
                        mySponsor.Name = row["SponsorName"].ToString();
                        if (mySponsor.Name == string.Empty)
                        {
                            mySponsor.Name = "Undefine";
                        }
                        mySponsor.MyColleague = await colleages.GetByIdAsync(ColleagueID);

                        await sponsors.InsertAsync(mySponsor);
                        await sponsors.saveAsync();

                        mySponsor = await sponsors.GetByPhoneNumberAsync(phoneNumber);
                        if (mySponsor == null)
                        {
                            continue;
                        }
                    }

                    try
                    {
                        st.Amount = Convert.ToDecimal(row["Amount"]);
                        st.LastFourNumbersOfBankCard = Convert.ToInt16(row["CardNumber"]);
                        st.TrackingNumber = row["TrackingNumber"].ToString();
                        DateTime date = Convert.ToDateTime(row["Date"].ToString()).Date;
                        TimeSpan time;
                        try
                        {
                            time = TimeSpan.Parse(row["Time"].ToString());
                        }
                        catch (Exception)
                        {
                            time = Convert.ToDateTime(row["Time"].ToString()).TimeOfDay;
                        }
                        st.TransactionDate = date + time;
                        st.SponsorID = mySponsor.SponsorID;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    
                    await transactions.InsertAsync(st);
                }

                await transactions.saveAsync();
            }
            deleteFile(path);

            return RedirectToAction( "Details","Colleague", new {id = ColleagueID});
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Export()
        //{
        //    return;
        //}

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

        private string saveFile(IFormFile file)
        {
            string path = string.Empty;
            if (file!= null)
            {
                string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                path = _hostingEnvironment.WebRootPath
                                + @"\Catch\"
                                + fileName;

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            return path;
        }

        private void deleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
