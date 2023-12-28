namespace YsmStore.Models
{
    public class CustomersByEmailQuery : Query
    {
        private string _queryText;
        public string EmailText
        {
            get => _queryText;
            set { _queryText = value; InvokePropertyChanged(nameof(EmailText)); }
        }
    }
}
