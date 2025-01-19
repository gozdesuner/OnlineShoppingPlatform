using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OnlineShoppingPlatform.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }// Sipariş kimlik numarası (primary key).

        [Required]
        public DateTime OrderDate { get; set; }// Sipariş tarihi.

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }// Siparişin toplam tutarı.

        [Required]
        public int CustomerId { get; set; }// Siparişin sahibi olan müşteri kimlik numarası (foreign key).

        [JsonIgnore]
        public User? Customer { get; set; }// Siparişin sahibi müşteri (navigation property).

        [JsonIgnore]
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
