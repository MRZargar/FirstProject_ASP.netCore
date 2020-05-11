using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer
{
    public class SponsorTransaction
    {
        [Key]
        public int SponsorTransactionsID { get; set; }

        [Required]
        public int SponsorID { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Display(Name = "Tracking Naumber")]
        public string TrackingNumber { get; set; }

        [Display(Name = "Caed Number")]
        public int LastFourNumbersOfBankCard { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Is Valid?")]
        public bool isValid { get; set; }

        public virtual Sponsor MySponsor { get; set; }
        
        public SponsorTransaction()
        {

        }
    }
}