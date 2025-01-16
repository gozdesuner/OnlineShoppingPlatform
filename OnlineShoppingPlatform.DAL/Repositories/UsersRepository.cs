using Microsoft.EntityFrameworkCore;
using OnlineShoppingPlatform.DAL.Entities;
using System.Linq.Expressions;

namespace OnlineShoppingPlatform.DAL.Repositories
{
    public class UsersRepository : Repository<User>, IUsersRepository
    {
        private readonly AppDbContext _context;

        public UsersRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Email == email && u.Password == password);
        }

        // SingleOrDefaultAsync metodu
        public async Task<User?> SingleOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Users.SingleOrDefaultAsync(predicate);
        }
    }
}
