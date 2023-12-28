namespace YsmStore.Models
{
    public class AuthData : YsmStoreModel
    {
        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; InvokePropertyChanged(nameof(Login)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; InvokePropertyChanged(nameof(Password)); }
        }

        private bool _rememberMe;
        public bool RememberMe
        {
            get => _rememberMe;
            set { _rememberMe = value; InvokePropertyChanged(nameof(RememberMe)); }
        }
    }
}
