using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace OnlineShoppingPlatform.DAL.Repositories
{
    // Generic repository sınıfı, IRepository arayüzünü uygular.
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _context; // DbContext örneği
        private readonly DbSet<T> _dbSet;// Veritabanındaki tablolarla etkileşim kurmak için DbSet

        public Repository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();// T tipi için ilgili DbSet'i alır
        }

        public async Task<T?> GetByIdAsync(int id)// Nullable dönüş tipi
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }
    }
}