namespace YsmStore.Models
{
    public class RecoveryData : YsmStoreModel
    {
        private string _login;
        public string Login
        {
            get => _login;
            set { _login = value; InvokePropertyChanged(nameof(Login)); }
        }

        private string _recoveryPassword;
        public string RecoveryPassword
        {
            get => _recoveryPassword;
            set { _recoveryPassword = value; InvokePropertyChanged(nameof(RecoveryPassword)); }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; InvokePropertyChanged(nameof(NewPassword)); }
        }

        private string _newPasswordRepeat;
        public string NewPasswordRepeat
        {
            get => _newPasswordRepeat;
            set { _newPasswordRepeat = value; InvokePropertyChanged(nameof(NewPasswordRepeat)); }
        }

        public RecoveryData() { }

        public RecoveryData(string login)
        {
            Login = login;
        }
    }
}
