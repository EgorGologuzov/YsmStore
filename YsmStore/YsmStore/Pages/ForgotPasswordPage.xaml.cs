using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPasswordPage : ContentPage
    {
        private RecoveryData _data;
        private DateTime? _lastRecoveryTime = null;

        public ForgotPasswordPage(AuthData data)
        {
            InitializeComponent();

            _data = new RecoveryData(data.Login);
            this.BindingContext = _data;
        }

        private async void SendButton_Tapped(object sender, EventArgs e)
        {
            if (_lastRecoveryTime == null || (DateTime.Now - _lastRecoveryTime).Value.TotalMinutes >= 1)
            {
                if (_lastRecoveryTime != null)
                {
                    await DisplayAlert("Сообщение", "Письмо отправлено повторно", "ОК");
                }

                try
                {
                    await AuthSystem.SendRecoveryRequest(_data.Login);
                    dataStackLayout.IsVisible = true;
                    _lastRecoveryTime = DateTime.Now;
                }
                catch (YsmStoreException ex)
                {
                    await DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
                }
            }
            else
            {
                await DisplayAlert("Ошибка", "Следующее письмо можно отправить через минуту", "ОК");
            }
        }

        private async void RecoveryButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                await AuthSystem.Login(_data);
            }
            catch (YsmStoreException ex)
            {
                await DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }
    }
}