using System.Collections.Generic;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class UserAdapter
    {
        private static Dictionary<string, User> _list = new Dictionary<string, User>();

        public static User Get(string token)
        {
            //try load user data by token
            //if fail throw YsmStoreException, if success
            if (_list.ContainsKey(token))
            {
                //update user data
                return _list[token];
            }
            //if user not in list
            if (token == TestingData.CUSTOMER_TEST_TOKEN)
            {
                Customer customer = new Customer(token) { FullName = "Customer Test Name" };
                _list[token] = customer;
                return customer;
            }
            if (token == TestingData.ADMIN_TEST_TOKEN)
            {
                Admin admin = new Admin(token);
                _list[token] = admin;
                return admin;
            }

            throw new YsmStoreException(string.Empty);
        }

        public static int Execute(CustomersByEmailQuery query)
        {
            if (query == null)
                throw new YsmStoreException("Query cannot be null");

            int customersCount = query.TargetList.Count;

            for (int i = 0; i < query.LoadStep; i++)
            {
                Customer customer = new Customer($"customer_token_{customersCount + 1}")
                {
                    FullName = $"FullName{customersCount + i}",
                    UserName = $"UserName{customersCount + i}",
                    Login = (string.IsNullOrEmpty(query.EmailText) ? "login" : query.EmailText) + $"_{customersCount + i}@gmail.com"
                };

                _list[customer.Token] = customer;
                query.TargetList.Add(customer);
            }

            return query.TargetList.Count - customersCount;
        }

        public static void AddToCart(Customer customer, ProductAmount productAmount)
        {

        }

        public static void SetProductAmountInCart(Customer customer, ProductAmount productAmount)
        {

        }

        public static int LoadCart(CustomerCartQuery query)
        {
            int count = query.TargetList.Count;

            for (int i = 0; i < query.LoadStep; i++)
            {
                query.TargetList.Add(new ProductAmount(count + i, i + 1));
            }

            return query.TargetList.Count - count;
        }

        public static List<ProductAmount> GetCart(Customer customer)
        {
            var cart = new List<ProductAmount>()
            {
                new ProductAmount(0, 1),
                new ProductAmount(1, 3),
                new ProductAmount(2, 6)
            };

            return cart;
        }
    }
}
