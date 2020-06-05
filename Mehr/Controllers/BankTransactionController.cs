using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer;
using DataLayer.Exceptions;
using Mehr.Classes;
using Microsoft.AspNetCore.Mvc;

namespace Mehr.Controllers
{
    public class BankTransactionController : Controller
    {
        public IBankRepository banks;

        public BankTransactionController(MyContext context)
        {
            banks = new BankRepository(context);
        }

        // GET: App/BankTransaction/Create
        public IActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.err = new NotFoundException();
            }

            ViewBag.BankID = id.Value;
            return View();
        }

        // POST: App/BankTransaction/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BankTransactionID,BankID,Transaction")] BankTransaction bankTransaction,
                                                string TransactionDate)
        {
            var view = RedirectToAction("Details", "Bank", new { id = bankTransaction.BankID });

            if (ModelState.IsValid)
            {
                try
                {
                    bankTransaction.Transaction.TransactionDate = Convert.ToDateTime(TransactionDate.ToAD());
                }
                catch (Exception)
                {
                    this.SetViewMessage("Please Complete fields ...", WebMessageType.Warning);
                    return view;
                }

                try
                {
                    await banks.InsertTransactionAsync(bankTransaction);
                    await banks.saveAsync();
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
    }
}