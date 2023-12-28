namespace YsmStore.Models
{
    public class ProductQuery : Query
    {
        private string _filter;
        public string Filter
        {
            get => _filter;
            set { _filter = value; InvokePropertyChanged(nameof(Filter)); }
        }

        private string _sortVariant;
        public string SortVariant
        {
            get => _sortVariant;
            set { _sortVariant = value; InvokePropertyChanged(nameof(SortVariant)); }
        }
    }
}
