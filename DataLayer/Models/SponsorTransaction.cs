using DataLayer.Models;
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
        public int ColleagueID { get; set; }

        [MaxLength(500)]
        [Display(Name = "Cause of support")]
        public string CauseOfSupport { get; set; }

        [MaxLength(500)]
        [Display(Name = "Other support")]
        public string OtherSupport { get; set; }

        public BankData MyTransaction { get; set; }

        public ReceiptData MyReceipt { get; set; }

        [Display(Name = "Is Valid?")]
        public bool isValid { get; set; }

        public virtual Sponsor MySponsor { get; set; }
        
        public SponsorTransaction()
        {
            isValid = false;
        }
    }
}