using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingPlatform.DAL.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]//Model Validation.
        [MaxLength(100, ErrorMessage = "Ürün adı 100 karakterden uzun olamaz.")]
        public string ProductName { get; set; } = "";

        [Required(ErrorMessage = "Fiyat alanı zorunludur.")]//Model Validation.
        [Range(0.01, double.MaxValue, ErrorMessage = "Fiyat sıfırdan büyük olmalıdır.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]//Model Validation.
        [Range(0, int.MaxValue, ErrorMessage = "Stok miktarı negatif olamaz.")]
        public int StockQuantity { get; set; }
    }
}
