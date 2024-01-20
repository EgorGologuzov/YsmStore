namespace YsmStore.API.Models
{
    public class OrderedProduct
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Amount { get; set; }
    }
}
