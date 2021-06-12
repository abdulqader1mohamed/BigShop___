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
    public class WishListProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishListProducts
        public ActionResult Index()
        {
            var wishListsProducts = db.WishListsProducts.Include(w => w.Product).Include(w => w.WishList);
            return View(wishListsProducts.ToList());
        }

        // GET: WishListProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListProducts wishListProducts = db.WishListsProducts.Find(id);
            if (wishListProducts == null)
            {
                return HttpNotFound();
            }
            return View(wishListProducts);
        }

        // GET: WishListProducts/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            ViewBag.WishListID = new SelectList(db.WishLists, "WishListID", "WishListID");
            return View();
        }

        // POST: WishListProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,WishListID")] WishListProducts wishListProducts)
        {
            if (ModelState.IsValid)
            {
                db.WishListsProducts.Add(wishListProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", wishListProducts.ProductID);
            ViewBag.WishListID = new SelectList(db.WishLists, "WishListID", "WishListID", wishListProducts.WishListID);
            return View(wishListProducts);
        }

        // GET: WishListProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListProducts wishListProducts = db.WishListsProducts.Find(id);
            if (wishListProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", wishListProducts.ProductID);
            ViewBag.WishListID = new SelectList(db.WishLists, "WishListID", "WishListID", wishListProducts.WishListID);
            return View(wishListProducts);
        }

        // POST: WishListProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,WishListID")] WishListProducts wishListProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishListProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", wishListProducts.ProductID);
            ViewBag.WishListID = new SelectList(db.WishLists, "WishListID", "WishListID", wishListProducts.WishListID);
            return View(wishListProducts);
        }

        // GET: WishListProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishListProducts wishListProducts = db.WishListsProducts.Find(id);
            if (wishListProducts == null)
            {
                return HttpNotFound();
            }
            return View(wishListProducts);
        }

        // POST: WishListProducts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteConfirmed([Bind(Include = "ProductID")] Product pt)
        {
            string userid = User.Identity.GetUserId();
            List<WishListProducts> Wishlistsproducts = db.WishListsProducts.Where(cp => cp.WishList.User.Id == userid).ToList();

            WishListProducts wishlistproducts = Wishlistsproducts.FirstOrDefault(p => p.ProductID == pt.ProductID);
            db.WishListsProducts.Remove(wishlistproducts);
            db.SaveChanges();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Addtowishlist([Bind(Include = "ProductID,User_Id")] Product product_)
        {

            if (product_.User_Id != null)
            {
                WishListProducts wishlistProducts = new WishListProducts();

                wishlistProducts.WishListID = db.WishLists.FirstOrDefault(c => c.User.Id == product_.User_Id).WishListID;
                wishlistProducts.ProductID = product_.ProductID;


                if (db.WishListsProducts.Where(c => c.WishListID == wishlistProducts.WishListID).FirstOrDefault(x => x.ProductID == product_.ProductID) != null)

                {
                    //db.Entry(db.WishListsProducts.FirstOrDefault(c => c.WishListID == db.WishListsProducts.FirstOrDefault(x => x.ProductID == wishlistProducts.ProductID).WishListID)).State = EntityState.Modified;
                }
                else
                {
                    db.WishListsProducts.Add(wishlistProducts);
                }
                db.SaveChanges();
                return RedirectToAction("index", "Home");
            }
            return RedirectToAction("login", "Account");


        }
        [HttpGet]
        [Authorize]
      
        public ActionResult wishlist()
        {
            string userid = User.Identity.GetUserId();
            List<WishListProducts>  wishListProducts = db.WishListsProducts.Where(cp => cp.WishList.User.Id == userid).ToList();
            return View(wishListProducts);
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
