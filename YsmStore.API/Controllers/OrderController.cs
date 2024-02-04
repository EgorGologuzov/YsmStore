using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Principal;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Dto;
using YsmStore.API.Models;
using YsmStore.API.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/order")]
    public class OrderController : GeneralController<Order>
    {
        private readonly IOrderRepository _repos;

        public OrderController(IOrderRepository repos, ILogger<OrderController> logger, IMapper mapper) : base(repos, logger, mapper)
        {
            _repos = repos;
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Order result = await _repos.GetById(id);

            if (result is null)
            {
                return EntityNotFound();
            }

            string role = GetClaimValue(ClaimTypes.Role);

            if (role == ClientRoles.Customer)
            {
                Guid customerId = new(GetClaimValue(ClaimTypes.PrimarySid));
                if (result.CustomerId != customerId)
                {
                    return Forbid();
                }
            }

            OrderReturnDto returnResult = Mapper.Map<OrderReturnDto>(result);

            return Ok(returnResult);
        }

        [Authorize(Roles = ClientRoles.Customer)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (data.Products.Count == 0)
            {
                return NoProductsInOrder();
            }

            Order order = Mapper.Map<Order>(data);
            order.CustomerId = new(GetClaimValue(ClaimTypes.PrimarySid));
            order.OrderDate = DateTime.Now;
            order.DeliveryDate = DateTime.Now + TimeSpan.FromDays(7);
            order.Status = OrderStatus.Processed;

            try
            {
                Order result = await _repos.Create(order);
                result = await _repos.GetById(result.Id);
                OrderReturnDto returnResult = Mapper.Map<OrderReturnDto>(result);

                return Ok(returnResult);
            }
            catch (AmountIsNotPositiveException)
            {
                return AmountIsNotPositive();
            }
            catch (NotEnoughtProductException)
            {
                return NotEnoughtProduct();
            }
            catch (DbUpdateException)
            {
                return InvalidData();
            }
        }

        [Authorize(Roles = ClientRoles.Admin)]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] OrderUpdateDto data)
        {
            IActionResult result = await UpdateGeneric(data.Id, data);

            if (result is OkObjectResult r)
            {
                var value = (Order)r.Value;
                r.Value = await GetReturnDtoByEntityId<OrderReturnDto>(value.Id);
            }

            return result;
        }

        [Authorize]
        [HttpGet("query/customerorders")]
        public async Task<IActionResult> GetCustomerOrders([FromQuery] CustomerOrdersRequestDto data)
        {
            string role = GetClaimValue(ClaimTypes.Role);

            if (role == ClientRoles.Customer)
            {
                data.CustomerId = new(GetClaimValue(ClaimTypes.PrimarySid));
            }

            if (data.CustomerId is null)
            {
                return InvalidData();
            }

            IList<Order> result = await _repos.GetByCustomerId(data.CustomerId.Value, data.Offset, data.Limit);
            IList<OrderReturnDto> returnResult = Mapper.Map<IList<OrderReturnDto>>(result);

            return Ok(returnResult);
        }

        [Authorize(Roles = ClientRoles.Admin)]
        [HttpGet("query/bystatusbetween")]
        public async Task<IActionResult> GetOrdersByStatusBetween([FromQuery] OrdersRequestDto data)
        {
            IList<Order> result = await _repos.GetByStatusBetween(data.StatusFilter, data.StartDate, data.EndDate, data.Offset, data.Limit);
            IList<OrderReturnDto> returnResult = Mapper.Map<IList<OrderReturnDto>>(result);

            return Ok(returnResult);
        }
    }
}
