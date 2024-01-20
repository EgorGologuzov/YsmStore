using System.Threading.Tasks;
using YsmStore.API.Models;

namespace YsmStore.API.Data.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        string[] GetSortVariants();
        Task<IList<Product>> GetByCategory(string category, string sortVariant, int offset, int limit);
        Task<Option> GetOption1(string title);
        Task<Option> GetOption2(string title);
        Task<Product> Get(int id);
        Task<Product> Get(string title, string option1, string option2);
        Task<string[]> GetCategories();
        Task<IList<OrderedProduct>> GetProductsForOrder(int orderId);
        Task<Dictionary<string, string>> GetProperties(int id);
        Task<IList<Product>> GetWithTitleLike(string title, int offset, int limit);
    }
}
