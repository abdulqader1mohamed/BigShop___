namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sanor7 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "First_Name", c => c.String(nullable: false));
            AlterColumn("dbo.AspNetUsers", "Last_Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Last_Name", c => c.String());
            AlterColumn("dbo.AspNetUsers", "First_Name", c => c.String());
        }
    }
}
