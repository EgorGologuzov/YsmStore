using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class CustomerListView : LoadingViewListView<Customer, CustomerView, CustomersByEmailQuery>
    {
        public CustomerListView() : base(UserAdapter.Execute)
        {
            Search = new Command(Search_Execute, Search_CanExecute);
            LoadNext.Execute(null);
        }

        public Command Search { get; private set; }

        private void Search_Execute()
        {
            this.Items.Models.Clear();
            this.LoadNext.Execute(null);
        }

        private bool Search_CanExecute() => true;
    }
}
