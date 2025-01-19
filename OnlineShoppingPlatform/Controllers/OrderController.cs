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
        private readonly IUserService _userService; // IUserService bağımlılığını alıyor

        public OrderController(IOrderService orderService, IUserService userService) // Dependency Injection(Constructor, Dependency Injection ile IOrderService ve IUserService bağımlılığını alıyor)
        {
            _orderService = orderService;// Dependency Injection ile gelen servisi ata
            _userService = userService; // Dependency Injection ile gelen servisi ata
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                // Kullanıcının varlığını kontrol et
                var user = await _userService.GetUserByIdAsync(createOrderDto.CustomerId);
                if (user == null)
                    return BadRequest($"CustomerId: {createOrderDto.CustomerId} olan müşteri bulunamadı.");

                var order = new Order
                {
                    CustomerId = createOrderDto.CustomerId,
                    TotalAmount = createOrderDto.TotalAmount,
                    OrderDate = createOrderDto.OrderDate,
                    OrderProducts = createOrderDto.ProductIds.Select(productId => new OrderProduct
                    {
                        ProductId = productId,
                        Quantity = 1
                    }).ToList()
                };

                await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrderById), new { id = order.Id }, order);
            }
            catch (Exception ex)
            {
                // Hata mesajını loglama
                Console.WriteLine($"Hata: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { Message = ex.Message, Detail = ex.StackTrace });
            }
        }

        // Sipariş güncelle
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] CreateOrderDto updateOrderDto)
        {
            try
            {
                // Kullanıcının varlığını kontrol et
                var user = await _userService.GetUserByIdAsync(updateOrderDto.CustomerId);
                if (user == null)
                    return BadRequest($"CustomerId: {updateOrderDto.CustomerId} olan müşteri bulunamadı.");

                // Güncellenecek siparişin var olup olmadığını kontrol et
                var existingOrder = await _orderService.GetOrderByIdAsync(id);
                if (existingOrder == null)
                    return NotFound("Sipariş bulunamadı.");

                // Mevcut siparişi güncelle
                existingOrder.CustomerId = updateOrderDto.CustomerId;
                existingOrder.TotalAmount = updateOrderDto.TotalAmount;
                existingOrder.OrderDate = updateOrderDto.OrderDate;
                existingOrder.OrderProducts = updateOrderDto.ProductIds.Select(productId => new OrderProduct
                {
                    OrderId = id,
                    ProductId = productId,
                    Quantity = 1
                }).ToList();

                var updated = await _orderService.UpdateOrderAsync(existingOrder);
                if (!updated)
                    return StatusCode(500, "Sipariş güncellenirken bir hata oluştu.");

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return StatusCode(500, new { Message = ex.Message, Detail = ex.StackTrace });
            }
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
