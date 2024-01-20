using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Models;
using YsmStore.Services.Utils;

namespace YsmStore.API.Data.Implementations
{
    public class CustomerRepository : GeneralRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(PostgresContext context, ILogger<CustomerRepository> logger) : base(context, logger)
        {
        }

        public async Task<Customer> GetByLoginAndPassword(string login, string password)
        {
            string passwordHash = password.ToSha256Hash();

            IList<Customer> list = await Context.Customers
                .Where(e => e.Login == login && e.Password == passwordHash)
                .ToListAsync();

            if (list.Count == 0)
            {
                throw new IncorrectLoginOrPasswordException();
            }

            return list.FirstOrDefault();
        }

        public async Task<Customer> UpdatePassword(Guid id, string oldPassword, string newPassword)
        {
            string passwordHash = oldPassword.ToSha256Hash();

            IList<Customer> list = await Context.Customers
                .Where(e => e.Id == id && e.Password == passwordHash)
                .ToListAsync();

            if (list.Count == 0)
            {
                throw new IncorrectLoginOrPasswordException();
            }

            Customer customer = list.FirstOrDefault();
            customer.Password = newPassword.ToSha256Hash();
            await Context.SaveChangesAsync();

            Logger.LogInformation("Password updated for customer {customer}", customer.Id);

            return customer;
        }

        public async Task<Customer> ResetPassword(Guid id, string newPassword)
        {
            Customer customer = await Context.Customers.FindAsync(id);

            if (customer is null)
            {
                throw new EntityNotFoundException();
            }

            customer.Password = newPassword.ToSha256Hash();
            await Context.SaveChangesAsync();

            Logger.LogInformation("Password reset for customer {customer}", customer.Id);

            return customer;
        }

        public async Task<Customer> GetByLogin(string login)
        {
            Customer customer = await Context.Customers.Where(c => c.Login == login).FirstOrDefaultAsync();

            if (customer is null)
            {
                throw new IncorrectLoginOrPasswordException();
            }

            return customer;
        }

        public async Task<IList<Customer>> GetByEmail(string email, int offset, int limit)
        {
            IList<Customer> list = await Context.Customers
                .Where(c => c.Login.StartsWith(email))
                .OrderBy(c => c.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();

            return list;
        }

        public async Task AddToCart(Guid customerId, int productId, int amount)
        {
            if (amount <= 0)
            {
                throw new AmountIsNotPositiveException();
            }

            ProductInCart pic = await Context.ProductInCarts.FindAsync(customerId, productId);

            if (pic is null)
            {
                await Context.ProductInCarts.AddAsync(new() { CustomerId = customerId, ProductId = productId, Amount = amount });
            }
            else
            {
                pic.Amount += amount;
            }

            await Context.SaveChangesAsync();
        }

        public async Task SetProductsAmountInCart(Guid customerId, int productId, int amount)
        {
            if (amount < 0)
            {
                throw new AmountIsNotPositiveException();
            }

            ProductInCart pic = await Context.ProductInCarts.FindAsync(customerId, productId);

            if (pic is not null)
            {
                if (amount == 0)
                {
                    Context.ProductInCarts.Remove(pic);
                }
                else
                {
                    pic.Amount = amount;
                }
            }
            else if (amount != 0)
            {
                await Context.ProductInCarts.AddAsync(new() { CustomerId = customerId, ProductId = productId, Amount = amount });
            }

            await Context.SaveChangesAsync();
        }

        public async Task<IList<ProductInCart>> GetCart(Guid id)
        {
            IList<ProductInCart> list = await Context.ProductInCarts
                .Where(pic => pic.CustomerId == id)
                .ToListAsync();

            return list;
        }

        public override Task<Customer> Create(Customer customer)
        {
            customer.Password = customer.Password.ToSha256Hash();

            return base.Create(customer);
        }
    }
}
