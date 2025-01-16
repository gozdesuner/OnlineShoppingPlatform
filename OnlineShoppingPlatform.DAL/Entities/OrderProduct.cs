namespace OnlineShoppingPlatform.DAL.Entities
{
    public class OrderProduct
    {
        public int OrderId { get; set; }// Sipariş kimlik numarası (foreign key).
        public Order? Order { get; set; } // Sipariş ile ilişki (navigation property).
        public int ProductId { get; set; }// Ürün kimlik numarası (foreign key).
        public Product? Product { get; set; }// Ürün ile ilişki (navigation property).
        public int Quantity { get; set; }// Sipariş edilen ürün miktarı.
    }
}
