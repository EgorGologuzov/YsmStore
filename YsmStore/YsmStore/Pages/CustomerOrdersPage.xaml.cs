using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerOrdersPage : ContentPage
    {
        private CustomerOrdersListView _view;

        public CustomerOrdersPage(Customer customer)
        {
            InitializeComponent();

            _view = new CustomerOrdersListView(customer);
            this.BindingContext = _view;
        }

        private void Order_Tapped(object sender, EventArgs e)
        {
            Order order = ((OrderView)((View)sender).BindingContext).Model;

            if (AuthSystem.LoginedUser is Customer)
            {
                Navigation.PushAsync(new CustomerOrderPage(order));
            }
            else if (AuthSystem.LoginedUser is Admin)
            {
                Navigation.PushAsync(new AdminOrderPage(order));
            }
        }
    }
}