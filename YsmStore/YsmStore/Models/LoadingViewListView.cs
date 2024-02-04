using System;
using Xamarin.Forms;

namespace YsmStore.Models
{
    public class LoadingViewListView<TModel, TView, TQuery> : YsmStoreModel
        where TModel : YsmStoreModel
        where TView : YsmStoreView<TModel>
        where TQuery : Query, new()
    {
        private ViewList<TModel, TView> _items;
        private TQuery _query;
        private Func<TQuery, int> _method;
        private Action<TQuery> _methodAsync;
        private Command _loadNext;
        public ViewList<TModel, TView> Items { get => _items; }
        public TQuery Query { get => _query; }
        public Func<TQuery, int> Method { get => _method; }
        public Action<TQuery> MethodAsync { get => _methodAsync; }
        public Command LoadNext
        {
            get => _loadNext;
            set { _loadNext = value; InvokePropertyChanged(nameof(LoadNext)); }
        }
        public LoadingViewListView(Func<TQuery, int> loadMethod)
        {
            Initialize();
            _method = loadMethod;
        }

        public LoadingViewListView(Action<TQuery> loadMethod)
        {
            Initialize();
            _methodAsync = loadMethod;
        }

        private void Initialize()
        {
            _items = new ViewList<TModel, TView>();
            _query = new TQuery();
            _query.TargetList = _items.Models;
            _loadNext = new Command(LoadNext_Execute, LoadNext_CanExecute);
        }
        private void LoadNext_Execute()
        {
            if (Query.IsEndReached)
            {
                return;
            }

            if (Method != null)
            {
                Method.Invoke(Query);
            }

            if (MethodAsync != null)
            {
                MethodAsync.Invoke(Query);
            }
        }

        private bool LoadNext_CanExecute()
        {
            if (Items == null || Query == null)
                throw new YsmStoreException("Items or Query in LoadingViewListView is null");
            return true;
        }
    }
}
