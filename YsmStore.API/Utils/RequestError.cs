namespace YsmStore.API.Utils
{
    public static class RequestError
    {
        public record ErrorData(string Code, string Message);

        public static readonly ErrorData TokenDoesNotContainId = new("ERROR-1", "Id claim was expected in token but not found");
        public static readonly ErrorData EntityNotFound = new("ERROR-2", "Entity with that id not found");
        public static readonly ErrorData InvalidData = new("ERROR-3", "Mistake in data. May be some field skiped or has wrong data format or some id not exists");
        public static readonly ErrorData IncorrestLoginOrPassword = new("ERROR-4", "Incorrect login or password");
        public static readonly ErrorData LoginNotAvailable = new("ERROR-5", "User with that login already exists");
        public static readonly ErrorData LoginNotFound = new("ERROR-6", "User with that login not found");
        public static readonly ErrorData AmountIsNotPositive = new("ERROR-7", "Amount must be more than 0");
        public static readonly ErrorData OptionNumberOutOfRange = new("ERROR-8", "Option number must be 1 or 2");
    }
}
