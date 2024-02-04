using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Models;

namespace YsmStore.API.Data.Implementations
{
    public class ProductRepository : GeneralRepository<Product>, IProductRepository
    {
        public readonly string[] SortVariants = new string[]
            {
                "Сначала дешевые",
                "Сначала дорогие",
                "Новинки",
                "Популярные"
            };

        public ProductRepository(PostgresContext context, ILogger<ProductRepository> logger) : base(context, logger)
        {
        }

        public Task<string[]> GetCategories()
        {
            return Context.Products.GroupBy(p => p.Category).Select(p => p.Key).ToArrayAsync();
        }

        public string[] GetSortVariants()
        {
            return SortVariants;
        }

        public async Task<IList<Product>> GetByCategory(string category, string sortVariant, int offset, int limit)
        {
            IQueryable<Product> query = Context.Products
                .Join(
                    Context.Products.GroupBy(p => p.Title).Select(g => g.Min(p => p.Id)),
                    p => p.Id,
                    i => i,
                    (p, i) => p
                );

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(p => p.Category == category);
            }

            int index = Array.IndexOf(SortVariants, sortVariant);
            switch (index)
            {
                case 0:
                    query = query.OrderBy(p => p.Price);
                    break;
                case 1:
                    query = query.OrderByDescending(p => p.Price);
                    break;
                case 2:
                    query = query.OrderBy(p => p.Quantity);
                    break;
                case 3:
                    query = query.OrderByDescending(p => p.Quantity);
                    break;
                default:
                    query = query.OrderBy(p => p.Id);
                    break;
            }

            return await query.Skip(offset).Take(limit).ToListAsync();
        }

        public async Task<Product> Get(string title, string option1, string option2)
        {
            return await Context.Products
                .Where(p => p.Title == title && p.Option1 == (option1 ?? string.Empty) && p.Option2 == (option2 ?? string.Empty))
                .FirstOrDefaultAsync();
        }

        public async Task<Option> GetOption1(string title)
        {
            Option option = await Context.Options.FindAsync(title, 1);
            option.Variants = await Context.Products
                .Where(p => p.Title == title)
                .Select(p => p.Option1)
                .Distinct()
                .ToArrayAsync();

            return option;
        }

        public async Task<Option> GetOption2(string title)
        {
            Option option = await Context.Options.FindAsync(title, 2);
            option.Variants = await Context.Products
                .Where(p => p.Title == title)
                .Select(p => p.Option2)
                .Distinct()
                .ToArrayAsync();

            return option;
        }

        public async Task<Product> Get(int id)
        {
            return await Context.Products.FindAsync(id);
        }

        public async Task<IList<OrderedProduct>> GetProductsForOrder(int orderId)
        {
            return await Context.OrderedProducts.Where(op => op.OrderId == orderId).ToListAsync();
        }

        public async Task<Dictionary<string, string>> GetProperties(int id)
        {
            return await Context.Products.Where(p => p.Id == id).Select(p => p.Properties).FirstOrDefaultAsync();
        }

        public async Task<IList<Product>> GetWithTitleLike(string title, int offset, int limit)
        {
            return await Context.Products
                .OrderBy(p => p.Id)
                .Where(p => EF.Functions.ILike(p.Title, $"{title}%"))
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }
    }
}
