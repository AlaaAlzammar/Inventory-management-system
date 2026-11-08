using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Assigment.Model
{
    internal class IventoryDBContext : DbContext
    {
        public IventoryDBContext() : base("ConStr") {
            
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Supplier> Suppliers {  get; set; }
        public DbSet<Supply> Supplies { get; set; }
        public DbSet<SupplyDetail> SupplyDetails { get; set; } 

        public DbSet<Stock> Stocks {  get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoicesItem { get; set; }
        public DbSet<User> Users { get; set;  }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Entity<Stock>()
                .HasRequired(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .WillCascadeOnDelete(false);
            modelBuilder.Entity<Stock>()
               .HasRequired(s => s.SupplyDetail)
               .WithMany()
               .HasForeignKey(s => s.SupplyDetailId)
               .WillCascadeOnDelete(false);
            modelBuilder.Entity<SupplyDetail>()
              .HasRequired(s => s.Supply)
              .WithMany()
              .HasForeignKey(s => s.SupplyId)
              .WillCascadeOnDelete(false);
            modelBuilder.Entity<SupplyDetail>()
                .HasRequired(s => s.Item)
                .WithMany()
                .HasForeignKey(s => s.ItemId)
                .WillCascadeOnDelete(false);
             modelBuilder.Entity<Supply>()
                .HasRequired(s => s.Supplier)
                .WithMany()
                .HasForeignKey(s => s.SupplierId)
                .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Invoice>()
                .HasRequired(i => i.Customer)
                .WithMany()
                .HasForeignKey(c => c.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceItem>()
               .HasRequired(i => i.Invoice)
               .WithMany()
               .HasForeignKey(c => c.InvoiceId)
               .WillCascadeOnDelete(false);

            modelBuilder.Entity<InvoiceItem>()
              .HasRequired(i => i.Item)
              .WithMany()
              .HasForeignKey(c => c.ItemId)
              .WillCascadeOnDelete(false);
        }
    }
}
