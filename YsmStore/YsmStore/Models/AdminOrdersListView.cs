using System;
using System.Collections.Specialized;
using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class AdminOrdersListView : LoadingViewListView<Order, OrderView, AdminOrdersQuery>
    {
        public string[] StatusFilterVariants { get => StatusFilterToStringConverter.FilterVariants; }
        public Command Search { get; set; }

        public AdminOrdersListView() : base(OrderAdapter.Execute)
        {
            this.Query.StartDate = DateTime.Now - TimeSpan.FromDays(300);
            this.Query.EndDate = DateTime.Now + TimeSpan.FromDays(1);
            Search = new Command(ExecuteQuery, () => true);
            Items.CollectionChanged += OnItemsCollectionChanged;
            LoadNext.Execute(null);
        }

        public async void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
            {
                return;
            }

            foreach (var item in e.NewItems)
            {
                OrderView view = (OrderView)item;
                view.LoadProducts();
            }
        }

        private void ExecuteQuery()
        {
            this.Items.Clear();
            this.Query.IsEndReached = false;
            this.LoadNext.Execute(null);
        }
    }
}
