namespace AbstractCafeService.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CustomerFIO = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Choices",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    CustomerId = c.Int(nullable: false),
                    MenuId = c.Int(nullable: false),
                    ChefId = c.Int(),
                    Count = c.Int(nullable: false),
                    Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                    Status = c.Int(nullable: false),
                    DateCreate = c.DateTime(nullable: false),
                    DateImplement = c.DateTime(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.CustomerId, cascadeDelete: true)
                .ForeignKey("dbo.Chefs", t => t.ChefId)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.CustomerId)
                .Index(t => t.MenuId)
                .Index(t => t.ChefId);

            CreateTable(
                "dbo.Chefs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ChefFIO = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Menus",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MenuName = c.String(nullable: false),
                    Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.MenuDishs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    MenuId = c.Int(nullable: false),
                    DishId = c.Int(nullable: false),
                    Count = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishs", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Menus", t => t.MenuId, cascadeDelete: true)
                .Index(t => t.MenuId)
                .Index(t => t.DishId);

            CreateTable(
                "dbo.Dishs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DishName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.KitchenDishs",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    KitchenId = c.Int(nullable: false),
                    DishId = c.Int(nullable: false),
                    Count = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Dishs", t => t.DishId, cascadeDelete: true)
                .ForeignKey("dbo.Kitchens", t => t.KitchenId, cascadeDelete: true)
                .Index(t => t.KitchenId)
                .Index(t => t.DishId);

            CreateTable(
                "dbo.Kitchens",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    KitchenName = c.String(nullable: false),
                })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.MenuDishs", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.KitchenDishs", "KitchenId", "dbo.Kitchens");
            DropForeignKey("dbo.KitchenDishs", "DishId", "dbo.Dishs");
            DropForeignKey("dbo.MenuDishs", "DishId", "dbo.Dishs");
            DropForeignKey("dbo.Choices", "MenuId", "dbo.Menus");
            DropForeignKey("dbo.Choices", "ChefId", "dbo.Chefs");
            DropForeignKey("dbo.Choices", "CustomerId", "dbo.Customers");
            DropIndex("dbo.KitchenDishs", new[] { "DishId" });
            DropIndex("dbo.KitchenDishs", new[] { "KitchenId" });
            DropIndex("dbo.MenuDishs", new[] { "DishId" });
            DropIndex("dbo.MenuDishs", new[] { "MenuId" });
            DropIndex("dbo.Choices", new[] { "ChefId" });
            DropIndex("dbo.Choices", new[] { "MenuId" });
            DropIndex("dbo.Choices", new[] { "CustomerId" });
            DropTable("dbo.Kitchens");
            DropTable("dbo.KitchenDishs");
            DropTable("dbo.Dishs");
            DropTable("dbo.MenuDishs");
            DropTable("dbo.Menus");
            DropTable("dbo.Chefs");
            DropTable("dbo.Choices");
            DropTable("dbo.Customers");
        }
    }
}
