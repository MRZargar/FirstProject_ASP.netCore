using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Sponsor
    {
        [Key]
        public int SponsorID { get; set; }

        public bool isMale { get; set; }

        [Required]
        public int ColleagueID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public long PhoneNumber { get; set; }
 
        [MaxLength(500)]
        public string CauseOfSupport { get; set; }

        [MaxLength(500)]
        public string OtherSupport { get; set; }

        [MaxLength(500)]
        public string picName { get; set; }

        public virtual Colleague MyColleague { get; set; }

        public virtual List<SponsorTransaction> MyTransactions { get; set;}

        public Sponsor()
        {

        }
    }
}