using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingPlatform.DAL.Entities
{
    public class CreateOrderDto
    {
        [Required(ErrorMessage = "Müşteri ID'si zorunludur.")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Sipariş tarihi zorunludur.")]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Toplam tutar zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Toplam tutar 0'dan büyük olmalıdır.")]
        public decimal TotalAmount { get; set; }

        [Required(ErrorMessage = "En az bir ürün seçilmelidir.")]
        public List<int> ProductIds { get; set; }
    }
} 