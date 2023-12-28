using System.IO;
using System.Threading.Tasks;

namespace YsmStore.Data
{
    public interface IPhotoPickerService
    {
        Task<Stream> GetImageStreamAsync();
    }
}
