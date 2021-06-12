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
    public class OrderController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Order
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }

        // GET: Order/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Order/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Order/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,OrderDiscount,userid")] Order order)
        {

            if (ModelState.IsValid)
            {
                order.OrderDate = DateTime.Now;
                order.User = db.Users.FirstOrDefault(p => p.Id == order.userid);
                db.Orders.Add(order);
                db.SaveChanges();

                int orderId = order.OrderID;

                List<CartProducts> cp = db.CartProducts.Where(c => c.Cart.User.Id == order.userid).ToList();
                foreach (var item in cp)
                {
                    OrderProducts op = new OrderProducts();
                    
                    op.OrderID = orderId;
                    op.ProductID = item.ProductID;
                    op.ProductQuantity = item.ProductQuantity;
                    op.ProductNewPrice = item.Product.ProductNewPrice;
                    op.ProductOldPrice = item.Product.ProductOldPrice;

                    item.Product.ProductStock -= item.ProductQuantity; 
                    db.OrderProducts.Add(op);
                    db.SaveChanges();
                }

                foreach (var item in cp)
                {
                    db.CartProducts.Remove(item);
                    db.SaveChanges();
                }


                return RedirectToAction("Index", "Home");
            }

            return View(order);
        }

        // GET: Order/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,OrderDiscount")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: Order/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Order/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult userorders()
        {
            string currentUserid = User.Identity.GetUserId();
            ViewBag.Currentuser = db.Users.FirstOrDefault(u=>u.Id == currentUserid); 
            List<Order> CurrentUserOrders = db.Orders.Where(or=>or.User.Id == currentUserid).ToList();
            return View(CurrentUserOrders);
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
