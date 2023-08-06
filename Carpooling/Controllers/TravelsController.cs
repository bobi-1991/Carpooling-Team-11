using Carpooling.BusinessLayer.Services.Contracts;
using CarPooling.Data.Exceptions;
using CarPooling.Data.Models;
using CarPooling.Data.Models.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Carpooling.Controllers
{
    public class TravelsController : Controller
    {
        private readonly IUserService userService;
        private readonly ICarService carService;
        private readonly IFeedbackService feedbackService;
        private readonly ITravelService travelService;

        public TravelsController(IUserService userService, ICarService carService, IFeedbackService feedbackService, ITravelService travelService)
        {
            this.userService = userService;
            this.carService = carService;
            this.feedbackService = feedbackService;
            this.travelService = travelService;
        }

        [HttpGet]
        public async Task<IActionResult> Index( [FromQuery] TravelQueryParameters searchQuery)
        {
            PaginatedList<Travel> travels = await this.travelService.FilterBy(searchQuery);

            return this.View(travels);
        }

        [HttpGet]
        public  async Task<IActionResult> Details(int id)
        {
            try
            {
                var travel = await this.travelService.GetByIdAsync(id);

                return this.View(travel);
            }
            catch (EntityNotFoundException e)
            {
                this.Response.StatusCode = StatusCodes.Status404NotFound;
                this.ViewData["ErrorMessage"] = e.Message;

                return this.View("Error");
            }
        }
    }
}
