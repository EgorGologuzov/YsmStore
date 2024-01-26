using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Dto;
using YsmStore.API.Models;
using YsmStore.API.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : GeneralController<Product>
    {
        private readonly IProductRepository _repos;

        public ProductController(
            IProductRepository repos,
            ILogger<ProductController> logger,
            IMapper mapper) : base(repos, logger, mapper)
        {
            _repos = repos;
        }

        [Authorize]
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] result = await _repos.GetCategories();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("sortvariants")]
        public IActionResult GetSortVariants()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string[] result = _repos.GetSortVariants();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("query/bycategory")]
        public async Task<IActionResult> GetByCategory([FromBody] ProductByCategoryRequestDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IList<Product> result = await _repos.GetByCategory(data.Category, data.SortVariant, data.Offset, data.Limit);
            IList<ProductReturnDto> returnResult = Mapper.Map<IList<ProductReturnDto>>(result);

            return Ok(returnResult);
        }

        [Authorize]
        [HttpGet("query/bytitleandoptions")]
        public async Task<IActionResult> GetByTitleAndOptions([FromBody] ProductByTitleAndOptionsDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product result = await _repos.Get(data.Title, data.Option1, data.Option2);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return await GetByIdGeneric(id);
        }

        [Authorize]
        [HttpGet("option/{title}/{number}")]
        public async Task<IActionResult> GetOption(string title, int number)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Option option = null;

            switch (number)
            {
                case 1:
                    option = await _repos.GetOption1(title);
                    break;
                case 2:
                    option = await _repos.GetOption2(title);
                    break;
                default:
                    return OptionNumberOutOfRange();
            }

            return Ok(option);
        }

        [Authorize]
        [HttpGet("inorder/{orderId}")]
        public async Task<IActionResult> GetProdcutsForOrder(int orderId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IList<OrderedProduct> result = await _repos.GetProductsForOrder(orderId);
            IList<OrderedProductDto> returnResult = Mapper.Map<IList<OrderedProductDto>>(result);

            return Ok(returnResult);
        }

        [Authorize]
        [HttpGet("properties/{id}")]
        public async Task<IActionResult> GetProperties(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Dictionary<string, string> result = await _repos.GetProperties(id);

            return Ok(result);
        }

        [Authorize]
        [HttpGet("query/titlelike")]
        public async Task<IActionResult> GetWithTitleLike([FromBody] ProductByTitleRequestDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IList<Product> result = await _repos.GetWithTitleLike(data.Title, data.Offset, data.Limit);
            IList<ProductReturnDto> returnResult = Mapper.Map<IList<ProductReturnDto>>(result);

            return Ok(returnResult);
        }

        [Authorize(Roles = ClientRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] ProductUpdateDto data)
        {
            return await UpdateGeneric(data.Id, data);
        }
    }
}
