namespace YsmStore.API.Data.Interfaces
{
    public interface IEmailService
    {
        void SendRecoveryEmail(string login, string recoveryPassword);
    }
}
