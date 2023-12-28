using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminMenuPage : ContentPage
    {
        public AdminMenuPage()
        {
            InitializeComponent();
        }

        private void LogoutButton_Tapped(object sender, EventArgs e)
        {
            AuthSystem.Logout();
        }

        private void CustomersButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CustomerListViewPage());
        }

        private void OrdersButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AdminOrdersPage());
        }

        private void StorageButton_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new StoragePage());
        }
    }
}