using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için gerekli kütüphane
using Microsoft.AspNetCore.Mvc; // API controller'lar için gerekli
using OnlineShoppingPlatform.BL.Helpers; // PasswordProtector sınıfını dahil eder
using OnlineShoppingPlatform.BL.Interfaces; // Business Logic katmanı arayüzleri için
using OnlineShoppingPlatform.DAL.Entities; // Kullanıcı sınıfını dahil eder

namespace OnlineShoppingPlatform.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IUserService _userService;
        private readonly PasswordProtector _passwordProtector;

        // Constructor: Gerekli servisleri DI ile alır
        public AuthController(ITokenService tokenService, IUserService userService, PasswordProtector passwordProtector)
        {
            _tokenService = tokenService;
            _userService = userService;
            _passwordProtector = passwordProtector;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginUser)
        {
            // Email ile kullanıcıyı veritabanında bul
            var users = await _userService.GetAllUsersAsync();
            var user = users.FirstOrDefault(u => u.Email == loginUser.Email);

            // Kullanıcı bulunamazsa veya şifre doğrulanamazsa 401 Unauthorized döner
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre");

            try
            {
                // Şifreyi çöz ve doğrula
                var decryptedPassword = _passwordProtector.Unprotect(user.Password);
                if (decryptedPassword != loginUser.Password)
                    return Unauthorized("Geçersiz kullanıcı adı veya şifre");
            }
            catch
            {
                return Unauthorized("Şifre çözme sırasında bir hata oluştu");
            }

            // Kullanıcı doğrulandıktan sonra token oluştur ve döndür
            var token = _tokenService.GenerateToken(user);
            return Ok(new { Token = token });
        }

        [Authorize]
        [HttpGet("secure-data")]
        public IActionResult GetSecureData()
        {
            return Ok("Bu endpoint'e yalnızca yetkili kullanıcılar erişebilir.");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Model doğrulama hatalarını döndür

            // Kullanıcının şifresini şifrele
            user.Password = _passwordProtector.Protect(user.Password);
            await _userService.AddUserAsync(user); // Kullanıcıyı veritabanına kaydet
            return Ok("Kullanıcı başarıyla kaydedildi.");
        }
    }
}
