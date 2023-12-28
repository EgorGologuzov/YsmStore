using System;
using Xamarin.Forms;

namespace YsmStore.Models
{
    public class LoadingViewListView<TModel, TView, TQuery> : YsmStoreModel
        where TModel : YsmStoreModel
        where TView : YsmStoreView<TModel>
        where TQuery : Query, new()
    {
        public LoadingViewListView(Func<TQuery, int> loadMethod)
        {
            _items = new ViewList<TModel, TView>();
            _query = new TQuery();
            _query.TargetList = _items.Models;
            _method = loadMethod;
            _loadNext = new Command(LoadNext_Execute, LoadNext_CanExecute);
        }

        private ViewList<TModel, TView> _items;
        public ViewList<TModel, TView> Items
        {
            get => _items;
        }

        private TQuery _query;
        public TQuery Query
        {
            get => _query;
        }

        private Func<TQuery, int> _method;
        public Func<TQuery, int> Method
        {
            get => _method;
        }

        private Command _loadNext;
        public Command LoadNext
        {
            get => _loadNext;
            set { _loadNext = value; InvokePropertyChanged(nameof(LoadNext)); }
        }

        private void LoadNext_Execute()
        {
            if (Query.IsEndReached == false)
            {
                Method.Invoke(Query);
            }
        }

        private bool LoadNext_CanExecute()
        {
            if (Items == null || Query == null)
                return false;
            return true;
        }
    }
}
