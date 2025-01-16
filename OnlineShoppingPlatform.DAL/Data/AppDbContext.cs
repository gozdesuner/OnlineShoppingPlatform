using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.DAL
{
    public class AppDbContext : DbContext
    {
        // Bağlantı seçeneklerini yapılandırmak için bir constructor tanımladım.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Veritabanı tablolarını temsil eden DbSet'ler.
        public DbSet<User> Users { get; set; }// Kullanıcı tablosu.
        public DbSet<Product> Products { get; set; }// Ürün tablosu.
        public DbSet<Order> Orders { get; set; }// Sipariş tablosu.
        public DbSet<OrderProduct> OrderProducts { get; set; }// Sipariş-Ürün bağlantı tablosu.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Çoka çok ilişki tanımlaması
            modelBuilder.Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Order)// Sipariş ile bağlantı.
                .WithMany(o => o.OrderProducts)// Bir sipariş birden çok OrderProduct içerir
                .HasForeignKey(op => op.OrderId);// Foreign key olarak OrderId kullanılır

            modelBuilder.Entity<OrderProduct>()
                .HasOne(op => op.Product)// Ürün ile bağlantı.
                .WithMany()// Bir ürün birden çok OrderProduct içerir
                .HasForeignKey(op => op.ProductId);// Foreign key olarak ProductId kullanılır


        }
    }
}
