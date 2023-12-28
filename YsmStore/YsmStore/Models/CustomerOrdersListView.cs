using YsmStore.Data;

namespace YsmStore.Models
{
    public class CustomerOrdersListView : LoadingViewListView<Order, OrderView, CustomerOrdersQuery>
    {
        public Customer Customer { get; private set; }

        public CustomerOrdersListView(Customer customer) : base(OrderAdapter.Execute)
        {
            Customer = customer;
            Query.CustomerToken = customer.Token;
            LoadNext.Execute(null);
        }
    }
}
