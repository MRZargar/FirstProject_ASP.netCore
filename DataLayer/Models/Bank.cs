using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Bank
    {
        [Key]
        public int BankID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid Account number")]
        public long AccountNumber { get; set; }

        [Required]
        [Display(Name = "Card Number")]
        public long CardNumber { get; set; }

        [Required]
        [MaxLength(26)]
        [Display(Name = "Sheba Number")]
        public string ShebaNumber { get; set; }

        [MaxLength(500)]

        [Display(Name = "Bank Picture")]
        public string pic { get; set; }

        public virtual List<BankTransaction> Transactions { get; set; }

        public Bank()
        {

        }
    }
}