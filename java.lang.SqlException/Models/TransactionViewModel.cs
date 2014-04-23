using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class TransactionViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Date of Sale")]
        public DateTime DateOfSale { get; set; }
        [Display(Name = "Number Bought")]
        public int NumberOfUnits { get; set; }
        [Display(Name = "Account")]
        public int AccountNumber { get; set; }
        [Display(Name = "Advertisement")]
        public int AdId { get; set; }
        public string Username { get; set; }

        public bool SearchByUserName { get; set; }
        public bool SearchByItemName { get; set; }
        public string ItemName { get; set; }

    }
}
