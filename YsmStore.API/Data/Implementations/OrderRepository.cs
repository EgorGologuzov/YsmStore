using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using MimeKit.Encodings;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Models;
using YsmStore.Services.Utils;

namespace YsmStore.API.Data.Implementations
{
    public class OrderRepository : GeneralRepository<Order>, IOrderRepository
    {
        public OrderRepository(PostgresContext context, ILogger<OrderRepository> logger) : base(context, logger)
        {
        }

        public override Task<Order> GetById(object id)
        {
            return Context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .Where(o => o.Id == (int)id)
                .FirstOrDefaultAsync();
        }

        public override async Task<Order> Create(Order entity)
        {
            IEnumerable<int> orderedIds = entity.Products.Select(op => op.ProductId);

            IEnumerable<Product> products = await Context.Products.Where(p => orderedIds.Contains(p.Id)).ToListAsync();

            var join = entity.Products.Join(products, op => op.ProductId, p => p.Id, (op, p) => new { Product = p, OrderedAmount = op.Amount });

            foreach (var anonim in join)
            {
                if (anonim.OrderedAmount <= 0)
                {
                    throw new AmountIsNotPositiveException();
                }

                if (anonim.OrderedAmount > anonim.Product.Quantity)
                {
                    throw new NotEnoughtProductException(anonim.Product.Id);
                }

                anonim.Product.Quantity -= anonim.OrderedAmount;
            }

            return await base.Create(entity);
        }

        public override async Task<Order> Update(object id, Order entity)
        {
            Order oldEntity = await Context.Orders.FindAsync(id);

            if (oldEntity is null)
            {
                throw new EntityNotFoundException();
            }

            Context.Entry(oldEntity).CurrentValues.SetValues(entity);
            EntityEntry<Order> entry = Context.Orders.Update(oldEntity);
            Order result = entry.Entity;
            var original = (Order)entry.OriginalValues.ToObject();

            await Context.SaveChangesAsync();

            if (original.Customer is not null)
            {
                original.Customer.Orders = null;
            }

            if (result.Customer is not null)
            {
                result.Customer.Orders = null;
            }

            Logger.LogInformation("Updated {type} from {oldData} to {newData}", typeof(Order).Name, original.ToJson(), result.ToJson());

            return result;
        }

        public async Task<IList<Order>> GetByCustomerId(Guid customerId, int offset, int limit)
        {
            return await Context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .OrderBy(o => o.Id)
                .Where(o => o.CustomerId == customerId)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        public async Task<IList<Order>> GetByStatusBetween(OrderStatus? status, DateTime start, DateTime end, int offset, int limit)
        {
            IQueryable<Order> query = Context.Orders.Include(o => o.Customer).Include(o => o.Products);

            if (status is not null)
            {
                query = query.Where(o => o.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .Where(o => o.OrderDate >= start && o.OrderDate <= end)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
