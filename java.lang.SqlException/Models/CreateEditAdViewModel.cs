using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class CreateEditAdViewModel
    {
        public int Id { get; set; }
        public string Company { get; set; }
        [Display(Name = "Number Available")]
        public int NumUnits { get; set; }
        [Display(Name = "Number Sold")]
        public int NumSold { get; set; }
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
        [Display(Name = "Ad Type")]
        public string AdType { get; set; }
        public string Content { get; set; }

    }
}