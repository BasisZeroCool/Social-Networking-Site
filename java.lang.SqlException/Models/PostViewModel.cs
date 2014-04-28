using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace java.lang.SqlException.Models
{
    public class PostViewModel
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }

        [DisplayName("Content")]
        public string Content { get; set; }
        
        public int PageId { get; set; }

        public int AuthorId { get; set; }

        public String Author { get; set; }

        public List<CommentViewModel> Comments { get; set; }

        public bool UserLikes {get; set;}

        public bool UserCanEdit { get; set; }

    }
}