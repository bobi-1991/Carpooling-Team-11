using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Carpooling.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/triprequests")]
    public class TripRequestsApiController : ControllerBase
    {
        private readonly ITripRequestService tripRequestService;
        private readonly IAuthValidator authValidator;

        public TripRequestsApiController(IAuthValidator authValidator, ITripRequestService tripRequestService)
        {
            this.authValidator = authValidator;
            this.tripRequestService = tripRequestService;
        }

        [HttpGet()]
        [Route("")]
        public async Task<IActionResult> GetAllAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.GetAllAsync());
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

        [HttpGet()]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.GetByIdAsync(id));
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

        [HttpPost("")]
        public async Task<IActionResult> CreateTripRequest([FromHeader] string credentials, TripRequestRequest tripRequest)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.CreateAsync(loggedUser, tripRequest));

            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DublicateEntityException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTripRequest([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.DeleteAsync(loggedUser, id));
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
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> RespondToTripRequest([FromHeader] string credentials, int id, [FromBody] string answer)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.EditRequestAsync(loggedUser, id, answer));

            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("driver/{id}")]
        public async Task<IActionResult> GetAllHisDriverRequestsAsync([FromHeader] string credentials, string id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.SeeAllHisDriverRequestsAsync(loggedUser, id));
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("passenger/{id}")]
        public async Task<IActionResult> GetAllHisPassengerRequests([FromHeader] string credentials, string id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                return Ok(await tripRequestService.SeeAllHisPassengerRequestsAsync(loggedUser, id));
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }



    }
}
