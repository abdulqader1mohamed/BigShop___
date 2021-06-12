namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig2223 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserproductReviews", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserproductReviews", "UserName");
        }
    }
}
