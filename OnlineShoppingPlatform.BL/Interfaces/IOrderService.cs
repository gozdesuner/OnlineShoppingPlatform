using OnlineShoppingPlatform.DAL.Entities;

namespace OnlineShoppingPlatform.BL.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync(); // Tüm siparişler
        Task<Order?> GetOrderByIdAsync(int id); // ID'ye göre sipariş
        Task CreateOrderAsync(Order order); // Sipariş oluştur
        Task<bool> UpdateOrderAsync(Order order); // Sipariş güncelle
        Task<bool> DeleteOrderAsync(int id); // Sipariş sil
    }
}
