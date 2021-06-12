namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newmig_25_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "isAvailable", c => c.Boolean(nullable: false));
            AddColumn("dbo.OrderProducts", "ProductOldPrice", c => c.Int(nullable: false));
            AddColumn("dbo.OrderProducts", "ProductNewPrice", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderProducts", "ProductNewPrice");
            DropColumn("dbo.OrderProducts", "ProductOldPrice");
            DropColumn("dbo.Categories", "isAvailable");
        }
    }
}
