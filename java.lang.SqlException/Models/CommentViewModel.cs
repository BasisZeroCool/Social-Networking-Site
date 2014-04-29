using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace java.lang.SqlException.Models
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        [DisplayName("Content")]
        public string Content { get; set; }

        public int PostId { get; set; }

        public int AuthorId { get; set; }

        public String Author { get; set; }

        public bool UserLikes { get; set; }

        public bool UserCanEdit { get; set; }
    }
}
