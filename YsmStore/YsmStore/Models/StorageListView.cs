using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class StorageListView : LoadingViewListView<Product, ProductView, StorageQuery>
    {
        public StorageListView() : base(ProductAdapter.Execute)
        {
            Search = new Command(Execute_Search, () => true);
            this.LoadNext.Execute(null);
        }

        public Command Search { get; private set; }

        private void Execute_Search()
        {
            this.Items.Clear();
            this.Query.IsEndReached = false;
            this.LoadNext.Execute(null);
        }
    }
}
