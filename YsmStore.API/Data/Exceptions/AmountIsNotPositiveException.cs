namespace YsmStore.API.Data.Exceptions
{
    public class AmountIsNotPositiveException : Exception
    {
        public AmountIsNotPositiveException() : base()
        {
        }

        public AmountIsNotPositiveException(string? message) : base(message)
        {
        }

        public AmountIsNotPositiveException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
