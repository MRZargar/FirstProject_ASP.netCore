using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mehr.Controllers
{
    public class BankTransactionController : Controller
    {
        //// GET: App/BankTransaction/Create
        //public Task<IActionResult> Create()
        //{
        //    return View();
        //}

        //// POST: App/BankTransaction/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(string id, [Bind("SponsorTransactionsID,SponsorID,ColleagueID,CauseOfSupport,OtherSupport,MyTransaction,MyReceipt")] SponsorTransaction sponsorTransaction,
        //                                        string TransactionDate, bool TransactionType)
        //{
        //    return null;
        //    //RedirectToActionResult view;
        //    //if (id == "Colleague")
        //    //{
        //    //    view = RedirectToAction("Details", "Colleague", new { id = sponsorTransaction.ColleagueID });
        //    //}
        //    //else if (id == "Sponsor")
        //    //{
        //    //    view = RedirectToAction("Details", "Sponsor", new { id = sponsorTransaction.SponsorID });
        //    //}
        //    //else
        //    //{
        //    //    ViewBag.err = new NotFoundException();
        //    //    return View("Error");
        //    //}

        //    //if (TransactionType)
        //    //{
        //    //    ModelState["MyTransaction.TrackingNumber"].ValidationState = ModelValidationState.Skipped;
        //    //    ModelState["MyTransaction.Amount"].ValidationState = ModelValidationState.Skipped;
        //    //    ModelState["MyTransaction.LastFourNumbersOfBankCard"].ValidationState = ModelValidationState.Skipped;
        //    //    sponsorTransaction.MyTransaction = null;
        //    //}
        //    //else
        //    //{
        //    //    ModelState["MyReceipt.ReceiptNumber"].ValidationState = ModelValidationState.Skipped;
        //    //    ModelState["MyReceipt.Amount"].ValidationState = ModelValidationState.Skipped;
        //    //    sponsorTransaction.MyReceipt = null;
        //    //}

        //    //if (ModelState.IsValid)
        //    //{
        //    //    try
        //    //    {
        //    //        DateTime date = Convert.ToDateTime(TransactionDate.ToAD());
        //    //        if (TransactionType)
        //    //        {
        //    //            sponsorTransaction.MyReceipt.TransactionDate = date;
        //    //        }
        //    //        else
        //    //        {
        //    //            sponsorTransaction.MyTransaction.TransactionDate = date;
        //    //        }
        //    //    }
        //    //    catch (Exception)
        //    //    {
        //    //        this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
        //    //        return view;
        //    //    }

        //    //    try
        //    //    {
        //    //        await sponsors.InsertTransactionAsync(sponsorTransaction);
        //    //        await sponsors.saveAsync();
        //    //        this.SetViewMessage("A new transaction was registered successfully.", WebMessageType.Success);
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        this.SetViewMessage(ex.Message, WebMessageType.Danger);
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
        //    //}
        //    //return view;
        //}
    }
}