using YsmStore.Data;

namespace YsmStore.Models
{
    public class CartView : LoadingViewListView<ProductAmount, ProductAmountView, CustomerCartQuery>
    {
        public Customer Customer { get; private set; }

        public CartView(Customer customer) : base(UserAdapter.LoadCart)
        {
            this.Customer = customer;
            Query.CustomerToken = customer.Token;
            LoadNext.Execute(null);
        }
    }
}
