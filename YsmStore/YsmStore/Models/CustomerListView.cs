using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class CustomerListView : LoadingViewListView<Customer, CustomerView, CustomersByEmailQuery>
    {
        public Command Search { get; set; }

        public CustomerListView() : base(UserAdapter.Execute)
        {
            Search = new Command(Search_Execute, () => true);
            LoadNext.Execute(null);
        }

        private void Search_Execute()
        {
            this.Items.Clear();
            this.Query.IsEndReached = false;
            this.LoadNext.Execute(null);
        }
    }
}
