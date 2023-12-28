using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class ProductAmountView : YsmStoreView<ProductAmount>
    {
        public ProductAmountView(ProductAmount productAmount) : base(productAmount)
        {
            Product = ProductAdapter.Get(productAmount.ProductId);

            AddOne = new Command(() => { Model.Amount++; RefreshCanExecute(); }, () => Model.Amount < Product.Quantity);
            RemoveOne = new Command(() => { Model.Amount--; RefreshCanExecute(); }, () => Model.Amount > 1);

            Model.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Model.Amount))
                    InvokePropertyChanged(nameof(IsAmountMoreThanQuantity));
            };
        }

        public Product Product { get; private set; }

        public string Info { get => Product.Title + (Product.Option1 != null ? $", {Product.Option1}" : "") + (Product.Option2 != null ? $", {Product.Option2}" : ""); }

        public Command AddOne { get; private set; }

        public Command RemoveOne { get; private set; }

        private void RefreshCanExecute()
        {
            AddOne.ChangeCanExecute();
            RemoveOne.ChangeCanExecute();
        }

        public bool IsAmountMoreThanQuantity { get => Model.Amount > Product.Quantity; }

    }
}
