using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace YsmStore.Models
{
    public class ViewList<TModel, TView> : ObservableCollection<TView>
        where TModel : YsmStoreModel
        where TView : YsmStoreView<TModel>
    {
        public ViewList()
        {
            Models = new ObservableCollection<TModel>();
            Subscribe_ViewsCollectionChanged(true);
        }

        private ObservableCollection<TModel> _models;
        public ObservableCollection<TModel> Models
        {
            get => _models;
            set
            {
                if (_models != null)
                    Subscribe_ModelsCollectionChanged(false);

                _models = value;
                this.ResetViews();

                if (_models != null)
                {
                    Subscribe_ModelsCollectionChanged(true);
                    this.AddViews(_models);
                }
            }
        }

        private bool IsSubscribed_OnModelsCollectionChanged = false;
        private void Subscribe_ModelsCollectionChanged(bool enabled)
        {
            if (enabled == false)
                _models.CollectionChanged -= OnModelsCollectionChanged;
            else if (IsSubscribed_OnModelsCollectionChanged == false)
                _models.CollectionChanged += OnModelsCollectionChanged;

            IsSubscribed_OnModelsCollectionChanged = enabled;
        }

        private bool IsSubscribed_OnViewsCollectionChanged = false;
        private void Subscribe_ViewsCollectionChanged(bool enabled)
        {
            if (enabled == false)
                this.CollectionChanged -= OnViewsCollectionChanged;
            else if (IsSubscribed_OnViewsCollectionChanged == false)
                this.CollectionChanged += OnViewsCollectionChanged;

            IsSubscribed_OnViewsCollectionChanged = enabled;
        }

        private void OnModelsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: AddViews(e.NewItems); break;
                case NotifyCollectionChangedAction.Remove: RemoveViews(e.OldItems); break;
                case NotifyCollectionChangedAction.Reset: ResetViews(); break;
            }
        }

        private void OnViewsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add: AddModels(e.NewItems); break;
                case NotifyCollectionChangedAction.Remove: RemoveModels(e.OldItems); break;
                case NotifyCollectionChangedAction.Reset: ResetModels(); break;
            }
        }

        private void AddViews(IList models)
        {
            if (models == null) return;

            Subscribe_ViewsCollectionChanged(false);

            foreach (TModel model in models)
            {
                TView view = (TView)Activator.CreateInstance(typeof(TView), model);
                this.Add(view);
            }

            Subscribe_ViewsCollectionChanged(true);
        }

        private void RemoveViews(IList models)
        {
            if (models == null) return;

            Subscribe_ViewsCollectionChanged(false);

            foreach (TModel model in models)
            {
                TView view = this.Where(v => v.Model == model).First();
                this.Remove(view);
            }

            Subscribe_ViewsCollectionChanged(true);
        }

        private void AddModels(IList views)
        {
            if (views == null) return;

            Subscribe_ModelsCollectionChanged(false);

            foreach (TView view in views)
            {
                Models.Add(view.Model);
            }

            Subscribe_ModelsCollectionChanged(true);
        }

        private void RemoveModels(IList views)
        {
            if (views == null) return;

            Subscribe_ModelsCollectionChanged(false);

            foreach (TView view in views)
            {
                Models.Remove(view.Model);
            }

            Subscribe_ModelsCollectionChanged(true);
        }

        private void ResetModels()
        {
            Subscribe_ModelsCollectionChanged(false);
            _models.Clear();
            Subscribe_ModelsCollectionChanged(true);
        }

        private void ResetViews()
        {
            Subscribe_ViewsCollectionChanged(false);
            this.Clear();
            Subscribe_ViewsCollectionChanged(true);
        }
    }
}
