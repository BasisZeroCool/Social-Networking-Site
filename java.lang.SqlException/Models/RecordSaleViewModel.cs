using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class RecordSaleViewModel
    {
        [Display(Name = "Advertisement")]
        public int AdId { get; set; }
        [Display(Name = "Account")]
        public int AccountId { get; set; }
        [Display(Name = "Num Bought")]
        public int NumUnits { get; set; }

        public List<SelectListItem> Ads { get; set; }
        public List<SelectListItem> Accounts { get; set; }
    }
}