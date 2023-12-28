using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoragePage : ContentPage
    {
        private StorageListView _view;

        public StoragePage()
        {
            InitializeComponent();

            _view = new StorageListView();
            this.BindingContext = _view;
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductView)((View)sender).BindingContext).Model;
            Navigation.PushAsync(new AdminProductPage(product));
        }
    }
}