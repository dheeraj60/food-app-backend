using HotPot.Models;
using Microsoft.EntityFrameworkCore;

namespace HotPot.Contexts
{
    public class RequestTrackerContext : DbContext
    {
        public RequestTrackerContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<City>? Cities { get; set; }
        public DbSet<DeliveryPartner>? DeliveryPartners { get; set; }
        public DbSet<Menu>? Menus { get; set; }
        public DbSet<NutritionalInfo>? NutritionalInfos { get; set; }
        public DbSet<Order>? Orders { get; set; }
        public DbSet<OrderItem>? OrderItems { get; set; }
        public DbSet<Payment>? Payments { get; set; }
        public DbSet<Restaurant>? Restaurants { get; set; }
        public DbSet<RestaurantSpeciality>? RestaurantSpecialities { get; set; }
        public DbSet<State>? States { get; set; }
        public DbSet<Customer>? Customers { get; set; }
        public DbSet<CustomerAddress>? CustomerAddresses { get; set; }
        public DbSet<CustomerReview>? CustomerReviews { get; set; }
        public DbSet<User>? Users { get; set; }
        public DbSet<RestaurantOwner>? RestaurantOwners { get; set; }
        public DbSet<Cart>? Carts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define composite key for OrderItem table
            modelBuilder.Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.MenuId });

            // Define relationships for OrderItems with ON DELETE NO ACTION to avoid cascading conflicts
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany()
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Restrict); // Changed CASCADE to RESTRICT or NO ACTION

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Menu)
                .WithMany()
                .HasForeignKey(oi => oi.MenuId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade delete for MenuItem

            // Define relationships for Cart
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithMany() // Since Customer does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(c => c.CustomerId);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Restaurant)
                .WithMany() // Since Restaurant does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(c => c.RestaurantId)
                .OnDelete(DeleteBehavior.Restrict); // Changed to Restrict to avoid cascading conflicts

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Menu)
                .WithMany() // Since Menu does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(c => c.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade); // Keep cascade delete for MenuItem

            // Define relationships for Customer
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithMany() // Since User does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(c => c.UserName);

            // Define relationships for Menu
            modelBuilder.Entity<Menu>()
                .HasOne(m => m.NutritionalInfo)
                .WithMany() // Since NutritionalInfo does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(m => m.NutritionId);

            modelBuilder.Entity<Menu>()
                .HasOne(m => m.Restaurant)
                .WithMany() // Since Restaurant does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(m => m.RestaurantId);

            // Define relationships for Restaurant
            modelBuilder.Entity<Restaurant>()
                .HasOne(r => r.City)
                .WithMany() // Since City does not have a navigation property, you can leave this as 'WithMany'
                .HasForeignKey(r => r.CityId);

            // Other necessary configurations for related models if any...
        }
    }
}
