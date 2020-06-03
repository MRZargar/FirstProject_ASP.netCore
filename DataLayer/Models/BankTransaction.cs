using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class BankTransaction
    {
        [Key]
        public int BankTransactionID { get; set; }

        public BankData Transaction { get; set; }

        public BankTransaction()
        {

        }
    }
}