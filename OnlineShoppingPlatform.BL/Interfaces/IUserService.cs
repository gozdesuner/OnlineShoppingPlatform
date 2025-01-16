using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.BL.Interfaces
{
    public interface IUserService
    {
        Task<User?> ValidateUserAsync(string email, string password); // Kullanıcı doğrulama metodu
        Task<IEnumerable<User>> GetAllUsersAsync(); // Tüm kullanıcıları getir
        Task<User?> GetUserByIdAsync(int id); // ID'ye göre kullanıcı getir
        Task AddUserAsync(User user); // Yeni kullanıcı ekle
        Task UpdateUserAsync(User user); // Kullanıcıyı güncelle
        Task DeleteUserAsync(int id); // Kullanıcıyı sil
    }
}
