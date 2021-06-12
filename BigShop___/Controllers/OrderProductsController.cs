using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BigShop___.Models;

namespace BigShop___.Controllers
{
    public class OrderProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: OrderProducts
        public ActionResult Index()
        {
            var orderProducts = db.OrderProducts.Include(o => o.Order).Include(o => o.Product);
            return View(orderProducts.ToList());
        }

        // GET: OrderProducts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProducts orderProducts = db.OrderProducts.Find(id);
            if (orderProducts == null)
            {
                return HttpNotFound();
            }
            return View(orderProducts);
        }

        // GET: OrderProducts/Create
        public ActionResult Create()
        {
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID");
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName");
            return View();
        }

        // POST: OrderProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,OrderID,ProductQuantity")] OrderProducts orderProducts)
        {
            if (ModelState.IsValid)
            {
                db.OrderProducts.Add(orderProducts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderProducts.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderProducts.ProductID);
            return View(orderProducts);
        }

        // GET: OrderProducts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProducts orderProducts = db.OrderProducts.Find(id);
            if (orderProducts == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderProducts.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderProducts.ProductID);
            return View(orderProducts);
        }

        // POST: OrderProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,OrderID,ProductQuantity")] OrderProducts orderProducts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderProducts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrderID = new SelectList(db.Orders, "OrderID", "OrderID", orderProducts.OrderID);
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", orderProducts.ProductID);
            return View(orderProducts);
        }

        // GET: OrderProducts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProducts orderProducts = db.OrderProducts.Find(id);
            if (orderProducts == null)
            {
                return HttpNotFound();
            }
            return View(orderProducts);
        }

        // POST: OrderProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderProducts orderProducts = db.OrderProducts.Find(id);
            db.OrderProducts.Remove(orderProducts);
            db.SaveChanges();
            return RedirectToAction("Index");
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
