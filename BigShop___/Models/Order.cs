using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class Order
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual List<OrderProducts> OrderProducts { get; set; }
        //public virtual List<Product> Products { get; set; }
        public int OrderDiscount { get; set; }
        [NotMapped]
        public string userid { get; set; }
    }
}