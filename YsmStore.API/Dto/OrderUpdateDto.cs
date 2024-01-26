using YsmStore.API.Models;

namespace YsmStore.API.Dto
{
    public class OrderUpdateDto
    {
        public int Id { get; set; }
        public DateTime DeliveryDate { get; set; }
        public OrderStatus Status { get; set; }
    }
}
