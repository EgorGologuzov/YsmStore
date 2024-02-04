using System;

namespace YsmStore.Models
{
    public class Customer : User
    {
        private Guid _id;
        private string _fullName;
        private string _userName;

        public Guid Id
        {
            get => _id;
            set { _id = value; InvokePropertyChanged(nameof(Id)); }
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; InvokePropertyChanged(nameof(FullName)); }
        }

        public string UserName
        {
            get => _userName;
            set { _userName = value; InvokePropertyChanged(nameof(UserName)); }
        }
    }
}
