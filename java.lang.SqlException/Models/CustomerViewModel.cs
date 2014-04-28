using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        [DisplayName("First Name")]
        public String FirstName { get; set; }
        [DisplayName("Last Name")]
        public String LastName { get; set; }
        [DisplayName("Uername")]
        public String Username { get; set; }
        public int? Rating { get; set; }
    }
}