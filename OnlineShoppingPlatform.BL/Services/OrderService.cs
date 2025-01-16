using OnlineShoppingPlatform.BL.Interfaces;
using OnlineShoppingPlatform.DAL.Entities;
using OnlineShoppingPlatform.DAL.UnitOfWork;

namespace OnlineShoppingPlatform.BL.Services
{
    public class OrderService : IOrderService // IOrderService arayüzünü implement eder
    {
        private readonly IUnitOfWork _unitOfWork; // Veritabanı işlemleri için UnitOfWork kullanıyoruz

        // Constructor, Dependency Injection ile UnitOfWork'ü alır
        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Tüm siparişleri getir
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _unitOfWork.Orders.GetAllAsync(); // UnitOfWork aracılığıyla tüm siparişleri al
        }

        // ID'ye göre sipariş getir
        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _unitOfWork.Orders.GetByIdAsync(id); // ID'ye göre siparişi getir
        }

        // Yeni sipariş oluştur
        public async Task CreateOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order); // Yeni siparişi veritabanına ekle
            await _unitOfWork.CompleteAsync(); // Değişiklikleri kaydet
        }

        // Mevcut siparişi güncelle
        public async Task<bool> UpdateOrderAsync(Order order)
        {
            // Güncellenmek istenen siparişi veritabanında ara
            var existingOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
            if (existingOrder == null) // Sipariş bulunamazsa false döndür
                return false;

            // Sipariş bilgilerini güncelle
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.TotalAmount = order.TotalAmount;
            _unitOfWork.Orders.Update(existingOrder); // Güncellemeyi UnitOfWork üzerinden yap
            await _unitOfWork.CompleteAsync(); // Değişiklikleri kaydet
            return true; // Başarılı bir şekilde güncellendi
        }

        // Siparişi sil
        public async Task<bool> DeleteOrderAsync(int id)
        {
            // Silinmek istenen siparişi veritabanında ara
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null) // Sipariş bulunamazsa false döndür
                return false;

            _unitOfWork.Orders.Remove(order); // Siparişi sil
            await _unitOfWork.CompleteAsync(); // Değişiklikleri kaydet
            return true; // Başarılı bir şekilde silindi
        }
    }
}

