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

            // Relationship configurations

            // Article -> Inventory (One-to-Many)
            modelBuilder.Entity<Article>()
                .HasMany(a => a.Inventories)
                .WithOne(i => i.Article)
                .HasForeignKey(i => i.ArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer -> Order (One-to-Many)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Order -> OrderItem (One-to-Many)
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Items)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // OrderItem -> Inventory (Many-to-One, optional)
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Inventory)
                .WithMany() // Inventory can be associated with multiple OrderItems
                .HasForeignKey(oi => oi.InventoryId)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete to avoid deleting inventory when an order item is removed

            // PackingList -> Order (One-to-Many)
            modelBuilder.Entity<PackingList>()
                .HasOne(pl => pl.Order)
                .WithMany(o => o.PackingLists)
                .HasForeignKey(pl => pl.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // PackingList -> PackingListItem (One-to-Many)
            modelBuilder.Entity<PackingList>()
                .HasMany(pl => pl.Items)
                .WithOne(pli => pli.PackingList)
                .HasForeignKey(pli => pli.PackingListId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice -> InvoiceItem (One-to-Many)
            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Items)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Invoice -> Order (Optional relationship)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Order)
                .WithMany()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.SetNull); // Allow invoices without a linked order

            // Invoice -> PackingList (Optional relationship)
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.PackingList)
                .WithMany()
                .HasForeignKey(i => i.PackingListId)
                .OnDelete(DeleteBehavior.SetNull); // Allow invoices without a linked packing list

            // Ensure DateTime consistency for OrderDate and ClientOrderDate
            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(o => o.OrderDate)
                    .HasColumnType("timestamp with time zone") // Ensure PostgreSQL stores in UTC
                    .HasDefaultValueSql("CURRENT_TIMESTAMP"); // Set default value to current UTC time

                entity.Property(o => o.ClientOrderDate)
                    .HasColumnType("timestamp with time zone") // Ensure PostgreSQL stores in UTC
                    .IsRequired(false); // Explicitly allow nulls
            });
        }
    }
}
