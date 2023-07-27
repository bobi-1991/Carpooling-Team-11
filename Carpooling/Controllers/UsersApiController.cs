using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var users = await userService.GetAllAsync();

                return StatusCode(StatusCodes.Status200OK, users);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromHeader] string credentials, string id)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var user = await this.userService.GetByIdAsync(id);

                return StatusCode(StatusCodes.Status200OK, user);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRequest userRequest)
        {
            try
            {
                var userResponse = await this.userService.RegisterAsync(userRequest);

                return Ok(userResponse);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromHeader] string credentials, string id)
        {
            try
            {
                User loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var result = await userService.DeleteAsync(loggedUser, id);

                return StatusCode(StatusCodes.Status200OK, result);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetByUsernameAsync([FromHeader] string credentials, string username)
        {
            try
            {
                User loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var user = await userService.GetByUsernameAsync(username);

                return Ok(user);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> TravelHistoryAsync([FromHeader] string credentials, string id)
        {
            try
            {
                User loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var travels = await userService.TravelHistoryAsync(loggedUser, id);

                return Ok(travels);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
        }
    }
}