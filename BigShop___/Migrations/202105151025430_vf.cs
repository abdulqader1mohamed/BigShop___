namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class vf : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductOfferValue", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductOfferValue", c => c.Int(nullable: false));
        }
    }
}
