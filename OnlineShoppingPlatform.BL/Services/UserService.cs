using OnlineShoppingPlatform.BL.Helpers; // PasswordProtector sınıfını dahil eder.
using OnlineShoppingPlatform.BL.Interfaces; // IUserService arayüzünü kullanabilmek için eklenir.
using OnlineShoppingPlatform.DAL.Entities; // Kullanıcı veritabanı nesnelerini kullanmak için eklenir.
using OnlineShoppingPlatform.DAL.UnitOfWork; // UnitOfWork desenini kullanarak veritabanı işlemlerini yönetmek için eklenir.

namespace OnlineShoppingPlatform.BL.Services
{
    public class UserService : IUserService // IUserService arayüzünü implement eder.
    {
        private readonly IUnitOfWork _unitOfWork; // Veritabanı işlemlerini yönetmek için UnitOfWork nesnesi.
        private readonly PasswordProtector _passwordProtector;

        public UserService(IUnitOfWork unitOfWork, PasswordProtector passwordProtector)
        {
            _unitOfWork = unitOfWork;
            _passwordProtector = passwordProtector; // DI ile gelen PasswordProtector nesnesini atar.
        }

        public async Task<User?> ValidateUserAsync(string email, string password)
        {
            // Veritabanında email ile eşleşen kullanıcıyı bul
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.Email == email);

            // Kullanıcı bulunamazsa null döndür
            if (user == null)
                return null;

            // Şifre doğrulama
            try
            {
                var decryptedPassword = _passwordProtector.Unprotect(user.Password); // Şifreyi çöz
                if (decryptedPassword != password)
                    return null;
            }
            catch
            {
                return null; // Şifre çözülemezse null döndür
            }

            return user; // Kullanıcı doğruysa döndür
        }


        public async Task AddUserAsync(User user)
        {
            // Kullanıcının şifresini şifrele
            user.Password = _passwordProtector.Protect(user.Password);

            // Kullanıcıyı veritabanına ekle
            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();
        }


        public async Task<IEnumerable<User>> GetAllUsersAsync() // Tüm kullanıcıları listeleme metodu.
        {
            // Veritabanındaki tüm kullanıcıları döndürür.
            return await _unitOfWork.Users.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id) // ID ile kullanıcıyı bulma metodu.
        {
            // Veritabanında ID ile eşleşen kullanıcıyı döndürür, bulunamazsa null döner.
            return await _unitOfWork.Users.GetByIdAsync(id);
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _unitOfWork.Users.GetByIdAsync(user.Id);
            if (existingUser == null)
                throw new Exception("Kullanıcı bulunamadı.");

            // Şifre değişmişse yeni şifreyi şifrele
            if (user.Password != existingUser.Password)
            {
                user.Password = _passwordProtector.Protect(user.Password);
            }

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
        }


        public async Task DeleteUserAsync(int id) // Kullanıcıyı silme metodu.
        {
            // Silinmek istenen kullanıcıyı veritabanında arar.
            var user = await _unitOfWork.Users.GetByIdAsync(id);

            // Eğer kullanıcı bulunursa silme işlemini gerçekleştirir.
            if (user != null)
            {
                _unitOfWork.Users.Remove(user); // Kullanıcıyı siler.
                await _unitOfWork.CompleteAsync(); // Değişiklikleri kaydeder.
            }
        }
    }
}
