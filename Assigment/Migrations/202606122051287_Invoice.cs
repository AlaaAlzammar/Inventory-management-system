namespace Assigment.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Invoice : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SupplyDetails", "SupplyId", "dbo.Supplies");
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        PhoneNumber = c.String(maxLength: 15),
                        Address = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.CustomerId);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        InvoiceId = c.Int(nullable: false, identity: true),
                        CustomerId = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        ExtraCharges = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PaidAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.InvoiceId)
                .ForeignKey("dbo.Customers", t => t.CustomerId)
                .Index(t => t.CustomerId);
            
            CreateTable(
                "dbo.InvoiceItems",
                c => new
                    {
                        InvoiceItemId = c.Int(nullable: false, identity: true),
                        InvoiceId = c.Int(nullable: false),
                        ItemId = c.Int(nullable: false),
                        Quantity = c.Double(nullable: false),
                        SalePrice = c.Double(nullable: false),
                        Invoice_InvoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.InvoiceItemId)
                .ForeignKey("dbo.Invoices", t => t.InvoiceId)
                .ForeignKey("dbo.Items", t => t.ItemId)
                .ForeignKey("dbo.Invoices", t => t.Invoice_InvoiceId)
                .Index(t => t.InvoiceId)
                .Index(t => t.ItemId)
                .Index(t => t.Invoice_InvoiceId);
            
            AddColumn("dbo.SupplyDetails", "Supply_SupplyId", c => c.Int());
            CreateIndex("dbo.SupplyDetails", "Supply_SupplyId");
            AddForeignKey("dbo.SupplyDetails", "Supply_SupplyId", "dbo.Supplies", "SupplyId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SupplyDetails", "Supply_SupplyId", "dbo.Supplies");
            DropForeignKey("dbo.InvoiceItems", "Invoice_InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.InvoiceItems", "ItemId", "dbo.Items");
            DropForeignKey("dbo.InvoiceItems", "InvoiceId", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "CustomerId", "dbo.Customers");
            DropIndex("dbo.SupplyDetails", new[] { "Supply_SupplyId" });
            DropIndex("dbo.InvoiceItems", new[] { "Invoice_InvoiceId" });
            DropIndex("dbo.InvoiceItems", new[] { "ItemId" });
            DropIndex("dbo.InvoiceItems", new[] { "InvoiceId" });
            DropIndex("dbo.Invoices", new[] { "CustomerId" });
            DropColumn("dbo.SupplyDetails", "Supply_SupplyId");
            DropTable("dbo.InvoiceItems");
            DropTable("dbo.Invoices");
            DropTable("dbo.Customers");
            AddForeignKey("dbo.SupplyDetails", "SupplyId", "dbo.Supplies", "SupplyId");
        }
    }
}
