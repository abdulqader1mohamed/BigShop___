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
    public class WishListController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: WishList
        public ActionResult Index()
        {
            return View(db.WishLists.ToList());
        }

        // GET: WishList/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // GET: WishList/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WishList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WishListID,userId")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                if (db.WishLists.FirstOrDefault(w => w.User.Id == wishList.userId) == null)
                {
                    wishList.User = db.Users.FirstOrDefault(w => w.Id == wishList.userId);
                    db.WishLists.Add(wishList);
                    db.SaveChanges();
                }
                return RedirectToAction("index", "Home");

            }

            return View(wishList);
        }

        // GET: WishList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // POST: WishList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "WishListID")] WishList wishList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wishList);
        }

        // GET: WishList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WishList wishList = db.WishLists.Find(id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // POST: WishList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WishList wishList = db.WishLists.Find(id);
            db.WishLists.Remove(wishList);
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
