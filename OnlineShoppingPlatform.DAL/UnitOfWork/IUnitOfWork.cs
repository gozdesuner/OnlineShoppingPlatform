using OnlineShoppingPlatform.DAL.Entities;
using OnlineShoppingPlatform.DAL.Repositories;

namespace OnlineShoppingPlatform.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUsersRepository Users { get; }
        IRepository<Product> Products { get; }
        IRepository<Order> Orders { get; }
        IRepository<OrderProduct> OrderProducts { get; }
        Task<int> CompleteAsync();
    }
}
