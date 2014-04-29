using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class CircleViewModel
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "A Circle Name is required.")]
        public string CircleName { get; set; }

        [DisplayName("Type")]
        public string CircleType { get; set; }

        [DisplayName("Owner")]
        public string OwnerName { get; set; }

    }
}