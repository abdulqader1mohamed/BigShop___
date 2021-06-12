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
    public class UserproductReviewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: UserproductReview
        public ActionResult Index()
        {
            return View(db.UserproductReviews.ToList());
        }

        // GET: UserproductReview/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserproductReview userproductReview = db.UserproductReviews.Find(id);
            if (userproductReview == null)
            {
                return HttpNotFound();
            }
            return View(userproductReview);
        }

        // GET: UserproductReview/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserproductReview/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ApplicationuserID,ProductRate,UserComment,Commentlikes,CommentDate")] UserproductReview userproductReview)
        {
            if (ModelState.IsValid)
            {
                db.UserproductReviews.Add(userproductReview);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userproductReview);
        }

        // GET: UserproductReview/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserproductReview userproductReview = db.UserproductReviews.Find(id);
            if (userproductReview == null)
            {
                return HttpNotFound();
            }
            return View(userproductReview);
        }

        // POST: UserproductReview/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ApplicationuserID,ProductRate,UserComment,Commentlikes,CommentDate")] UserproductReview userproductReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userproductReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userproductReview);
        }

        // GET: UserproductReview/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserproductReview userproductReview = db.UserproductReviews.Find(id);
            if (userproductReview == null)
            {
                return HttpNotFound();
            }
            return View(userproductReview);
        }

        // POST: UserproductReview/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserproductReview userproductReview = db.UserproductReviews.Find(id);
            db.UserproductReviews.Remove(userproductReview);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public string CurrentUserName()
        {
            var Currentuserid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(u => u.Id == Currentuserid);
            
            return user.First_Name+" "+user.Last_Name;
        }

        public int AverageReviewRate(int id)
        {
           var  CurrentProductReviews = db.UserproductReviews.Where(r => r.ProductID == id);
            var CurrentProductRate = CurrentProductReviews.Count();

            var TotalReviewRate = 0;
            var AvarageReviewRate = 0;
            foreach (var item in CurrentProductReviews)
            {
                TotalReviewRate += item.ProductRate;

            }
            if (CurrentProductRate == 0) { CurrentProductRate = 1; }

            AvarageReviewRate = TotalReviewRate / CurrentProductRate;


            return AvarageReviewRate;
        }

        public void AddRate([Bind(Include = "ProductID,ProductRate")] Product product_)
        {
            var CurrentUserId = User.Identity.GetUserId();
            var CurrentUserName = db.Users.FirstOrDefault(u => u.Id == CurrentUserId).First_Name + " " + db.Users.FirstOrDefault(u => u.Id == CurrentUserId).Last_Name;
            var CurrentProductReviews = db.UserproductReviews.Where(r => r.ProductID == product_.ProductID);
            
            UserproductReview Newreview = new UserproductReview();
            Newreview.ProductID = product_.ProductID;
            Newreview.ApplicationuserID = CurrentUserId;
            Newreview.UserName = CurrentUserName;
            Newreview.ProductRate = product_.ProductRate;
            Newreview.CommentDate = DateTime.Now;

            
            db.UserproductReviews.Add(Newreview);
            db.SaveChanges();


        }

     public string ReviewBar(int ProductID)
        {
            var CurrentProductReviews = db.UserproductReviews.Where(r=>r.ProductID == ProductID);
            var TotalReviews = CurrentProductReviews.Count();

            var Total_5_star = CurrentProductReviews.Where(r => r.ProductRate == 5).Count();
            var Total_4_star = CurrentProductReviews.Where(r => r.ProductRate == 4).Count();
            var Total_3_star = CurrentProductReviews.Where(r => r.ProductRate == 3).Count();
            var Total_2_star = CurrentProductReviews.Where(r => r.ProductRate == 2).Count();
            var Total_1_star = CurrentProductReviews.Where(r => r.ProductRate == 1).Count();

            double Rate5 = (Total_5_star*100) / TotalReviews;
            double Rate4 = (Total_4_star*100) / TotalReviews;
            double Rate3 = (Total_3_star*100) / TotalReviews;
            double Rate2 = (Total_2_star*100) / TotalReviews;
            double Rate1 = (Total_1_star*100) / TotalReviews;


            return Rate5 + "&" + Rate4 + "&" + Rate3 + "&" + Rate2+ "&" + Rate1 + "&" + TotalReviews;
        }

        public void AddComment([Bind(Include = "ProductID,UserComment")] UserproductReview UserProductReview_) {
            var CurrentUserid = User.Identity.GetUserId();
            var CurrentProductReviews = db.UserproductReviews.Where(r => r.ProductID == UserProductReview_.ProductID).FirstOrDefault(u=>u.ApplicationuserID == CurrentUserid);

            CurrentProductReviews.UserComment = UserProductReview_.UserComment;
            CurrentProductReviews.CommentDate = DateTime.Now;
            db.SaveChanges();

        }


        public bool HasReviewForCurrentProduct(int ProductID)
        {
            var CurrentUserid = User.Identity.GetUserId();
            var CurrentProductReviews = db.UserproductReviews.Where(r => r.ProductID == ProductID);
            var userreview = CurrentProductReviews.FirstOrDefault(r=>r.ApplicationuserID==CurrentUserid);
            if (userreview == null)
                return false;
            else
            {
                return true;

            }
        }


        public string GetUserName(string Userid)
        {
            
            string Username = db.Users.FirstOrDefault(u=>u.Id== Userid).First_Name+" "+ db.Users.FirstOrDefault(u => u.Id == Userid).Last_Name;
            return Username;
        }
        public string GetUserImage(string Userid)
        {
            
            string UserImage = db.Users.FirstOrDefault(u=>u.Id== Userid).ImageURL;
            return UserImage;
        }

        public int GetUserRate(int productid)
        {
            string Userid = User.Identity.GetUserId();


            var UserProductReview = db.UserproductReviews.Where(p => p.ProductID == productid).FirstOrDefault(r => r.ApplicationuserID == Userid);

            int x = UserProductReview.ProductRate;
            return x;
        }

        public int IncreaseLike([Bind(Include = "ProductID,ProductRate")] UserproductReview UserProductReview_)
        {

            var CurrentUserProductReview = db.UserproductReviews.Where(p => p.ProductID == UserProductReview_.ProductID).FirstOrDefault(u => u.ApplicationuserID == UserProductReview_.ApplicationuserID);
            CurrentUserProductReview.Commentlikes++;
            db.SaveChanges();
            return 0;
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
