using System.Collections.Generic;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class ProductAdapter
    {
        private const string NO_FILTER = "[нет фильтра]";
        private const string NO_SORT = "[без сортировки]";

        private static Dictionary<int, Product> _list = new Dictionary<int, Product>();

        public static int Execute(ProductQuery query)
        {
            int count = query.TargetList.Count;

            for (int i = 0; i < query.LoadStep; i++)
            {
                Product product = new Product(count + i)
                {
                    Title = (query.Filter == NO_FILTER ? "" : query.Filter) + " Model " + (count + i),
                    Avatar = "https://img.mvideo.ru/Big/30052885bb.jpg",
                    Price = 250000,
                    Description = TestingData.TEST_DESCRIPTION,
                    Quantity = 5
                };

                _list[product.Id] = product;
                query.TargetList.Add(product);
            }

            return query.TargetList.Count - count;
        }

        public static string[] GetAllCategories()
        {
            return new string[]
            {
                NO_FILTER,
                "iPhone",
                "Dyson",
                "Apple watch",
                "Macbook",
                "Sony"
            };
        }

        public static string[] GetAllSortVariants()
        {
            return new string[]
            {
                NO_SORT,
                "Сначала дешевые",
                "Сначала дорогие",
                "Новинки",
                "Популярные"
            };
        }

        public static Product Get(string title, string option1, string option2)
        {
            Product product = new Product(100)
            {
                Title = title,
                Avatar = "https://img.mvideo.ru/Big/30052885bb.jpg",
                Option1 = option1,
                Option2 = option2,
                Price = 250000,
                Description = TestingData.TEST_DESCRIPTION,
                Quantity = 5
            };

            _list[product.Id] = product;
            return product;
        }

        public static Option GetOption1(Product product)
        {
            string name = "Цвет";
            List<OptionVariant> variants = new List<OptionVariant>()
                {
                    new OptionVariant("Red", true, product.Option1 == "Red"),
                    new OptionVariant("Green", true, product.Option1 == "Green"),
                    new OptionVariant("Pink", true, product.Option1 == "Pink"),
                    new OptionVariant("Gold", false, product.Option1 == "Gold"),
                    new OptionVariant("Silver", false, product.Option1 == "Silver")
                };

            return new Option(name, variants, true);
        }

        public static Option GetOption2(Product product)
        {
            string name = "Память";
            List<OptionVariant> variants = new List<OptionVariant>()
                {
                    new OptionVariant("128gb", false, product.Option2 == "128gb"),
                    new OptionVariant("256gb", true, product.Option2 == "256gb"),
                    new OptionVariant("512gb", true, product.Option2 == "512gb")
                };

            return new Option(name, variants, true);
        }

        public static Product Get(int id)
        {
            if (_list.ContainsKey(id))
                return _list[id];

            Product product = new Product(id)
            {
                Title = $"Product {id}",
                Avatar = "https://img.mvideo.ru/Big/30052885bb.jpg",
                Price = 250000,
                Description = TestingData.TEST_DESCRIPTION,
                Quantity = 5
            };

            return product;
        }

        public static List<ProductAmount> GetAllProducts(Order order)
        {
            ProductAmount amount1 = new ProductAmount(0, 1);
            ProductAmount amount2 = new ProductAmount(1, 2);

            return new List<ProductAmount>() { amount1, amount2 };
        }

        public static Dictionary<string, string> GetProperties(Product product)
        {
            return TestingData.TEST_PROPERTIES;
        }

        public static int Execute(StorageQuery query)
        {
            int count = query.TargetList.Count;

            if (string.IsNullOrEmpty(query.QueryText) == false)
            {
                int id;
                if (int.TryParse(query.QueryText, out id))
                {
                    query.TargetList.Add(Get(id));
                    query.IsEndReached = true;
                    return 1;
                }
                else
                {
                    Product product = Get(id);
                    product.Title = query.QueryText;
                    query.TargetList.Add(product);
                    query.IsEndReached = true;
                    return 1;
                }
            }

            for (int i = 0; i < query.LoadStep; i++)
            {
                query.TargetList.Add(Get(count + i));
            }

            return query.TargetList.Count - count;
        }

        public static void Push(Product product, IList<KeyValuePair<string, string>> properties)
        {

        }

        public static void Pull(Product product)
        {
            product.Title = $"Product {product.Id}";
            product.Avatar = "https://img.mvideo.ru/Big/30052885bb.jpg";
            product.Price = 250000;
            product.Description = TestingData.TEST_DESCRIPTION;
            product.Quantity = 5;
        }
    }
}
