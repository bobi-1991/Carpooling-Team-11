using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers
{
    public class PersonalController : Controller
    {
        private readonly IUserService userService;
        private readonly ICarService carService;
        private readonly IFeedbackService feedbackService;

        public PersonalController(IUserService userService, ICarService carService, IFeedbackService feedbackService)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
        }

        [HttpGet]
        public async Task<IActionResult> DriverInfo(string id)
        {
            var user = await userService.GetByIdAsync(id);
            var cars = await carService.GetAllAsync();
            var feedbacks = await feedbackService.GetAllAsync();
            var driverFeedbacks = feedbacks.Where(x => x.DriverId == id);

            var car = cars.FirstOrDefault(x => x.DriverId == id);

            var driverModel = new DriverViewInfoModel
            {
                Username = user.Username,
                AverageRating = user.AverageRating,
                Feedbacks = driverFeedbacks,
                Capacity = Convert.ToString(car?.AvailableSeats) ?? Convert.ToString("-"),
                CarBrand = car?.Brand ?? "-",
                CarModel = car?.Model ?? "-",
                CarColor = car?.Color ?? "-",
                Registration = car?.Registration ?? "-",
                CanSmoke = car?.CanSmoke ?? false
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
    }


}


