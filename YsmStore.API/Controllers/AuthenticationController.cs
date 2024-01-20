using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Dto;
using YsmStore.API.Models;
using YsmStore.API.Utils;
using YsmStore.Services.Utils;

namespace YsmStore.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthenticationController : GeneralController<Customer>
    {
        private readonly JWTSettings _options;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmailService _emailService;

        public AuthenticationController(
            IOptions<JWTSettings> optionsAccess,
            ICustomerRepository customerRepository,
            ILogger<AuthenticationController> logger,
            IMapper mapper,
            IEmailService emailService) : base(customerRepository, logger, mapper)
        {
            _options = optionsAccess.Value;
            _customerRepository = customerRepository;
            _emailService = emailService;
        }

        [HttpGet("{login}/{password}")]
        public async Task<IActionResult> GetToken(string login, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (login == _options.AdminLogin && password.ToSha256Hash() == _options.AdminPassword)
            {
                List<Claim> claims = new()
                {
                    new(ClaimTypes.Role, ClientRoles.Admin)
                };

                return Ok(GenerateToken(claims));
            }

            try
            {
                Customer customer = await _customerRepository.GetByLoginAndPassword(login, password);

                List<Claim> claims = new()
                {
                    new(ClaimTypes.Role, ClientRoles.Customer),
                    new(ClaimTypes.PrimarySid, customer.Id.ToString())
                };

                return Ok(GenerateToken(claims));
            }
            catch (IncorrectLoginOrPasswordException)
            {
                return Forbid();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registration(CustomerCreateDto data)
        {
            if (!ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            Customer newCustomer = Mapper.Map<Customer>(data);

            try
            {
                Customer result = await _customerRepository.Create(newCustomer);

                List<Claim> claims = new()
                {
                    new(ClaimTypes.Role, ClientRoles.Customer),
                    new(ClaimTypes.PrimarySid, result.Id.ToString())
                };

                return Ok(GenerateToken(claims));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is PostgresException pex && pex.ConstraintName == "IX_Customers_Login")
                {
                    return LoginNotAvailable();
                }

                return InvalidData();
            }
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> RecoveryRequest(string email)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Customer customer = await _customerRepository.GetByLogin(email);
                string recoveryPassword = (_options.SecretKey + DateTime.Now.ToString()).ToSha256Hash().Substring(0, 15);
                customer.RecoveryPassword = recoveryPassword.ToSha256Hash();

                _emailService.SendRecoveryEmail(email, recoveryPassword);
                await _customerRepository.Update(customer.Id, customer);

                return Ok();
            }
            catch (IncorrectLoginOrPasswordException)
            {
                return LoginNotFound();
            }
        }

        [HttpPut]
        public async Task<IActionResult> RecoveryFinish([FromBody] CustomerRecoveryDto data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                Customer customer = await _customerRepository.GetByLogin(data.Login);

                if (customer.RecoveryPassword != data.RecoveryPassword.ToSha256Hash())
                {
                    return IncorrectLoginOrPassword();
                }

                customer.Password = data.NewPassword.ToSha256Hash();
                customer.RecoveryPassword = null;
                await _customerRepository.Update(customer.Id, customer);

                List<Claim> claims = new()
                {
                    new(ClaimTypes.Role, ClientRoles.Customer),
                    new(ClaimTypes.PrimarySid, customer.Id.ToString())
                };

                return Ok(GenerateToken(claims));
            }
            catch (IncorrectLoginOrPasswordException)
            {
                return LoginNotFound();
            }
        }

        [NonAction]
        public string GenerateToken(IList<Claim> claims)
        {
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(_options.SecretKey));

            JwtSecurityToken token =
                new(
                    issuer: _options.Issuer,
                    audience: _options.Audience,
                    claims: claims,
                    notBefore: DateTime.UtcNow,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(10)),
                    signingCredentials: new(signingKey, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}