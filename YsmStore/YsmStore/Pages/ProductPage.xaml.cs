using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {
        private ProductView _view;

        public ProductPage(Product product)
        {
            InitializeComponent();
            SetView(new ProductView(product));
        }

        private void Option1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptionVariant variant = e.CurrentSelection[0] as OptionVariant;
            SetView(new ProductView(ProductAdapter.Get(_view.Model.Title, variant.Text, _view.Model.Option2)));
        }

        private void Option2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptionVariant variant = e.CurrentSelection[0] as OptionVariant;
            SetView(new ProductView(ProductAdapter.Get(_view.Model.Title, _view.Model.Option1, variant.Text)));
        }

        private void SetView(ProductView view)
        {
            _view = view;
            BindingContext = view;
        }

        private void OrderButton_Tapped(object sender, EventArgs e)
        {
            Product product = ((ProductView)((Button)sender).BindingContext).Model;
            Navigation.PushAsync(
                new OrderPage(
                    (Customer)AuthSystem.LoginedUser,
                    new List<ProductAmount>() { new ProductAmount(product.Id, 1) }
                    )
                );
        }

        private void CartButton_Tapped(object sender, EventArgs e)
        {
            if (_view.CanOrder == false)
            {
                DisplayAlert("Ошибка", "Сначала выберите доступные опции для товара", "ОК");
                return;
            }

            try
            {
                UserAdapter.AddToCart((Customer)AuthSystem.LoginedUser, new ProductAmount(_view.Model.Id, 1));
                DisplayAlert("Сообщение", "Товар добавлен в корзину", "ОК");
            }
            catch (YsmStoreException ex)
            {
                DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }
    }
}