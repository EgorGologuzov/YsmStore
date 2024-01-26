namespace YsmStore.API.Data.Exceptions
{
    public class NotEnoughtProductException : Exception
    {
        public readonly int ProductId;

        public NotEnoughtProductException(int productId)
        {
            ProductId = productId;
        }
    }
}
