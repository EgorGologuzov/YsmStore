using System;
using Xamarin.Forms;
using YsmStore.Data;

namespace YsmStore.Models
{
    public class AdminOrdersListView : LoadingViewListView<Order, OrderView, AdminOrdersQuery>
    {
        public AdminOrdersListView() : base(OrderAdapter.Execute)
        {
            this.Query.StartDate = DateTime.Now;
            this.Query.EndDate = DateTime.Now;
            Search = new Command(ExecuteQuery, () => true);
            LoadNext.Execute(null);
        }

        public string[] StatusFilterVariants { get => StatusFilterToStringConverter.FilterVariants; }

        public Command Search { get; private set; }

        private void ExecuteQuery()
        {
            this.Items.Clear();
            this.Query.IsEndReached = false;
            this.LoadNext.Execute(null);
        }
    }
}
