namespace YsmStore.Models
{
    public class RegData : YsmStoreModel
    {
        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; InvokePropertyChanged(nameof(Login)); }
        }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; InvokePropertyChanged(nameof(FullName)); }
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set { _userName = value; InvokePropertyChanged(nameof(UserName)); }
        }

        private string _password;
        public string Password
        {
            get => _password;
            set { _password = value; InvokePropertyChanged(nameof(Password)); }
        }

        private string _passwordRepeat;
        public string PasswordRepeat
        {
            get => _passwordRepeat;
            set { _passwordRepeat = value; InvokePropertyChanged(nameof(PasswordRepeat)); }
        }
    }
}
