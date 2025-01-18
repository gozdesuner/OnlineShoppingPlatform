using OnlineShoppingPlatform.DAL.Entities;
using System.Linq.Expressions;

namespace OnlineShoppingPlatform.DAL.Repositories
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User?> ValidateUserAsync(string email, string password);// - Geri dönüş: Eşleşen kullanıcı varsa `User`, yoksa `null`

        // Kullanıcıyı belirli bir koşula göre bulur
        Task<User?> SingleOrDefaultAsync(Expression<Func<User, bool>> predicate);
    }
}
