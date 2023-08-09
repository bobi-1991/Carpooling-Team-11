using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public PersonalController(IUserService userService, ICarService carService,
            IFeedbackService feedbackService, UserManager<User> userManager, 
            IMapper mapper, ITripRequestService tripRequestService)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
            this.mapper = mapper;
            this.tripRequestService = tripRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> DriverInfo(string id)
        {
          //  var user = await userService.GetByIdAsync(id);
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
                //Capacity = Convert.ToString(car?.AvailableSeats) ?? Convert.ToString("-"),
                //CarBrand = car?.Brand ?? "-",
                //CarModel = car?.Model ?? "-",
                //CarColor = car?.Color ?? "-",
                //Registration = car?.Registration ?? "-",
                //CanSmoke = car?.CanSmoke ?? false
            };

            return View(driverModel);
        }

        [HttpGet]
        public async Task<IActionResult> PassengerInfo(string id)
        {
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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> CreateCar()
        {
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
                return this.RedirectToAction("DriverInfo", "Personal", new {id = user.Id});
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (DuplicateEntityException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
        }

        [HttpGet]
        public async Task<IActionResult> CreateTripRequest([FromRoute] int id)
        {
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
            catch (EntityNotFoundException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (UnauthorizedOperationException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                this.ViewData["ErrorMessage"] = ex.Message;
                return View("Error");
            }
            catch (DuplicateEntityException ex)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
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
    }


}


