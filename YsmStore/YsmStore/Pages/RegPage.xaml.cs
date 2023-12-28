using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YsmStore.Data;
using YsmStore.Models;

namespace YsmStore.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegPage : ContentPage
    {
        private RegData _data;

        public RegPage()
        {
            InitializeComponent();

            _data = new RegData();
            this.BindingContext = _data;
        }

        private void RegButton_Tapped(object sender, EventArgs e)
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