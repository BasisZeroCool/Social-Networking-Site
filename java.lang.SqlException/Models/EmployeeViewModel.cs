using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }

        [MaxLength(32)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(32)]
        [Required]
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
        public string EmailAddress { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }


        [Required]
        [MaxLength(1)]
        public string Gender { get; set; }


        [Required]
        [MaxLength(50)]
        public string Password { get; set; }


        [Required]
        public int SSN { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public Decimal HourlyRate { get; set; }
        


    }
}
