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

            _view = new OrderView(order);
            this.BindingContext = _view;
        }

        private void SaveButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                OrderAdapter.Push(_view.Model);
                Navigation.PopAsync();
            }
            catch (YsmStoreException ex)
            {
                DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            OrderAdapter.Pull(_view.Model);
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductAmountView)((View)sender).BindingContext).Product;
            Navigation.PushAsync(new AdminProductPage(product));
        }
    }
}