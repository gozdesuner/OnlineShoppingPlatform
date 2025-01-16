using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineShoppingPlatform.API.Controllers
{
    [Authorize] // Bu controller'daki tüm endpoint'ler yetkilendirme gerektirir
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        // Sadece Admin rolüne izin verilir
        [Authorize(Roles = "Admin")]
        [HttpGet("admin-only")]
        public IActionResult AdminOnly()
        {
            return Ok("Bu endpoint yalnızca Admin rolündeki kullanıcılar için.");
        }

        // Sadece Customer rolüne izin verilir
        [Authorize(Roles = "Customer")]
        [HttpGet("customer-only")]
        public IActionResult CustomerOnly()
        {
            return Ok("Bu endpoint yalnızca Customer rolündeki kullanıcılar için.");
        }


    }
}
//Güvenlik:Sadece yetkili kullanıcıların (örneğin, Admin veya Customer) belirli işlemleri gerçekleştirmesine izin vererek uygulama güvenliğini artırır.
//Esneklik:Her endpoint'e farklı roller için erişim izni verebilirsiniz (örneğin, sadece Admin'in kullanıcıları silebilmesi, ama hem Admin hem de Customer'ın raporları görüntüleyebilmesi).
//Rol Yönetimi:Kullanıcı rollerine bağlı olarak API erişimini kolayca yönetebilirsiniz. Örneğin, yeni bir rol eklemek (örneğin, "Manager") ve yetkilendirme kuralları oluşturmak oldukça basittir.