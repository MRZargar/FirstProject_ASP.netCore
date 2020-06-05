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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using DataLayer.Models;

namespace Mehr.Controllers
{
    public class SponsorTransactionController : Controller
    {
        private ISponsorRepository sponsors;
        private IColleageRepository colleages;
        private readonly IHostingEnvironment _hostingEnvironment;


        public SponsorTransactionController(IHostingEnvironment hostingEnvironment, MyContext context)
        {
            sponsors = new SponsorRepository(context);
            colleages = new ColleagueRepository(context);
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: App/SponsorTransaction/Create
        public async Task<IActionResult> CreateAsync(int? id, string RedirectTo)
        {
            if (id == null)
            {
                return View();
            }

            Colleague c;
            try
            {
                c = await colleages.GetByIdAsync(id.Value);
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            ViewBag.ColleagueID = id.Value;
            ViewBag.RedirectTo = RedirectTo;
            ViewData["SponsorID"] = new SelectList(c.Sponsors, "SponsorID", "Name");
            return View();
        }

        // POST: App/SponsorTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("SponsorTransactionsID,SponsorID,ColleagueID,CauseOfSupport,OtherSupport,MyTransaction,MyReceipt")] SponsorTransaction sponsorTransaction,
                                                string TransactionDate, bool TransactionType)
        {
            RedirectToActionResult view;
            if (id == "Colleague")
            {
                view = RedirectToAction("Details", "Colleague", new { id = sponsorTransaction.ColleagueID });
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

            if (TransactionType)
            {
                ModelState["MyTransaction.TrackingNumber"].ValidationState = ModelValidationState.Skipped;
                ModelState["MyTransaction.Amount"].ValidationState = ModelValidationState.Skipped;
                ModelState["MyTransaction.LastFourNumbersOfBankCard"].ValidationState = ModelValidationState.Skipped;
                sponsorTransaction.MyTransaction = null;
            }
            else
            {
                ModelState["MyReceipt.ReceiptNumber"].ValidationState = ModelValidationState.Skipped;
                ModelState["MyReceipt.Amount"].ValidationState = ModelValidationState.Skipped;
                sponsorTransaction.MyReceipt = null;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DateTime date = Convert.ToDateTime(TransactionDate.ToAD());
                    if (TransactionType)
                    {
                        sponsorTransaction.MyReceipt.TransactionDate = date;
                    }
                    else
                    {
                        sponsorTransaction.MyTransaction.TransactionDate = date;
                    }
                }
                catch (Exception)
                {
                    this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
                    return view;
                }

                try
                {
                    await sponsors.InsertTransactionAsync(sponsorTransaction);
                    await sponsors.saveAsync();
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
                DataTable Errors = dt.Clone();
                var ErrorMessages = new List<ErrorMessage>();

                foreach (DataRow row in dt.Rows)
                {
                    if (row["Phone"].ToString() == string.Empty)
                    {
                        Errors.Rows.Add(row.ItemArray);
                        ErrorMessages.Add(ErrorMessage.Phone_number_not_entered);
                        continue;
                    }

                    var st = new SponsorTransaction();

                    long phoneNumber = long.Parse(row["Phone"].ToString());

                    Sponsor mySponsor;
                    try
                    {
                        mySponsor = await sponsors.GetByPhoneNumberAsync(phoneNumber);
                        
                        if (mySponsor.ColleagueID != ColleagueID)
                        {
                            Errors.Rows.Add(row.ItemArray);
                            ErrorMessages.Add(ErrorMessage.This_sponsor_is_related_to_another_colleague);
                            continue;
                        }
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
                            Errors.Rows.Add(row.ItemArray);
                            ErrorMessages.Add(ErrorMessage.There_is_a_problem_when_adding_a_new_sponsor);
                            continue;
                        }
                    }

                    try
                    {
                        st.SponsorID = mySponsor.SponsorID;
                        st.ColleagueID = ColleagueID;

                        DateTime date = Convert.ToDateTime(row["Date"].ToString().ToAD()).Date;
                        TimeSpan time;
                        try
                        {
                            time = TimeSpan.Parse(row["Time"].ToString());
                        }
                        catch (Exception)
                        {
                            time = Convert.ToDateTime(row["Time"].ToString()).TimeOfDay;
                        }

                        if (row["CardNumber"].ToString() != "" && row["TrackingNumber"].ToString() != "")
                        {
                            st.MyTransaction = new BankData();
                            st.MyTransaction.Amount = Convert.ToDouble(row["Amount"]);
                            st.MyTransaction.LastFourNumbersOfBankCard = Convert.ToInt16(row["CardNumber"]);
                            st.MyTransaction.TrackingNumber = row["TrackingNumber"].ToString();
                            st.MyTransaction.TransactionDate = date + time;
                        }
                        else if (row["ReceiptNumber"].ToString() != "")
                        {
                            st.MyReceipt = new ReceiptData();
                            st.MyReceipt.Amount = Convert.ToDouble(row["Amount"]);
                            st.MyReceipt.TransactionDate = date + time;
                        }
                        else
                        {
                            Errors.Rows.Add(row.ItemArray);
                            ErrorMessages.Add(ErrorMessage.No_transaction_information_entered);
                            continue;
                        }   
                    }
                    catch (Exception)
                    {
                        Errors.Rows.Add(row.ItemArray);
                        ErrorMessages.Add(ErrorMessage.Correct_the_type_of_input_information);
                        continue;
                    }

                    try
                    {
                        await sponsors.InsertTransactionAsync(st);
                    }
                    catch (DuplicateTransactionException)
                    {
                        Errors.Rows.Add(row.ItemArray);
                        ErrorMessages.Add(ErrorMessage.Duplicate);
                        continue;
                    }
                }
                await sponsors.saveAsync();

                for (int i = 0; i < ErrorMessages.Count; i++)
                {
                    var err = new SponsorTransactionError();
                    DataRow row = dt.Rows[i];
                    err.SponsorName = row["SponsorName"].ToString();
                    err.Phone = row["Phone"].ToString();
                    err.Date = row["Date"].ToString();
                    err.Time = row["Time"].ToString();
                    err.ReceiptNumber = row["ReceiptNumber"].ToString();
                    err.CardNumber = row["CardNumber"].ToString();
                    err.TrackingNumber = row["TrackingNumber"].ToString();
                    err.Amount = row["Amount"].ToString();
                    err.ErrorMessage = ErrorMessages[i].ToString().Replace('_', ' ');
                    err.ColleagueID = ColleagueID;

                    await colleages.InsertErrorAsync(err);
                }
                await colleages.saveAsync();
            }
            deleteFile(path);

            return RedirectToAction( "Details","Colleague", new {id = ColleagueID});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Export(int? ColleagueID, int? SponsorID, string FromDate, string ToDate)   
        {
            string path = _hostingEnvironment.WebRootPath + @"\Catch\";
            string fileName = string.Empty;
            DateTime From = new DateTime();
            DateTime To = new DateTime();
            DataTable dt = new DataTable();

            dt.Columns.Add("SponsorName");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Date");
            dt.Columns.Add("Time");
            dt.Columns.Add("ReceiptNumber");
            dt.Columns.Add("CardNumber");
            dt.Columns.Add("TrackingNumber");
            dt.Columns.Add("Amount");

            try
            {
                this.GetFromTo_default_FirstMonthToNow(ref From, ref To, FromDate.ToSolar(), ToDate.ToSolar());
            }
            catch (Exception ex)
            {
                ViewBag.err = ex;
                return View("Error");
            }

            IEnumerable<SponsorTransaction> transactions;
            if (ColleagueID != null)
            {
                transactions = colleages.GetFromToTransactionByColleagueIdAsync(ColleagueID.Value, From, To);
                fileName += colleages.GetByIdAsync(ColleagueID.Value).Result.Name.ToString();
            }
            else if (SponsorID != null)
            {
                transactions = await sponsors.GetFromToTransactionBySponsorIdAsync(SponsorID.Value, From, To);
                fileName += transactions.First().MySponsor.Name.ToString();
            }
            else
            {
                transactions = await sponsors.GetFromToTransactionAsync(From, To);
                fileName += "_";
            }

            fileName += "_From" + From.ToSolar().Replace('/', '.') + "To" + To.ToSolar().Replace('/','.') + ".xlsx";

            foreach (var trnsctn in transactions)
            {
                DataRow row = dt.NewRow();

                row["SponsorName"] = trnsctn.MySponsor.Name.ToString();
                row["Phone"] = trnsctn.MySponsor.PhoneNumber.ToString();
                row["Date"] = trnsctn.MyTransaction?.TransactionDate.ToSolar() ?? trnsctn.MyReceipt.TransactionDate.ToSolar();
                row["Time"] = trnsctn.MyTransaction?.TransactionDate.TimeOfDay.ToString() ?? trnsctn.MyReceipt.TransactionDate.TimeOfDay.ToString();
                row["ReceiptNumber"] = trnsctn.MyReceipt?.ReceiptNumber.ToString() ?? string.Empty;
                row["CardNumber"] = trnsctn.MyTransaction?.LastFourNumbersOfBankCard.ToString() ?? string.Empty;
                row["TrackingNumber"] = trnsctn.MyTransaction?.TrackingNumber.ToString() ?? string.Empty;
                row["Amount"] = trnsctn.MyTransaction?.Amount.ToString() ?? trnsctn.MyReceipt.Amount.ToString();

                dt.Rows.Add(row);
            }

            Excel.Write(path + fileName, dt);
            return null;
        }

        // GET: App/SponsorTransaction/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sponsorTransaction = await sponsors.GetTransactionByIdAsync(id.Value);
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
                    await sponsors.UpdateTransactionAsync(sponsorTransaction);
                    await sponsors.saveAsync();
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

            var sponsorTransaction = await sponsors.GetTransactionByIdAsync(id.Value);
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
            await sponsors.DeleteTransactionAsync(id);
            await sponsors.saveAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SponsorTransactionExists(int id)
        {
            return sponsors.GetTransactionByIdAsync(id) != null;
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
