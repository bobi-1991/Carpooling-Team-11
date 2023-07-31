using CarPooling.Data.Exceptions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation.Contracts;
using System.Net;
using Carpooling.Service.Dto_s.Requests;
using Carpooling.BusinessLayer.Exceptions;

namespace Carpooling.Controllers
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
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var travels = await this.travelService.GetAllAsync();
                return this.Ok(travels);
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTravelByIdAsync([FromHeader]string credentials, int id)
        {
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);
                var travel = await this.travelService.GetByIdAsync(id);
                return this.Ok(travel);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return this.StatusCode(500, ex.Message);
            }
        }

        //[HttpPut("")]

        //public async Task<IActionResult> UpdateTravelAsync([BindRequired] int travelId, DateTime departureTime, DateTime arrivalTime, int availableSeats, bool breaks, int startLocationId, int destinationLocationId, int carId, int statusId)
        //{
        //    try
        //    {
        //        var travelToUpdate = await this.travelService.UpdateAsync(travelId, departureTime, arrivalTime, availableSeats, breaks, startLocationId, destinationLocationId, carId, statusId);
        //        return this.Ok(travelToUpdate);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        [HttpPost("")]
        public async Task<IActionResult> CreateTravelAsync([FromHeader] string credentials, TravelRequest travelRequest)
        {
           
            try
            {
                var loggedUser = await this.authValidator.ValidateCredentialAsync(credentials);

                var travelToCreate = await this.travelService.CreateTravelAsync(loggedUser, travelRequest);
                return this.Ok(travelToCreate);
            }
            catch (EntityUnauthorizatedException e)
            {
                return Unauthorized(e.Message);
            }
            catch (EntityNotFoundException ex)
            {
                return this.NotFound(ex.Message);
            }
            catch (UnauthorizedOperationException ex)
            {
                return this.Forbid(ex.Message);
            }
         
            //catch (Exception ex)
            //{
            //    return StatusCode(500, ex.Message);
            //}
        }

        //[HttpDelete("{id}")]
        //[SwaggerOperation(Summary = "Delete Travel",
        //    Description = "Delete a specific Travel by Id Property")]
        //public async Task<IActionResult> DeleteTravelAsync(int id)
        //{
        //    try
        //    {
        //        var travelToDelete = await this.travelService.DeleteOwnTravelAsync(id);
        //        return this.Ok(travelToDelete);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("/DepartureTime")]
        //[SwaggerOperation(Summary = "Filter by DepartureTime",
        //    Description = "Filter All Travels by DepartureTime property")]
        //public async Task<IActionResult> GetByDepartureTimeAsync(DateTime dateTime)
        //{
        //    try
        //    {
        //        var travels = await this.travelService.GetByDepartureTimeAsync(dateTime);
        //        return this.Ok(travels);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("/Destination")]
        //[SwaggerOperation(Summary = "Filter by Destination",
        //    Description = "Filter All Travels by Destination Property")]
        //public async Task<IActionResult> GetTravelByDestinationAsync(int destinationLocationId)
        //{
        //    try
        //    {
        //        var travels = await this.travelService.GetByDestinationAsync(destinationLocationId);
        //        return this.Ok(travels);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("/StartLocation")]
        //[SwaggerOperation(Summary = "Filter by StartLocation",
        //    Description = "Filter All Travels by StartLocation Property")]
        //public async Task<IActionResult> GetByTravelByStartLocationAsync(int startlocationId)
        //{
        //    try
        //    {
        //        var travels = await this.travelService.GetByStartLocationAsync(startlocationId);
        //        return this.Ok(travels);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("/AvailableSeats")]
        //[SwaggerOperation(Summary = "Filter by AvailableSeats",
        //    Description = "Filter All Travels by AvailableSeats")]
        //public async Task<IActionResult> GetByAvailableSeatsAsync()
        //{
        //    try
        //    {
        //        var travels = await this.travelService.GetByAvailableSeatsAsync();
        //        return this.Ok(travels);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpGet("/Status")]
        //[SwaggerOperation(Summary = "Filter by Status",
        //    Description = "Filter All Travels by Status")]
        //public async Task<IActionResult> GetByStatusAsync(Status status)
        //{
        //    try
        //    {
        //        var travels = await this.travelService.GetByStatusAsync(status);
        //        return this.Ok(travels);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpPost("/AddUserToTravel")]
        //[SwaggerOperation(Summary = "Add User To Travel",
        //    Description = "Add a specific User to a specific Travel")]
        //public async Task<IActionResult> AddUserToTravelAsync([BindRequired] int ownerId, [BindRequired] int travelId, [BindRequired] int userId)
        //{
        //    try
        //    {
        //        var travel = await this.travelService.AddUserToTravelAsync(ownerId, travelId, userId);
        //        return this.Ok(travel);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        return NotFound(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }

}
