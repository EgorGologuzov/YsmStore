using System.Threading.Tasks;
using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class ProductAmountView : YsmStoreView<ProductAmount>
    {
        private Product _product;
        public Product Product
        {
            get => _product;
            set { _product = value; InvokePropertyChanged(nameof(Product)); }
        }

        public string Info { get => Product.Title + (!string.IsNullOrEmpty(Product.Option1) ? $", {Product.Option1}" : "") + (!string.IsNullOrEmpty(Product.Option2) ? $", {Product.Option2}" : ""); }
        public bool IsAmountMoreThanQuantity { get => Model.Amount > Product.Quantity; }

        public Command AddOne { get; private set; }
        public Command RemoveOne { get; private set; }

        public ProductAmountView(ProductAmount productAmount) : base(productAmount)
        {
            Product = new Product(0);

            AddOne = new Command(() => { Model.Amount++; RefreshCanExecute(); }, () => Model.Amount < Product.Quantity);
            RemoveOne = new Command(() => { Model.Amount--; RefreshCanExecute(); }, () => Model.Amount > 1);

            Model.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Model.Amount))
                    InvokePropertyChanged(nameof(IsAmountMoreThanQuantity));
            };

            this.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Product))
                {
                    InvokePropertyChanged(nameof(Info));
                    InvokePropertyChanged(nameof(IsAmountMoreThanQuantity));
                    RefreshCanExecute();
                }
            };
        }

        public async Task LoadProduct()
        {
            Product = await ProductAdapter.Get(Model.ProductId);
            InvokePropertyChanged(nameof(Product));
        }

        private void RefreshCanExecute()
        {
            AddOne.ChangeCanExecute();
            RemoveOne.ChangeCanExecute();
        }
    }
}
