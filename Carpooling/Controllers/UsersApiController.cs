using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthValidator authValidator;

        public UserApiController(IUserService userService, IAuthValidator authValidator)
        {
            this.userService = userService;
            this.authValidator = authValidator;
        }

        [HttpGet()]
        [Route("")]
        public async Task<IActionResult> GetAllAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = this.authValidator.ValidateCredentialAsync(credentials);
                var users = await userService.GetAllAsync();

                return StatusCode(StatusCodes.Status200OK, users);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}