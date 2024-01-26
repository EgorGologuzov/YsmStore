using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using YsmStore.API.Data.Exceptions;
using YsmStore.API.Data.Interfaces;
using YsmStore.API.Utils;
using YsmStore.Services.Utils;

namespace YsmStore.API.Controllers
{
    public class GeneralController<TEntity> : ControllerBase
    {
        protected readonly IRepository<TEntity> Repository;
        protected readonly ILogger Logger;
        protected readonly IMapper Mapper;

        public GeneralController(
            IRepository<TEntity> repos,
            ILogger logger,
            IMapper mapper)
        {
            Repository = repos;
            Logger = logger;
            Mapper = mapper;
        }

        [NonAction]
        public IActionResult EntityNotFound()
        {
            ModelState.AddModelError(RequestError.EntityNotFound.Code, RequestError.EntityNotFound.Message);
            return NotFound(ModelState);
        }

        [NonAction]
        public IActionResult InvalidData()
        {
            ModelState.AddModelError(RequestError.InvalidData.Code, RequestError.InvalidData.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult IncorrectLoginOrPassword()
        {
            ModelState.AddModelError(RequestError.IncorrestLoginOrPassword.Code, RequestError.IncorrestLoginOrPassword.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult LoginNotAvailable()
        {
            ModelState.AddModelError(RequestError.LoginNotAvailable.Code, RequestError.LoginNotAvailable.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult LoginNotFound()
        {
            ModelState.AddModelError(RequestError.LoginNotFound.Code, RequestError.LoginNotFound.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult AmountIsNotPositive()
        {
            ModelState.AddModelError(RequestError.AmountIsNotPositive.Code, RequestError.AmountIsNotPositive.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult OptionNumberOutOfRange()
        {
            ModelState.AddModelError(RequestError.OptionNumberOutOfRange.Code, RequestError.OptionNumberOutOfRange.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult NoProductsInOrder()
        {
            ModelState.AddModelError(RequestError.NoProductsInOrder.Code, RequestError.NoProductsInOrder.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public IActionResult NotEnoughtProduct()
        {
            ModelState.AddModelError(RequestError.NotEnoughtProduct.Code, RequestError.NotEnoughtProduct.Message);
            return BadRequest(ModelState);
        }

        [NonAction]
        public string GetClaimValue(string type)
        {
            Claim claim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == type);

            return claim is null ? null : claim.Value;
        }

        [NonAction]
        protected virtual async Task<TReturnDto> GetReturnDtoByEntityId<TReturnDto>(object id)
        {
            return Mapper.Map<TReturnDto>(await Repository.GetById(id));
        }

        [NonAction]
        protected virtual async Task<IActionResult> GetByIdGeneric(object id)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = await Repository.GetById(id);

            if (entity is null)
            {
                return EntityNotFound();
            }

            return Ok(entity);
        }

        [NonAction]
        protected virtual async Task<IActionResult> CreateGeneric<TCreateDto>(TCreateDto data)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = Mapper.Map<TCreateDto, TEntity>(data);

            try
            {
                TEntity result = await Repository.Create(entity);

                return Ok(result);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                Logger.LogError("Failed create {type} with data {data}", typeof(TEntity).Name, data.ToJson());
                return InvalidData();
            }
        }

        [NonAction]
        protected virtual async Task<IActionResult> UpdateGeneric<TUpdateDto>(object id, TUpdateDto data)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            TEntity entity = await Repository.GetById(id);
            Mapper.Map(data, entity);

            try
            {
                TEntity result = await Repository.Update(id, entity);

                return Ok(result);
            }
            catch (EntityNotFoundException)
            {
                return EntityNotFound();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                Logger.LogError("Failed update {type} with data {data}", typeof(TEntity).Name, data.ToJson());
                return InvalidData();
            }
        }

        [NonAction]
        protected virtual async Task<IActionResult> DeleteGeneric(object id)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            try
            {
                TEntity result = await Repository.Delete(id);

                return Ok(result);
            }
            catch (EntityNotFoundException)
            {
                return EntityNotFound();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                Logger.LogError("Failed delete {type} with data {id}", typeof(TEntity).Name, id);
                return InvalidData();
            }
        }
    }
}
