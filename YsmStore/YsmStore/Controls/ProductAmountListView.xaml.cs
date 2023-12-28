using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductAmountListView : CollectionView
    {
        public delegate void ItemTappedEventHandler(object sender, ItemTappedEventArgs e);
        public event ItemTappedEventHandler ProductTapped;

        public ProductAmountListView()
        {
            InitializeComponent();
        }

        private void DeleteButton_Tapped(object sender, EventArgs e)
        {
            ProductAmountView view = (ProductAmountView)((Button)sender).BindingContext;
            ((ViewList<ProductAmount, ProductAmountView>)this.ItemsSource).Remove(view);
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductAmountView)((View)sender).BindingContext).Product;
            ProductTapped?.Invoke(this, new ItemTappedEventArgs(this.ItemsSource, product, 0));
        }
    }
}