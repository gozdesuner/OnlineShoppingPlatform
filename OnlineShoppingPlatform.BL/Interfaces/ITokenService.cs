using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.BL.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
