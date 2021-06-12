using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using BigShop___.Models;
using System.Net;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace BigShop___.Controllers
{
    
    public class AccountController : Controller
    {
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model, string roletype)
        {
            if (ModelState.IsValid)
            {

                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, First_Name = model.FirstName, Last_Name = model.LastName };
                user.RegisterDate = DateTime.Now;
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    await UserManager.AddToRoleAsync(user.Id, roletype);
                    addCart(user.Id);
                    addWishlist(user.Id);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    // return RedirectToAction("register_secondPhase", "Account");
                    return RedirectToAction("index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [Authorize]
        public ActionResult register_secondPhase()
        {

            return View();
        }
        [Authorize]
        public ActionResult register_thirdPhase()
        {

            return View();
        }
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }
        [AllowAnonymous]
        public ActionResult img()
        {
            string iddd = User.Identity.GetUserId();
            string ii= db.Users.FirstOrDefault(i => i.Id ==iddd ).ImageURL;
            return Content(ii);
        }
        public ActionResult orderCounter()
        {
            string iddd = User.Identity.GetUserId();
            string count = db.Orders.Where(i => i.User.Id == iddd).Count().ToString();
            return Content(count);
        }
        public ActionResult cartCounter()
        {
            string iddd = User.Identity.GetUserId();
            int cid = db.Carts.FirstOrDefault(i => i.User.Id == iddd).CartID;
            int cr = 0;
            foreach (var item in db.CartProducts.Where(s => s.CartID == cid))
            {
                cr += item.ProductQuantity;
            }
            string count = cr.ToString();
            return Content(count);
        }
        public ActionResult wishlistCounter()
        {
            string iddd = User.Identity.GetUserId();
            int wid = db.WishLists.FirstOrDefault(i => i.User.Id == iddd).WishListID;

            string count = db.WishListsProducts.Where(s => s.WishListID == wid).Count().ToString();
            return Content(count);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }



        // ----------------------------------------------------------------------------
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult show()
        {
            return View(db.Users.ToList());
        }


        //[HttpGet]
        //// GET: ORDERs/Edit/5
        //public ActionResult Edit(string id, string roletype)
        //{

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    ApplicationUser User = db.Users.Find(id);
        //    if (User == null)
        //    {
        //        return HttpNotFound();
        //    }
            

        //    return View(User);
        //}
        //[HttpPost, ActionName("Edit")]
        //// GET: ORDERs/Edit/5
        //public  void EditPost(ApplicationUser User, string roletype)
        //{
            
        //    if (roletype != null) { 
           
        //    UserManager.RemoveFromRoles(User.Id, roletype);
        //    UserManager.AddToRole(User.Id, roletype);
        //    }


        //    db.Entry(User).State = EntityState.Modified;

        //    db.SaveChanges();

        //}

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser User = db.Users.Find(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        // POST: ORDERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser User = db.Users.FirstOrDefault(s => s.Id == id);
            db.Users.Remove(User);
            db.SaveChanges();
            return RedirectToAction("show");
        }

      
        public ActionResult userorders()
        {
            string userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(a => a.Id == userid);
            return View(user);
        }
        public ActionResult rememberpass()
        {
            string userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(a => a.Id == userid);
            return View(user);
        }
        public ActionResult profilefavoret()
        {
            return View();
        }

        [HttpGet]
        public ActionResult profilesetting()
        {
            string userid = User.Identity.GetUserId();
            var user = db.Users.FirstOrDefault(a => a.Id == userid);
            return View(user);
        }
        [HttpPost]
        public ActionResult profilesetting(ApplicationUser user, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userold = db.Users.FirstOrDefault(b => b.Id == user.Id);
                string imageName = user.Id.ToString() + "." + image.FileName.Split('.')[1];
                image.SaveAs(Server.MapPath("~/Images/") + imageName);
                user.ImageURL = imageName;
                userold.First_Name = user.First_Name;
                userold.Last_Name = user.Last_Name;
                userold.Email = user.Email;
                userold.UserName = user.UserName;
                if (user.ImageURL != null)
                {
                    userold.ImageURL = user.ImageURL;
                }
                //db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("profilesetting");
            }
            return View(user);
        }

        public string userfullname()
        {
            var currentuserid = User.Identity.GetUserId();
            ApplicationUser currentuser = db.Users.FirstOrDefault(u=>u.Id ==currentuserid);
            return (currentuser.First_Name +" "+currentuser.Last_Name);
        }


        public int TotalPrice()
        {
            var CurrentUser = User.Identity.GetUserId();
            var ProductsInHisCart = db.CartProducts.Where(c => c.Cart.User.Id == CurrentUser);
            int TotalProductsPrice = 0;
            var product = new Product();
            foreach (var item in ProductsInHisCart)
            {
               
               
                TotalProductsPrice += item.ProductQuantity * product.ProductNewPrice;
            }
            return TotalProductsPrice;
        }


        private void addCart(string id)
        {
            Cart cart = new Cart();
            cart.User = db.Users.FirstOrDefault(p => p.Id == id);
            cart.userId = id;
            cart.Discount = 25;
            db.Carts.Add(cart);
            db.SaveChanges();
        }
        private void addWishlist(string id)
        {
            WishList wish = new WishList();
            wish.User = db.Users.FirstOrDefault(p => p.Id == id);
            wish.userId = id;

            db.WishLists.Add(wish);
            db.SaveChanges();
        }

        
        public ActionResult carttotalprice()
        {
            string iddd = User.Identity.GetUserId();
            int cid = db.Carts.FirstOrDefault(i => i.User.Id == iddd).CartID;

            int price = 0;
            foreach (var item in db.CartProducts.Where(s => s.CartID == cid))
            {

                price = item.ProductQuantity * item.Product.ProductNewPrice;
            }
            string count = price.ToString();
            return Content(count);
        }





        [Authorize(Roles = "Administrator,Employee")]

        public ActionResult showw()
        {
            //return View("Showw");
            return View(db.Users.ToList());
        }

        // GET: ORDERs/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser User = db.Users.Find(id);

            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }


        [Authorize(Roles = "Administrator")]
        [HttpGet]
        // GET: ORDERs/Edit/5
        public ActionResult Edit(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser User = db.Users.Find(id);
            if (User == null)
            {
                return HttpNotFound();
            }

            var oldUser = UserManager.FindById(User.Id);
            var oldRoleId = oldUser.Roles.SingleOrDefault().RoleId;
            var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Id;
            //ViewBag.roId = oldRoleName;
            List<IdentityRole> identityRoles = db.Roles.ToList();
            SelectList rolll = new SelectList(identityRoles, "Id", "Name", oldRoleName);
            ViewBag.roId = rolll;
            //var roleManager = new RoleManager<IdentityRole, string>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            //var olduser = UserManager.FindById(User.Id);
            //var oldrole = roleManager.FindById(olduser.Roles.FirstOrDefault().RoleId);

            ////var role = roleManager.FindById(roletype);

            //UserManager.RemoveFromRole(User.Id, oldrole.na);
            //UserManager.AddToRoleAsync(User.Id, roletype);

            return View(User);
        }
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(ApplicationUser User, string roletype, HttpPostedFileBase image)
        {

            if (ModelState.IsValid)
            {
                ApplicationUser userold = db.Users.FirstOrDefault(b => b.Id == User.Id);
                var oldRoleId = userold.Roles.SingleOrDefault().RoleId;
                var oldRoleName = db.Roles.SingleOrDefault(r => r.Id == oldRoleId).Name;
                string imageName = User.Id.ToString() + "." + image.FileName.Split('.')[1];
                image.SaveAs(Server.MapPath("~/Images/") + imageName);
                User.ImageURL = imageName;
                userold.First_Name = User.First_Name;
                userold.Last_Name = User.Last_Name;
                userold.Email = User.Email;
                userold.UserName = User.UserName;
                if (User.ImageURL != null)
                {
                    userold.ImageURL = User.ImageURL;
                }
                if (oldRoleName != roletype)
                {
                    UserManager.RemoveFromRole(User.Id, oldRoleName);
                    var Nameee = db.Roles.SingleOrDefault(r => r.Id == roletype).Name;
                    UserManager.AddToRole(User.Id, Nameee);
                }
                db.SaveChanges();
                return RedirectToAction("Showw");
            }
            return View(User);
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}