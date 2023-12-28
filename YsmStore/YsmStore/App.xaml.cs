using Xamarin.Forms;
using YsmStore.Data;
using YsmStore.Models;
using YsmStore.Pages;

namespace YsmStore
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            TryLogin();
        }

        private void TryLogin()
        {
            try
            {
                AuthSystem.Login(AuthSystem.SavedData);
            }
            catch (YsmStoreException)
            {
                MainPage = new NavigationPage(new AuthPage());
            }
        }

        public void SetStartPageForLoginedUser()
        {
            Page page = null;

            if (AuthSystem.LoginedUser == null)
                page = new NavigationPage(new AuthPage());
            if (AuthSystem.LoginedUser is Customer)
                page = new NavigationPage(new CustomerMenuPage());
            if (AuthSystem.LoginedUser is Admin)
                page = new NavigationPage(new AdminMenuPage());

            if (page == null)
                throw new YsmStoreException(string.Empty);

            MainPage = page;
        }
    }
}
