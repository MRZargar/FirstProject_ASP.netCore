using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class SponsorTransaction
    {
        [Key]
        public int SponsorTransactionsID { get; set; }

        [Required]
        public int SponsorID { get; set; }

        [Required]
        [MaxLength(50)]
        public string BankName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string TrackingNumber { get; set; }

        public int LateFourNumbersOfBankCard { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public virtual Sponsor MySponsor { get; set; }
        
        public SponsorTransaction()
        {

        }
    }
}