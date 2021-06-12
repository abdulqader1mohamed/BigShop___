using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public bool isAvailable { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}