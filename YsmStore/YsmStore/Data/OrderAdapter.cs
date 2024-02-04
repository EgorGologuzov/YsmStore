using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class OrderAdapter
    {
        private static HttpClient _client = new HttpClient();

        private static bool IsBusyCustomerOrdersQuery = false;
        private static bool IsBusyAdminOrdersQuery = false;

        public static Order Create(Customer customer)
        {
            return new Order();
        }

        public static async Task<Order> Get(int id)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/order/{id}")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode($"Не удалось загрузить заказ по коду {id}");
                string json = await response.Content.ReadAsStringAsync();

                return json.FromJson<Order>();
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async Task<string[]> GetCities()
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/location/cities")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось загрузить города");

            var json = await response.Content.ReadAsStringAsync();
            request.Dispose();
            response.Dispose();

            var result = JArray.Parse(json).Select(l => l["city"].Value<string>()).ToArray();
            Array.Sort(result);

            return result.ToArray();
        }

        public static async Task<string[]> GetAdresses(string city)
        {
            if (string.IsNullOrEmpty(city))
            {
                return new string[0];
            }

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/location/adresses/{city}")
            };

            await request.AddActualToken();

            var response = await _client.SendAsync(request);

            response.ThrowYsmStoreExceptionIfNotSuccessStatusCode($"Не удалось загрузить адреса для города {city}");

            var json = await response.Content.ReadAsStringAsync();
            request.Dispose();
            response.Dispose();

            return json.FromJson<string[]>();
        }

        public static async Task Push(Order order, IList<ProductAmount> orderedProducts)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/order"),
                Content = new { PickUpAdress = order.PickUpAdress, PhoneNumber = order.PhoneNumber, Products = orderedProducts }.ToStringContent()
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new YsmStoreException("Не удалось создать заказ, попробуйте позже");
                }
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async void Execute(CustomerOrdersQuery query)
        {
            if (IsBusyCustomerOrdersQuery)
            {
                return;
            }

            IsBusyCustomerOrdersQuery = true;

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/order/query/customerorders" +
                $"?offset={query.TargetList.Count}" +
                $"&limit={query.LoadStep}" +
                $"&customerId={query.CustomerId}")
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode($"Не удалось загрузить список заказов клиента");
                var json = await response.Content.ReadAsStringAsync();
                var result = json.FromJson<List<Order>>();
                query.TargetList.AddRange(result);
                query.IsEndReached = result.Count < query.LoadStep;
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
                IsBusyCustomerOrdersQuery = false;
            }
        }

        public static async Task Push(Order order)
        {
            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/order"),
                Content = order.ToStringContent()
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    throw new YsmStoreException("Не удалось обновить заказ, проверьте правильность введенных данных");
                }
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
            }
        }

        public static async void Execute(AdminOrdersQuery query)
        {
            if (IsBusyAdminOrdersQuery)
            {
                return;
            }

            IsBusyAdminOrdersQuery = true;

            if (query.OrderId != null)
            {
                try
                {
                    Order result = await Get(query.OrderId.Value);
                    query.TargetList.Add(result);
                    query.IsEndReached = true;
                }
                catch (YsmStoreException) { }

                IsBusyAdminOrdersQuery = false;
                return;
            }

            var request = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiOptions.RootUrl}/order/query/bystatusbetween" +
                    $"?offset={query.TargetList.Count}" +
                    $"&limit={query.LoadStep}" +
                    $"&statusFilter={query.StatusFilter}" +
                    $"&startDate={query.StartDate.ToString("s")}" +
                    $"&endDate={query.EndDate.ToString("s")}"
                )
            };

            await request.AddActualToken();

            HttpResponseMessage response = null;

            try
            {
                response = await _client.SendAsync(request);
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode($"Не удалось загрузить список для администратора");
                var json = await response.Content.ReadAsStringAsync();
                var result = json.FromJson<List<Order>>();
                query.TargetList.AddRange(result);
                query.IsEndReached = result.Count < query.LoadStep;
            }
            finally
            {
                request?.Dispose();
                response?.Dispose();
                IsBusyAdminOrdersQuery = false;
            }
        }

        public static async Task Pull(Order order)
        {
            Order oldData = await Get(order.Id);
            order.DeliveryDate = oldData.DeliveryDate;
            order.Status = oldData.Status;
        }
    }
}
