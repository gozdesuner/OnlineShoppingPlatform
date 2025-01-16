using OnlineShoppingPlatform.DAL.Entities;
using OnlineShoppingPlatform.DAL.Repositories;

namespace OnlineShoppingPlatform.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            // Repository'leri initialize et
            Users = new UsersRepository(_context); // IUsersRepository türünde
            Products = new Repository<Product>(_context);
            Orders = new Repository<Order>(_context);
            OrderProducts = new Repository<OrderProduct>(_context);
        }

        // IUnitOfWork arayüzündeki özellikler
        public IUsersRepository Users { get; private set; } // IUsersRepository türünde
        public IRepository<Product> Products { get; private set; }
        public IRepository<Order> Orders { get; private set; }
        public IRepository<OrderProduct> OrderProducts { get; private set; }

        // Veritabanı işlemlerini kaydet
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose işlemi
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
