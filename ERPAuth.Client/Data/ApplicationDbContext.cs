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
                // You can inject Configuration here or use another method to fetch connection string
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

            // Define one-to-one relationship between OrderItem and Inventory
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Inventory)
                .WithMany() // Inventory can be associated with multiple order items
                .HasForeignKey(oi => oi.InventoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete
        }



    }
}
