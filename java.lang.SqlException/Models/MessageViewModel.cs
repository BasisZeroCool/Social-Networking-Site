using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class MessageViewModel
    {
        public int Id { get; set; }

        [DisplayName("Subject")]
        [Required]
        public string Subject { get; set; }

        [DisplayName("Message")]
        [Required]
        public string Content { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }

        [DisplayName("From")]
        public string Sender { get; set; }

        [DisplayName("To")]
        public string Receiver { get; set; }

        [DisplayName("To")]
        [Required]
        public int ReceiverId { get; set; }


        public List<SelectListItem> MessageRecipients { get; set; }

    }
}