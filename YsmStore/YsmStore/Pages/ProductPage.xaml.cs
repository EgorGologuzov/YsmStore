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
            SetView(product.Title, product.Option1, product.Option2);
        }

        private void Option1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptionVariant variant = e.CurrentSelection[0] as OptionVariant;

            SetView(_view.Model.Title, variant.Text, _view.Model.Option2);
        }

        private void Option2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OptionVariant variant = e.CurrentSelection[0] as OptionVariant;

            SetView(_view.Model.Title, _view.Model.Option1, variant.Text);
        }

        private async void SetView(string title, string option1, string option2)
        {
            Product product = await ProductAdapter.Get(title, option1, option2);

            if (product == null)
            {
                if (_view == null)
                {
                    throw new YsmStoreException("Uploaded by title options product and ProductView is null at the same time");
                }

                _view.Model.Option1 = option1;
                _view.Model.Option2 = option2;
                await _view.LoadOptions();
                _view.Model.Quantity = 0;
            }
            else
            {
                _view = new ProductView(product);
                await _view.LoadOptions();
                this.BindingContext = _view;
            }
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

        private async void CartButton_Tapped(object sender, EventArgs e)
        {
            if (_view.CanOrder == false)
            {
                await DisplayAlert("Ошибка", "Сначала выберите доступные опции для товара", "ОК");
                return;
            }

            try
            {
                await UserAdapter.AddToCart(new ProductAmount(_view.Model.Id, 1));
                await DisplayAlert("Сообщение", "Товар добавлен в корзину", "ОК");
            }
            catch (YsmStoreException ex)
            {
                await DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }
    }
}