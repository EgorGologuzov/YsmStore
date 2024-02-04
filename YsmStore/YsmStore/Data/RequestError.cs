using System;
using System.Collections.Generic;
using System.Text;

namespace YsmStore.Data
{
    public static class RequestError
    {
        public const string TokenDoesNotContainId = "ERROR-1";
        public const string EntityNotFound = "ERROR-2";
        public const string InvalidData = "ERROR-3";
        public const string IncorrestLoginOrPassword = "ERROR-4";
        public const string LoginNotAvailable = "ERROR-5";
        public const string LoginNotFound = "ERROR-6";
        public const string AmountIsNotPositive = "ERROR-7";
        public const string OptionNumberOutOfRange = "ERROR-8";
        public const string CdekApiBadRequest = "ERROR-9";
        public const string NoProductsInOrder = "ERROR-10";
        public const string NotEnoughtProduct = "ERROR-11";
    }
}
