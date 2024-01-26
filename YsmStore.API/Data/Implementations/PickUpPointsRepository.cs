using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Models;
using YsmStore.Services.Utils;

namespace YsmStore.API.Data.Implementations
{
    public class PickUpPointsRepository : IPickUpPointsRepository
    {
        private const double _citiesUpdateTimeSpan = 24;
        private const double _adressesUpdateTimeSpan = 24;
        private const double _tokenUpdateTimeSpan = 0.5;
        private const string _clientId = "EMscd6r9JnFiQ3bLoyjJY6eM78JrJceI";
        private const string _clientSecret = "PjLZkKBHEiLK3YsjtNrt3TGNG0ahs3kG";

        private readonly HttpClient _client;

        private readonly Dictionary<int, DateTime> _adressesLastUpdate;
        private readonly Dictionary<int, string[]> _adresses;
        private DateTime _tokenLastUpdate;
        private string _token;

        private DateTime _citiesLastUpdate;
        private Locality[] _cities;
        public PickUpPointsRepository()
        {
            _client = new HttpClient();
            _adressesLastUpdate = new();
            _adresses = new();
        }

        public async Task<Locality[]> GetCities()
        {
            if (_cities is null || (DateTime.Now - _citiesLastUpdate) > TimeSpan.FromHours(_citiesUpdateTimeSpan))
            {
                await UpdateCities();
            }

            return _cities;
        }

        public async Task<string[]> GetAdresses(int cityId)
        {
            if (_adressesLastUpdate.ContainsKey(cityId) == false || (DateTime.Now - _adressesLastUpdate[cityId]) > TimeSpan.FromHours(_adressesUpdateTimeSpan))
            {
                await UpdateAdresses(cityId);
            }

            return _adresses[cityId];
        }

        public async Task<int> GetCityCode(string cityName)
        {
            return (await GetCities()).FirstOrDefault(l => l.City == cityName).Code;
        }

        private async Task UpdateAdresses(int cityId)
        {
            if (_token is null || (DateTime.Now - _tokenLastUpdate) > TimeSpan.FromHours(_tokenUpdateTimeSpan))
            {
                await UpdateToken();
            }

            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.edu.cdek.ru/v2/deliverypoints?city_code={cityId}")
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            JObject[] result = JsonConvert.DeserializeObject<JObject[]>(await response.Content.ReadAsStringAsync());
            string[] adresses = result.Select(j => j["location"]["city"].Value<string>() + ", " + j["location"]["address"].Value<string>()).ToArray();

            _adresses[cityId] = adresses;
            _adressesLastUpdate[cityId] = DateTime.Now;
        }

        private async Task UpdateCities()
        {
            if (_token is null || (DateTime.Now - _tokenLastUpdate) > TimeSpan.FromHours(_tokenUpdateTimeSpan))
            {
                await UpdateToken();
            }

            StringContent content = new(
                    (new { country_codes = new string[] { "RU" } }).ToJson(),
                    Encoding.UTF8,
                    "application/json"
                );

            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.edu.cdek.ru/v2/location/cities"),
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _token);

            using HttpResponseMessage response = await _client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string result = await response.Content.ReadAsStringAsync();
            _cities = result.FromJson<Locality[]>();
            _citiesLastUpdate = DateTime.Now;
        }

        private async Task UpdateToken()
        {
            string request = $"https://api.edu.cdek.ru/v2/oauth/token?grant_type=client_credentials&client_id={_clientId}&client_secret={_clientSecret}";
            using HttpResponseMessage response = await _client.PostAsync(request, null);
            response.EnsureSuccessStatusCode();

            JObject result = JObject.Parse(await response.Content.ReadAsStringAsync());
            _token = (string)result["access_token"];
            _tokenLastUpdate = DateTime.Now;
        }
    }
}
