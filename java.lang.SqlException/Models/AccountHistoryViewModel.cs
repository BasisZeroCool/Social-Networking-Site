using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class AccountHistoryViewModel
    {
        [Display(Name ="Date of Sale")]
        public DateTime DateOfSale {get; set;}
        [Display(Name = "Item Name")]
        public String ItemName { get; set; }
        public String Company { get; set; }
        [Display(Name = "Number Bought")]
        public int NumBought { get; set; }
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; }
    }
}