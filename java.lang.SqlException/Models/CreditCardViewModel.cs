using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace java.lang.SqlException.Models
{
    public class CreditCardViewModel
    {
        public int Id { get; set; }
        [DisplayName("Card Number")]
        public string CardNumber { get; set; }
        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }
        public int CustomerId { get; set; }
    }
}
