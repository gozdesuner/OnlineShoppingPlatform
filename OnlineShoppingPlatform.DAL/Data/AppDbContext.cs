using Microsoft.EntityFrameworkCore; // Entity Framework Core kütüphanesi (DbContext ve veritabanı işlemleri için)
using OnlineShoppingPlatform.DAL.Entities; // Projedeki Entity sınıflarını kullanmak için gerekli namespace

namespace OnlineShoppingPlatform.DAL // Data Access Layer için namespace
{
    public class AppDbContext : DbContext // DbContext sınıfını miras alan uygulama bağlamı
    {
        // Bağlantı seçeneklerini yapılandırmak için bir constructor tanımladım.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } // DbContext yapılandırma ayarları constructor üzerinden alınır

        // Veritabanı tablolarını temsil eden DbSet'ler.
        public DbSet<User> Users { get; set; } // Kullanıcı tablosu
        public DbSet<Product> Products { get; set; } // Ürün tablosu
        public DbSet<Order> Orders { get; set; } // Sipariş tablosu
        public DbSet<OrderProduct> OrderProducts { get; set; } // Sipariş-Ürün bağlantı tablosu (çoka çok ilişki)

        protected override void OnModelCreating(ModelBuilder modelBuilder) // Veritabanı modeli oluşturma
        {
            base.OnModelCreating(modelBuilder); // Varsayılan yapılandırmayı uygular

            // Çoka çok ilişki tanımlaması
            modelBuilder.Entity<OrderProduct>() // OrderProduct tablosu için yapılandırma
                .HasKey(op => new { op.OrderId, op.ProductId }); // Birleşik birincil anahtar tanımlandı (OrderId, ProductId)

            modelBuilder.Entity<OrderProduct>() // OrderProduct için ilişki tanımlaması
                .HasOne(op => op.Order) // Her OrderProduct bir siparişe bağlıdır
                .WithMany(o => o.OrderProducts) // Bir sipariş birden çok OrderProduct'a sahip olabilir
                .HasForeignKey(op => op.OrderId); // Yabancı anahtar olarak OrderId tanımlandı

            modelBuilder.Entity<OrderProduct>() // OrderProduct için ilişki tanımlaması
                .HasOne(op => op.Product) // Her OrderProduct bir ürüne bağlıdır
                .WithMany() // Bir ürün birden çok OrderProduct'a sahip olabilir
                .HasForeignKey(op => op.ProductId); // Yabancı anahtar olarak ProductId tanımlandı
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) // DbContext yapılandırması
        {
            base.OnConfiguring(optionsBuilder); // Varsayılan yapılandırmayı uygular
            optionsBuilder.EnableSensitiveDataLogging(); // Hata ayıklama için hassas veri loglamasını etkinleştirir
        }
    }
}

