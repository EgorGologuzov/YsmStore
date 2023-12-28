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

        private void SendButton_Tapped(object sender, EventArgs e)
        {
            if (_lastRecoveryTime == null || (DateTime.Now - _lastRecoveryTime).Value.TotalMinutes >= 1)
            {
                if (_lastRecoveryTime != null)
                {
                    DisplayAlert("Сообщение", "Письмо отправлено повторно", "ОК");
                }

                _lastRecoveryTime = DateTime.Now;
                AuthSystem.SendRecoveryRequest(_data.Login);
                dataStackLayout.IsVisible = true;
            }
            else
            {
                DisplayAlert("Ошибка", "Следующее письмо можно отправить через минуту", "ОК");
            }
        }

        private void RecoveryButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                AuthSystem.Login(_data);
            }
            catch (YsmStoreException ex)
            {
                DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
                return;
            }
        }
    }
}