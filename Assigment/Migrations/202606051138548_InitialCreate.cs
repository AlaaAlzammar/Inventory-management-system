namespace Assigment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Details = c.String(maxLength: 300),
                        Price = c.Double(nullable: false),
                        MasureingUnite = c.String(),
                        ReOrederLevel = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
            CreateTable(
                "dbo.Stocks",
                c => new
                    {
                        StockId = c.Int(nullable: false, identity: true),
                        ItemId = c.Int(nullable: false),
                        SupplyDetailId = c.Int(nullable: false),
                        StockValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.StockId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.SupplyDetails", t => t.SupplyDetailId)
                .Index(t => t.ItemId)
                .Index(t => t.SupplyDetailId);
            
            CreateTable(
                "dbo.SupplyDetails",
                c => new
                    {
                        SupplyDetailId = c.Int(nullable: false, identity: true),
                        SupplyId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        PurchasePrice = c.Double(nullable: false),
                        Quantity = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.SupplyDetailId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Supplies", t => t.SupplyId)
                .Index(t => t.SupplyId)
                .Index(t => t.ItemId);
            
            CreateTable(
                "dbo.Supplies",
                c => new
                    {
                        SupplyId = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SupplyId)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId)
                .Index(t => t.SupplierId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Phone = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Stocks", "SupplyDetailId", "dbo.SupplyDetails");
            DropForeignKey("dbo.SupplyDetails", "SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.Supplies", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.SupplyDetails", "ItemId", "dbo.Items");
            DropForeignKey("dbo.Stocks", "ItemId", "dbo.Items");
            DropIndex("dbo.Supplies", new[] { "SupplierId" });
            DropIndex("dbo.SupplyDetails", new[] { "ItemId" });
            DropIndex("dbo.SupplyDetails", new[] { "SupplyId" });
            DropIndex("dbo.Stocks", new[] { "SupplyDetailId" });
            DropIndex("dbo.Stocks", new[] { "ItemId" });
            DropTable("dbo.Suppliers");
            DropTable("dbo.Supplies");
            DropTable("dbo.SupplyDetails");
            DropTable("dbo.Stocks");
            DropTable("dbo.Items");
        }
    }
}
