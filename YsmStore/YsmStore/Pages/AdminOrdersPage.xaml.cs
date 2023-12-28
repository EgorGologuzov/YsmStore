using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOrdersPage : ContentPage
    {
        private AdminOrdersListView _view;

        public AdminOrdersPage()
        {
            InitializeComponent();

            _view = new AdminOrdersListView();
            this.BindingContext = _view;
        }

        private void Order_Tapped(object sender, EventArgs e)
        {
            Order order = ((OrderView)((View)sender).BindingContext).Model;
            Navigation.PushAsync(new AdminOrderPage(order));
        }

        private void OrderIdString_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue) && _view.Query.OrderId != null)
            {
                _view.Query.OrderId = null;
            }
        }
    }
}