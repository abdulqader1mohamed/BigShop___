using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class UserproductReview
    {
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }
        [Key, Column(Order = 2)]
        public string ApplicationuserID { get; set; }
        public string UserName { get; set; }
        public int ProductRate { get; set; }
        public string UserComment { get; set; }
        public int Commentlikes { get; set; }
        public DateTime CommentDate { get; set; }

    }
}