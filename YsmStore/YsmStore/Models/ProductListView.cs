using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    internal class ProductListView : LoadingViewListView<Product, ProductView, ProductQuery>
    {
        public ProductListView() : base(ProductAdapter.Execute)
        {
            Refresh = new Command(Refresh_Execute, () => true);
            LoadNext.Execute(null);
        }

        public string[] AllTitles { get => ProductAdapter.GetAllCategories(); }
        public string[] AllSortVariants { get => ProductAdapter.GetAllSortVariants(); }

        public Command Refresh { get; private set; }

        private void Refresh_Execute()
        {
            Items.Clear();
            LoadNext.Execute(null);
        }
    }
}
