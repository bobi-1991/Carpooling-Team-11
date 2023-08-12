using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.BusinessLayer.Validation;
using Carpooling.Models;
using Carpooling.Service.Dto_s.Responses;
using CarPooling.Data.Data;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Carpooling.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IUserService userService;
        private readonly ICarService carService;
        private readonly IFeedbackService feedbackService;
        private readonly ITripRequestService tripRequestService;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;
        private readonly ITravelService travelService;
        private readonly CarPoolingDbContext dbContext;


        public PersonalController(IUserService userService, ICarService carService, IFeedbackService feedbackService, UserManager<User> userManager, IMapper mapper, ITravelService travelService, ITripRequestService tripRequestService, CarPoolingDbContext dbContext)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tripRequestService = tripRequestService;
            this.travelService = travelService;
            this.tripRequestService = tripRequestService;
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> DriverInfo([FromRoute] string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            if (id == null)
            {
                var user1 = await userManager.GetUserAsync(User);
                id = user1.Id;
            }
            var cars = await carService.GetAllAsync();
            var feedbacks = await feedbackService.GetAllAsync();
            var driverFeedbacks = feedbacks.Where(x => x.DriverId == id);


            var user = await userManager.Users.Include(c => c.Cars)
                   .SingleAsync(x => x.Id.Equals(id));

            var driverCars = user.Cars;


            var driverModel = new DriverViewInfoModel
            {
                Username = user.UserName,
                AverageRating = user.AverageRating,
                Feedbacks = driverFeedbacks,
                Cars = driverCars
            };

            return View(driverModel);
        }

        [HttpGet]
        public async Task<IActionResult> PassengerInfo([FromRoute] string id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            if (id == null)
            {
                var user1 = await userManager.GetUserAsync(User);
                id = user1.Id;
            }
            var user = await userService.GetByIdAsync(id);
            var feedbacks = await feedbackService.GetAllAsync();
            var passengerFeedbacks = feedbacks.Where(x => x.PassengerId == id);

            var passengerModel = new PassengerViewInfoModel
            {
                Username = user.Username,
                AverageRating = user.AverageRating,
                Feedbacks = passengerFeedbacks

            };

            return View(passengerModel);
        }

        [HttpGet]
        public async Task<IActionResult> Menu()
        {
            return View("~/Views/Shared/_MenuPartial.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            var carViewModel = new CarViewModel();
            return View(carViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(CarViewModel carViewModel)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            if (!this.ModelState.IsValid)
            {
                return this.View(carViewModel);
            }
            try
            {

                var user = await userManager.Users.Include(c => c.Cars)
                    .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
                var car = mapper.Map<Car>(carViewModel);
                var createdCar = await carService.CreateAsync(car, user);
                return this.RedirectToAction("DriverInfo", "Personal", new { id = user.Id });
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (DublicateEntityException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
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
        public async Task<IActionResult> MyCars()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Challenge();
                }
                var user = await userManager.Users.Include(x => x.Cars)
                       .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
                var carHistory = user.Cars;
                var cars = new List<Car>();
                foreach (var car in carHistory)
                {
                    var car1 = await this.carService.GetByIdAsync(car.Id);
                    if (car.IsDeleted == false)
                    {
                        cars.Add(car1);
                    }
                }
                return this.View(cars);
            }
            catch (EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<IActionResult> MyTravels()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Challenge();
            }
            try
            {
                if (!User.Identity.IsAuthenticated)
                {
                    return Challenge();
                }
                var user = await userManager.Users.Include(x => x.TravelHistory)
                       .SingleAsync(x => x.UserName.Equals(User.Identity.Name));

                var history = user.TravelHistory;
                var trips = new List<TravelViewResponseWithId>();


                foreach (var travel in history)
                {
                    var isCompleted = travel.IsCompleted;
                    var trip = await this.travelService.GetByIdAsync(travel.Id);

                    trips.Add(new TravelViewResponseWithId
                    {
                        Id = travel.Id,
                        StartLocationName = trip.StartLocationName,
                        DestinationName = trip.DestinationName,
                        DepartureTime = trip.DepartureTime,
                        ArrivalTime = trip.ArrivalTime,
                        AvailableSeats = trip.AvailableSeats,
                        IsCompleted = (bool)isCompleted,
                        CarRegistration = trip.CarRegistration,
                    });
                }

                return this.View(trips);
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
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

                return RedirectToAction("MyBookings", "Personal");
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

                return RedirectToAction("MyRequests", "Personal", new { id = user.Id });
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





