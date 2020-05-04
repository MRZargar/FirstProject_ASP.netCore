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
        public string BankName { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TrackingNumber { get; set; }

        public int LateFourNumbersOfBankCard { get; set; }

        [Required]
        public Double Amount { get; set; }
        
        public BankData()
        {

        }
    }
}