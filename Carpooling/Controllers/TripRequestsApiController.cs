using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Carpooling.Controllers
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
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                return this.Ok(await this.tripRequestService.GetAllAsync());
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        [HttpGet()]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                return this.Ok(await this.tripRequestService.GetByIdAsync(id));
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
                return this.StatusCode(500, ex.Message);
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateTripRequest([FromHeader] string credentials, TripRequestRequest tripRequest)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                return this.Ok(await this.tripRequestService.CreateAsync(loggedUser, tripRequest));

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
                return this.StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]     
        public async Task<IActionResult> DeleteTripRequest([FromHeader]string credentials, int id)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                return this.Ok(await this.tripRequestService.DeleteAsync(loggedUser, id));
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
        public async Task<IActionResult> RespondToTripRequest([FromHeader]string credentials,int id,[FromBody]  string answer)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                return this.Ok(await this.tripRequestService.EditRequestAsync(loggedUser, id ,answer));
               
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
            //catch (Exception ex)
            //{
            //    return NotFound(ex.Message);
            //}
        }





    }
}
