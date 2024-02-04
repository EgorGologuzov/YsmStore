namespace YsmStore.Models
{
    public class CustomersByEmailQuery : Query
    {
        private string _emailText;
        public string EmailText
        {
            get => _emailText;
            set { _emailText = value; InvokePropertyChanged(nameof(EmailText)); }
        }
    }
}
