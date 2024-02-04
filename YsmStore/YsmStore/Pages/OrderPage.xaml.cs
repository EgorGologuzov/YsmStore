using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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

            SetView(customer, products);
        }

        private async void SetView(Customer customer, IList<ProductAmount> products)
        {
            _view = new OrderView(products, customer);

            var tasks = _view.Products.Select(p => p.LoadProduct());
            await Task.WhenAll(tasks);
            _view.LoadCities();

            _view.PropertyChanged += AlertIfHasNoAdressesInCity;

            this.BindingContext = _view;
        }

        private void AlertIfHasNoAdressesInCity(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(_view.Adresses))
            {
                return;
            }

            if (_view.Adresses == null || _view.Adresses.Length == 0)
            {
                DisplayAlert("Сообщение", "Похоже, в вашем городе нет отделений СДЕК (((", "ОК");
            }
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
                await OrderAdapter.Push(_view.Model, _view.Products.Models);
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