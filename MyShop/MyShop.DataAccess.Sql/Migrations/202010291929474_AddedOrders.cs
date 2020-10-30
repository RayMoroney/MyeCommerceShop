namespace MyShop.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OrderItemModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrderId = c.String(),
                        ProductId = c.String(),
                        ProductName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Image = c.String(),
                        Quantity = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        OrderModel_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.OrderModels", t => t.OrderModel_Id)
                .Index(t => t.OrderModel_Id);
            
            CreateTable(
                "dbo.OrderModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        Street = c.String(),
                        City = c.String(),
                        State = c.String(),
                        ZipCode = c.String(),
                        OrderStatus = c.String(),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OrderItemModels", "OrderModel_Id", "dbo.OrderModels");
            DropIndex("dbo.OrderItemModels", new[] { "OrderModel_Id" });
            DropTable("dbo.OrderModels");
            DropTable("dbo.OrderItemModels");
        }
    }
}
