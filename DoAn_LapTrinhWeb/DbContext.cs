using System.Data.Entity;
using DoAn_LapTrinhWeb.Migrations;
using DoAn_LapTrinhWeb.Model;
using DoAn_LapTrinhWeb.Models;

namespace DoAn_LapTrinhWeb
{
    public class DbContext : System.Data.Entity.DbContext
    {
        public DbContext()
            : base("Model11")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DbContext, Configuration>());
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Delivery> Deliveries { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<ReplyFeedback> ReplyFeedbacks { get; set; }
        public virtual DbSet<ProductImages> ProductImages { get; set; }
        public virtual DbSet<OrderAddress> OrderAddresses { get; set; }
        public virtual DbSet<AccountAddress> AccountAddresses { get; set; }
        public virtual DbSet<Wards> Wards { get; set; }
        public virtual DbSet<Districts> Districts { get; set; }
        public virtual DbSet<Provinces> Provinces { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Oder_Detail> Oder_Detail { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>()
                .Property(e => e.password)
                .IsUnicode(false);


            modelBuilder.Entity<Account>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Account)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Brand>()
                .Property(e => e.create_by)
                .IsUnicode(false);


            modelBuilder.Entity<Brand>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Brand)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Delivery>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Delivery>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Delivery)
                .WillCascadeOnDelete(false);


            modelBuilder.Entity<Feedback>()
                .Property(e => e.rate_star);          
              

            modelBuilder.Entity<Feedback>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Feedback>()
                .Property(e => e.stastus)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Genre)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Oder_Detail>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Oder_Detail>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .HasMany(e => e.Oder_Detail)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Payment>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Payment)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.price);


            modelBuilder.Entity<Product>()
                .Property(e => e.quantity)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.status)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Feedbacks)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.disscount_id})
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasMany(e => e.Oder_Detail)
                .WithRequired(e => e.Product)
                .HasForeignKey(e => new {e.product_id, e.genre_id, e.disscount_id})
                .WillCascadeOnDelete(false);
        }
    }
}