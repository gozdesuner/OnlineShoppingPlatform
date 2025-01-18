using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShoppingPlatform.BL.Interfaces;
using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.API.Controllers
{
    [Authorize] // Bu controller'a erişim için yetkilendirme gerekli
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService; // OrderService üzerinden işlemleri yapacağız.

        public OrderController(IOrderService orderService) // Dependency Injection(Constructor, Dependency Injection ile IOrderService bağımlılığını alıyor)
        {
            _orderService = orderService;// Dependency Injection ile gelen servisi ata
        }

        // Tüm siparişleri getir
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders); // 200 OK ve siparişler listesi döner
        }

        // ID'ye göre sipariş getir
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound("Sipariş bulunamadı."); // 404 Not Found

            return Ok(order); // 200 OK ve sipariş bilgisi döner
        }

        // Yeni sipariş ekle
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // 400 Bad Request ve validasyon hataları

            await _orderService.CreateOrderAsync(order);
            return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order); // 201 Created
        }

        // Sipariş güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)// İstek gövdesinden güncel sipariş bilgisi alınır
        {
            if (id != order.Id)
                return BadRequest("ID uyuşmazlığı."); // 400 Bad Request

            var updated = await _orderService.UpdateOrderAsync(order);
            if (!updated)
                return NotFound("Sipariş bulunamadı."); // 404 Not Found

            return NoContent(); // 204 No Content
        }

        // Sipariş sil
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _orderService.DeleteOrderAsync(id);
            if (!deleted)
                return NotFound("Sipariş bulunamadı."); // 404 Not Found

            return NoContent(); // 204 No Content
        }
    }
}
