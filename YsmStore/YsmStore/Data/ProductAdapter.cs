using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class ProductAdapter
    {
        private const string NO_FILTER = "[нет фильтра]";
        private const string NO_SORT = "[без сортировки]";

        private static HttpClient _client = new HttpClient();
        private static bool IsBusyStorageQuery = false;
        private static bool IsBusyProductQuery = false;

        public static async void Execute(ProductQuery query)
        {
            if (IsBusyProductQuery)
            {
                return;
            }

            IsBusyProductQuery = true;

            string finalCategory = string.IsNullOrEmpty(query.Filter) || query.Filter == NO_FILTER ? string.Empty : query.Filter;
            string finalSort = string.IsNullOrEmpty(query.SortVariant) || query.Filter == NO_SORT ? string.Empty : query.SortVariant;

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/query/bycategory" +
                    $"?offset={query.TargetList.Count}" +
                    $"&limit={query.LoadStep}" +
                    $"&category={finalCategory}" +
                    $"&sortVariant={finalSort}"
                )
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось выполнить запрос продукты 0");

            var json = await response.Content.ReadAsStringAsync();
            var result = json.FromJson<List<Product>>();
            query.TargetList.AddRange(result);
            query.IsEndReached = result.Count < query.LoadStep;

            request.Dispose();
            response.Dispose();

            IsBusyProductQuery = false;
        }

        public static async Task<string[]> GetAllCategories()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/categories")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить категории");

            var json = await response.Content.ReadAsStringAsync();

            request.Dispose();
            response.Dispose();

            var result = json.FromJson<List<string>>();
            result.Insert(0, NO_FILTER);

            return result.ToArray();
        }

        public static async Task<string[]> GetAllSortVariants()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/sortvariants")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить варианты сортировки");

            var json = await response.Content.ReadAsStringAsync();

            request.Dispose();
            response.Dispose();

            var result = json.FromJson<List<string>>();
            result.Insert(0, NO_SORT);

            return result.ToArray();
        }

        public static async Task<Product> Get(string title, string option1, string option2)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/query/bytitleandoptions" +
                    $"?title={title}" +
                    $"&option1={option1}" +
                    $"&option2={option2}"
                )
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось выполнить запрос продукт по названию и опциям");

            var json = await response.Content.ReadAsStringAsync();
            var result = json.FromJson<Product>();

            request.Dispose();
            response.Dispose();

            return result;
        }

        public static async Task<Option> GetOption1(Product product)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/option/{product.Title}/1")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
            {
                return new Option(null, null, false);
            }

            JToken jToken = JToken.Parse(json);
            string[] vars = jToken["variants"].ToObject<string[]>();
            var optionVariants = new List<OptionVariant>();

            var tasks = vars.Select(v => Get(product.Title, v, product.Option2)).ToList();
            var result = await Task.WhenAll(tasks);

            for (int i = 0; i < vars.Length; i++)
            {
                optionVariants.Add(
                    new OptionVariant(
                        vars[i],
                        result[i] != null && result[i].Quantity > 0,
                        product.Option1 == vars[i])
                    );
            }

            request.Dispose();
            response.Dispose();

            return new Option(jToken["name"].Value<string>(), optionVariants, true);
        }

        public static async Task<Option> GetOption2(Product product)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/option/{product.Title}/2")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();

            if (response.StatusCode != HttpStatusCode.OK || string.IsNullOrEmpty(json))
            {
                return new Option(null, null, false);
            }

            JToken jToken = JToken.Parse(json);
            string[] vars = jToken["variants"].ToObject<string[]>();
            var optionVariants = new List<OptionVariant>();

            var tasks = vars.Select(v => Get(product.Title, product.Option1, v)).ToList();
            var result = await Task.WhenAll(tasks);

            for (int i = 0; i < vars.Length; i++)
            {
                optionVariants.Add(
                    new OptionVariant(
                        vars[i],
                        result[i] != null && result[i].Quantity > 0,
                        product.Option2 == vars[i])
                    );
            }

            request.Dispose();
            response.Dispose();

            return new Option(jToken["name"].Value<string>(), optionVariants, true);
        }

        public static async Task<Product> Get(int id)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/{id}")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить продукт по коду");

            var json = await response.Content.ReadAsStringAsync();
            Product result = json.FromJson<Product>();

            request.Dispose();
            response.Dispose();

            return result;
        }

        public static async Task<List<ProductAmount>> GetAllProducts(Order order)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/inorder/{order.Id}")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить свойства продукта");
                var json = await response.Content.ReadAsStringAsync();

                return json.FromJson<List<ProductAmount>>();
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async Task<Dictionary<string, string>> GetProperties(Product product)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product/properties/{product.Id}")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new Dictionary<string, string>();
            }

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить свойства продукта");

            var json = await response.Content.ReadAsStringAsync();

            request.Dispose();
            response.Dispose();

            return json.FromJson<Dictionary<string, string>>();
        }

        public static async void Execute(StorageQuery query)
        {
            if (IsBusyStorageQuery)
            {
                return;
            }

            IsBusyStorageQuery = true;

            int id = 0;
            if (int.TryParse(query.QueryText, out id))
            {
                try
                {
                    Product result = await Get(id);
                    query.TargetList.Add(result);
                    query.IsEndReached = true;
                }
                catch (YsmStoreException)
                {
                }
            }
            else
            {
                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{ApiOptions.RootUrl}/product/query/titlelike" +
                        $"?offset={query.TargetList.Count}" +
                        $"&limit={query.LoadStep}" +
                        $"&title={query.QueryText ?? string.Empty}"
                    )
                };

                await request.AddActualToken();

                var response = await _client.SendAsync(request);

                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось выполнить запрос склада 1");

                var json = await response.Content.ReadAsStringAsync();
                var result = json.FromJson<List<Product>>();
                query.TargetList.AddRange(result);
                query.IsEndReached = result.Count < query.LoadStep;

                request.Dispose();
                response.Dispose();
            }

            IsBusyStorageQuery = false;
        }

        public static async Task Push(Product product, IList<KeyValuePair<string, string>> properties)
        {
            string json = product.ToJson();
            JToken jToken = JToken.Parse(json);
            jToken["properties"] = JToken.Parse(properties.ToDictionary(p => p.Key, p => p.Value).ToJson());

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/product"),
                Content = new StringContent(jToken.ToString(), Encoding.UTF8, "application/json")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            request.Dispose();
            response.Dispose();

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось обновить данные, проверьте правильность введенных данных");
        }

        public static async Task Pull(Product product)
        {
            Product oldData = await Get(product.Id);
            product.Title = oldData.Title;
            product.Category = oldData.Category;
            product.Price = oldData.Price;
            product.Description = oldData.Description;
            product.Avatar = oldData.Avatar;
            product.Option1 = oldData.Option1;
            product.Option2 = oldData.Option2;
            product.Quantity = oldData.Quantity;
        }
    }
}
