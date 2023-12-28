namespace YsmStore.Models
{
    public class ProductAmount : YsmStoreModel
    {
        public int ProductId { get; set; }

        private int _amount;
        public int Amount
        {
            get => _amount;
            set
            {
                if (value < 0)
                    throw new YsmStoreException("Amount must be >= 0");
                _amount = value;
                InvokePropertyChanged(nameof(Amount));
            }
        }

        public ProductAmount(int productId, int amount)
        {
            ProductId = productId;
            Amount = amount;
        }
    }
}
