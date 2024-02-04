using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOrderPage : ContentPage
    {
        private OrderView _view;

        public AdminOrderPage(Order order)
        {
            InitializeComponent();

            SetView(order);
        }

        private async void SetView(Order order)
        {
            _view = new OrderView(order);
            await _view.LoadProducts();
            this.BindingContext = _view;
        }

        private async void SaveButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                await OrderAdapter.Push(_view.Model);
                await Navigation.PopAsync();
            }
            catch (YsmStoreException ex)
            {
                await DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();

            await OrderAdapter.Pull(_view.Model);
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductAmountView)((View)sender).BindingContext).Product;
            Navigation.PushAsync(new AdminProductPage(product));
        }
    }
}