using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ERPAuth.Client.Models;

namespace ERPAuth.Client.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for your models
        public DbSet<Article> Articles { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<PackingList> PackingLists { get; set; }
        public DbSet<PackingListItem> PackingListItems { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Provider> Providers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure PostgreSQL connection string
                optionsBuilder.UseNpgsql("Your PostgreSQL connection string here");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define one-to-many relationship between Article and Inventory
            modelBuilder.Entity<Article>()
                .HasMany(a => a.Inventories)
                .WithOne(i => i.Article)
                .HasForeignKey(i => i.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationship between Customer and Order
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationship between Order and OrderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-one relationship between OrderItem and Inventory
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Inventory)
                .WithMany() // Inventory can be associated with multiple order items
                .HasForeignKey(oi => oi.InventoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

            // Define one-to-one relationship between Order and PackingList
            modelBuilder.Entity<PackingList>()
                .HasOne(pl => pl.Order)
                .WithMany(o => o.PackingLists)
                .HasForeignKey(pl => pl.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationship between PackingList and PackingListItem
            modelBuilder.Entity<PackingList>()
                .HasMany(pl => pl.Items)
                .WithOne(pli => pli.PackingList)
                .HasForeignKey(pli => pli.PackingListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define one-to-many relationship between Invoice and InvoiceItem
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Items)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Define optional relationships for Invoice
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithMany()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.SetNull); // Allow standalone invoices

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.PackingList)
                .WithMany()
                .HasForeignKey(i => i.PackingListId)
                .OnDelete(DeleteBehavior.SetNull); // Allow standalone invoices
        }
    }
}
