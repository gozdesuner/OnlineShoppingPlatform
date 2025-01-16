using System.Linq.Expressions;

namespace OnlineShoppingPlatform.DAL.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id); // ID'ye göre tek bir kayıt alır
        Task<IEnumerable<T>> GetAllAsync(); // Tüm kayıtları alır
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate); // Şarta göre kayıtları bulur
        Task AddAsync(T entity); // Yeni bir kayıt ekler
        void Update(T entity); // Mevcut bir kaydı günceller
        void Remove(T entity); // Bir kaydı siler
    }
}