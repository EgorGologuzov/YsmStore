using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms.Internals;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class OrderView : YsmStoreView<Order>
    {
        public OrderView(Order order) : base(order)
        {
            Initialize();
            Products.Models = new ObservableCollection<ProductAmount>(ProductAdapter.GetAllProducts(order));
        }

        public OrderView(IList<ProductAmount> products, Customer customer) : base(OrderAdapter.Create(customer))
        {
            Initialize();
            Products.Models = new ObservableCollection<ProductAmount>(products);
        }

        private void Initialize()
        {
            Model.PropertyChanged += OnModelPropertyChanged;

            Products = new ViewList<ProductAmount, ProductAmountView>();
            Products.CollectionChanged += OnProductCollectionChanged;
        }

        public ViewList<ProductAmount, ProductAmountView> Products { get; private set; }

        public string[] Cities { get => OrderAdapter.GetCities(); }

        public string[] Adresses { get => OrderAdapter.GetAdresses(Model.City); }

        public string[] OrderStatusVariants { get => OrderStatusToStringConverter.StatusStrings; }

        public decimal Total
        {
            get
            {
                decimal total = 0;
                Products.ForEach(view => total += view.Product.Price * view.Model.Amount);
                return total;
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.City))
                InvokePropertyChanged(nameof(Adresses));
        }

        private void OnProductCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnProductsCollectionCahnged();

            if (e.NewItems == null)
                return;

            foreach (ProductAmountView view in e.NewItems)
            {
                view.Model.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(view.Model.Amount))
                        OnProductsCollectionCahnged();
                };
            }
        }

        public bool AreSomeProductsNotEnough { get => this.Products.Where(v => v.Product.Quantity < v.Model.Amount).Count() != 0; }

        private void OnProductsCollectionCahnged()
        {
            InvokePropertyChanged(nameof(Total));
            InvokePropertyChanged(nameof(AreSomeProductsNotEnough));
        }
    }
}
