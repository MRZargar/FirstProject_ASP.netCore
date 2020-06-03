using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer.Models
{
    public class ReceiptData
    {
        [Key]
        public int ReceiptDataID { get; set; }

        [Display(Name = "Date")]
        public DateTime TransactionDate { get; set; }

        [Required]
        [Display(Name = "Receipt Number")]
        public long ReceiptNumber { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public Double Amount { get; set; }
    }
}
