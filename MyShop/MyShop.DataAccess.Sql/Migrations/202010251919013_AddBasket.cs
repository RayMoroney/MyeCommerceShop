namespace MyShop.DataAccess.Sql.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBasket : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BasketItemModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        BasketId = c.String(),
                        ProductId = c.String(),
                        Quantity = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        BasketModel_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BasketModels", t => t.BasketModel_Id)
                .Index(t => t.BasketModel_Id);
            
            CreateTable(
                "dbo.BasketModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BasketItemModels", "BasketModel_Id", "dbo.BasketModels");
            DropIndex("dbo.BasketItemModels", new[] { "BasketModel_Id" });
            DropTable("dbo.BasketModels");
            DropTable("dbo.BasketItemModels");
        }
    }
}
