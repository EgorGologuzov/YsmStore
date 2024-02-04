using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class ProductView : YsmStoreView<Product>
    {
        public Option Option1 { get; private set; }
        public Option Option2 { get; private set; }
        public ObservableCollection<KeyValuePair<string, string>> Properties { get; set; }

        public bool CanOrder
        {
            get
            {
                if (Model.Quantity == 0)
                {
                    return false;
                }

                if (Option1.IsSet)
                {
                    if (Option1.Variants.Where(v => v.IsNotAvailable == false && v.IsSelected == true).Count() == 1)
                    {
                        if (Option2.IsSet)
                        {
                            return Option2.Variants.Where(v => v.IsNotAvailable == false && v.IsSelected == true).Count() == 1;
                        }
                        return true;
                    }
                    return false;
                }
                return true;
            }
        }

        public string Info { get => Model.Title + (!string.IsNullOrEmpty(Model.Option1) ? $", {Model.Option1}" : "") + (!string.IsNullOrEmpty(Model.Option2) ? $", {Model.Option2}" : ""); }

        public string PriceString
        {
            get => Model.Price.ToString();
            set
            {
                double price;
                if (double.TryParse(value, out price))
                {
                    Model.Price = price;
                }
            }
        }

        public ProductView(Product product) : base(product)
        {
            Model.PropertyChanged += OnProductPropertyChanged;
        }

        public async Task LoadOptions()
        {
            Option1 = await ProductAdapter.GetOption1(Model);
            Option2 = await ProductAdapter.GetOption2(Model);
            Properties = new ObservableCollection<KeyValuePair<string, string>>(await ProductAdapter.GetProperties(Model));
            Model.Description = (await ProductAdapter.Get(Model.Id)).Description;
            InvokePropertyChanged(nameof(Option1));
            InvokePropertyChanged(nameof(Option2));
            InvokePropertyChanged(nameof(Properties));
            InvokePropertyChanged(nameof(Model));
        }

        private void OnProductPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.Title) || e.PropertyName == nameof(Model.Option1) || e.PropertyName == nameof(Model.Option2))
            {
                InvokePropertyChanged(nameof(Info));
            }

            if (e.PropertyName == nameof(Model.Quantity))
            {
                InvokePropertyChanged(nameof(CanOrder));
            }
        }
    }
}
