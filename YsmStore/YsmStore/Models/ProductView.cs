using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class ProductView : YsmStoreView<Product>
    {
        public ProductView(Product product) : base(product)
        {
            Option1 = ProductAdapter.GetOption1(product);
            Option2 = ProductAdapter.GetOption2(product);

            Model.PropertyChanged += OnProductPropertyChanged;
        }

        public Option Option1 { get; private set; }
        public Option Option2 { get; private set; }

        public bool CanOrder
        {
            get
            {
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

        private ObservableCollection<KeyValuePair<string, string>> _properties;
        public ObservableCollection<KeyValuePair<string, string>> Properties
        {
            get
            {
                if (_properties == null)
                {
                    _properties = new ObservableCollection<KeyValuePair<string, string>>(ProductAdapter.GetProperties(Model));
                }

                return _properties;
            }
        }

        public string Info { get => Model.Title + (Model.Option1 != null ? $", {Model.Option1}" : "") + (Model.Option2 != null ? $", {Model.Option2}" : ""); }

        private void OnProductPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.Title) || e.PropertyName == nameof(Model.Option1) || e.PropertyName == nameof(Model.Option2))
            {
                InvokePropertyChanged(nameof(Info));
            }
        }
    }
}
