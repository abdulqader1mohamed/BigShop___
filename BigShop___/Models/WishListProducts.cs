using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class WishListProducts
    {
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        [Key, Column(Order = 2)]
        public int WishListID { get; set; }
        public virtual Product Product { get; set; }
        public virtual WishList WishList { get; set; }

    }
}