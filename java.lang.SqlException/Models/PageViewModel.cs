using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace java.lang.SqlException.Models
{
    public class PageViewModel
    {
        public int Id { get; set; }

        public int PostCount { get; set; }

        public int CircleId { get; set; }

        public String CircleName { get; set; }

        public String CircleType { get; set; }

        public List<PostViewModel> Posts { get; set; }
    }
}