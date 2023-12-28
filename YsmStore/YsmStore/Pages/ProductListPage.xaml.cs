using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductListPage : ContentPage
    {
        public ProductListPage()
        {
            InitializeComponent();
        }

        private void SortButton_Tapped(object sender, EventArgs e)
        {
            sortPicker.Focus();
        }

        private void FilterButton_Tapped(object sender, EventArgs e)
        {
            filterPicker.Focus();
        }

        private void QueryChanged(object sender, EventArgs e)
        {
            viewList.Refresh.Execute(null);
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductView)((View)sender).BindingContext).Model;
            Navigation.PushAsync(new ProductPage(product));
        }

        private void MenuButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PopToRootAsync();
        }
    }
}