using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerListViewPage : ContentPage
    {
        private CustomerListView _list;

        public CustomerListViewPage()
        {
            InitializeComponent();

            _list = new CustomerListView();
            this.BindingContext = _list;
        }

        private void OrdersButton_Tapped(object sender, EventArgs e)
        {
            Customer customer = ((CustomerView)((Button)sender).BindingContext).Model;
            Navigation.PushAsync(new CustomerOrdersPage(customer));
        }
    }
}