using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class Cart
    {
        public int CartID { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual List<Product> Products { get; set; }
        public int Discount { get; set; }
        
        [NotMapped]
        public string userId { get; set; }

    }
}