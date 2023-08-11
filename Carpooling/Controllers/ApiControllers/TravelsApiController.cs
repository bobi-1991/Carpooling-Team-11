using CarPooling.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using System.Net;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Dto_s.Requests;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;

namespace Carpooling.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/travels")]
    public class TravelsApiController : ControllerBase
    {
        private readonly ITravelService travelService;
        private readonly IAuthValidator authValidator;

        public TravelsApiController(ITravelService travelService, IAuthValidator authValidator)
        {
            this.travelService = travelService;
            this.authValidator = authValidator;
        }

        [HttpGet()]
        [Route("")]
        public async Task<IActionResult> GetAllAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var travels = await travelService.GetAllAsync();
                return Ok(travels);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTravelByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var travel = await travelService.GetByIdAsync(id);
                return Ok(travel);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("complete/{id}")]
        public async Task<IActionResult> SetTravelToIsCompleteAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await travelService.SetTravelToIsCompleteAsync(loggedUser, id));
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        // Shoud be tested
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTravelAsync([FromHeader] string credentials, [FromRoute] int id, TravelUpdateDto travelDataForUpdate)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await travelService.UpdateAsync(loggedUser, id, travelDataForUpdate));
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedOperationException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTravelAsync([FromHeader] string credentials, TravelRequest travelRequest)
        {

            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);

                var travelToCreate = await travelService.CreateTravelAsync(loggedUser, travelRequest);
                return Ok(travelToCreate);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTravelAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await travelService.DeleteAsync(loggedUser, id));
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

   

        [HttpGet("filter")]
        public async Task<ActionResult> FilterTravelsAndSortAsync([FromHeader] string credentials, [FromQuery] string filter)
        {
            try
            {
                var loggerUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await travelService.FilterTravelsAndSortAsync(filter));
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EmptyListException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}

