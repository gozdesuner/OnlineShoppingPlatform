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
            try
            {
                // Güncellenecek siparişi veritabanında ara
                var existingOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
                if (existingOrder == null)
                    return false;

                try
                {
                    // Sipariş bilgilerini güncelle
                    existingOrder.OrderDate = order.OrderDate;
                    existingOrder.TotalAmount = order.TotalAmount;
                    existingOrder.CustomerId = order.CustomerId;
                    _unitOfWork.Orders.Update(existingOrder);

                    // Önce tüm mevcut OrderProducts kayıtlarını sil
                    var existingOrderProducts = await _unitOfWork.OrderProducts.GetAllAsync();
                    var productsToDelete = existingOrderProducts.Where(op => op.OrderId == order.Id).ToList();
                    
                    if (productsToDelete.Any())
                    {
                        foreach (var item in productsToDelete)
                        {
                            _unitOfWork.OrderProducts.Remove(item);
                        }
                    }

                    // Değişiklikleri kaydet
                    await _unitOfWork.CompleteAsync();

                    // Yeni OrderProducts ekle
                    if (order.OrderProducts != null && order.OrderProducts.Any())
                    {
                        var distinctProducts = order.OrderProducts
                            .GroupBy(op => op.ProductId)
                            .Select(g => new OrderProduct
                            {
                                OrderId = existingOrder.Id,
                                ProductId = g.Key,
                                Quantity = g.Sum(x => x.Quantity)
                            })
                            .ToList();

                        foreach (var orderProduct in distinctProducts)
                        {
                            await _unitOfWork.OrderProducts.AddAsync(orderProduct);
                        }

                        // Son değişiklikleri kaydet
                        await _unitOfWork.CompleteAsync();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Update Error: {ex.Message}");
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
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

