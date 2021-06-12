namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_review : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserproductReviews",
                c => new
                    {
                        ProductID = c.Int(nullable: false),
                        UserID = c.String(nullable: false, maxLength: 128),
                        ProductRate = c.Int(nullable: false),
                        UserComment = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.ProductID, t.UserID })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserproductReviews", "ProductID", "dbo.Products");
            DropForeignKey("dbo.UserproductReviews", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserproductReviews", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.UserproductReviews", new[] { "ProductID" });
            DropTable("dbo.UserproductReviews");
        }
    }
}
