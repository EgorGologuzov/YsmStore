using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class CartView : LoadingViewListView<ProductAmount, ProductAmountView, CustomerCartQuery>
    {
        public Customer Customer { get; set; }

        public CartView(Customer customer) : base(UserAdapter.Execute)
        {
            this.Customer = customer;
            Query.CustomerId = customer.Id;
            Items.CollectionChanged += OnItemsCollectionChanged;
            LoadNext.Execute(null);
        }

        private async void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }

            foreach (var item in e.NewItems)
            {
                ProductAmountView view = (ProductAmountView)item;
                view.LoadProduct();
            }
        }
    }
}
