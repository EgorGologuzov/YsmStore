using System;
using System.Collections.Generic;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminProductPage : ContentPage
    {
        private ProductView _view;

        public AdminProductPage(Product product)
        {
            InitializeComponent();

            _view = new ProductView(product);
            this.BindingContext = _view;
        }

        private async void AddPropertyButton_Tapped(object sender, EventArgs e)
        {
            string name = await DisplayPromptAsync("Вопрос", "Введите название характеристики");
            if (string.IsNullOrEmpty(name))
            {
                await DisplayAlert("Ошибка", "Имя характеристики не может быть пустым", "ОК");
                return;
            }

            string value = await DisplayPromptAsync("Вопрос", "Введите значение характеристики");
            if (string.IsNullOrEmpty(value))
            {
                await DisplayAlert("Ошибка", "Значание характеристики не может быть пустым", "ОК");
                return;
            }

            _view.Properties.Add(new KeyValuePair<string, string>(name, value));
            propertyView.ItemsSource = _view.Properties;
        }

        private void RemovePropertyButton_Tapped(object sender, EventArgs e)
        {
            if (propertyView.SelectedItem == null)
            {
                DisplayAlert("Ошибка", "Сначала выберите характеристику для удаления", "ОК");
                return;
            }

            _view.Properties.Remove((KeyValuePair<string, string>)propertyView.SelectedItem);
        }

        private void SaveButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                ProductAdapter.Push(_view.Model, _view.Properties);
                Navigation.PopAsync();
            }
            catch (YsmStoreException ex)
            {
                DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }

        private async void ChooseImageButton_Tapped(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Stream stream = await DependencyService.Get<IPhotoPickerService>().GetImageStreamAsync();
            if (stream == null)
            {
                (sender as Button).IsEnabled = true;
                return;
            }

            ImageSource src = ImageSource.FromStream(() => stream);
            string url = PhotoAdapter.Upload(src);

            if (string.IsNullOrEmpty(url))
            {
                avatarImage.Source = src;
            }
            else
            {
                _view.Model.Avatar = url;
            }

            (sender as Button).IsEnabled = true;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ProductAdapter.Pull(_view.Model);
        }
    }
}