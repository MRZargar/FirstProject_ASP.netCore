using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class SponsorTransactionError
    {
        [Key]
        public int ErrorID { get; set; }
        public string SponsorName { get; set; }
        public string Phone { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string ReceiptNumber { get; set; }
        public string CardNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string Amount { get; set; }

        [Required]
        [MaxLength(500)]
        public string ErrorMessage { get; set; }

        [Required]
        public virtual int ColleagueID { get; set; }

        public SponsorTransactionError()
        {

        }
    }

    public enum ErrorMessage
    {
        Phone_number_not_entered,
        This_sponsor_is_related_to_another_colleague,
        There_is_a_problem_when_adding_a_new_sponsor,
        No_transaction_information_entered,
        Correct_the_type_of_input_information,
        Duplicate,
        This_transaction_is_not_available_in_bank_transactions
    }
}