using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigShop___.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName  { get; set; }
        [Range(1,100)]
        public int ProductStock { get; set; }
        [Range(0, 5)]

        public int ProductRate { get; set; }
        public int ProductOldPrice { get; set; }
        public int ProductNewPrice { get; set; }
        public string ProductDescription { get; set; }
        public bool ProductHasOffer { get; set; }
        public int? ProductOfferValue { get; set; }
        public string ProductImageURL { get; set; }
        public int CategoryID { get; set; }
        [NotMapped]
        public string User_Id { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
        public virtual List<OrderProducts> OrderProducts { get; set; }
        public virtual List<UserproductReview> UserproductReviews{ get; set; }

        public virtual Category Category { get; set; }
        public bool IsDeleted { get; set; }


    }
}