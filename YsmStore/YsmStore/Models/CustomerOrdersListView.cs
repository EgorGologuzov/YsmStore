using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class CustomerOrdersListView : LoadingViewListView<Order, OrderView, CustomerOrdersQuery>
    {
        public Customer Customer { get; set; }

        public CustomerOrdersListView(Customer customer) : base(OrderAdapter.Execute)
        {
            Customer = customer;
            Query.CustomerId = Customer.Id;
            Items.CollectionChanged += OnItemsCollectionChanged;
            LoadNext.Execute(null);
        }

        public async void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }

            foreach (var item in e.NewItems)
            {
                OrderView view = (OrderView)item;
                view.LoadProducts();
            }
        }
    }
}
