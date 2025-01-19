using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace OnlineShoppingPlatform.DAL.Entities
{
    public class OrderProduct
    {
        [Key]
        [Column(Order = 1)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ProductId { get; set; }

        public Order? Order { get; set; }

        [JsonIgnore]
        public Product? Product { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Miktar 1'den büyük olmalıdır.")]
        public int Quantity { get; set; }
    }
}
