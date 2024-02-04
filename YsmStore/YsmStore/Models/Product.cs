namespace YsmStore.Models
{
    public class Product : YsmStoreModel
    {
        private string _title;
        private string _category;
        private double _price;
        private string _description;
        private string _avatar;
        private string _option1;
        private string _option2;
        private int _quantity;

        public int Id { get; private set; }

        public string Title
        {
            get => _title;
            set { _title = value; InvokePropertyChanged(nameof(Title)); }
        }

        public string Category
        {
            get => _category;
            set { _category = value; InvokePropertyChanged(nameof(Category)); }
        }

        public double Price
        {
            get => _price;
            set { _price = value; InvokePropertyChanged(nameof(Price)); }
        }

        public string Description
        {
            get => _description;
            set { _description = value; InvokePropertyChanged(nameof(Description)); }
        }

        public string Avatar
        {
            get => _avatar;
            set { _avatar = value; InvokePropertyChanged(nameof(Avatar)); }
        }

        public string Option1
        {
            get => _option1;
            set { _option1 = value; InvokePropertyChanged(nameof(Option1)); }
        }

        public string Option2
        {
            get => _option2;
            set { _option2 = value; InvokePropertyChanged(nameof(Option2)); }
        }

        public int Quantity
        {
            get => _quantity;
            set { _quantity = value; InvokePropertyChanged(nameof(Quantity)); }
        }

        public Product(int id)
        {
            Id = id;
        }
    }
}
