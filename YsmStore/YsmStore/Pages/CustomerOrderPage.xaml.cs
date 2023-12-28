using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerOrderPage : ContentPage
    {
        private OrderView _view;

        public CustomerOrderPage(Order order)
        {
            InitializeComponent();

            _view = new OrderView(order);
            this.BindingContext = _view;
        }

        private void Product_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductAmountView)((View)sender).BindingContext).Product;
            Navigation.PushAsync(new ProductPage(product));
        }
    }
}