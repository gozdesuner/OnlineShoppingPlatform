using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.BL.Helpers;

namespace OnlineShoppingPlatform.API.Controllers
{
    [Authorize(Roles = "Admin")] // Yalnızca admin kullanıcılar erişebilir
    [Route("api/[controller]")]
    [ApiController]
    public class MaintenanceController : ControllerBase
    {
        private readonly MaintenanceState _maintenanceState;

        public MaintenanceController(MaintenanceState maintenanceState)
        {
            _maintenanceState = maintenanceState;
        }

        [HttpPost("enable")]
        public IActionResult EnableMaintenance()
        {
            _maintenanceState.SetMaintenanceMode(true); // Bakım modunu aktif et
            return Ok("Bakım modu aktif edildi.");
        }

        [HttpPost("disable")]
        public IActionResult DisableMaintenance()
        {
            _maintenanceState.SetMaintenanceMode(false); // Bakım modunu kapat
            return Ok("Bakım modu devre dışı bırakıldı.");
        }

        [HttpGet("status")]
        public IActionResult GetMaintenanceStatus()
        {
            var status = _maintenanceState.IsInMaintenance();
            return Ok(new { MaintenanceMode = status }); // Bakım modunun durumunu döndür
        }
    }
}


