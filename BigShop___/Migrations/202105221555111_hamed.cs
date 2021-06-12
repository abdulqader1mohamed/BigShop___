namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hamed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "passwordUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "passwordUser");
        }
    }
}
