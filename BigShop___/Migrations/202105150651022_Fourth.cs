namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Fourth : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ImageURL", c => c.String());
            AddColumn("dbo.AspNetUsers", "First_Name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Last_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Last_Name");
            DropColumn("dbo.AspNetUsers", "First_Name");
            DropColumn("dbo.AspNetUsers", "ImageURL");
        }
    }
}
