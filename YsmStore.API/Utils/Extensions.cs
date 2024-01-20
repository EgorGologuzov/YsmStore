using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace YsmStore.Services.Utils
{
    public static class Extensions
    {
        public static string ToSha256Hash(this string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] hash = SHA256.HashData(bytes);
            return Convert.ToHexString(hash).ToLower();
        }

        public static string ToJson<T>(this T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented);
        }

        public static T FromJson<T>(this string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T FromJson<T>(this JsonElement value)
        {
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public static T UpdateFromJson<T>(this T target, JsonElement source)
        {
            JsonConvert.PopulateObject(source.ToString(), target);

            return target;
        }
    }
}