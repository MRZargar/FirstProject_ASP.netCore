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

        [MaxLength(500)]
        [Display(Name = "Cause of support")]
        public string CauseOfSupport { get; set; }

        [MaxLength(500)]
        [Display(Name = "Other support")]
        public string OtherSupport { get; set; }

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
        public int isValid { get; set; }
        // 0 --> not valid
        // 1 --> valid
        // -1 --> repeated

        public virtual Sponsor MySponsor { get; set; }
        
        public SponsorTransaction()
        {
            isValid = 0;
        }
    }
}