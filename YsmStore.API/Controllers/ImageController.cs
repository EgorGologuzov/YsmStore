using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YsmStore.API.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/image")]
    public class ImageController : ControllerBase
    {
        private const string domain = "technocorestore.ru";
        private readonly string[] _availableTypes = new string[] { "jpeg", "png" };

        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly ILogger<ImageController> _logger;

        public ImageController(IWebHostEnvironment hostEnvironment, ILogger<ImageController> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Content", fileName);
            string fileExtension = Path.GetExtension(fileName);

            try
            {
                return PhysicalFile(filePath, $"image/{fileExtension}");
            }
            catch (FileNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize(Roles = ClientRoles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            string[] type = file.ContentType.Split('/');

            if (type.Length != 2 || type[0] != "image" || !_availableTypes.Contains(type[1]))
            {
                _logger.LogWarning("Failed upload image with content type {type}", file.ContentType);
                return BadRequest();
            }

            Guid id = Guid.NewGuid();
            string fileName = Path.ChangeExtension(id.ToString(), type[1]);
            string imageUrl = $"https://{domain}/api/image/{fileName}";

            string filePath = Path.Combine(_hostEnvironment.ContentRootPath, "Content", fileName);

            using (FileStream stream = new(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok(imageUrl);
        }
    }
}
