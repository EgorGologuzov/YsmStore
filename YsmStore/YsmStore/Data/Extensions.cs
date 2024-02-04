using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class Extensions
    {
        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static async Task<string> FirstErrorKey(this HttpResponseMessage response)
        {
            return (await response.Content.ReadAsStringAsync()).FromJson<Dictionary<string, string[]>>().FirstOrDefault().Key;
        }

        public static StringContent ToStringContent(this object data)
        {
            return new StringContent(
                data.ToJson(),
                System.Text.Encoding.UTF8,
                "application/json"
            );
        }

        public static void AddRange<T>(this IList list, IList<T> newElements)
        {
            foreach (var obj in newElements)
            {
                list.Add(obj);
            }
        }

        public static async Task AddActualToken(this HttpRequestMessage request)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await ApiOptions.GetActualToken());
        }

        public static void ThrowYsmStoreExceptionIfNotSuccessStatusCode(this HttpResponseMessage response, string message)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new YsmStoreException($"{message}; {response.RequestMessage.Method}; {response.RequestMessage.RequestUri}", ex) ;
            }
        }
    }
}
