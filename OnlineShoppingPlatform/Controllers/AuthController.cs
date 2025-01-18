using Microsoft.AspNetCore.Authorization; // Yetkilendirme işlemleri için gerekli kütüphane
using Microsoft.AspNetCore.Mvc; // API controller'lar için gerekli
using OnlineShoppingPlatform.API.Filters;
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
        [ServiceFilter(typeof(TimeRestrictedAccessFilter))]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kullanıcıyı email ile bul
            var user = (await _userService.GetAllUsersAsync())
                .FirstOrDefault(u => u.Email == loginDto.Email);

            if (user == null)
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            // Şifre doğrulama
            if (!_passwordProtector.ValidatePassword(loginDto.Password, user.Password))
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");

            // JWT token oluştur
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

            await _userService.AddUserAsync(user); // Kullanıcıyı veritabanına kaydet
            return Ok("Kullanıcı başarıyla kaydedildi.");
        }
    }
}
