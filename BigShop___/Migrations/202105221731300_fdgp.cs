namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fdgp : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "passwordUser");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "passwordUser", c => c.String());
        }
    }
}
