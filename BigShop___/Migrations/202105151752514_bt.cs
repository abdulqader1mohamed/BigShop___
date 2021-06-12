namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Carts", "Discount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "Discount", c => c.String());
        }
    }
}
