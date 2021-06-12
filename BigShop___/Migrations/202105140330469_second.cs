namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CartProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        CartID = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.CartID })
                .ForeignKey("dbo.Carts", t => t.CartID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.CartID);
            
            CreateTable(
                "dbo.Carts",
                c => new
                    {
                        CartID = c.Int(nullable: false, identity: true),
                        Discount = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.CartID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        ProductStock = c.Int(nullable: false),
                        ProductRate = c.Int(nullable: false),
                        ProductOldPrice = c.Int(nullable: false),
                        ProductNewPrice = c.Int(nullable: false),
                        ProductDescription = c.String(),
                        ProductHasOffer = c.Boolean(nullable: false),
                        ProductOfferValue = c.Int(nullable: false),
                        ProductImageURL = c.String(),
                        CategoryID = c.Int(nullable: false),
                        Cart_CartID = c.Int(),
                        WishList_WishListID = c.Int(),
                    })
                .PrimaryKey(t => t.ProductID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Carts", t => t.Cart_CartID)
                .ForeignKey("dbo.WishLists", t => t.WishList_WishListID)
                .Index(t => t.CategoryID)
                .Index(t => t.Cart_CartID)
                .Index(t => t.WishList_WishListID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        CategoryID = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(),
                    })
                .PrimaryKey(t => t.CategoryID);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        OrderDiscount = c.Int(nullable: false),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.OrderProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        OrderID = c.Int(nullable: false),
                        ProductQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.OrderID })
                .ForeignKey("dbo.Orders", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.OrderID);
            
            CreateTable(
                "dbo.WishLists",
                c => new
                    {
                        WishListID = c.Int(nullable: false, identity: true),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.WishListID)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.WishListProducts",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        WishListID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.WishListID })
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .ForeignKey("dbo.WishLists", t => t.WishListID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.WishListID);
            
            CreateTable(
                "dbo.OrderProducts1",
                c => new
                    {
                        Order_OrderID = c.Int(nullable: false),
                        Product_ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_OrderID, t.Product_ProductID })
                .ForeignKey("dbo.Orders", t => t.Order_OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.Product_ProductID, cascadeDelete: true)
                .Index(t => t.Order_OrderID)
                .Index(t => t.Product_ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.WishListProducts", "WishListID", "dbo.WishLists");
            DropForeignKey("dbo.WishListProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.WishLists", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "WishList_WishListID", "dbo.WishLists");
            DropForeignKey("dbo.OrderProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderProducts", "OrderID", "dbo.Orders");
            DropForeignKey("dbo.CartProducts", "ProductID", "dbo.Products");
            DropForeignKey("dbo.CartProducts", "CartID", "dbo.Carts");
            DropForeignKey("dbo.Carts", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Products", "Cart_CartID", "dbo.Carts");
            DropForeignKey("dbo.Orders", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.OrderProducts1", "Product_ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderProducts1", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.Categories");
            DropIndex("dbo.OrderProducts1", new[] { "Product_ProductID" });
            DropIndex("dbo.OrderProducts1", new[] { "Order_OrderID" });
            DropIndex("dbo.WishListProducts", new[] { "WishListID" });
            DropIndex("dbo.WishListProducts", new[] { "ProductID" });
            DropIndex("dbo.WishLists", new[] { "User_Id" });
            DropIndex("dbo.OrderProducts", new[] { "OrderID" });
            DropIndex("dbo.OrderProducts", new[] { "ProductID" });
            DropIndex("dbo.Orders", new[] { "User_Id" });
            DropIndex("dbo.Products", new[] { "WishList_WishListID" });
            DropIndex("dbo.Products", new[] { "Cart_CartID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropIndex("dbo.Carts", new[] { "User_Id" });
            DropIndex("dbo.CartProducts", new[] { "CartID" });
            DropIndex("dbo.CartProducts", new[] { "ProductID" });
            DropTable("dbo.OrderProducts1");
            DropTable("dbo.WishListProducts");
            DropTable("dbo.WishLists");
            DropTable("dbo.OrderProducts");
            DropTable("dbo.Orders");
            DropTable("dbo.Categories");
            DropTable("dbo.Products");
            DropTable("dbo.Carts");
            DropTable("dbo.CartProducts");
        }
    }
}
