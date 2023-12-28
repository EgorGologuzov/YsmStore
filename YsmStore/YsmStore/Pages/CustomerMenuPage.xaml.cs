using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerMenuPage : ContentPage
    {
        private CustomerView _customerView;

        public CustomerMenuPage()
        {
            InitializeComponent();

            _customerView = new CustomerView((Customer)AuthSystem.LoginedUser);
            this.BindingContext = _customerView;
        }

        private void LogoutButton_Tapped(object sender, EventArgs e)
        {
            AuthSystem.Logout();
        }

        private void ChooseProductButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProductListPage());
        }

        private void CartButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CustomerCartPage());
        }

        private void OrdersButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CustomerOrdersPage((Customer)AuthSystem.LoginedUser));
        }
    }
}