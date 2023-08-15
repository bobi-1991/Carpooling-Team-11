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
        //private readonly ITripRequestService tripRequestService;
        private readonly UserManager<User> userManager;

        public PersonalController(IUserService userService, ICarService carService, IFeedbackService feedbackService, UserManager<User> userManager)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.userManager = userManager;
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
            var feedbacksAsPassenger = feedbacks.Where(x=>x.PassengerId == id);

            var user = await userManager.Users.Include(c => c.Cars)
                   .SingleAsync(x => x.Id.Equals(id));

            var driverCars = user.Cars;


            var driverModel = new DriverViewInfoModel
            {
                Username = user.UserName,
                AverageRating = user.AverageRating,
                DriverFeedbacks = driverFeedbacks,
                FeedbacksAsPessanger = feedbacksAsPassenger,
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
            var passengerFeedbacks = feedbacks.Where(x => x.DriverId == id);
            
          //  var currentUser = new User();

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

        //[HttpGet]
        //public async Task<IActionResult> CreateCar()
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Challenge();
        //    }
        //    var carViewModel = new CarViewModel();
        //    return View(carViewModel);
        //}

        //[HttpPost]
        //public async Task<IActionResult> CreateCar(CarViewModel carViewModel)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return Challenge();
        //    }
        //    if (!this.ModelState.IsValid)
        //    {
        //        return this.View(carViewModel);
        //    }
        //    try
        //    {

        //        var user = await userManager.Users.Include(c => c.Cars)
        //            .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
        //        var car = mapper.Map<Car>(carViewModel);
        //        var createdCar = await carService.CreateAsync(car, user);
        //        return this.RedirectToAction("DriverInfo", "Personal", new { id = user.Id });
        //    }
        //    catch (UnauthorizedOperationException ex)
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        //        this.ViewData["ErrorMessage"] = ex.Message;
        //        return View("Error");
        //    }
        //    catch (DublicateEntityException ex)
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
        //        this.ViewData["ErrorMessage"] = ex.Message;
        //        return View("Error");
        //    }
        //}

        
        //[HttpGet]
        //public async Task<IActionResult> MyCars()
        //{
        //    try
        //    {
        //        if (!User.Identity.IsAuthenticated)
        //        {
        //            return Challenge();
        //        }
        //        var user = await userManager.Users.Include(x => x.Cars)
        //               .SingleAsync(x => x.UserName.Equals(User.Identity.Name));
        //        var carHistory = user.Cars;
        //        var cars = new List<Car>();
        //        foreach (var car in carHistory)
        //        {
        //            var car1 = await this.carService.GetByIdAsync(car.Id);
        //            if (car.IsDeleted == false)
        //            {
        //                cars.Add(car1);
        //            }
        //        }
        //        return this.View(cars);
        //    }
        //    catch (EntityNotFoundException ex)
        //    {
        //        HttpContext.Response.StatusCode = StatusCodes.Status404NotFound;
        //        this.ViewData["ErrorMessage"] = ex.Message;
        //        return View("Error");
        //    }
        //}
    }
}





