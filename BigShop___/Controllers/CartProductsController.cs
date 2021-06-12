using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BigShop___.Models;
using Microsoft.AspNet.Identity;

namespace BigShop___.Controllers
{
    public class CartProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CartProducts
        public ActionResult Index()
        {
            var cartProducts = db.CartProducts.Include(c => c.Cart).Include(c => c.Product);
            return View(cartProducts.ToList());
        }

        // GET: CartProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartProducts cartProducts = db.CartProducts.Find(id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            return View(cartProducts);
        }

        // GET: CartProducts/Create
        [Authorize]
        public ActionResult Create(int? id)
        {
            string US = User.Identity.GetUserId();
            Cart cr = db.Carts.FirstOrDefault(a => a.User.Id == US);
            ViewBag.CartID = cr;
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID");
            ViewBag.ProductID = id;
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: CartProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create([Bind(Include = "ProductID,CartID,ProductQuantity")] CartProducts cartProducts, string id)
        {
            if (ModelState.IsValid)
            {
                cartProducts.ProductQuantity = 1;
                cartProducts.CartID = db.Carts.FirstOrDefault(c => c.User.Id == id).CartID;
                db.CartProducts.Add(cartProducts);
                db.SaveChanges();
                //return RedirectToAction("Index");
            }

            //ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartProducts.CartID);
            //ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", cartProducts.ProductID);
            //return View(cartProducts);
        }

        // GET: CartProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CartProducts cartProducts = db.CartProducts.Find(id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartProducts.CartID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", cartProducts.ProductID);
            return View(cartProducts);
        }

        // POST: CartProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,CartID,ProductQuantity")] CartProducts cartProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cartProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CartID = new SelectList(db.Carts, "CartID", "CartID", cartProducts.CartID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", cartProducts.ProductID);
            return View(cartProducts);
        }

        // GET: CartProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            string userid = User.Identity.GetUserId();
            List<CartProducts> Cartproducts = db.CartProducts.Where(cp => cp.Cart.User.Id == userid).ToList();

            CartProducts cartProducts = Cartproducts.FirstOrDefault(p => p.ProductID == id);
            if (cartProducts == null)
            {
                return HttpNotFound();
            }
            return View(cartProducts);
        }
        //التعديل بتاع المسح هنا 


        // POST: CartProducts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        public ActionResult DeleteConfirmed([Bind(Include = "ProductID")] Product pt)
        {
            string userid = User.Identity.GetUserId();
            List<CartProducts> Cartproducts = db.CartProducts.Where(cp => cp.Cart.User.Id == userid).ToList();

            CartProducts cartProducts = Cartproducts.FirstOrDefault(p => p.ProductID == pt.ProductID);
            if (cartProducts.ProductQuantity>1)
            {
                cartProducts.ProductQuantity--;
            }
            else
            {
                db.CartProducts.Remove(cartProducts);
            }
           
            db.SaveChanges();
            return RedirectToAction("addtocart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Deleteall([Bind(Include = "ProductID")] Product pt)
        {
            string userid = User.Identity.GetUserId();
            List<CartProducts> Cartproducts = db.CartProducts.Where(cp => cp.Cart.User.Id == userid).ToList();

            CartProducts cartProducts = Cartproducts.FirstOrDefault(p => p.ProductID == pt.ProductID);

            db.CartProducts.Remove(cartProducts);


            db.SaveChanges();
            return RedirectToAction("addtocart");
        }

        [Authorize]
        [HttpGet]
        public ActionResult addtocart()
        {
            string userid = User.Identity.GetUserId();
            List<CartProducts> Cartproducts = db.CartProducts.Where(cp => cp.Cart.User.Id == userid).ToList();

            return View(Cartproducts);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addtocart([Bind(Include = "ProductID,User_Id,quantity")] Product product_)
        {


            if (product_.User_Id != null)
            {
                CartProducts cartProduct = new CartProducts();

                cartProduct.CartID = db.Carts.FirstOrDefault(c => c.User.Id == product_.User_Id).CartID;
                cartProduct.ProductID = product_.ProductID;


                if (db.CartProducts.Where(c => c.CartID == cartProduct.CartID).FirstOrDefault(x => x.ProductID == product_.ProductID) != null)

                {
                    db.CartProducts.Where(c => c.CartID == cartProduct.CartID).FirstOrDefault(x => x.ProductID == product_.ProductID).ProductQuantity += product_.Quantity;
                    db.Entry(db.CartProducts.FirstOrDefault(c => c.CartID == db.CartProducts.FirstOrDefault(x => x.ProductID == cartProduct.ProductID).CartID)).State = EntityState.Modified;

                }

                else
                {
                    cartProduct.ProductQuantity = 1;
                    db.CartProducts.Add(cartProduct);
                }
                db.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("login", "Account");


        }
      
        public double Subtotal()
        {
            var total = 0;

            string userid = User.Identity.GetUserId();
            List<CartProducts> Cartproducts_ = db.CartProducts.Where(cp => cp.Cart.User.Id == userid).ToList();
            foreach (var item in Cartproducts_)
            {
                total += item.Product.ProductNewPrice * item.ProductQuantity;

            }

            return total;
        }
      
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
