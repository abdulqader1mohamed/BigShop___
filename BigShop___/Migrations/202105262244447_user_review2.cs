namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class user_review2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserproductReviews", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UserproductReviews", new[] { "ApplicationUser_Id" });
            RenameColumn(table: "dbo.UserproductReviews", name: "ApplicationUser_Id", newName: "ApplicationuserID");
            DropPrimaryKey("dbo.UserproductReviews");
            AlterColumn("dbo.UserproductReviews", "ApplicationuserID", c => c.String(nullable: false, maxLength: 128));
            AddPrimaryKey("dbo.UserproductReviews", new[] { "ProductID", "ApplicationuserID" });
            CreateIndex("dbo.UserproductReviews", "ApplicationuserID");
            AddForeignKey("dbo.UserproductReviews", "ApplicationuserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.UserproductReviews", "UserID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserproductReviews", "UserID", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.UserproductReviews", "ApplicationuserID", "dbo.AspNetUsers");
            DropIndex("dbo.UserproductReviews", new[] { "ApplicationuserID" });
            DropPrimaryKey("dbo.UserproductReviews");
            AlterColumn("dbo.UserproductReviews", "ApplicationuserID", c => c.String(maxLength: 128));
            AddPrimaryKey("dbo.UserproductReviews", new[] { "ProductID", "UserID" });
            RenameColumn(table: "dbo.UserproductReviews", name: "ApplicationuserID", newName: "ApplicationUser_Id");
            CreateIndex("dbo.UserproductReviews", "ApplicationUser_Id");
            AddForeignKey("dbo.UserproductReviews", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
