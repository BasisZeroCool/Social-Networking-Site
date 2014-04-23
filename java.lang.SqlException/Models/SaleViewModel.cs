using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Account to Use")]
        public int AccountId { get; set; }
        [Required]
        [Display(Name = "Advertisement")]
        public int AdId { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage="You must buy at least 1")]
        [Display(Name = "Num to Buy")]
        public int Num { get; set; }

        public AdViewModel Ad { get; set; }
        public List<SelectListItem> Accounts { get; set; }
    }
}