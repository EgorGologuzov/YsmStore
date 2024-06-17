using YsmStore.API.Models;
using YsmStore.Services.Utils;

namespace YsmStore.API.Utils
{
    public class PickUpPointsFileCash
    {
        private Dictionary<string, string[]> _adresses;
        private Locality[] _cities;

        public Dictionary<string, string[]> Adresses => _adresses ??= File.ReadAllText("Utils/adresses.json").FromJson<Dictionary<string, string[]>>();
        public Locality[] Cities => _cities ??= File.ReadAllText("Utils/cities.json").FromJson<Locality[]>();
    }
}
