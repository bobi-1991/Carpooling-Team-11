using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Controllers
{
    public class TripRequestController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ITripRequestService tripRequestService;
        private readonly IMapper mapper;
        public TripRequestController(UserManager<User> userManager, ITripRequestService tripRequestService, IMapper mapper)
        {
            this.userManager = userManager;
            this.tripRequestService = tripRequestService;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> CreateTripRequest([FromRoute] int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            var tripRequestViewModel = new TripRequestViewModel();
            tripRequestViewModel.TravelId = id;
            return View(tripRequestViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> CreateTripRequest(TripRequestViewModel tripRequestViewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(tripRequestViewModel);
            }
            try
            {
                var user = await userManager.Users.Include(c => c.Cars)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
                tripRequestViewModel.Status = CarPooling.Data.Models.Enums.TripRequestEnum.Pending;
                var tripRequest = mapper.Map<TripRequest>(tripRequestViewModel);
                var createdTripRequest = await tripRequestService.CreateTripRequestForMVCAsync(user, tripRequest);
                return this.RedirectToAction("DriverInfo", "Personal", new { id = user.Id });
            }
            catch (EntityNotFoundException e)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (DublicateEntityException ex)
            {
                this.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (DriverPassengerMachingException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> MyBookings()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            try
            {
                var user = await userManager.Users
                    .Include(c => c.Cars)
                    .Include(t => t.PassengerTripRequests)
                    .Include(t => t.DriverTripRequests)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                IEnumerable<TripRequestViewResponseModel> result = await this.tripRequestService.SeeAllHisPassengerRequestsMVCAsync(user, user.Id);


                return this.View(result);


            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");

            }

        }

        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            try
            {
                var user = await userManager.Users
                    .Include(c => c.Cars)
                    .Include(t => t.PassengerTripRequests)
                    .Include(t => t.DriverTripRequests)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                IEnumerable<TripRequestViewResponseModel> result = await this.tripRequestService.SeeAllHisDriverRequestsMVCAsync(user, user.Id);

                return this.View(result);
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");

            }

        }

        [HttpPost]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            try
            {
                var user = await userManager.Users
                 .Include(c => c.Cars)
                .Include(t => t.PassengerTripRequests)
                 .Include(t => t.DriverTripRequests)
                 .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                var result = await tripRequestService.DeleteAsync(user, id);

                return RedirectToAction("MyBookings", "TripRequest");
            }
            catch (EntityUnauthorizatedException e)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }
            catch (EntityNotFoundException e)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;
                return View("Error");
            }

        }



        [HttpPost]
        public async Task<IActionResult> EditTripRequest(int tripRequestId, string response)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }

            try
            {
                var user = await userManager.Users.Include(c => c.Cars).Include(t => t.PassengerTripRequests).Include(t => t.DriverTripRequests)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                await tripRequestService.EditRequestMVCAsync(user, tripRequestId, response);

                return RedirectToAction("MyRequests", "TripRequest", new { id = user.Id });
            }
            catch (EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (UnauthorizedAccessException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage2"] = ex.Message;
                return View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> MyPassengers()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            try
            {
                var user = await userManager.Users
                    .Include(c => c.Cars)
                    .Include(t => t.PassengerTripRequests)
                    .Include(t => t.DriverTripRequests)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));



                var result = await this.tripRequestService.SeeAllHisDriverPassengersMVCAsync(user, user.Id);


                return this.View(result);


            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");

            }
        }
    }
}
