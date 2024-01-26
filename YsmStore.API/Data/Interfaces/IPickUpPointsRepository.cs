using YsmStore.API.Models;

namespace YsmStore.API.Data.Interfaces
{
    public interface IPickUpPointsRepository
    {
        Task<Locality[]> GetCities();
        Task<string[]> GetAdresses(int cityId);
        Task<int> GetCityCode(string cityName);
    }
}
