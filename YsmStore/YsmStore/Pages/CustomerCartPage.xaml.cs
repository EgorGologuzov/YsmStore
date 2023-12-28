using System;
using System.Collections.Generic;
using System.Collections.Specialized;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomerCartPage : ContentPage
    {
        private CartView _view;

        public CustomerCartPage()
        {
            InitializeComponent();

            _view = new CartView((Customer)AuthSystem.LoginedUser);
            BindingContext = _view;
        }

        private void OnProductCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;

            foreach (ProductAmountView view in e.NewItems)
            {
                view.Model.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(view.Model.Amount))
                        UpdateProductAmountInCart(view.Model);
                };
            }
        }

        private void UpdateProductAmountInCart(ProductAmount productAmount)
        {
            UserAdapter.SetProductAmountInCart(_view.Customer, productAmount);
        }

        private void OrderButton_Tapped(object sender, EventArgs e)
        {
            IList<ProductAmount> cart = UserAdapter.GetCart(_view.Customer);
            Navigation.PushAsync(new OrderPage(_view.Customer, cart));
        }

        private void Product_Tapped(object sender, ItemTappedEventArgs e)
        {
            Navigation.PushAsync(new ProductPage((Product)e.Item));
        }
    }
}