using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class OrderProducts
    {
        [Key, Column(Order = 1)]
        public int ProductID { get; set; }

        [Key, Column(Order = 2)]
        public int OrderID { get; set; }

        public int ProductOldPrice { get; set; }
        public int ProductNewPrice { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
        public int ProductQuantity { get; set; }

    }
}