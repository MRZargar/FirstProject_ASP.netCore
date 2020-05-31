using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace DataLayer
{
    public class Sponsor
    {
        [Key]
        public int SponsorID { get; set; }

        [Display(Name = "Gender")]
        public bool isMale { get; set; }

        [Required]
        public int ColleagueID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Phone")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
        public long PhoneNumber { get; set; }
 
        [MaxLength(500)]
        [Display(Name = "Cause of support")]
        public string CauseOfSupport { get; set; }

        [MaxLength(500)]
        [Display(Name = "Other support")]
        public string OtherSupport { get; set; }

        [MaxLength(500)]
        public string picName { get; set; }

        public bool isActive { get; set; }

        public virtual Colleague MyColleague { get; set; }

        public virtual List<SponsorTransaction> MyTransactions { get; set;}

        public Sponsor()
        {

        }
    }
}