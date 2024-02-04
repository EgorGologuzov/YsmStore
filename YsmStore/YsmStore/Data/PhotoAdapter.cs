using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.Forms;
using YsmStore.Models;

namespace YsmStore.Data
{
    public static class PhotoAdapter
    {
        private static HttpClient _client = new HttpClient();

        public static async Task<string> Upload(Stream imageStream)
        {
            using (var content = new MultipartFormDataContent())
            {
                var imageContent = new StreamContent(imageStream);
                string imageType = DetectTypeFromStream(imageStream);

                if (imageType == null)
                {
                    throw new YsmStoreException("Поддерживаются только изображения в формате png и jpeg");
                }

                imageContent.Headers.ContentType = new MediaTypeHeaderValue($"image/{imageType}");
                content.Add(imageContent, "file", $"file.{imageType}");

                var request = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("http://158.160.78.11/api/image"),
                    Content = content
                };

                await request.AddActualToken();

                var response = await _client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.RequestEntityTooLarge)
                    {
                        throw new YsmStoreException("Допустимый размер изображения не более 20 Мб");
                    }

                    throw new YsmStoreException("Не удалось загрузить изображение");
                }

                return await response.Content.ReadAsStringAsync();
            }
        }

        public static string DetectTypeFromStream(Stream imageStream)
        {
            byte[] buffer = new byte[4];
            imageStream.Read(buffer, 0, 4);
            string result = null;

            if (buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47)
            {
                result = "png";
            }
            else if (buffer[0] == 0xFF && buffer[1] == 0xD8)
            {
                result = "jpeg";
            }

            imageStream.Seek(0, SeekOrigin.Begin);

            return result;
        }
    }
}
