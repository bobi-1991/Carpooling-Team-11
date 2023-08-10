using Carpooling.BusinessLayer.Dto_s.AdminModels;
using Carpooling.BusinessLayer.Dto_s.UpdateModels;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Carpooling.Controllers.ApiControllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersApiController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IAuthValidator authValidator;

        public UsersApiController(IUserService userService, IAuthValidator authValidator)
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
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var users = await userService.GetAllAsync();

                return StatusCode(StatusCodes.Status200OK, users);
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

        [HttpGet("id/{id}")]
        public async Task<IActionResult> GetById([FromHeader] string credentials, [FromRoute] string id)
        {
            try
            {
                var loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var user = await userService.GetByIdAsync(id);

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
                var userResponse = await userService.RegisterAsync(userRequest);

                return Ok(userResponse);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (DublicateEntityException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync([FromHeader] string credentials, string id)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
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

        [HttpGet("username/{username}")]
        public async Task<IActionResult> GetByUsernameAsync([FromHeader] string credentials, [FromRoute] string username)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
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
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetByEmailAsync([FromHeader] string credentials, [FromRoute] string email)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var user = await userService.GetByEmailAsync(email);
                
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
        [HttpGet("phoneNumber/{phoneNumber}")]
        public async Task<IActionResult> GetByPhoneNUmberAsync([FromHeader] string credentials, [FromRoute] string phoneNumber)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var user = await userService.GetByPhoneNumberAsync(phoneNumber);

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
        [HttpGet("travel-history/{id}")]
        public async Task<IActionResult> TravelHistoryAsync([FromHeader] string credentials, string id)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var travels = await userService.TravelHistoryAsync(loggedUser, id);

                return Ok(travels);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync([FromHeader] string credentials, string id, [FromBody] UserUpdateDto userUpdateDto)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                UserResponse updatedUser = await userService.UpdateAsync(loggedUser, id, userUpdateDto);

                return StatusCode(StatusCodes.Status200OK, updatedUser);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (DublicateEntityException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }

        }

        [HttpPut("ban")]
        public async Task<IActionResult> BanAsync([FromHeader] string credentials, [FromBody] BanOrUnBanDto userToBeBanned)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var message = await userService.BanUser(loggedUser, userToBeBanned);
                return Ok(message);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

        [HttpPut("unban")]
        public async Task<IActionResult> UnBanAsync([FromHeader] string credentials, [FromBody] BanOrUnBanDto userToBeUnBanned)
        {
            try
            {
                User loggedUser = await authValidator.ValidateCredentialAsync(credentials);
                var message = await userService.UnBanUser(loggedUser, userToBeUnBanned);
                return StatusCode(StatusCodes.Status200OK, message);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (ArgumentNullException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (ArgumentException e)
            {
                return StatusCode(StatusCodes.Status403Forbidden, e.Message);
            }
        }

    }
}