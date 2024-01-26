using YsmStore.API.Models;

namespace YsmStore.API.Dto
{
    public class OrdersRequestDto
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
        public OrderStatus? StatusFilter { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
