namespace YsmStore.Models
{
    public class Product : YsmStoreModel
    {
        public Product(int id)
        {
            Id = id;
        }

        public int Id { get; private set; }

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; InvokePropertyChanged(nameof(Title)); }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set { _category = value; InvokePropertyChanged(nameof(Category)); }
        }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set { _price = value; InvokePropertyChanged(nameof(Price)); }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set { _description = value; InvokePropertyChanged(nameof(Description)); }
        }

        private string _avatar;
        public string Avatar
        {
            get => _avatar;
            set { _avatar = value; InvokePropertyChanged(nameof(Avatar)); }
        }

        private string _option1;
        public string Option1
        {
            get => _option1;
            set { _option1 = value; InvokePropertyChanged(nameof(Option1)); }
        }

        private string _option2;
        public string Option2
        {
            get => _option2;
            set { _option2 = value; InvokePropertyChanged(nameof(Option2)); }
        }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; InvokePropertyChanged(nameof(Quantity)); }
        }
    }
}
