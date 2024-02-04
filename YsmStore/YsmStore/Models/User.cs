namespace YsmStore.Models
{
    public class User : YsmStoreModel
    {
        private string _login;
        private string _password;
        public string Token { get; set; }
        public string Login
        {
            get => _login;
            set { _login = value; InvokePropertyChanged(nameof(Login)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; InvokePropertyChanged(nameof(Password)); }
        }
    }
}
