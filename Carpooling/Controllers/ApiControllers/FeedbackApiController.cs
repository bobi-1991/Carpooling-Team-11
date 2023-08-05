using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers.ApiControllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackApiController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IAuthValidator _authValidator;
        private readonly IMapper _mapper;
        public FeedbackApiController(IFeedbackService feedbackService, IAuthValidator authValidator, IMapper mapper)
        {
            _feedbackService = feedbackService;
            _authValidator = authValidator;
            _mapper = mapper;
        }
        //Create Delete Update
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FeedbackDTO>>> GetAllFeedbacksAsync([FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                var feedbacks = await _feedbackService.GetAllAsync();

                return StatusCode(StatusCodes.Status200OK, feedbacks);
            }

            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FeedbackDTO>> GetFeedbackByIdAsync([FromHeader] string credentials, int id)
        {
            try
            {
                var loggerUser = await _authValidator.ValidateCredentialAsync(credentials);
                var feedback = _mapper.Map<FeedbackDTO>(await _feedbackService.GetByIdAsync(id));
                return StatusCode(StatusCodes.Status200OK, feedback);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<FeedbackDTO>> CreateFeedbackAsync([FromHeader] string credentials, [FromBody] FeedbackDTO feedbackDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);

                Feedback feedback = _mapper.Map<Feedback>(feedbackDTO);
                Feedback feedbackToCreate = await _feedbackService.CreateAsync(feedback, loggedUser);

                return StatusCode(StatusCodes.Status201Created, feedbackToCreate);
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFeedbackAsync(int id, [FromHeader] string credentials, [FromBody] FeedbackDTO feedbackDTO)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                Feedback feedback = _mapper.Map<Feedback>(feedbackDTO);
                Feedback feedbackToUpdate = await _feedbackService.UpdateAsync(id, loggedUser, feedback);

                return NoContent();
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeedbackAsync(int id, [FromHeader] string credentials)
        {
            try
            {
                var loggedUser = await _authValidator.ValidateCredentialAsync(credentials);
                await _feedbackService.DeleteAsync(id, loggedUser);
                return NoContent();
            }
            catch (UnauthorizedOperationException e)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, e.Message);
            }
            catch (EntityNotFoundException e)
            {
                return StatusCode(StatusCodes.Status404NotFound, e.Message);
            }
        }
    }
}
