using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
//using Toolbelt.ComponentModel.DataAnnotations.Schema;

namespace DataLayer
{
    public class Colleague
    {
        [Key]
        public int ColleagueID { get; set; }

        [Display(Name = "Gender")]
        public bool isMale { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Phone]
        [Required]
        //[Index(IsUnique = true)]
        [Display(Name = "Phone")]
        public long PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Birth Day")]
        public DateTime BirthDay { get; set; }

        [Required]
        [Display(Name = "Start activity date")]
        public virtual DateTime StartActivity { get; set; }

        public int code { get; set; }      

        [MaxLength(7)]
        public String color { get; set; }

        [MaxLength(500)]
        public string picName { get; set; }

        public List<Sponsor> Sponsors { get; set; }

        public Colleague()
        {

        }
    }
}