using YsmStore.API.Models;

namespace YsmStore.API.Data.Interfaces
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<Customer> GetByLoginAndPassword(string login, string password);
        Task<Customer> UpdatePassword(Guid id, string oldPassword, string newPassword);
        Task<Customer> ResetPassword(Guid id, string newPassword);
        Task<Customer> GetByLogin(string login);
        Task<IList<Customer>> GetByEmail(string email, int offset, int limit);
        Task AddToCart(Guid customerId, int productId, int amount);
        Task SetProductsAmountInCart(Guid customerId, int productId, int amount);
        Task<IList<ProductInCart>> GetCart(Guid id);
    }
}
