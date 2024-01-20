using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Dto;
using YsmStore.API.Models;
using YsmStore.API.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : GeneralController<Customer>
    {
        private readonly ICustomerRepository _repos;

        public CustomerController(
            ICustomerRepository repos,
            ILogger<CustomerController> logger,
            IMapper mapper) : base(repos, logger, mapper)
        {
            _repos = repos;
        }

        [Authorize(Roles = ClientRoles.Customer)]
        [HttpGet]
        public async Task<IActionResult> GetMe()
        {
            Guid id = new(GetClaimValue(ClaimTypes.PrimarySid));

            IActionResult result = await GetByIdGeneric(id);

            if (result is OkObjectResult r)
            {
                var value = (Customer)r.Value;
                r.Value = Mapper.Map<CustomerReturnDto>(value);
            }

            return result;
        }

        [Authorize(Roles = ClientRoles.Admin)]
        [HttpGet("search")]
        public async Task<IActionResult> GetByEmail([FromBody] CustomerByEmailRequestDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IList<Customer> result = await _repos.GetByEmail(data.Email, data.Offset, data.Limit);
            IList<CustomerReturnDto> returnResult = Mapper.Map<IList<CustomerReturnDto>>(result);

            return Ok(returnResult);
        }

        [Authorize(Roles = ClientRoles.Customer)]
        [HttpPost("cart/{productId}/{amount}")]
        public async Task<IActionResult> AddToCart(int productId, int amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid id = new(GetClaimValue(ClaimTypes.PrimarySid));

            try
            {
                await _repos.AddToCart(id, productId, amount);

                return Ok();
            }
            catch (AmountIsNotPositiveException)
            {
                return AmountIsNotPositive();
            }
            catch (DbUpdateException)
            {
                return InvalidData();
            }
            catch (InvalidOperationException)
            {
                return InvalidData();
            }
        }

        [Authorize(Roles = ClientRoles.Customer)]
        [HttpPut("cart/{productId}/{amount}")]
        public async Task<IActionResult> SetProductAmountInCart(int productId, int amount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid id = new(GetClaimValue(ClaimTypes.PrimarySid));

            try
            {
                await _repos.SetProductsAmountInCart(id, productId, amount);

                return Ok();
            }
            catch (AmountIsNotPositiveException)
            {
                return AmountIsNotPositive();
            }
            catch (DbUpdateException)
            {
                return InvalidData();
            }
            catch (InvalidOperationException)
            {
                return InvalidData();
            }
        }

        [Authorize(Roles = ClientRoles.Customer)]
        [HttpGet("cart")]
        public async Task<IActionResult> GetCart()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Guid id = new(GetClaimValue(ClaimTypes.PrimarySid));

            var result = await _repos.GetCart(id);
            var returnResult = Mapper.Map<IList<ProductInCartReturnDto>>(result);

            return Ok(returnResult);
        }
    }
}
