using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BigShop___.Models;
using PagedList;

namespace BigShop___.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            
            var products = db.Products.Where(p => p.IsDeleted == false).Include(p => p.Category);

            return View(products.ToList());
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,ProductStock,ProductRate,ProductOldPrice,ProductNewPrice,ProductDescription,ProductHasOffer,ProductOfferValue,ProductImageURL,CategoryID")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                string imageName = product.ProductID.ToString() + "." + image.FileName.Split('.')[1];
                image.SaveAs(Server.MapPath("~/Images/") + imageName);
                product.ProductImageURL = imageName;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,ProductStock,ProductRate,ProductOldPrice,ProductNewPrice,ProductDescription,ProductHasOffer,ProductOfferValue,ProductImageURL,CategoryID")] Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                string imageName = product.ProductID.ToString() + "." + image.FileName.Split('.')[1];
                image.SaveAs(Server.MapPath("~/Images/") + imageName);
                product.ProductImageURL = imageName;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", product.CategoryID);
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [Authorize(Roles = "Administrator")]
        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            //db.Products.Remove(product);
            product.IsDeleted = true;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult products(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var products = from s in db.Products
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Category.CategoryName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    break;
                case "Date":
                    products = products.OrderBy(s => s.ProductNewPrice);
                    break;
                case "date_desc":
                    products = products.OrderByDescending(s => s.ProductNewPrice);
                    break;
                default:
                    products = products.OrderBy(s => s.ProductID);
                    break;
            }

            int pageSize = 6;
            int pageNumber = (page ?? 1);
            return View(products.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ProductDetails(int id) {
            var product = db.Products.FirstOrDefault(p => p.ProductID == id);

            return View(product);
        }
        public ActionResult Search(string term)
        {
            var names = db.Products.Where(p => p.ProductName.Contains(term)).Select(p => p.ProductName).ToList();
            return Json(names, JsonRequestBehavior.AllowGet);
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
