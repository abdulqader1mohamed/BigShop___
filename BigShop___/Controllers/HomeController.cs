using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

using BigShop___.Models;
using Microsoft.AspNet.Identity;

namespace BigShop___.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            
            var products = db.Products.Where(p => p.IsDeleted == false).Include(p => p.Category);
            ViewBag.categories = db.Categories.ToList();
            return View(products.ToList());
        }

        public ActionResult aboutus()
        {

            return View();
        }
        public ActionResult contactus()
        {
            return View();
        }
        public ActionResult search(string id)
        {
            Product product = db.Products.FirstOrDefault(n => n.ProductName.Equals(id));

            if (product == null)
            {
                return View("PageNotFound");
            }
            return RedirectToAction("productdetails", "product", new { id = product.ProductID });
        }


        public ActionResult paymentpage()
        {
            string currentuser = User.Identity.GetUserId();
         List <CartProducts> cartproducts = db.CartProducts.Where(c => c.Cart.User.Id == currentuser).ToList();

            return View(cartproducts);
        }
    }
}