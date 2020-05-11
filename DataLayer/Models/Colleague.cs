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

        //[Phone]
        [Required]
        //[Index(IsUnique = true)]
        [Display(Name = "Phone")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Not a valid phone number")]
        public long PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Birth Day")]
        public DateTime BirthDay { get; set; }

        [Required]
        [Display(Name = "Start Activity Date")]
        public virtual DateTime StartActivity { get; set; }

        [Display(Name = "Code")]
        public int code { get; set; }      

        [MaxLength(7)]
        [Display(Name = "Color")]
        public String color { get; set; }

        [MaxLength(500)]
        public string picName { get; set; }

        public List<Sponsor> Sponsors { get; set; }

        public Colleague()
        {

        }
    }
}