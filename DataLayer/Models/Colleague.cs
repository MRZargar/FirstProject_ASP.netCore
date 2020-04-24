using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DataLayer
{
    public class Colleague
    {
        [Key]
        public int ColleagueID { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public long PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDay { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public virtual DateTime StartActivity { get; set; }

        public int code { get; set; }      

        public List<Sponsor> Sponsors { get; set; }

        public Colleague()
        {

        }
    }
}