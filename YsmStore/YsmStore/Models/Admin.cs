namespace YsmStore.Models
{
    public class Admin : User
    {
        private int _sessionCode;
        public int SessionCode
        {
            get => _sessionCode;
            set { _sessionCode = value; InvokePropertyChanged(nameof(SessionCode)); }
        }

        public Admin(string token) : base(token) { }
    }
}
