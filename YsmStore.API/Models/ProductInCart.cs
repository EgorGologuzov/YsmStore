namespace YsmStore.API.Models
{
    public class ProductInCart
    {
        public Guid CustomerId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
