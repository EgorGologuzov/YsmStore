using System;

namespace YsmStore.Models
{
    public class YsmStoreException : Exception
    {
        public string Caption = "Ошибка";
        public string OkButtonText = "ОК";

        public YsmStoreException(string message) : base(message) { }

        public YsmStoreException() : base()
        {
        }

        public YsmStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
