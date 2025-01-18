using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingPlatform.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }// Sipariş kimlik numarası (primary key).

        [Required]
        public DateTime OrderDate { get; set; }// Sipariş tarihi.

        [Range(0, double.MaxValue, ErrorMessage = "Toplam tutar negatif olamaz.")]
        public decimal TotalAmount { get; set; }// Siparişin toplam tutarı.

        [Required]
        public int CustomerId { get; set; }// Siparişin sahibi olan müşteri kimlik numarası (foreign key).
        public User? Customer { get; set; }// Siparişin sahibi müşteri (navigation property).
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
