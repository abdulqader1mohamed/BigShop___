namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig2 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserproductReviews", "CommentDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserproductReviews", "CommentDate", c => c.String());
        }
    }
}
