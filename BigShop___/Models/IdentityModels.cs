using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BigShop___.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegisterDate { get; set; }
        //public DateTime LastLoginDate { get; set; }
        public string Country { get; set; }
        public string ImageURL { get; set; }
        [Required(ErrorMessage = "Please insert First Name")]
        public string First_Name { get; set; }
        [Required(ErrorMessage = "Please insert last Name")]
        public string Last_Name { get; set; }
        public virtual List<UserproductReview> UserproductReviews{ get; set; }

        public virtual List<Order> Orders  { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<WishList> WishLists { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<CartProducts> CartProducts { get; set; }
        public virtual DbSet<OrderProducts> OrderProducts { get; set; }
        public virtual DbSet<WishListProducts> WishListsProducts { get; set; }

        public virtual DbSet<UserproductReview> UserproductReviews { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<BigShop___.Models.ApplicationUser> ApplicationUsers { get; set; }

        //public System.Data.Entity.DbSet<BigShop___.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}