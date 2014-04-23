using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;

namespace java.lang.SqlException.Models
{
    public class SalesReportViewModel
    {
       public List<String> ListOfMonths { get; set; }
       public int SelectedMonth { get; set; }
       public string Company { get; set; }
       public string UnitPrice { get; set; }
       public DateTime DateOfSale { get; set; }
       public int NumUnitsBought { get; set; }
       public int Year { get; set; }
        public int Id { get; set; }
        public int AdId { get; set; }
        public int AccountNumber { get; set; }
    }
}
