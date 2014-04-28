using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class ProfileViewModel
    {
        [Required]
        public int Id { get; set; }

        public bool IsCustomer { get; set; }
        public bool IsManager { get; set; }
        public bool IsCustRep { get; set; }

        [MaxLength(32)]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MaxLength(32)]
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [MaxLength(128)]
        [Required]
        public string Address { get; set; }

        [MaxLength(128)]
        [Required]
        public string City { get; set; }

        [MaxLength(2)]
        [Required]
        public string State { get; set; }


        [MaxLength(5)]
        [Required]
        public string ZipCode { get; set; }

        [MaxLength(10)]
        [Required]
        public string Telephone { get; set; }


        [Required]
        [MaxLength(128)]
        [Display(Name = "Email")]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "DOB")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(1)]
        public string Gender { get; set; }

        [Required]
        public string Password { get; set; }


        public int? Rating { get; set; }

        public string Preferences { get; set; }

        public int SSN { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Salary")]
        public Decimal HourlyRate { get; set; }
        
        public List<CreditCardViewModel> Accounts {get; set;}
    }
}