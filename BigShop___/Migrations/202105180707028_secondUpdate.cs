namespace BigShop___.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secondUpdate : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderProducts1", "Order_OrderID", "dbo.Orders");
            DropForeignKey("dbo.OrderProducts1", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.OrderProducts1", new[] { "Order_OrderID" });
            DropIndex("dbo.OrderProducts1", new[] { "Product_ProductID" });
                  }
        
        public override void Down()
        {
            CreateTable(
                "dbo.OrderProducts1",
                c => new
                    {
                        Order_OrderID = c.Int(nullable: false),
                        Product_ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_OrderID, t.Product_ProductID });
            
            CreateIndex("dbo.OrderProducts1", "Product_ProductID");
            CreateIndex("dbo.OrderProducts1", "Order_OrderID");
            AddForeignKey("dbo.OrderProducts1", "Product_ProductID", "dbo.Products", "ProductID", cascadeDelete: true);
            AddForeignKey("dbo.OrderProducts1", "Order_OrderID", "dbo.Orders", "OrderID", cascadeDelete: true);
        }
    }
}
