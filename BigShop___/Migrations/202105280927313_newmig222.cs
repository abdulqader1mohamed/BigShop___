namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserproductReviews", "CommentDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserproductReviews", "CommentDate");
        }
    }
}
