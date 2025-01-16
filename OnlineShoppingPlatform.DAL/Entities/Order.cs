namespace OnlineShoppingPlatform.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }// Sipariş kimlik numarası (primary key).
        public DateTime OrderDate { get; set; }// Sipariş tarihi.
        public decimal TotalAmount { get; set; }// Siparişin toplam tutarı.
        public int CustomerId { get; set; }// Siparişin sahibi olan müşteri kimlik numarası (foreign key).
        public User? Customer { get; set; }// Siparişin sahibi müşteri (navigation property).
        public ICollection<OrderProduct>? OrderProducts { get; set; }
    }
}
