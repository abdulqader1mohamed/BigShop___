namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cv : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Carts", "Discount", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Carts", "Discount", c => c.Int(nullable: false));
        }
    }
}
