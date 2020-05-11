using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class BankData
    {
        [Key]
        public int BankDataID { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Tracking Number")]
        public string TrackingNumber { get; set; }

        [Display(Name = "Card Number")]
        public int LastFourNumbersOfBankCard { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public Double Amount { get; set; }
        
        public BankData()
        {

        }
    }
}