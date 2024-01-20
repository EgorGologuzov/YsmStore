using System.Runtime.Serialization;

namespace YsmStore.API.Data.Exceptions
{
    [Serializable]
    internal class IncorrectLoginOrPasswordException : Exception
    {
        public IncorrectLoginOrPasswordException()
        {
        }

        public IncorrectLoginOrPasswordException(string? message) : base(message)
        {
        }

        public IncorrectLoginOrPasswordException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected IncorrectLoginOrPasswordException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}