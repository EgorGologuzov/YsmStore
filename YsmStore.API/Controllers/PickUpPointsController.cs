using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Models;
using YsmStore.API.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/location")]
    public class PickUpPointsController : ControllerBase
    {
        private readonly IPickUpPointsRepository _repos;

        public PickUpPointsController(IPickUpPointsRepository repos)
        {
            _repos = repos;
        }

        [Authorize]
        [HttpGet("cities")]
        public async Task<IActionResult> GetCities()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Locality[] result = await _repos.GetCities();

                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return CdekApiBadRequest();
            }
        }

        [Authorize]
        [HttpGet("adresses/{cityName}")]
        public async Task<IActionResult> GetAdresses(string cityName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                string[] result = await _repos.GetAdresses(await _repos.GetCityCode(cityName));

                return Ok(result);
            }
            catch (HttpRequestException)
            {
                return CdekApiBadRequest();
            }
        }

        public IActionResult CdekApiBadRequest()
        {
            ModelState.AddModelError(RequestError.CdekApiBadRequest.Code, RequestError.CdekApiBadRequest.Message);
            return BadRequest(ModelState);
        }
    }
}
