using YsmStore.API.Models;

namespace YsmStore.API.Data.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IList<Order>> GetByCustomerId(Guid customerId, int offset, int limit);
        Task<IList<Order>> GetByStatusBetween(OrderStatus? status, DateTime start, DateTime end, int offset, int limit);
    }
}
