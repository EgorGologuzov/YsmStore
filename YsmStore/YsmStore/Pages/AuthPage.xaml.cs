using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AuthPage : ContentPage
    {
        private AuthData _data;

        public AuthPage()
        {
            InitializeComponent();

            _data = AuthSystem.SavedData ?? new AuthData();
            this.BindingContext = _data;
        }

        private void CreateAccountLabel_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegPage());
        }

        private void ForgotPassword_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ForgotPasswordPage(_data));
        }

        private void LoginButton_Tapped(object sender, EventArgs e)
        {
            try
            {
                AuthSystem.Login(_data);
            }
            catch (YsmStoreException ex)
            {
                DisplayAlert(ex.Caption, ex.Message, ex.OkButtonText);
            }
        }
    }
}