using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
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



    }
}
