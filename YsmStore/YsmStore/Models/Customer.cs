namespace YsmStore.Models
{
    public class Customer : User
    {
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

        public Customer(string token) : base(token) { }
    }
}
