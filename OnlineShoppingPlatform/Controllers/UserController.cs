using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.BL.Interfaces;
using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(User user)
        {
            await _userService.AddUserAsync(user); // Kullanıcıyı ekle (şifre hashlenmiş olacak)
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user); // 201 Created döndür
        }

        [HttpGet("cause-error")]
        public IActionResult CauseError()
        {
            throw new Exception("Bu bir test hatasıdır.");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.Id) // ID uyumsuzluğu kontrolü
                return BadRequest("ID'ler eşleşmiyor.");

            var existingUser = await _userService.GetUserByIdAsync(id); // Kullanıcıyı kontrol et
            if (existingUser == null) // Kullanıcı yoksa
                return NotFound("Kullanıcı bulunamadı."); // 404 Not Found döndür

            await _userService.UpdateUserAsync(user); // Güncelleme işlemini yap
            return NoContent(); // Başarılı olduğunda 204 No Content döndür
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id); // Kullanıcıyı veri tabanından getir
            if (user == null) // Eğer kullanıcı yoksa
                return NotFound("Kullanıcı bulunamadı."); // 404 Not Found döndür

            await _userService.DeleteUserAsync(id); // Kullanıcıyı sil
            return NoContent(); // Başarılı olduğunda 204 No Content döndür  
        }
    }
}

