using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class UserAdapter
    {
        private static HttpClient _client = new HttpClient();

        private static bool IsBusyCustomersByEMail = false;
        private static bool IsBusyCartQuery = false;

        public static async Task<User> Get(string token)
        {
            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{ApiOptions.RootUrl}/customer"),
                Method = HttpMethod.Get
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Forbidden)
            {
                return new Admin() { Token = token };
            }

            string json = await response.Content.ReadAsStringAsync();

            User user = json.FromJson<Customer>();
            user.Token = token;

            return user;
        }

        public static async void Execute(CustomersByEmailQuery query)
        {
            if (IsBusyCustomersByEMail)
            {
                return;
            }

            IsBusyCustomersByEMail = true;

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/customer/search" +
                    $"?email={query.EmailText ?? string.Empty}" +
                    $"&offset={query.TargetList.Count}" +
                    $"&limit={query.LoadStep}"
                )
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);
            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить список пользователей");

            var json = await response.Content.ReadAsStringAsync();
            var result = json.FromJson<List<Customer>>();
            query.TargetList.AddRange(result);

            query.IsEndReached = result.Count < query.LoadStep;
            IsBusyCustomersByEMail = false;
        }

        public static async Task AddToCart(ProductAmount productAmount)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/customer/cart/{productAmount.ProductId}/{productAmount.Amount}")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new YsmStoreException("Не удалось добавить товар в корзину");
                }
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async Task SetProductAmountInCart(Customer customer, ProductAmount productAmount)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/customer/cart/{productAmount.ProductId}/{productAmount.Amount}")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);

                if (response.IsSuccessStatusCode == false)
                {
                    throw new YsmStoreException($"Не удалось обновить корзину: {response.StatusCode}; {request.RequestUri}");
                }
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async void Execute(CustomerCartQuery query)
        {
            if (IsBusyCartQuery)
            {
                return;
            }

            IsBusyCartQuery = true;

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/customer/cart")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode($"Не удалось загрузить список продуктов в корзине");
                var json = await response.Content.ReadAsStringAsync();
                var result = json.FromJson<List<ProductAmount>>();
                query.TargetList.AddRange(result);
                query.IsEndReached = true;
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
                IsBusyCartQuery = false;
            }
        }
    }
}
