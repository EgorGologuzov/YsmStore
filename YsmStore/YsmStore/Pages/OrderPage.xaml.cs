using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderPage : ContentPage
    {
        private OrderView _view;

        public OrderPage(Customer customer, IList<ProductAmount> products)
        {
            InitializeComponent();

            _view = new OrderView(products, customer);
            this.BindingContext = _view;
        }

        private async void OrderButton_Tapped(object sender, EventArgs e)
        {
            if (_view.Products.Count == 0)
            {
                await DisplayAlert("Ошибка", "В заказе нет ни одного товара", "ОК");
                return;
            }
            if (_view.AreSomeProductsNotEnough)
            {
                await DisplayAlert("Ошибка", "Некоторых товаров недостаточно на складе. Уменьшите их количестов или удалите их, чтобы сделать заказ.", "ОК");
                return;
            }
            if (_view.Model.City == null)
            {
                await DisplayAlert("Ошибка", "Не заполнен город", "ОК");
                return;
            }
            if (_view.Model.PickUpAdress == null)
            {
                await DisplayAlert("Ошибка", "Не заполнен адрес отделения СДЕК", "ОК");
                return;
            }
            if (Regex.IsMatch(_view.Model.PhoneNumber ?? string.Empty, @"\+7 \(\d\d\d\) \d\d\d-\d\d-\d\d") == false)
            {
                await DisplayAlert("Ошибка", "Неверно введен телефон", "ОК");
                return;
            }

            try
            {
                OrderAdapter.Push(_view.Model, _view.Products.Models);
                await DisplayAlert("Сообщение", "Заказа оформлен, вы можете увидеть его в разделе \"Купленные товары\"", "ОК");
                await Navigation.PopAsync();
            }
            catch (YsmStoreException ex)
            {
                await DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }
    }
}