namespace YsmStore.Models
{
    public abstract class YsmStoreView<TModel> : YsmStoreModel where TModel : YsmStoreModel
    {
        public TModel Model { get; }

        public YsmStoreView(TModel model)
        {
            if (model == null)
                throw new YsmStoreException("Model in view cant be null");

            Model = model;
        }
    }
}
