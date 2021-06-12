namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserproductReviews", "Commentlikes", c => c.Int(nullable: false));
            AddColumn("dbo.UserproductReviews", "CommentDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserproductReviews", "CommentDate");
            DropColumn("dbo.UserproductReviews", "Commentlikes");
        }
    }
}
