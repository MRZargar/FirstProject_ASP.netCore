using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class BankTransaction
    {
        [Key]
        public int BankTransactionID { get; set; }

        [Required]
        public int BankID { get; set; }

        public BankData Transaction { get; set; }

        public virtual Bank MyBank { get; set; }

        public BankTransaction()
        {

        }
    }
}