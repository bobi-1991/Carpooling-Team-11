using Carpooling.BusinessLayer.Services.Contracts;
using Carpooling.Models;
using CarPooling.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService userService;
        private readonly ITravelService travelService;

        public HomeController(IUserService userService, ITravelService travelService)
        {
            this.userService = userService;
            this.travelService = travelService;
        }

        public async Task<IActionResult> Index()
        {
            var HomeView = new HomeViewModel
            {
                TopTravelOrganizers = await userService.TopTravelOrganizers(10),
                TopPassengers = await userService.TopPassengers(10),
                TotalTravelCount = (await travelService.GetAllAsync()).Count(),
                TotalUsersCount = (await userService.GetAllAsync()).Count()
            };

            return View(HomeView);
        }
    }
}
