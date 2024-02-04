using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    internal class ProductListView : LoadingViewListView<Product, ProductView, ProductQuery>
    {
        public string[] AllTitles { get; private set; }
        public string[] AllSortVariants { get; private set; }

        public Command Refresh { get; set; }

        public ProductListView() : base(ProductAdapter.Execute)
        {
            Refresh = new Command(Refresh_Execute, () => true);
            LoadAsync();
            LoadNext.Execute(null);
        }

        public async void LoadAsync()
        {
            AllTitles = await ProductAdapter.GetAllCategories();
            AllSortVariants = await ProductAdapter.GetAllSortVariants();
            InvokePropertyChanged(nameof(AllTitles));
            InvokePropertyChanged(nameof(AllSortVariants));
        }

        private void Refresh_Execute()
        {
            Items.Clear();
            Query.IsEndReached = false;
            LoadNext.Execute(null);
        }
    }
}
