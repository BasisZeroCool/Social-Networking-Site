using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace java.lang.SqlException.Models
{
    public class EditCircleViewModel
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "A Circle Name is required.")]
        public string CircleName { get; set; }

        [DisplayName("Type")]
        public string CircleType { get; set; }

        public List<CustomerViewModel> CurrentUsers { get; set; }
        public List<CustomerViewModel> InvitedUsers { get; set; }
        public List<CustomerViewModel> RequestingUsers { get; set; }
        public List<SelectListItem> UsersWhoDoNotBelongToCircle { get; set; }
    }
}