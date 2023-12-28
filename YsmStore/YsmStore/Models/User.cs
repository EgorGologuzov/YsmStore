namespace YsmStore.Models
{
    public class User : YsmStoreModel
    {
        public string Token { get; }

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

        protected User(string token)
        {
            Token = token;
        }
    }
}
