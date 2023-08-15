using AutoMapper;
using Carpooling.BusinessLayer.Exceptions;
using Carpooling.BusinessLayer.Services;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using Carpooling.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Carpooling.BusinessLayer.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Carpooling.Controllers
{
    public class CarController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly ICarService carService;
        private readonly IMapper mapper;

        public CarController(UserManager<User> userManager, ICarService carService, IMapper mapper)
        {
            this.userManager = userManager;
            this.carService = carService;
            this.mapper = mapper;

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
            catch (DuplicateEntityException ex)
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
    }
}
