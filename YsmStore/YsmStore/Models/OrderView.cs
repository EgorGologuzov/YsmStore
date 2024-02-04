using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms.Internals;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class OrderView : YsmStoreView<Order>
    {
        private string[] _cities;
        private string[] _adresses;
        private ViewList<ProductAmount, ProductAmountView> _products;

        public string[] Cities
        {
            get => _cities;
            set { _cities = value; InvokePropertyChanged(nameof(Cities)); }
        }

        public string[] Adresses
        {
            get => _adresses;
            set { _adresses = value; InvokePropertyChanged(nameof(Adresses)); }
        }

        public ViewList<ProductAmount, ProductAmountView> Products
        {
            get => _products;
            set { _products = value; InvokePropertyChanged(nameof(Products)); }
        }

        public string[] OrderStatusVariants { get => OrderStatusToStringConverter.StatusStrings; }

        public double Total
        {
            get
            {
                double total = 0;
                Products.ForEach(view => total += view.Product.Price * view.Model.Amount);
                return total;
            }
        }

        public bool AreSomeProductsNotEnough { get => this.Products.Where(v => v.Product.Quantity < v.Model.Amount).Count() != 0; }
        
        public OrderView(Order order) : base(order)
        {
            Initialize();
        }

        public OrderView(IList<ProductAmount> products, Customer customer) : base(OrderAdapter.Create(customer))
        {
            Initialize();
            Products.Models = new ObservableCollection<ProductAmount>(products);
        }

        public async Task LoadProducts()
        {
            var products = await ProductAdapter.GetAllProducts(Model);
            Products.Models = new ObservableCollection<ProductAmount>(products);
            var tasks = Products.Select(v => v.LoadProduct());
            await Task.WhenAll(tasks);
        }

        public async void LoadCities()
        {
            Cities = await OrderAdapter.GetCities();
        }

        private void Initialize()
        {
            Model.PropertyChanged += OnModelPropertyChanged;
            Products = new ViewList<ProductAmount, ProductAmountView>();
            Products.CollectionChanged += OnProductCollectionChanged;
        }

        private async void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Model.City))
            {
                Adresses = await OrderAdapter.GetAdresses(Model.City);
            }
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

        private void OnProductsCollectionCahnged()
        {
            InvokePropertyChanged(nameof(Total));
            InvokePropertyChanged(nameof(AreSomeProductsNotEnough));
        }
    }
}
