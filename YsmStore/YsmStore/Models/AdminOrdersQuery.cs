using System;

namespace YsmStore.Models
{
    public class AdminOrdersQuery : Query
    {
        public int? OrderId { get; set; }
        public OrderStatus? StatusFilter { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now - TimeSpan.FromDays(300);
        public DateTime EndDate { get; set; } = DateTime.Now + TimeSpan.FromDays(1);
    }
}
