using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class ApiOptions
    {
        public const string RootUrl = "http://158.160.78.11/api";
        public static HttpClient Client = new HttpClient();
        private static readonly TimeSpan _tokenUpdateInterval = TimeSpan.FromMinutes(1);
        private static DateTime _lastTokenUpdate = DateTime.MinValue;
        private static User _user => AuthSystem.LoginedUser;

        public static async Task<string> GetActualToken()
        {
            if (_user == null)
            {
                throw new YsmStoreException("Cannot get actual token: LoginedUser is null");
            }

            if (DateTime.Now - _lastTokenUpdate > _tokenUpdateInterval)
            {
                var response = await Client.GetAsync($"{RootUrl}/auth/{_user.Login}/{_user.Password}");
                response.ThrowYsmStoreExceptionIfNotSuccessStatusCode("Не удалось обновить токен");
                _user.Token = await response.Content.ReadAsStringAsync();
                _lastTokenUpdate = DateTime.Now;
            }

            return _user.Token;
        }
    }
}
